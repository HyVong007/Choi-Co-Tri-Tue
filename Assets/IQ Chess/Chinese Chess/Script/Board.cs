using UnityEngine;
using sd = RotaryHeart.Lib.SerializableDictionary;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;


namespace IQChess.ChineseChess
{
	public sealed class Board : BoardBase<Player.IDType, ChessPiece, Board, Player>
	{
		[Serializable] public sealed class ChessPieceList { public List<ChessPiece> list; }
		[Serializable] public sealed class Name_PieceList_Dict : sd.SerializableDictionaryBase<ChessPiece.Name, ChessPieceList> { }
		[Serializable] public sealed class ID_Name_PieceList_Dict : sd.SerializableDictionaryBase<Player.IDType, Name_PieceList_Dict> { }
		[SerializeField] private ID_Name_PieceList_Dict pieces;

		private readonly IReadOnlyDictionary<Player.IDType, RectInt> countries = new Dictionary<Player.IDType, RectInt>()
		{
			[Player.IDType.BLUE] = new RectInt(0, 0, 9, 5),
			[Player.IDType.RED] = new RectInt(0, 5, 9, 5)
		},
		castles = new Dictionary<Player.IDType, RectInt>()
		{
			[Player.IDType.BLUE] = new RectInt(3, 0, 3, 3),
			[Player.IDType.RED] = new RectInt(3, 7, 3, 3)
		};


		//  ============================================================================


		protected override void _Play(ref ActionData data, bool undo = false)
		{
			var start = data.pos[0];
			var stop = data.pos[1];
			if (!undo)
			{
				// CHƠI BÌNH THƯỜNG
				var deadEnemy = (data.customData != null ? data.customData : data.customData = array[stop.x][stop.y]) as ChessPiece;
				var myPiece = array[start.x][start.y];
				array[start.x][start.y] = null;
				array[stop.x][stop.y] = myPiece;
				var myID = data.playerID;
				var enemyID = 1 - data.playerID;
				if (myPiece.visible == false) myPiece.visible = true;
				Move(myPiece.transform, stop.ArrayToWorld()).ContinueWith((Task task) =>
				{
					deadEnemy?.gameObject.SetActive(false);
					if (deadEnemy?.name == ChessPiece.Name.GENERAL)
					{
						// Thắng
						_endGame?.Invoke(Player.playerDict[myID]);
						return;
					}

					if (IsCheckmateState(enemyID, null, null))
					{
						// Kiểm tra địch có bị chiếu bí không ? Nếu chiếu bí sẽ kết thúc trò chơi.
						foreach (var enemyPieceList in pieces[enemyID].Values)
							foreach (var enemyPiece in enemyPieceList.list)
								if (enemyPiece.gameObject.activeSelf)
									foreach (var target in FindAllPossibleTarget(enemyPiece))
										if (!IsCheckmateState(enemyID, enemyPiece.transform.position.WorldToArray(), target)) goto ENEMY_ALIVE;

						_endGame?.Invoke(Player.playerDict[myID]);
						return;
					ENEMY_ALIVE:;
					}
					movingCompleted?.Invoke();
				});
			}
			else
			{
				// HỦY NƯỚC ĐÃ ĐI
				var myPiece = array[stop.x][stop.y];
				array[start.x][start.y] = myPiece;
				var rebornEnemy = data.customData as ChessPiece;
				array[stop.x][stop.y] = rebornEnemy;
				rebornEnemy?.gameObject.SetActive(true);
				Move(myPiece.transform, start.ArrayToWorld()).ContinueWith((Task task) => movingCompleted?.Invoke());
			}
		}


		public static event Action movingCompleted;
		private const float moveSpeed = 0.1f;

		private async Task Move(Transform transform, Vector3 stop)
		{
			do
			{
				transform.position = Vector3.MoveTowards(transform.position, stop, moveSpeed);
				await Task.Delay(1);
			} while (transform.position != stop);
			transform.position = stop;
		}


