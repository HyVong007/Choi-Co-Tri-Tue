using UnityEngine;


namespace IQChess.Gomoku
{
	public sealed class GameManager : MonoBehaviour
	{
		[System.Serializable]
		public struct Config
		{
			public static Config instance;
			public bool isOnline;
		}

		public static GameManager instance { get; private set; }
		[SerializeField] private OfflineTurnManager offlineTurnPrefab;
		[SerializeField] private Online.OnlineTurnManager onlineTurnPrefab;



		private void Awake()
		{
			if (!instance) instance = this;
			else throw new TooManyInstanceException("Không thể tạo quá 1 Game Manager !");

			var config = Config.instance;
			if (config.isOnline) Instantiate(onlineTurnPrefab); else Instantiate(offlineTurnPrefab);
		}


		private void Start()
		{
			GlobalInformations.WaitForTypesInitialized(
				OfflineTurnManager.instance.BeginTurn,
				typeof(Board), typeof(Player), typeof(TurnManagerBase<Player.IDType, Player>));
		}
	}
}