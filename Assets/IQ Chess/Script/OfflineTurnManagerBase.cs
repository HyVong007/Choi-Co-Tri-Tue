using System;
using System.Collections.Generic;
using UnityEngine;


namespace IQChess
{
	/// <exception cref="TooManyInstanceException"></exception>
	public abstract class OfflineTurnManagerBase<I, P, T, B, C> : MonoBehaviour, ITurnManager<I, P> where I : Enum where P : PlayerBase<I, P> where T : OfflineTurnManagerBase<I, P, T, B, C> where B : BoardBase<I, C, B, P> where C : ChessPieceBase<I>
	{
		[Serializable]
		public class Config
		{
			public static Config instance;

			/// <summary>
			/// maxPlayerTimeSeconds phải lớn hơn maxTurnTimeSeconds.
			/// </summary>
			public float maxTurnTimeSeconds, maxPlayerTimeSeconds;
		}

		public static T instance { get; private set; }
		protected int _turn = -1;
		protected IEnumerator<P> nextPlayer = NextPlayer();

		protected static IEnumerator<P> NextPlayer()
		{
			while (true) foreach (P player in PlayerBase<I, P>.playerDict.Values) yield return player;
		}
		protected float startTurnTime, maxTurnTime, maxPlayerTime;
		protected readonly Dictionary<I, float> startPlayerTime = new Dictionary<I, float>(), elapsedPlayerTime = new Dictionary<I, float>();
		protected bool freezeTime = true;
		protected readonly Dictionary<ReportEvent, int> reportCount = new Dictionary<ReportEvent, int>();

		public int turn => _turn;

		public P player => nextPlayer.Current;

		public float elapsedTurnTime => Time.time - startTurnTime;

		public float remainTurnTime => Mathf.Clamp(maxTurnTime - elapsedTurnTime, 0, maxTurnTime);

		public bool isTurnTimeOver => remainTurnTime == 0;

		public float ElapsePlayerTime(P player) => elapsedPlayerTime[player.ID];

		public float RemainPlayerTime(P player) => Mathf.Clamp(maxPlayerTime - elapsedPlayerTime[player.ID], 0, maxPlayerTime);

		public bool IsPlayerTimeOver(P player) => RemainPlayerTime(player) == 0;

		protected IReadOnlyDictionary<I, P> playerDict = PlayerBase<I, P>.playerDict;


		protected void Awake()
		{
			if (instance == null) instance = this as T;
			else throw new TooManyInstanceException("Không thể tạo quá 1 instance !");

			var c = Config.instance;
			maxTurnTime = c.maxTurnTimeSeconds; maxPlayerTime = c.maxPlayerTimeSeconds;
			foreach (I id in Enum.GetValues(typeof(I)))
			{
				startPlayerTime[id] = elapsedPlayerTime[id] = 0;
			}
			foreach (ReportEvent ev in Enum.GetValues(typeof(ReportEvent))) reportCount[ev] = 0;
		}


		protected void OnDestroy()
		{
			instance = null;
		}


		protected void BeginTurn()
		{
			++_turn;
			nextPlayer.MoveNext();
			startTurnTime = Time.time;
			startPlayerTime[player.ID] = Time.time - elapsedPlayerTime[player.ID];
			freezeTime = true;
			if (GameManager.debug)
			{
				print($"Turn= {_turn}: OnTurnBegin()");
			}

			foreach (IListener<I, P> listener in playerDict.Values) listener.OnTurnBegin(_turn);
		}


		public virtual void Play(params Vector3Int[] data)
		{
			if (GameManager.debug)
			{
				string s = "";
				foreach (var v in data) s += $"{v}, ";
				print($"Turn= {_turn}: Play({s})");
			}

			freezeTime = true;
			foreach (IListener<I, P> listener in playerDict.Values) listener.OnPlayed(_turn, player, data);
		}


		public virtual void QuitTurn()
		{
			if (GameManager.debug)
			{
				print($"Turn= {_turn}: QuitTurn()");
			}

			freezeTime = true;
			foreach (IListener<I, P> listener in playerDict.Values) listener.OnTurnQuit(_turn);
		}


		public virtual void RequestDrawn()
		{
			throw new NotImplementedException();
		}


		public virtual void Surrender()
		{
			if (GameManager.debug)
			{
				print($"Turn= {_turn}: Surrender()");
			}

			freezeTime = true; P winner = null;
			foreach (P player in playerDict.Values) if (player != this.player) { winner = player; break; }
			foreach (IListener<I, P> listener in playerDict.Values) listener.OnGameEnd(_turn, EndGameEvent.SURRENDER, winner);
		}


		public virtual void Report(ReportEvent action, params object[] data)
		{
			switch (action)
			{
				case ReportEvent.DONE_TURN_BEGIN:
					if (++reportCount[ReportEvent.DONE_TURN_BEGIN] == playerDict.Count)
					{
						reportCount[ReportEvent.DONE_TURN_BEGIN] = 0;
						freezeTime = false;
					}
					break;

				case ReportEvent.DONE_TURN_QUIT:
					if (++reportCount[ReportEvent.DONE_TURN_QUIT] == playerDict.Count)
					{
						reportCount[ReportEvent.DONE_TURN_QUIT] = 0;
						freezeTime = false;
						BeginTurn();
					}
					break;

				case ReportEvent.DONE_PLAYER_PLAYED:
					if (++reportCount[ReportEvent.DONE_PLAYER_PLAYED] == playerDict.Count)
					{
						reportCount[ReportEvent.DONE_PLAYER_PLAYED] = 0;
						var array = new Vector3Int[data.Length];
						Array.Copy(data, array, data.Length);
						//if (BoardBase<C, B>.instance.IsWin(array)) foreach (IListener<I, P> listener in playerDict.Values) listener.OnGameEnd(_turn, EndGameEvent.WIN, player);
						//else BeginTurn();
					}
					break;

				case ReportEvent.DONE_TURN_TIME_OVER:
					if (++reportCount[ReportEvent.DONE_TURN_TIME_OVER] == playerDict.Count)
					{
						reportCount[ReportEvent.DONE_TURN_TIME_OVER] = 0;
						BeginTurn();
					}
					break;
			}
		}


		protected void Update()
		{
			if (!freezeTime)
			{
				elapsedPlayerTime[player.ID] = Time.time - startPlayerTime[player.ID];
				if (IsPlayerTimeOver(player))
				{
					if (GameManager.debug)
					{
						print($"Turn= {_turn} IsPlayerTimeOver . player.ID= {player.ID}");
					}

					freezeTime = true;
					P winner = null;
					foreach (P player in playerDict.Values) if (player != this.player) { winner = player; break; }
					foreach (IListener<I, P> listener in playerDict.Values) listener.OnGameEnd(_turn, EndGameEvent.PLAYER_TIME_OVER, winner);
				}
				else if (isTurnTimeOver)
				{
					if (GameManager.debug)
					{
						print($"Turn= {_turn}: isTurnTimeOver . player.ID= {player.ID}");
					}

					freezeTime = true;
					foreach (IListener<I, P> listener in playerDict.Values) listener.OnTurnTimeOver(_turn);
				}
			}
		}

		public void Request(RequestEvent ev)
		{
			throw new NotImplementedException();
		}
	}
}