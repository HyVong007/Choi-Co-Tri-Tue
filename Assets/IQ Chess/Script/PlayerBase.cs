using System.Collections.Generic;
using UnityEngine;


namespace IQChess
{
	/// <typeparam name="I">Kiểu của ID của người chơi.</typeparam>
	public abstract class PlayerBase<I, P> : MonoBehaviour, IListener<I, P> where I : struct where P : PlayerBase<I, P>
	{
		public enum Type
		{
			HUMAN, AI
		}

		public enum Connection
		{
			LOCAL, NETWORK
		}

		public I ID;
		public Type type;
		public Connection connection;

		public bool canPlay { get; protected set; }

		public static IReadOnlyDictionary<I, P> playerDict { get; private set; }


		//  =========================================================================


		protected void Awake()
		{
			if (playerDict?.ContainsKey(ID) == true) throw new System.Exception($"Không thể tạo 1 player 2 lần. ID= {ID}");
			var dict = playerDict as Dictionary<I, P> ?? new Dictionary<I, P>();
			dict[ID] = this as P;
			playerDict = dict;
		}


		public abstract void OnTurnBegin(int turn);

		public abstract void OnTurnQuit(int turn);

		public abstract void OnTimeOver(int turn, TurnbaseTime turnbaseTime);

		public abstract void OnGameEnd(int turn, EndGameSituation situation, P winner = null);

		public abstract void OnPlayed(int turn, P player, params Vector3Int[] pos);

		public abstract void OnDrawnRequest(int turn, P player);
	}
}