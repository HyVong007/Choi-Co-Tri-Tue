using UnityEngine;


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
			OfflineTurnManager.instance.Report(ReportEvent.DONE_TURN_BEGIN);
		}


		public override void OnTurnQuit(int turn)
		{
			OfflineTurnManager.instance.Report(ReportEvent.DONE_TURN_QUIT);
		}


		public override void OnTurnTimeOver(int turn)
		{
			OfflineTurnManager.instance.Report(ReportEvent.DONE_TURN_TIME_OVER);
		}


		public override void OnGameEnd(int turn, EndGameSituation situation, Player winner = null)
		{
			throw new System.NotImplementedException();
		}


		public override void OnPlayed(int turn, Player player, params Vector3Int[] pos)
		{
			OfflineTurnManager.instance.Report(ReportEvent.DONE_PLAYER_PLAYED, pos);
		}


		public override void OnDrawnRequest(int turn, Player player)
		{
			throw new System.NotImplementedException();
		}
	}
}