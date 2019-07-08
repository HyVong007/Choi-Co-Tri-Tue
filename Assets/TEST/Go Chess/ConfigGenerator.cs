using UnityEngine;
using IQChess;
using IQChess.GoChess;


namespace Test.GoChess
{
	public sealed class ConfigGenerator : MonoBehaviour
	{
		public IQChess.GoChess.GameManager.Config gameConfig;
		public Board.Config boardConfig;
		public OfflineTurnManager.Config turnConfig;
		public Player.Config playerConfig;



		private void Awake()
		{
			GlobalInformations.Reset();
			var cfg = GlobalInformations.allConfigs;
			cfg.Add(Board.Config.instance = boardConfig);
			if (boardConfig.stream.Length == 0) boardConfig.stream = null;

			cfg.Add(OfflineTurnManager.Config.instance = turnConfig);
			cfg.Add(Player.Config.instance = playerConfig);
			playerConfig.Save();
			cfg.Add(IQChess.GoChess.GameManager.Config.instance = gameConfig);
		}
	}
}