		public List<Vector3Int> FindTarget(Player.IDType playerID, Vector3Int pos)
		{
			var chessPiece = array[pos.x][pos.y];
			var result = new List<Vector3Int>();
			if (!chessPiece || chessPiece.playerID != playerID) return result;

			foreach (var target in FindAllPossibleTarget(chessPiece))
				if (!IsCheckmateState(playerID, pos, target)) result.Add(target);
			return result;
		}


		private readonly Vector3Int[] MAIN_DIRECTIONS = new Vector3Int[]
		{
			Vector3Int.left, Vector3Int.right, Vector3Int.up, Vector3Int.down
		};
		private readonly Vector3Int[] CROSS_DIRECTIONS = new Vector3Int[]
		{
			Vector3Int.left+Vector3Int.up, Vector3Int.right+Vector3Int.down,
			Vector3Int.left+Vector3Int.down, Vector3Int.right+Vector3Int.zero
		};
		private readonly IReadOnlyDictionary<Vector3Int, Vector3Int[]> HORSE_DIRECTIONS = new Dictionary<Vector3Int, Vector3Int[]>()
		{
			[Vector3Int.left] = new Vector3Int[] { Vector3Int.left + Vector3Int.up, Vector3Int.left + Vector3Int.down },
			[Vector3Int.right] = new Vector3Int[] { Vector3Int.right + Vector3Int.up, Vector3Int.right + Vector3Int.down },
			[Vector3Int.up] = new Vector3Int[] { Vector3Int.left + Vector3Int.up, Vector3Int.right + Vector3Int.up },
			[Vector3Int.down] = new Vector3Int[] { Vector3Int.left + Vector3Int.down, Vector3Int.right + Vector3Int.down }
		};
		private readonly IReadOnlyDictionary<Vector3Int, ChessPiece.Name> hiddenNameTables = new Dictionary<Vector3Int, ChessPiece.Name>()
		{
			[new Vector3Int(3, 0, 0)] = ChessPiece.Name.GUARD,
			[new Vector3Int(5, 0, 0)] = ChessPiece.Name.GUARD,
			[new Vector3Int(3, 9, 0)] = ChessPiece.Name.GUARD,
			[new Vector3Int(5, 9, 0)] = ChessPiece.Name.GUARD,
			[new Vector3Int(2, 0, 0)] = ChessPiece.Name.ELEPHANT,
			[new Vector3Int(6, 0, 0)] = ChessPiece.Name.ELEPHANT,
			[new Vector3Int(2, 9, 0)] = ChessPiece.Name.ELEPHANT,
			[new Vector3Int(6, 9, 0)] = ChessPiece.Name.ELEPHANT,
			[new Vector3Int(1, 0, 0)] = ChessPiece.Name.HORSE,
			[new Vector3Int(7, 0, 0)] = ChessPiece.Name.HORSE,
			[new Vector3Int(1, 9, 0)] = ChessPiece.Name.HORSE,
			[new Vector3Int(7, 9, 0)] = ChessPiece.Name.HORSE,
			[new Vector3Int(0, 0, 0)] = ChessPiece.Name.VEHICLE,
			[new Vector3Int(8, 0, 0)] = ChessPiece.Name.VEHICLE,
			[new Vector3Int(0, 9, 0)] = ChessPiece.Name.VEHICLE,
			[new Vector3Int(8, 9, 0)] = ChessPiece.Name.VEHICLE,
			[new Vector3Int(1, 2, 0)] = ChessPiece.Name.CANNON,
			[new Vector3Int(7, 2, 0)] = ChessPiece.Name.CANNON,
			[new Vector3Int(1, 7, 0)] = ChessPiece.Name.CANNON,
			[new Vector3Int(7, 7, 0)] = ChessPiece.Name.CANNON,
			[new Vector3Int(0, 3, 0)] = ChessPiece.Name.SOLDIER,
			[new Vector3Int(2, 3, 0)] = ChessPiece.Name.SOLDIER,
			[new Vector3Int(4, 3, 0)] = ChessPiece.Name.SOLDIER,
			[new Vector3Int(6, 3, 0)] = ChessPiece.Name.SOLDIER,
			[new Vector3Int(8, 3, 0)] = ChessPiece.Name.SOLDIER,
			[new Vector3Int(0, 6, 0)] = ChessPiece.Name.SOLDIER,
			[new Vector3Int(2, 6, 0)] = ChessPiece.Name.SOLDIER,
			[new Vector3Int(4, 6, 0)] = ChessPiece.Name.SOLDIER,
			[new Vector3Int(6, 6, 0)] = ChessPiece.Name.SOLDIER,
			[new Vector3Int(8, 6, 0)] = ChessPiece.Name.SOLDIER,
		};



