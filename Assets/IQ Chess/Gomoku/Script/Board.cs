using UnityEngine;


namespace IQChess.Gomoku
{
	public sealed class Board : BoardBase<Player.IDType?, Board>
	{
		public override bool IsWin(Vector3Int pos1, Vector3Int? pos2 = null)
		{
			throw new System.NotImplementedException();
		}
	}
}