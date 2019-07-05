using UnityEngine;


namespace IQChess.KingChess
{
	public sealed class ChessPiece : ChessPieceBase<Player.IDType>
	{
		public enum Name
		{
			KING, QUEEN, VEHICLE, ELEPHANT, HORSE, SOLDIER
		}
	}
}