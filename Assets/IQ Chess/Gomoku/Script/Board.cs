﻿using UnityEngine;


namespace IQChess.Gomoku
{
	public sealed class Board : BoardBase<Player.IDType, ChessPiece, Board, Player>
	{
		[System.Serializable]
		public new sealed class Config : BoardBase<Player.IDType, ChessPiece, Board, Player>.Config
		{
			/// <summary>
			/// Nếu true: con cờ nằm giữa ô. Nếu false thì con cờ nằm trên giao điểm đường kẻ.
			/// </summary>
			public bool gridCenter = true;
		}

		private readonly Vector3Int[][] vectors = new Vector3Int[][]
		{
			new Vector3Int[]{Vector3Int.left, Vector3Int.right},
			new Vector3Int[]{Vector3Int.up, Vector3Int.down},
			new Vector3Int[]{Vector3Int.left + Vector3Int.up, Vector3Int.right + Vector3Int.down},
			new Vector3Int[]{Vector3Int.left + Vector3Int.down, Vector3Int.right + Vector3Int.up}
		};

		#region [ Khởi tạo: Vẽ bàn cờ và con cờ ]
		[SerializeField] private Transform cellPrefab;
		[SerializeField] private Transform boardCells;

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
		#endregion


		//  =========================================================================


		protected override void _Play(ref ActionData data, bool undo = false)
		{
			var p = data.pos[0];
			if (!undo)
			{
				var chessPiece = array[p.x][p.y] = ChessPiece.Get(data.playerID, p.ArrayToWorld());
				var enemyID = chessPiece.playerID == Player.IDType.O ? Player.IDType.X : Player.IDType.O;
				foreach (var axe in vectors)
				{
					int count = 1, enemy = 0;
					foreach (var direction in axe)
					{
						for (var point = p + direction; count <= 6 && point.IsValidArray(); point += direction)
						{
							var id = array[point.x][point.y]?.playerID;
							if (id == chessPiece.playerID) { ++count; continue; }
							if (id == enemyID) ++enemy;
							break;
						}
						if (count > 5) goto CONTINUE_LOOP_AXE;
					}

					if (count == 5 && enemy < 2) { playerVictoryStates[chessPiece.playerID] = true; break; }
				CONTINUE_LOOP_AXE:;
				}
			}
			else
			{
				array[p.x][p.y].Recycle();
				array[p.x][p.y] = null;
			}
		}


		private struct JsonData
		{
			[System.Serializable]
			public struct RowItem
			{
				public bool hasPlayerID;
				public Player.IDType playerID;
			}

			[System.Serializable]
			public class Row
			{
				public RowItem[] items;
			}

			public Row[] array;
		}

		public override string SaveToJson()
		{
			var data = new JsonData();
			var size = new Vector2Int(array.Length, array[0].Length);
			data.array = new JsonData.Row[size.x];
			for (int x = 0; x < size.x; ++x)
			{
				data.array[x].items = new JsonData.RowItem[size.y];
				for (int y = 0; y < size.y; ++y)
				{
					var chessPiece = array[x][y];
					data.array[x].items[y] = new JsonData.RowItem() { hasPlayerID = chessPiece, playerID = chessPiece ? chessPiece.playerID : default };
				}
			}
			return JsonUtility.ToJson(data);
		}


		public override void Load(string json)
		{
			ChessPiece.RecycleAll();
			var data = JsonUtility.FromJson<JsonData>(json);
			var size = new Vector2Int(data.array.Length, data.array[0].items.Length);
			array = new ChessPiece[size.x][];
			var index = new Vector3Int();
			for (index.x = 0; index.x < size.x; ++index.x)
			{
				array[index.x] = new ChessPiece[size.y];
				for (index.y = 0; index.y < size.y; ++index.y)
				{
					var item = data.array[index.x].items[index.y];
					if (!item.hasPlayerID) continue;

					var chessPiece = ChessPiece.Get(item.playerID);
					chessPiece.transform.position = index.ArrayToWorld();
					array[index.x][index.y] = chessPiece;
				}
			}
		}


		public override byte[] SaveToStream()
		{
			throw new System.NotImplementedException();
		}


		public override void Load(byte[] stream)
		{
			throw new System.NotImplementedException();
		}
	}
}