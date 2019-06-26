using System;
using UnityEngine;
using System.Collections.Generic;


namespace IQChess.GoChess
{
	public sealed class Board : BoardBase<Player.IDType, ChessPiece, Board>
	{
		private readonly Vector3Int[] directions = new Vector3Int[]
		{
			Vector3Int.left, Vector3Int.right, Vector3Int.up, Vector3Int.down
		};

		private readonly IReadOnlyDictionary<Player.IDType, List<ChessPiece.Land>> lands = new Dictionary<Player.IDType, List<ChessPiece.Land>>()
		{
			[Player.IDType.BLACK] = new List<ChessPiece.Land>(),
			[Player.IDType.WHITE] = new List<ChessPiece.Land>()
		};


		//  =========================================================================


		public bool CanPlay(Player.IDType playerID, Vector3Int pos)
		{
			if (array[pos.x][pos.y]) return false;

			var info = new Dictionary<Player.IDType, Dictionary<ChessPiece.Land, int>>()
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

				if (!info[c.playerID].ContainsKey(c.land)) info[c.playerID][c.land] = 1;
				else ++info[c.playerID][c.land];
				CONTINUE_LOOP_DIRECTIONS:;
			}

			foreach (var kvp in info)
				foreach (var kvpValue in kvp.Value)
					if ((kvp.Key == playerID && kvpValue.Key.airHole > kvpValue.Value) || (kvp.Key != playerID && kvpValue.Key.airHole == kvpValue.Value)) return true;
			return false;
		}











		/// <summary>
		/// Đánh quân cờ chessPiece vào vị trí pos[0].
		/// </summary>
		protected override object _Play(ChessPiece chessPiece, bool undo, params Vector3Int[] pos)
		{
			if (!undo)
			{
				// CHƠI BÌNH THƯỜNG
				var info = new Dictionary<Player.IDType, Dictionary<ChessPiece.Land, int>>()
				{
					[Player.IDType.BLACK] = new Dictionary<ChessPiece.Land, int>(),
					[Player.IDType.WHITE] = new Dictionary<ChessPiece.Land, int>()
				};

				var deadEnemyLands = new List<ChessPiece.Land>();

				// Tìm các land và tiếp điểm của land với pos[0]
				foreach (var direction in directions)
				{
					var p = pos[0] + direction;
					if (!p.IsValidArray()) continue;
					var c = array[p.x][p.y];
					if (!c) continue;

					if (!info[c.playerID].ContainsKey(c.land)) info[c.playerID][c.land] = 1;
					else ++info[c.playerID][c.land];
				}

				// Giết land của địch hoặc giảm lỗ khí (air hole) của các land tiếp xúc.
				foreach (var kvp in info)
					foreach (var kvpValue in kvp.Value)
						if (kvp.Key != chessPiece.playerID && kvpValue.Key.airHole == kvpValue.Value)
						{
							// Giết land của địch
							var enemyLand = kvpValue.Key;
							deadEnemyLands.Add(enemyLand);
							foreach (var p in enemyLand.positions)
							{
								array[p.x][p.y].Recycle();
								array[p.x][p.y] = null;

								foreach (var direction in directions)
								{
									var _p = p + direction;
									if (!_p.IsValidArray()) continue;
									var land = array[_p.x][_p.y]?.land;
									if (land != null && land != enemyLand) ++land.airHole;
								}
							}
							// Giết xong rồi
						}
						else kvpValue.Key.airHole -= kvpValue.Value;

				// Tìm lỗ trống xung quanh pos[0]
				int newAirHole = 0;
				foreach (var direction in directions)
				{
					var p = pos[0] + direction;
					if (!p.IsValidArray()) continue;
					if (!array[p.x][p.y]) ++newAirHole;
				}

				// Tìm land lớn nhất làm chuẩn để gộp các land của mình và quân cờ đang đánh làm 1 land duy nhất.
				ChessPiece.Land maxLand = null;
				int existAirHole = 0;
				foreach (var land in info[chessPiece.playerID].Keys)
				{
					existAirHole += land.airHole;
					if (maxLand == null || land.positions.Count > maxLand.positions.Count) maxLand = land;
				}

				if (maxLand == null)
				{
					maxLand = new ChessPiece.Land();
					lands[chessPiece.playerID].Add(maxLand);
				}
				maxLand.airHole = existAirHole + newAirHole;
				maxLand.positions.Add(pos[0]);
				foreach (var land in info[chessPiece.playerID].Keys)
					if (land != maxLand)
					{
						maxLand.positions.AddRange(land.positions);
						foreach (var p in land.positions) array[p.x][p.y].land = maxLand;
					}

				var piece = ChessPiece.Get(chessPiece.playerID);
				array[pos[0].x][pos[0].y] = piece;
				piece.transform.position = pos[0].ArrayToWorld();
				piece.land = maxLand;

				return deadEnemyLands;
			}
			else
			{
				// HỦY HÀNH ĐỘNG
				throw new NotImplementedException();
			}
		}
	}
}

