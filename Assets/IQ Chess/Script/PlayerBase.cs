using System.Collections.Generic;
using UnityEngine;
using System;


namespace IQChess
{
	///<summary>Trong scene cần có sẵn 2 instance.</summary>
	/// <typeparam name="I">Kiểu của ID của người chơi.</typeparam>
	public abstract class PlayerBase<I, P> : MonoBehaviour, IListener<I, P> where I : Enum where P : PlayerBase<I, P>
	{
		[Serializable]
		public class Config
		{
			public static Config instance;
			public (I, Type, Connection) player1, player2;
		}

		public enum Type
		{
			HUMAN, AI
		}

		public enum Connection
		{
			LOCAL, NETWORK
		}

		public I ID { get; private set; }

		public Type type { get; private set; }

		public Connection connection { get; private set; }

		public bool canPlay { get; protected set; }

		public static readonly Dictionary<I, P> playerDict = new Dictionary<I, P>();


		//  =========================================================================


		protected void Awake()
		{
			var c = Config.instance;
			switch (playerDict.Count)
			{
				case 0: (ID, type, connection) = c.player1; break;
				case 1: (ID, type, connection) = c.player2; break;
				default: throw new Exception("Trong scene đã có sẵn nhiều hơn 2 instance !");
			}

			playerDict[ID] = this as P;
		}


		protected void OnDestroy()
		{
			(playerDict as Dictionary<I, P>).Remove(ID);
		}


		public abstract void OnTurnBegin(int turn);

		public abstract void OnTurnQuit(int turn);

		public abstract void OnTurnTimeOver(int turn);

		public abstract void OnGameEnd(int turn, EndGameSituation situation, P winner = null);

		public abstract void OnPlayed(int turn, P player, params Vector3Int[] pos);

		public abstract void OnDrawnRequest(int turn, P player);
	}
}