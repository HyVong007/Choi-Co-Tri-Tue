using UnityEngine;


namespace IQChess.GoChess
{
	public sealed class Board : BoardBase<Player.IDType, ChessPiece, Board>
	{
		protected override void _Play(ChessPiece chessPiece, bool undo, params Vector3Int[] pos)
		{

		}


		public bool CanPlay(Player.IDType playerID, Vector3Int pos)
		{
			throw new System.NotImplementedException();
		}
	}
}