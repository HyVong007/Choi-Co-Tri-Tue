using UnityEngine;


namespace IQChess.Gomoku
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
		#endregion


		private void Start()
		{
			GlobalInformations.initializedTypes.Add(GetType());
		}


		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				var b = Board.instance;
				var id = TurnManagerBase<Player.IDType, Player>.instance.player.ID;
				print($"Is win [{id}]= " + b.IsWin(id));
				print("Win line= " + b.winLine[0].WorldToArray() + ", " + b.winLine[1].WorldToArray());
			}
		}
	}
}