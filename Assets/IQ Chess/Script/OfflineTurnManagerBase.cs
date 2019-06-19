using System;
using System.Collections.Generic;
using UnityEngine;


namespace IQChess
{
	/// <typeparam name="I">Kiểu của ID</typeparam>
	public abstract class OfflineTurnManagerBase<I, P, T> : MonoBehaviour, ISender where I : struct where P : PlayerBase<I, P> where T : OfflineTurnManagerBase<I, P, T>
	{
		public enum TimeFlow
		{
			ELAPSE, REMAIN
		}

		public static T instance { get; private set; }

		public int turn { get; protected set; }

		public P currentPlayer { get; protected set; }

		protected IEnumerator<P> nextPlayer;

		protected IEnumerator<P> NextPlayer()
		{
			throw new NotImplementedException();
		}

		protected Dictionary<TurnbaseTime, float> maxTime;

		protected readonly Dictionary<TurnbaseTime, float> startTime = new Dictionary<TurnbaseTime, float>()
		{
			[TurnbaseTime.TURN_TIME] = -1f,
			[TurnbaseTime.PLAYER_TIME] = -1f,
			[TurnbaseTime.GAME_TIME] = -1f
		};


		//  =========================================================================


		protected void Awake()
		{
			if (instance == null) instance = this as T;
			else if (instance != this) throw new Exception("Không thể tạo 2 OfflineTurnManager !");
		}


		protected void OnDestroy()
		{
			if (instance == this) instance = null;
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