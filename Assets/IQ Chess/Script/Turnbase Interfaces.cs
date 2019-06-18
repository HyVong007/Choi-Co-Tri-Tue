using System.Collections.Generic;


namespace IQChess
{
	public interface ISender
	{
		void Play(IReadOnlyDictionary<string, object> data);

		void QuitTurn();

		void Report(TurnbaseReport action, IReadOnlyDictionary<string, object> data = null);
	}



	public enum TurnbaseReport
	{
		READY_TO_PLAY, DONE_TURN_BEGIN, DONE_TIME_OVER, DONE_PLAYER_PLAYED
	}



	public interface IListener<I, P> where I : struct where P : PlayerBase<I, P>
	{
		void OnTurnBegin(int turn);

		/// <param name="player">Người chơi đã hủy turn.</param>
		void OnTurnQuit(int turn, P player);

		void OnTurnTimeOver(int turn);

		/// <summary>
		/// Xảy ra khi có người chơi chiến thắng hoặc có người thoát game hoặc tất cả đồng ý hòa.
		/// <para>Hoặc đối thủ bị hết thời gian (Game Time Over). </para>
		/// </summary>
		/// <param name="turn"></param>
		/// <param name="winner">Nếu # null: người chơi đã chiến thắng (có thể do đối thủ thoát). Nếu == null: không ai thắng (hòa).</param>
		void OnGameEnd(int turn, P winner);

		void OnPlayed(int turn, P player, IReadOnlyDictionary<string, object> data);
	}
}