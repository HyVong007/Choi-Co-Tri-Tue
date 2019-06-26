using UnityEngine;
using sd = RotaryHeart.Lib.SerializableDictionary;
using System.Collections.Generic;
using System;


namespace IQChess.ChineseChess
{
	public sealed class Board : BoardBase<Player.IDType, ChessPiece, Board>
	{
		[Serializable] public sealed class ChessPieceList { public List<ChessPiece> list; }
		[Serializable] public sealed class Name_PieceList_Dict : sd.SerializableDictionaryBase<ChessPiece.Name, ChessPieceList> { }
		[Serializable] public sealed class ID_Name_PieceList_Dict : sd.SerializableDictionaryBase<Player.IDType, Name_PieceList_Dict> { }
		[SerializeField] private ID_Name_PieceList_Dict pieces;

		private readonly IReadOnlyDictionary<Player.IDType, RectInt> countries = new Dictionary<Player.IDType, RectInt>()
		{
			[Player.IDType.BLUE] = new RectInt() { },
			[Player.IDType.RED] = new RectInt() { }
		},
		castles = new Dictionary<Player.IDType, RectInt>()
		{
			[Player.IDType.BLUE] = new RectInt() { },
			[Player.IDType.RED] = new RectInt() { }
		};

		private readonly RectInt river = new RectInt() { };







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