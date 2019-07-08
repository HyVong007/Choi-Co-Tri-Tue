using System;
using UnityEngine;
using System.Collections.Generic;


namespace IQChess.GoChess
{
	public sealed class Board : BoardBase<Player.IDType, ChessPiece, Board, Player>
	{
		#region KHỞI TẠO
		[Serializable]
		public new sealed class Config : BoardBase<Player.IDType, ChessPiece, Board, Player>.Config
		{
			/// <summary>
			/// Nếu true: con cờ nằm giữa ô. Nếu false thì con cờ nằm trên giao điểm đường kẻ.
			/// </summary>
			public bool gridCenter = true;
		}

		private static readonly Vector3Int[] directions = new Vector3Int[]
		{
			Vector3Int.left, Vector3Int.right, Vector3Int.up, Vector3Int.down
		};

		private readonly IReadOnlyDictionary<Player.IDType, List<ChessPiece.Land>> lands = new Dictionary<Player.IDType, List<ChessPiece.Land>>()
		{
			[Player.IDType.BLACK] = new List<ChessPiece.Land>(),
			[Player.IDType.WHITE] = new List<ChessPiece.Land>()
		};
		[SerializeField] private Transform cellPrefab, boardCells;


		private new void Awake()
		{
			base.Awake();

			// Vẽ đường kẻ
			var config = Config.instance as Config;
			var boardSize = config.gridCenter ? Conversion.arraySize : Conversion.arraySize + Vector2Int.one;
			var origin = config.gridCenter ? Conversion.origin + Conversion.ZERO_DOT_FIVE : Conversion.origin;
			var pos = new Vector3();
			var index = new Vector2Int();
			for (pos.x = origin.x, index.x = 0; index.x < boardSize.x; ++pos.x, ++index.x)
				for (pos.y = origin.y, index.y = 0; index.y < boardSize.y; ++pos.y, ++index.y)
					Instantiate(cellPrefab, boardCells).localPosition = pos;

			// Vẽ hình nền Background
		}


		private void Start()
		{
			GlobalInformations.initializedTypes.Add(GetType());
		}
		#endregion


		#region SAVE/ LOAD
		public override string SaveToJson()
		{
			throw new NotImplementedException();
		}

		public override byte[] SaveToStream()
		{
			throw new NotImplementedException();
		}

		public override void Load(string json)
		{
			throw new NotImplementedException();
		}

		public override void Load(byte[] stream)
		{
			throw new NotImplementedException();
		}
		#endregion


		#region DO/ UNDO ACTION
		private struct State
		{
			public int emptyHole;
			public List<ChessPiece.Land> dyingEnemies;
			public Dictionary<ChessPiece.Land, int> enemies, allies;


			public static State Create(Board board, Player.IDType centerID, Vector3Int centerPos)
			{
				var data = new State();
				data.dyingEnemies = new List<ChessPiece.Land>();
				data.enemies = new Dictionary<ChessPiece.Land, int>();
				data.allies = new Dictionary<ChessPiece.Land, int>();
				var lands = new Dictionary<Player.IDType, Dictionary<ChessPiece.Land, int>>()
				{
					[Player.IDType.BLACK] = new Dictionary<ChessPiece.Land, int>(),
					[Player.IDType.WHITE] = new Dictionary<ChessPiece.Land, int>()
				};

				// Tìm lổ trống và tất cả land.
				foreach (var direction in directions)
				{
					var pos = centerPos + direction;
					if (!pos.IsValidArray()) continue;
					var c = board.array[pos.x][pos.y];
					if (!c) { ++data.emptyHole; continue; }
					if (!lands[c.playerID].ContainsKey(c.land)) lands[c.playerID][c.land] = 1;
					else ++lands[c.playerID][c.land];
				}

				// Xác định land địch sắp chết, land địch còn sống và land mình.
				foreach (var id_dict in lands)
					foreach (var land_point in id_dict.Value)
						if (id_dict.Key != centerID && land_point.Key.airHole == land_point.Value) data.dyingEnemies.Add(land_point.Key);
						else if (id_dict.Key != centerID) data.enemies[land_point.Key] = land_point.Value;
						else data.allies[land_point.Key] = land_point.Value;

				return data;
			}
		}

