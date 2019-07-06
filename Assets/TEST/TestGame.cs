using UnityEngine;
using UnityEngine.EventSystems;
using System;
using IQChess;
using IQChess.Gomoku;


public class TestGame : MonoBehaviour
{
	private void Start()
	{
		var @base = typeof(TurnManagerBase<Player.IDType, Player>);
		var online = typeof(IQChess.Gomoku.Online.OnlineTurnManager);
		var offline = typeof(OfflineTurnManager);

		print($"online.  IsSubclassOf(@base)= {online.IsSubclassOf(@base)}");
		print($"offline.  IsSubclassOf(@base)= {offline.IsSubclassOf(@base)}");
	}
}


