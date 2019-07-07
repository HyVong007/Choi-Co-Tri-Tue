using System;
using System.Collections.Generic;
using UnityEngine;


namespace IQChess
{
	/// <summary>Lớp con phải thông báo sẵn sàng chơi.</summary>
	/// <exception cref="TooManyInstanceException"></exception>
	public abstract class OfflineTurnManagerBase<I, P, T, B, C> : TurnManagerBase<I, P> where I : Enum where P : PlayerBase<I, P> where T : OfflineTurnManagerBase<I, P, T, B, C> where B : BoardBase<I, C, B, P> where C : ChessPieceBase<I>
	{
		#region KHỞI TẠO
		protected float startTurnTime;
		protected readonly Dictionary<I, float> startPlayerTime = new Dictionary<I, float>(), elapsedPlayerTime = new Dictionary<I, float>();



		protected new void Awake()
		{
			base.Awake();
			foreach (I id in Enum.GetValues(typeof(I))) startPlayerTime[id] = elapsedPlayerTime[id] = 0;
		}
		#endregion


		#region XỬ LÝ TURNBASE EVENTS VÀ CHỨC NĂNG CƠ BẢN
		private int _turn = -1;

		public override int turn
		{
			get => _turn;
			protected set
			{
				_turn = value;
				nextPlayer.MoveNext();
				startTurnTime = Time.time;
				startPlayerTime[player.ID] = Time.time - elapsedPlayerTime[player.ID];
				freezeTime = true;
				if (GameManager.debug)
				{
					print($"Turn= {turn}: OnTurnBegin()");
				}

				foreach (IListener<I, P> listener in PlayerBase<I, P>.playerDict.Values) listener.OnTurnBegin(turn);
			}
		}


		public override float elapsedTurnTime => Time.time - startTurnTime;

		public override float remainTurnTime => Mathf.Clamp(maxTurnTime - elapsedTurnTime, 0, maxTurnTime);

		public override bool isTurnTimeOver => remainTurnTime == 0;

		public override float ElapsePlayerTime(P player) => elapsedPlayerTime[player.ID];

		public override float RemainPlayerTime(P player) => Mathf.Clamp(maxPlayerTime - elapsedPlayerTime[player.ID], 0, maxPlayerTime);

		public override bool IsPlayerTimeOver(P player) => RemainPlayerTime(player) == 0;


		public override void Play(params Vector3Int[] data)
		{
			if (GameManager.debug)
			{
				string s = "";
				foreach (var v in data) s += $"{v}, ";
				print($"Turn= {turn}: Play({s})");
			}

			freezeTime = true;
			foreach (IListener<I, P> listener in PlayerBase<I, P>.playerDict.Values) listener.OnPlayed(turn, player, data);
		}


		public override void QuitTurn()
		{
			if (GameManager.debug)
			{
				print($"Turn= {turn}: QuitTurn()");
			}

			freezeTime = true;
			foreach (IListener<I, P> listener in PlayerBase<I, P>.playerDict.Values) listener.OnTurnQuit(turn);
		}


		public override void Surrender()
		{
			if (GameManager.debug)
			{
				print($"Turn= {turn}: Surrender()");
			}

			freezeTime = true; P winner = null;
			foreach (P player in PlayerBase<I, P>.playerDict.Values) if (player != this.player) { winner = player; break; }
			foreach (IListener<I, P> listener in PlayerBase<I, P>.playerDict.Values) listener.OnGameEnd(turn, EndGameEvent.SURRENDER, winner);
		}


		public override void Report(ReportEvent action, params object[] data)
		{
			switch (action)
			{
				case ReportEvent.DONE_TURN_BEGIN:
					if (++reportCount[ReportEvent.DONE_TURN_BEGIN] == PlayerBase<I, P>.playerDict.Count)
					{
						reportCount[ReportEvent.DONE_TURN_BEGIN] = 0;
						freezeTime = false;
					}
					break;

				case ReportEvent.DONE_TURN_QUIT:
					if (++reportCount[ReportEvent.DONE_TURN_QUIT] == PlayerBase<I, P>.playerDict.Count)
					{
						reportCount[ReportEvent.DONE_TURN_QUIT] = 0;
						freezeTime = false;
						++turn;
					}
					break;

				case ReportEvent.DONE_PLAYER_PLAYED:
					if (++reportCount[ReportEvent.DONE_PLAYER_PLAYED] == PlayerBase<I, P>.playerDict.Count)
					{
						reportCount[ReportEvent.DONE_PLAYER_PLAYED] = 0;
						if (BoardBase<I, C, B, P>.instance.hasWin) foreach (IListener<I, P> listener in PlayerBase<I, P>.playerDict.Values) listener.OnGameEnd(turn, EndGameEvent.WIN, player);
						else ++turn;
					}
					break;

				case ReportEvent.DONE_TURN_TIME_OVER:
					if (++reportCount[ReportEvent.DONE_TURN_TIME_OVER] == PlayerBase<I, P>.playerDict.Count)
					{
						reportCount[ReportEvent.DONE_TURN_TIME_OVER] = 0;
						++turn;
					}
					break;
			}
		}


		public override void Request(RequestEvent ev)
		{
			throw new NotImplementedException();
		}
		#endregion


		protected void Update()
		{
			if (!freezeTime)
			{
				elapsedPlayerTime[player.ID] = Time.time - startPlayerTime[player.ID];
				if (IsPlayerTimeOver(player))
				{
					if (GameManager.debug)
					{
						print($"Turn= {turn} IsPlayerTimeOver . player.ID= {player.ID}");
					}

					freezeTime = true;
					P winner = null;
					foreach (P player in PlayerBase<I, P>.playerDict.Values) if (player != this.player) { winner = player; break; }
					foreach (IListener<I, P> listener in PlayerBase<I, P>.playerDict.Values) listener.OnGameEnd(turn, EndGameEvent.PLAYER_TIME_OVER, winner);
				}
				else if (isTurnTimeOver)
				{
					if (GameManager.debug)
					{
						print($"Turn= {turn}: isTurnTimeOver . player.ID= {player.ID}");
					}

					freezeTime = true;
					foreach (IListener<I, P> listener in PlayerBase<I, P>.playerDict.Values) listener.OnTurnTimeOver(turn);
				}
			}
		}
	}
}