		private List<Vector3Int> FindAllPossibleTarget(ChessPiece chessPiece)
		{
			var result = new List<Vector3Int>();
			var center = chessPiece.transform.position.WorldToArray();

			bool normalRule = chessPiece.visible == null;
			switch (chessPiece.visible == false ? hiddenNameTables[center] : chessPiece.name)
			{
				case ChessPiece.Name.GENERAL:
				{
					var castle = castles[chessPiece.playerID];
					foreach (var direction in MAIN_DIRECTIONS)
					{
						var pos = center + direction;
						if (!castle.Contains(new Vector2Int(pos.x, pos.y))) continue;
						if (array[pos.x][pos.y]?.playerID != chessPiece.playerID) result.Add(pos);
					}
				}
				break;

				case ChessPiece.Name.GUARD:
				{
					var castle = castles[chessPiece.playerID];
					foreach (var direction in CROSS_DIRECTIONS)
					{
						var pos = center + direction;
						if ((normalRule && !castle.Contains(new Vector2Int(pos.x, pos.y))) || (!normalRule && !pos.IsValidArray())) continue;
						if (array[pos.x][pos.y]?.playerID != chessPiece.playerID) result.Add(pos);
					}
				}
				break;

				case ChessPiece.Name.ELEPHANT:
				{
					var country = countries[chessPiece.playerID];
					foreach (var direction in CROSS_DIRECTIONS)
					{
						var pos = center + direction;
						if ((normalRule && !country.Contains(new Vector2Int(pos.x, pos.y))) || (!normalRule && !pos.IsValidArray()) || array[pos.x][pos.y]) continue;
						pos += direction;
						if ((normalRule && !country.Contains(new Vector2Int(pos.x, pos.y))) || (!normalRule && !pos.IsValidArray()) || array[pos.x][pos.y]?.playerID == chessPiece.playerID) continue;
						result.Add(pos);
					}
				}
				break;

				case ChessPiece.Name.HORSE:
				{
					foreach (var direction in MAIN_DIRECTIONS)
					{
						var pos = center + direction;
						if (!pos.IsValidArray() || array[pos.x][pos.y]) continue;
						foreach (var hDir in HORSE_DIRECTIONS[direction])
						{
							var hPos = pos + hDir;
							if (!hPos.IsValidArray() || array[pos.x][pos.y]?.playerID == chessPiece.playerID) continue;
							result.Add(hPos);
						}
					}
				}
				break;

				case ChessPiece.Name.VEHICLE:
				{
					foreach (var direction in MAIN_DIRECTIONS)
						for (var pos = center + direction; pos.IsValidArray(); pos += direction)
						{
							var c = array[pos.x][pos.y];
							if (!c || c.playerID != chessPiece.playerID) result.Add(pos);
							if (c) break;
						}
				}
				break;

				case ChessPiece.Name.CANNON:
				{
					foreach (var direction in MAIN_DIRECTIONS)
					{
						bool active = false;
						for (var pos = center + direction; pos.IsValidArray(); pos += direction)
						{
							var c = array[pos.x][pos.y];
							if (c)
								if (active)
								{
									if (c.playerID != chessPiece.playerID) result.Add(pos);
									break;
								}
								else active = true;
							else if (!active) result.Add(pos);
						}
					}
				}
				break;

				case ChessPiece.Name.SOLDIER:
				{
					var forwardDir = chessPiece.playerID == Player.IDType.BLUE ? Vector3Int.up : Vector3Int.down;
					if (countries[chessPiece.playerID].Contains(new Vector2Int(center.x, center.y)))
					{
						var pos = center + forwardDir;
						if (array[pos.x][pos.y]?.playerID == chessPiece.playerID) break;
						result.Add(pos);
					}
					else foreach (var direction in MAIN_DIRECTIONS)
							if (direction != forwardDir * -1)
							{
								var pos = center + direction;
								if (!pos.IsValidArray() || array[pos.x][pos.y]?.playerID == chessPiece.playerID) continue;
								result.Add(pos);
							}
				}
				break;
			}
			return result;
		}


