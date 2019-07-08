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

		[SerializeField] private TurnManagerHelper helper;
		private TurnManagerBase<Player.IDType, Player> turnManager;


		private void Start()
		{
			turnManager = helper.Instantiate(Config.instance.isOnline) as TurnManagerBase<Player.IDType, Player>;
			GlobalInformations.initializedTypes.Add(GetType());
			GlobalInformations.WaitForTypesReady(turnManager.BeginFirstTurn, typeof(TurnManagerBase<Player.IDType, Player>), typeof(Board), typeof(Player));
		}
		#endregion


		[IngameDebugConsole.ConsoleMethod("u", "")]
		public static void Undo(char c) => Board.instance.Undo(c == 'o' ? Player.IDType.O : Player.IDType.X);

		[IngameDebugConsole.ConsoleMethod("r", "")]
		public static void Redo(char c) => Board.instance.Redo(c == 'o' ? Player.IDType.O : Player.IDType.X);

		[IngameDebugConsole.ConsoleMethod("cu", "")]
		public static bool CanUndo(char c) => Board.instance.CanUndo(c == 'o' ? Player.IDType.O : Player.IDType.X);

		[IngameDebugConsole.ConsoleMethod("cr", "")]
		public static bool CanRedo(char c) => Board.instance.CanRedo(c == 'o' ? Player.IDType.O : Player.IDType.X);

		[IngameDebugConsole.ConsoleMethod("s", "")]
		public static string Save() => Board.instance.SaveToJson();
	}
}