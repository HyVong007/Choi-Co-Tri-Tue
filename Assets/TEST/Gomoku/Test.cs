using UnityEngine;
using IQChess;
using IQChess.Gomoku;


public class Test : MonoBehaviour
{
	private void Awake()
	{
		Player.Config.instance = new Player.Config()
		{
			player1 = (Player.IDType.O, Player.Type.HUMAN, Player.Connection.LOCAL),
			player2 = (Player.IDType.X, Player.Type.HUMAN, Player.Connection.LOCAL)
		};

		Board.Config.instance = new Board.Config()
		{
			arraySize = new Vector2Int(10, 10)
		};

		OfflineTurnManager.Config.instance = new OfflineTurnManager.Config()
		{
			maxTurnTimeSeconds = 5,
			maxPlayerTimeSeconds = 12
		};


		int x = Board.MAX_STEP;
	}
}
