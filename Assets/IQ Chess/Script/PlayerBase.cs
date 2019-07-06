using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;


namespace IQChess
{
	///<summary>Trong scene cần có sẵn 2 instance.
	///<para>Lớp con phải thông báo sẵn sàng chơi.</para></summary>
	/// <typeparam name="I">Kiểu của ID của người chơi.</typeparam>
	/// <exception cref="TooManyInstanceException"></exception>
	[RequireComponent(typeof(SpriteRenderer))]
	public abstract class PlayerBase<I, P> : MonoBehaviour, IListener<I, P>, IPointerClickHandler where I : Enum where P : PlayerBase<I, P>
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

		private bool _canPlay;
		public bool canPlay
		{
			get => _canPlay;
			set
			{
				_canPlay = value;
				collider.gameObject.SetActive(value);
			}
		}

		public static readonly Dictionary<I, P> playerDict = new Dictionary<I, P>();

		[SerializeField] private new BoxCollider2D collider;
		private static TurnManagerBase<I, P> _turnManager;
		protected static TurnManagerBase<I, P> turnManager => _turnManager ? _turnManager : _turnManager = TurnManagerBase<I, P>.instance;


		//  =========================================================================


		protected void Awake()
		{
			var c = Config.instance;
			switch (playerDict.Count)
			{
				case 0: (ID, type, connection) = c.player1; break;
				case 1: (ID, type, connection) = c.player2; break;
				default: throw new TooManyInstanceException("Trong scene đã có sẵn nhiều hơn 2 instance !");
			}
			playerDict[ID] = this as P;
		}


		protected void Start()
		{
			var size = Conversion.arraySize;
			collider.transform.localScale = new Vector3(size.x, size.y, 0);
		}


		protected void OnDestroy()
		{
			(playerDict as Dictionary<I, P>).Remove(ID);
		}


		public abstract void OnTurnBegin(int turn);

		public abstract void OnTurnQuit(int turn);

		public abstract void OnTurnTimeOver(int turn);

		public abstract void OnGameEnd(int turn, EndGameEvent ev, P winner = null);

		public abstract void OnPlayed(int turn, P player, params Vector3Int[] pos);

		public abstract void OnRequestReceived(int turn, RequestEvent ev, P requester);

		public abstract void OnRequestDenied(int turn, RequestEvent ev);

		public abstract void OnPointerClick(PointerEventData eventData);
	}
}