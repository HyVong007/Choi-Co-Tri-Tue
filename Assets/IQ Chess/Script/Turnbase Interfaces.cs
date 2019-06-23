using UnityEngine;
using System;


namespace IQChess
{
	public interface ISender
	{
		void Play(params Vector3Int[] data);

		void QuitTurn();

		void Report(ReportEvent action, params object[] data);

		void Request(RequestEvent ev);

		void Surrender();
	}


	public enum ReportEvent
	{
		READY_TO_PLAY, DONE_TURN_BEGIN, DONE_TURN_TIME_OVER, DONE_PLAYER_PLAYED, DONE_TURN_QUIT,
		YES_DRAWN, NO_DRAWN, YES_UNDO, NO_UNDO, YES_REDO, NO_REDO
	}

	public enum EndGameEvent
	{
		WIN, PLAYER_TIME_OVER, SURRENDER, DRAWN
	}

	public enum RequestEvent
	{
		DRAWN, UNDO, REDO
	}

	public interface IListener<I, P> where I : Enum where P : PlayerBase<I, P>
	{
		void OnTurnBegin(int turn);

		void OnTurnQuit(int turn);

		void OnTurnTimeOver(int turn);

		void OnGameEnd(int turn, EndGameEvent ev, P winner = null);

		void OnPlayed(int turn, P player, params Vector3Int[] pos);

		/// <summary>
		/// Called on receiver.
		/// </summary>
		/// <param name="turn"></param>
		/// <param name="ev"></param>
		/// <param name="requester"></param>
		void OnRequestReceived(int turn, RequestEvent ev, P requester);

		/// <summary>
		/// Called on requester
		/// </summary>
		/// <param name="turn"></param>
		/// <param name="ev"></param>
		/// <param name="player"></param>
		void OnRequestDenied(int turn, RequestEvent ev);
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