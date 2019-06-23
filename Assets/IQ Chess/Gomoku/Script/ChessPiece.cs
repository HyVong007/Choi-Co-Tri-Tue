using UnityEngine;


namespace IQChess.Gomoku
{
	public sealed class ChessPiece : ChessPieceBase<Player.IDType>
	{
		public override Player.IDType playerID => throw new System.NotImplementedException();



		public override string SaveToJson()
		{
			throw new System.NotImplementedException();
		}


		public override byte[] SaveToStream()
		{
			throw new System.NotImplementedException();
		}


		protected override ChessPieceBase<Player.IDType> Initialize(byte[] stream)
		{
			throw new System.NotImplementedException();
		}


		protected override ChessPieceBase<Player.IDType> Initialize(string json)
		{
			throw new System.NotImplementedException();
		}
	}
}