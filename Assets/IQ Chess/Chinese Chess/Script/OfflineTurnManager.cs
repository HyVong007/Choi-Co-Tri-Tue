using UnityEngine;


namespace IQChess.ChineseChess
{
	/// <summary>
	/// Nên chạy sau cùng nhất, khi tất cả game object đã sẵn sàng để chơi.
	/// </summary>
	public sealed class OfflineTurnManager : OfflineTurnManagerBase<Player.IDType, Player, OfflineTurnManager, Board, ChessPiece>
	{
		private void Start()
		{
			GlobalInformations.WaitForTypesInitialized(() => ++turn, typeof(Board), typeof(Player), typeof(GameManager));
		}
	}
}