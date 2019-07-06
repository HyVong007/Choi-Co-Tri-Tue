using UnityEngine;


namespace IQChess.Gomoku
{
	public sealed class GameManager : MonoBehaviour
	{
		private void mStart()
		{
			GlobalInformations.WaitForTypesInitialized(
				OfflineTurnManager.instance.BeginTurn,
				typeof(Board), typeof(Player), typeof(OfflineTurnManager));
		}
	}
}