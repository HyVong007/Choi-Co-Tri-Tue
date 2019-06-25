using UnityEngine;


namespace IQChess.ChineseChess
{
	public sealed class ChessPiece : ChessPieceBase<Player.IDType>
	{
		public enum Name
		{
			GENERAL, GUARD, ELEPHANT, HORSE, VEHICLE, CANNON, SOLDIER
		}
		public new Name name;


		//  ==========================================================================


		public override string SaveToJson()
		{
			throw new System.NotImplementedException();
		}


		public override byte[] SaveToStream()
		{
			throw new System.NotImplementedException();
		}


		protected override ChessPieceBase<Player.IDType> Load(byte[] stream)
		{
			throw new System.NotImplementedException();
		}


		protected override ChessPieceBase<Player.IDType> Load(string json)
		{
			throw new System.NotImplementedException();
		}


		protected override void Load(ChessPieceBase<Player.IDType>.Config c)
		{
			throw new System.NotImplementedException();
		}
	}
}