		///<summary>
		///Chú ý: nếu con Tướng của playerID bị giết chết thì không phải là trạng thái Chiếu.
		///<para>Nếu con Tướng của playerID bị giết thì kết thúc trò chơi ngay.</para>
		/// </summary>
		private bool IsCheckmateState(Player.IDType playerID, Vector3Int? startMove, Vector3Int? stopMove)
		{
			var movedChessPiece = startMove != null ? array[startMove.Value.x][startMove.Value.y] : null;
			var generalPos = pieces[playerID][ChessPiece.Name.GENERAL].list[0].transform.position.WorldToArray();
			ChessPiece FindChessPiece(Vector3Int pos) => pos == startMove ? null : pos == stopMove ? movedChessPiece : array[pos.x][pos.y];

			// Kiểm tra lộ mặt Tướng
			{
				var enemyGeneralPos = pieces[(1 - playerID)][ChessPiece.Name.GENERAL].list[0].transform.position.WorldToArray();
				if (generalPos.x == enemyGeneralPos.x)
				{
					var direction = playerID == Player.IDType.BLUE ? Vector3Int.up : Vector3Int.down;
					for (var pos = generalPos + direction; pos.y != enemyGeneralPos.y; pos += direction)
						if (FindChessPiece(pos)) goto NO_OPPOSITE_GENERALS;
					return true;
				NO_OPPOSITE_GENERALS:;
				}
			}

			// Kiểm tra Ngựa
			foreach (var direction in MAIN_DIRECTIONS)
			{
				var pos = generalPos + direction;
				if (!pos.IsValidArray() || FindChessPiece(pos)) continue;
				foreach (var hDir in HORSE_DIRECTIONS[direction])
				{
					var hPos = pos + hDir;
					if (!hPos.IsValidArray()) continue;
					var c = FindChessPiece(hPos);
					if (c?.playerID != playerID && c?.name == ChessPiece.Name.HORSE) return true;
				}
			}

			// Kiểm tra Xe và Pháo
			foreach (var direction in MAIN_DIRECTIONS)
			{
				bool active = false;
				for (var pos = generalPos + direction; pos.IsValidArray(); pos += direction)
				{
					var c = FindChessPiece(pos);
					if (!c) continue;
					if (active)
					{
						if (c.name == ChessPiece.Name.CANNON && c.playerID != playerID) return true;
						break;
					}

					if (c.name == ChessPiece.Name.VEHICLE && c.playerID != playerID) return true;
					active = true;
				}
			}

			// Kiểm tra Tốt
			{
				var underDir = playerID == Player.IDType.BLUE ? Vector3Int.down : Vector3Int.up;
				foreach (var direction in MAIN_DIRECTIONS)
					if (direction != underDir)
					{
						var pos = generalPos + direction;
						var c = FindChessPiece(pos);
						if (c?.name == ChessPiece.Name.SOLDIER && c?.playerID != playerID) return true;
					}
			}
			return false;
		}


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
	}
}