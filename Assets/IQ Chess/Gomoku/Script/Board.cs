using UnityEngine;


namespace IQChess.Gomoku
{
	public sealed class Board : BoardBase<Player.IDType, ChessPiece, Board>
	{
		public override void Add(ChessPiece chessPiece, Vector3Int dest)
		{
			throw new System.NotImplementedException();
		}


		public override void Move(Vector3Int target, Vector3Int dest)
		{
			throw new System.NotImplementedException();
		}


		protected override void UndoAdd(ChessPiece chessPiece, Vector3Int dest)
		{
			throw new System.NotImplementedException();
		}


		protected override void UndoMove(Vector3Int target, Vector3Int dest)
		{
			throw new System.NotImplementedException();
		}
	}
}