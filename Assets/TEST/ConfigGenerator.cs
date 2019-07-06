using UnityEngine;
using IQChess;
using IQChess.Gomoku;


public sealed class ConfigGenerator : MonoBehaviour
{
	public IQChess.Gomoku.GameManager.Config gameConfig;
	public Board.Config boardConfig;
	public OfflineTurnManager.Config turnConfig;
	public Player.Config playerConfig;



	private void Awake()
	{
		GlobalInformations.Reset();
		var cfg = GlobalInformations.allConfigs;
		cfg.Add(Board.Config.instance = boardConfig);
		cfg.Add(OfflineTurnManager.Config.instance = turnConfig);
		cfg.Add(Player.Config.instance = playerConfig);
		playerConfig.Save();
		cfg.Add(IQChess.Gomoku.GameManager.Config.instance = gameConfig);
	}
}