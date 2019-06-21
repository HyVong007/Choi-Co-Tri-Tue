using UnityEngine;
using System.Collections.Generic;


namespace IQChess.Gomoku
{
	public sealed class Player : PlayerBase<Player.IDType, Player>
	{
		public new sealed class Config : PlayerBase<IDType, Player>.Config
		{

		}


		public enum IDType
		{
			O, X
		}



		public override void OnTurnBegin(int turn)
		{
			throw new System.NotImplementedException();
		}


		public override void OnTurnQuit(int turn)
		{
			throw new System.NotImplementedException();
		}

		public override void OnTimeOver(int turn, TurnbaseTime turnbaseTime)
		{
			throw new System.NotImplementedException();
		}

		public override void OnGameEnd(int turn, EndGameSituation situation, Player winner = null)
		{
			throw new System.NotImplementedException();
		}

		public override void OnPlayed(int turn, Player player, params Vector3Int[] pos)
		{
			throw new System.NotImplementedException();
		}

		public override void OnDrawnRequest(int turn, Player player)
		{
			throw new System.NotImplementedException();
		}
	}
}