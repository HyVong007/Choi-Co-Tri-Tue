using UnityEngine;


namespace IQChess.Gomoku
{
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