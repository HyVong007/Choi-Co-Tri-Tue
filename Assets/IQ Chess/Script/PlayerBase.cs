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

		public abstract void OnTurnTimeOver(int turn);

		public abstract void OnTurnQuit(int turn, P player);

		public abstract void OnGameEnd(int turn, P winner);

		public abstract void OnPlayed(int turn, P player, IReadOnlyDictionary<string, object> data);
	}
}