using UnityEngine;
using IQChess;
using IQChess.Gomoku;


public class Test : MonoBehaviour
{
	private void Start()
	{
		Player p = null;
		Player.Config c = new Player.Config();
		Player.Config.instance = c;
	}
}
