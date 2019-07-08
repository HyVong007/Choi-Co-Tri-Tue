using System;
using System.Collections.Generic;
using UnityEngine;


namespace IQChess
{
	/// <summary>Lớp con phải thông báo sẵn sàng chơi.</summary>
	public abstract class TurnManagerBase<I, P> : MonoBehaviour, ISender where I : Enum where P : PlayerBase<I, P>
	{
		#region KHỞI TẠO
		[Serializable]
		public class Config
		{
			public static Config instance;

			/// <summary>
			/// maxPlayerTimeSeconds phải lớn hơn maxTurnTimeSeconds.
			/// </summary>
			public float maxTurnTimeSeconds, maxPlayerTimeSeconds;
		}

		public static TurnManagerBase<I, P> instance { get; private set; }
		protected bool freezeTime = true;
		protected readonly Dictionary<ReportEvent, int> reportCount = new Dictionary<ReportEvent, int>();
		protected float maxTurnTime, maxPlayerTime;



		protected void Awake()
		{
			if (!instance) instance = this;
			else throw new TooManyInstanceException("Không thể tạo quá 1 Turn Manager !");

			foreach (ReportEvent ev in Enum.GetValues(typeof(ReportEvent))) reportCount[ev] = 0;
			var c = Config.instance;
			maxTurnTime = c.maxTurnTimeSeconds; maxPlayerTime = c.maxPlayerTimeSeconds;
		}


		protected void OnDestroy()
		{
			instance = null;
		}
		#endregion


		#region [ABSTRACT/ VIRTUAL] CHỨC NĂNG CHÍNH VÀ SỰ KIỆN TURNBASE
		/// <summary>
		/// Phải gọi: nextPlayer.MoveNext()
		/// </summary>
		public abstract int turn { get; protected set; }

		public void BeginFirstTurn() => turn = 0;

		protected readonly IEnumerator<P> nextPlayer = NextPlayer();

		private static IEnumerator<P> NextPlayer()
		{
			while (true) foreach (P player in PlayerBase<I, P>.playerDict.Values) yield return player;
		}

		public P player => nextPlayer.Current;

		public abstract float elapsedTurnTime { get; }

		public abstract float remainTurnTime { get; }

		public abstract bool isTurnTimeOver { get; }

		public abstract float ElapsePlayerTime(P player);

		public abstract float RemainPlayerTime(P player);

		public abstract bool IsPlayerTimeOver(P player);

		public abstract void Play(params Vector3Int[] data);

		public abstract void QuitTurn();

		public abstract void Report(ReportEvent action, params object[] data);

		public abstract void Request(RequestEvent ev);

		public abstract void Surrender();
		#endregion
	}



	[Serializable]
	public struct TurnManagerHelper
	{
		[SerializeField] private MonoBehaviour offlinePrefab, onlinePrefab;

		public MonoBehaviour Instantiate(bool isOnline) => UnityEngine.Object.Instantiate(isOnline ? onlinePrefab : offlinePrefab);
	}
}