		protected override void _Play(ref ActionData data, bool undo = false)
		{
			if (chessPieceCount.Count != 0) chessPieceCount.Clear();
			var centerPos = data.pos[0];
			var enemyID = data.playerID == Player.IDType.BLACK ? Player.IDType.WHITE : Player.IDType.BLACK;
			var state = (State)(data.customData != null ? data.customData : data.customData = State.Create(this, data.playerID, centerPos));
			if (!undo)
			{
				// CHƠI BÌNH THƯỜNG
				// Liên kết các land mình hiện tại tạo land mới cho cờ ở center.
				var chessPiece = ChessPiece.Get(data.playerID);
				chessPiece.transform.position = centerPos.ArrayToWorld();
				array[centerPos.x][centerPos.y] = chessPiece;
				chessPiece.land = new ChessPiece.Land() { airHole = state.emptyHole };
				lands[chessPiece.playerID].Add(chessPiece.land);
				chessPiece.land.positions.Add(centerPos);
				foreach (var land_point in state.allies)
				{
					chessPiece.land.airHole += (land_point.Key.airHole - land_point.Value);
					chessPiece.land.positions.AddRange(land_point.Key.positions);
					foreach (var pos in land_point.Key.positions) array[pos.x][pos.y].land = chessPiece.land;
					lands[chessPiece.playerID].Remove(land_point.Key);
				}

				// Trừ lổ thở land địch
				foreach (var enemy_point in state.enemies) enemy_point.Key.airHole -= enemy_point.Value;
				foreach (var enemy in state.dyingEnemies)
				{
					// Giết land địch
					lands[enemyID].Remove(enemy);
					foreach (var p in enemy.positions)
					{
						array[p.x][p.y].Recycle();
						array[p.x][p.y] = null;
						foreach (var direction in directions)
						{
							var _p = p + direction;
							if (!_p.IsValidArray()) continue;
							var land = array[_p.x][_p.y]?.land;
							if (land != null && land != enemy) ++land.airHole;
						}
					}
				}
			}
			else
			{
				// HỦY NƯỚC ĐÃ ĐI
				// Xóa con cờ đã đi
				array[centerPos.x][centerPos.y].Recycle();
				array[centerPos.x][centerPos.y] = null;

				// Khôi phục con trỏ land mình
				foreach (var land in state.allies.Keys)
					foreach (var pos in land.positions) array[pos.x][pos.y].land = land;

				// Khôi phục lổ thở land địch còn sống
				foreach (var enemy_point in state.enemies) enemy_point.Key.airHole += enemy_point.Value;

				// Khôi phục land địch bị giết
				foreach (var enemy in state.dyingEnemies)
				{
					lands[enemyID].Add(enemy);
					foreach (var pos in enemy.positions)
					{
						var chessPiece = ChessPiece.Get(enemyID);
						chessPiece.transform.position = pos.ArrayToWorld();
						chessPiece.land = enemy;
						array[pos.x][pos.y] = chessPiece;
					}
				}
			}
		}
		#endregion


		public bool CanPlay(Player.IDType playerID, Vector3Int pos)
		{
			if (array[pos.x][pos.y]) return false;

			var lands = new Dictionary<Player.IDType, Dictionary<ChessPiece.Land, int>>()
			{
				[Player.IDType.BLACK] = new Dictionary<ChessPiece.Land, int>(),
				[Player.IDType.WHITE] = new Dictionary<ChessPiece.Land, int>()
			};
			foreach (var direction in directions)
			{
				var p = pos + direction;
				if (!p.IsValidArray()) goto CONTINUE_LOOP_DIRECTIONS;
				var c = array[p.x][p.y];
				if (!c) return true;

				if (!lands[c.playerID].ContainsKey(c.land)) lands[c.playerID][c.land] = 1;
				else ++lands[c.playerID][c.land];
				CONTINUE_LOOP_DIRECTIONS:;
			}

			foreach (var kvp in lands)
				foreach (var kvpValue in kvp.Value)
					if ((kvp.Key == playerID && kvpValue.Key.airHole > kvpValue.Value) || (kvp.Key != playerID && kvpValue.Key.airHole == kvpValue.Value)) return true;
			return false;
		}


		/// <summary>
		/// Nếu có action (Do/Undo) thì phải Clear.
		/// </summary>
		private readonly Dictionary<Player.IDType, int> chessPieceCount = new Dictionary<Player.IDType, int>();

		public int ChessPieceCount(Player.IDType playerID)
		{
			int c = 0;
			foreach (var land in lands[playerID]) c += land.positions.Count;
			return c;
		}


		public override bool IsWin(Player.IDType playerID)
		{
			/*foreach (var key in lands.Keys)
				if (!chessPieceCount.ContainsKey(key))
				{
					int c = 0;
					foreach (var land in lands[key]) c += land.positions.Count;
					chessPieceCount[key] = c;
				}
			return chessPieceCount[playerID] > chessPieceCount[1 - playerID];*/

			return false;
		}
	}
}