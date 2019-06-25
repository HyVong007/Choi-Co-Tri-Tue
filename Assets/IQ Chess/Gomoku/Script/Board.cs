using UnityEngine;


namespace IQChess.Gomoku
{
	public sealed class Board : BoardBase<Player.IDType, ChessPiece, Board>
	{
		private readonly Vector3Int[][] vectors = new Vector3Int[][]
		{
			new Vector3Int[]{Vector3Int.left, Vector3Int.right},
			new Vector3Int[]{Vector3Int.up, Vector3Int.down},
			new Vector3Int[]{Vector3Int.left + Vector3Int.up, Vector3Int.right + Vector3Int.down},
			new Vector3Int[]{Vector3Int.left + Vector3Int.down, Vector3Int.right + Vector3Int.up}
		};


		protected override void _Play(ChessPiece chessPiece, bool undo, params Vector3Int[] pos)
		{
			var p = pos[0];
			if (!undo)
			{
				array[p.x][p.y] = chessPiece;
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
			else array[p.x][p.y] = null;
		}
	}
}