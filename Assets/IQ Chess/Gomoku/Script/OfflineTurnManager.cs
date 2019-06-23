using UnityEngine;


namespace IQChess.Gomoku
{
	public sealed class OfflineTurnManager : OfflineTurnManagerBase<Player.IDType, Player, OfflineTurnManager, Board, ChessPiece>
	{
		public bool begin;

		private new void Update()
		{
			if (begin)
			{
				begin = false;
				BeginTurn();
			}

			base.Update();
		}


		// Điểm bắt đầu chơi (tất cả đối tượng đã sẵn sàng).
		private void Start()
		{
			//BeginTurn();
		}


		public override void Play(params Vector3Int[] data)
		{
			base.Play(data);
		}


		public override void QuitTurn()
		{
			base.QuitTurn();
		}


		public override void Surrender()
		{
			base.Surrender();
		}


		public override void RequestDrawn()
		{
			base.RequestDrawn();
		}


		public override void Report(ReportEvent action, params object[] data)
		{
			base.Report(action, data);
		}
	}
}