using UnityEngine;
using UnityEngine.EventSystems;

namespace IQChess.ChineseChess
{
	public sealed class Player : PlayerBase<Player.IDType, Player>
	{
		#region KHỞI TẠO
		public enum IDType
		{
			BLUE, RED
		}


		private new void Start()
		{
			base.Start();
			if (playerDict.Count == 2) GlobalInformations.initializedTypes.Add(GetType());
		}
		#endregion


		public override void OnTurnBegin(int turn)
		{
			canPlay = turnManager.player == this;
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


		public override void OnPlayed(int turn, Player player, params Vector3Int[] pos)
		{
			OfflineTurnManager.instance.Report(ReportEvent.DONE_PLAYER_PLAYED, pos);
		}


		public override void OnGameEnd(int turn, EndGameEvent ev, Player winner = null)
		{
			throw new System.NotImplementedException();
		}


		public override void OnRequestReceived(int turn, RequestEvent ev, Player requester)
		{
			throw new System.NotImplementedException();
		}


		public override void OnRequestDenied(int turn, RequestEvent ev)
		{
			throw new System.NotImplementedException();
		}


		public override void OnPointerClick(PointerEventData eventData)
		{
			throw new System.NotImplementedException();
		}
	}
}