using UnityEngine;


namespace IQChess.GoChess
{
	public sealed class GameManager : MonoBehaviour
	{
		private void Start()
		{
			GlobalInformations.WaitForTypesInitialized(
				OfflineTurnManager.instance.BeginTurn,
				typeof(Board), typeof(Player), typeof(OfflineTurnManager));
		}
	}
}