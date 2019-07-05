using UnityEngine;


namespace IQChess.KingChess
{
	public sealed class GameManager : MonoBehaviour
	{
		public struct Config
		{
			public static Config instance;
		}



		private void Start()
		{
			GlobalInformations.WaitForTypesInitialized(
				OfflineTurnManager.instance.BeginTurn,
				typeof(Board), typeof(Player), typeof(OfflineTurnManager));
		}
	}
}