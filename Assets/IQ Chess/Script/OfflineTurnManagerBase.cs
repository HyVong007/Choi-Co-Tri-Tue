using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


namespace IQChess
{
	public abstract class OfflineTurnManagerBase<I, P, T> : MonoBehaviour, ISender where I : struct where P : PlayerBase<I, P> where T : OfflineTurnManagerBase<I, P, T>
	{
		[Serializable]
		public class Config
		{
			public static Config instance;
			public Dictionary<TurnbaseTime, float> maxTimeSeconds;
		}


		public enum TimeFlow
		{
			ELAPSE, REMAIN
		}

		public static T instance { get; private set; }

		public int turn { get; protected set; }

		public P currentPlayer { get; protected set; }

		protected IEnumerator<P> nextPlayer = NextPlayer();

		protected static IEnumerator<P> NextPlayer()
		{
			while (true) foreach (P player in PlayerBase<I, P>.playerDict.Values) yield return player;
		}

		protected readonly Dictionary<TurnbaseTime, float> maxTime = new Dictionary<TurnbaseTime, float>();

		protected readonly Dictionary<TurnbaseTime, float> startTime = new Dictionary<TurnbaseTime, float>()
		{
			[TurnbaseTime.TURN_TIME] = -1f,
			[TurnbaseTime.PLAYER_TIME] = -1f,
			[TurnbaseTime.GAME_TIME] = -1f
		};

		protected readonly Dictionary<TurnbaseTime, bool> isOverProcessed = new Dictionary<TurnbaseTime, bool>()
		{
			[TurnbaseTime.TURN_TIME] = false,
			[TurnbaseTime.PLAYER_TIME] = false,
			[TurnbaseTime.GAME_TIME] = false
		};


		//  =========================================================================


		protected void Awake()
		{
			if (instance == null) instance = this as T;
			else throw new Exception("Không thể tạo nhiều hơn 1 OfflineTurnManagerBase !");

			var c = Config.instance;
			foreach (var key in startTime.Keys) maxTime[key] = c.maxTimeSeconds[key];
		}


		protected void OnDestroy()
		{
			instance = null;
		}


		/// <summary>
		/// Trò chơi thực sự bắt đầu và người chơi có thể chơi khi bắt đầu turn đầu tiên !
		/// </summary>
		protected void BeginTurn()
		{
			throw new NotImplementedException();
		}


		public float GetTime(TimeFlow flow, TurnbaseTime type)
		{
			throw new NotImplementedException();
		}


		public bool IsTimeOver(TurnbaseTime type)
		{
			throw new NotImplementedException();
		}


		public void QuitTurn()
		{
			foreach (IListener<I, P> listener in PlayerBase<I, P>.playerDict.Values)
				listener.OnTurnQuit(turn);
		}


		public void Play(params Vector3Int[] data)
		{
			throw new NotImplementedException();
		}


		public void Report(Report action, params object[] data)
		{
			throw new NotImplementedException();
		}


		public void RequestDrawn()
		{
			throw new NotImplementedException();
		}


		public void Surrender()
		{
			throw new NotImplementedException();
		}
	}
}