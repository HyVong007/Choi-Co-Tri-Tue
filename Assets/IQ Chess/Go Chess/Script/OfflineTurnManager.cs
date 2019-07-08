using UnityEngine;


namespace IQChess.GoChess
{
	/// <summary>
	/// Nên chạy sau cùng nhất, khi tất cả game object đã sẵn sàng để chơi.
	/// </summary>
	public sealed class OfflineTurnManager : OfflineTurnManagerBase<Player.IDType, Player, OfflineTurnManager, Board, ChessPiece>
	{
		#region DEBUG
		[System.Serializable]
		public new sealed class Config : OfflineTurnManagerBase<Player.IDType, Player, OfflineTurnManager, Board, ChessPiece>.Config { }
		#endregion


		private void Start()
		{
			GlobalInformations.initializedTypes.Add(GetType());
		}
	}
}