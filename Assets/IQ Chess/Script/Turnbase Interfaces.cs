using UnityEngine;
using System;


namespace IQChess
{
	public interface ISender
	{
		void Play(params Vector3Int[] data);

		void QuitTurn();

		void Report(ReportEvent action, params object[] data);

		void RequestDrawn();

		void Surrender();
	}



	public enum ReportEvent
	{
		READY_TO_PLAY, DONE_TURN_BEGIN, DONE_TURN_TIME_OVER, DONE_PLAYER_PLAYED, DONE_TURN_QUIT, AGREE_DRAWN, DENY_DRAWN
	}


	public enum EndGameSituation
	{
		WIN, PLAYER_TIME_OVER, SURRENDER, DRAWN
	}


	public interface IListener<I, P> where I : Enum where P : PlayerBase<I, P>
	{
		void OnTurnBegin(int turn);

		void OnTurnQuit(int turn);

		void OnTurnTimeOver(int turn);

		void OnGameEnd(int turn, EndGameSituation situation, P winner = null);

		void OnPlayed(int turn, P player, params Vector3Int[] pos);

		/// <param name="player">Người chơi đang xin hòa.</param>
		void OnDrawnRequest(int turn, P player);
	}



	public interface ITurnManager<I, P> : ISender where I : Enum where P : PlayerBase<I, P>
	{
		int turn { get; }

		P player { get; }

		float elapsedTurnTime { get; }

		float remainTurnTime { get; }

		bool isTurnTimeOver { get; }

		float ElapsePlayerTime(P player);

		float RemainPlayerTime(P player);

		bool IsPlayerTimeOver(P player);
	}
}