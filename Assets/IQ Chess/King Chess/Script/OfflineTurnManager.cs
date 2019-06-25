using UnityEngine;


namespace IQChess.KingChess
{
	/// <summary>
	/// Nên chạy sau cùng nhất, khi tất cả game object đã sẵn sàng để chơi.
	/// </summary>
	public sealed class OfflineTurnManager : OfflineTurnManagerBase<Player.IDType, Player, OfflineTurnManager, Board, ChessPiece>
	{
		// Điểm bắt đầu chơi (tất cả đối tượng đã sẵn sàng).
		private void Start()
		{
			//BeginTurn();
		}
	}
}