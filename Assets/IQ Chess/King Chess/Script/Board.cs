using UnityEngine;
using sd = RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;


namespace IQChess.KingChess
{
	public sealed class Board : BoardBase<Player.IDType, ChessPiece, Board>
	{
		[Serializable] public sealed class ChessPieceList { public List<ChessPiece> list; }
		[Serializable] public sealed class Name_PieceList_Dict : sd.SerializableDictionaryBase<ChessPiece.Name, ChessPieceList> { }
		[Serializable] public sealed class ID_Name_PieceList_Dict : sd.SerializableDictionaryBase<Player.IDType, Name_PieceList_Dict> { }
		[SerializeField] private ID_Name_PieceList_Dict pieces;










		protected override object _Play(ChessPiece chessPiece, bool undo, params Vector3Int[] pos)
		{
			throw new NotImplementedException();
		}


		public Vector3Int[] FindTarget(Player.IDType playerID, Vector3Int pos)
		{
			throw new NotImplementedException();
		}
	}
}