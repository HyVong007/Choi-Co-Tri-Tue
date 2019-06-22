using UnityEngine;


namespace IQChess.Gomoku
{
	public sealed class Board : BoardBase<Player.IDType?, Board>
	{
		public override bool IsWin(params Vector3Int[] pos)
		{
			throw new System.NotImplementedException();
		}
	}
}