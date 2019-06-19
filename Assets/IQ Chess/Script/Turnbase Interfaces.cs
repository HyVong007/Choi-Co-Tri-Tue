using UnityEngine;


namespace IQChess
{
	public interface ISender
	{
		void Play(params Vector3Int[] data);

		void QuitTurn();

		void Report(Report action, params object[] data);

		void RequestDrawn();

		void Surrender();
	}



	public enum Report
	{
		READY_TO_PLAY, DONE_TURN_BEGIN, DONE_TIME_OVER, DONE_PLAYER_PLAYED, DONE_TURN_QUIT, AGREE_DRAWN, DENY_DRAWN
	}


	public enum EndGameSituation
	{
		WIN, PLAYER_TIME_OVER, SURRENDER, GAME_TIME_OVER, DRAWN
	}


	public enum TurnbaseTime
	{
		TURN_TIME, PLAYER_TIME, GAME_TIME
	}



	public interface IListener<in I, in P> where I : struct where P : PlayerBase<I, P>
	{
		void OnTurnBegin(int turn);

		void OnTurnQuit(int turn);

		void OnTimeOver(int turn, TurnbaseTime turnbaseTime);

		void OnGameEnd(int turn, EndGameSituation situation, P winner = null);

		void OnPlayed(int turn, P player, params Vector3Int[] pos);

		/// <param name="player">Người chơi đang xin hòa.</param>
		void OnDrawnRequest(int turn, P player);
	}
}