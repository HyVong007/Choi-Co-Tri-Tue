using UnityEngine;
using UnityEngine.EventSystems;

namespace IQChess.GoChess
{
	public sealed class Player : PlayerBase<Player.IDType, Player>
	{
		#region DEBUG
		[System.Serializable]
		public new sealed class Config : PlayerBase<IDType, Player>.Config
		{
			[System.Serializable]
			public struct Data
			{
				public IDType id;
				public Type type;
				public Connection connection;
			}
			public Data _player1, _player2;


			public void Save()
			{
				player1 = (_player1.id, _player1.type, _player1.connection);
				player2 = (_player2.id, _player2.type, _player2.connection);
			}
		}
		#endregion


		#region KHỞI TẠO
		public enum IDType
		{
			BLACK, WHITE
		}


		private new void Start()
		{
			base.Start();
			if (playerDict.Count == 2) GlobalInformations.initializedTypes.Add(GetType());
		}
		#endregion


		#region LISTENER CALLBACKS
		public override void OnTurnBegin(int turn)
		{
			canPlay = turnManager.player == this;
			turnManager.Report(ReportEvent.DONE_TURN_BEGIN);
		}


		public override void OnTurnQuit(int turn)
		{
			turnManager.Report(ReportEvent.DONE_TURN_QUIT);
		}


		public override void OnTurnTimeOver(int turn)
		{
			turnManager.Report(ReportEvent.DONE_TURN_TIME_OVER);
		}


		public override void OnPlayed(int turn, Player player, params Vector3Int[] pos)
		{
			if (player == this) Board.instance.Play(ID, pos);
			turnManager.Report(ReportEvent.DONE_PLAYER_PLAYED, pos);
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
		#endregion


		public override void OnPointerClick(PointerEventData eventData)
		{
			var pos = Input.mousePosition.ScreenToArray();
			if (Board.instance.CanPlay(ID, pos))
			{
				canPlay = false;
				turnManager.Play(pos);
			}
		}
	}
}