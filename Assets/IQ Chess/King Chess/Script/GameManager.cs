using UnityEngine;


namespace IQChess.KingChess
{
	public sealed class GameManager : MonoBehaviour
	{
		#region KHỞI TẠO
		[System.Serializable]
		public struct Config
		{
			public static Config instance;
			public bool isOnline;
		}

		[SerializeField] private TurnManagerHelper helper;
		private TurnManagerBase<Player.IDType, Player> turnManager;


		private void Start()
		{
			turnManager = helper.Instantiate(Config.instance.isOnline) as TurnManagerBase<Player.IDType, Player>;
			GlobalInformations.initializedTypes.Add(GetType());
			GlobalInformations.WaitForTypesReady(turnManager.BeginFirstTurn, typeof(TurnManagerBase<Player.IDType, Player>), typeof(Board), typeof(Player));
		}
		#endregion
	}
}