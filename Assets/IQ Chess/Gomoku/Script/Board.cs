using UnityEngine;


namespace IQChess.Gomoku
{
	public sealed class Board : BoardBase<Player.IDType, ChessPiece, Board>
	{
		protected override void _Play(ChessPiece chessPiece, bool undo, params Vector3Int[] pos)
		{
			var p = pos[0];
			if (!undo)
			{
				array[p.x][p.y] = chessPiece;

				// Kiểm tra win ?

				//playerVictoryStates[chessPiece.playerID] = true;
			}
			else
			{
				array[p.x][p.y] = null;
			}
		}
	}
}