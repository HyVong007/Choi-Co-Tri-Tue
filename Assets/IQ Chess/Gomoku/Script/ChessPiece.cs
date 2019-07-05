using System.Collections.Generic;
using UnityEngine;
using System;


namespace IQChess.Gomoku
{
	public sealed class ChessPiece : ChessPieceBase<Player.IDType>
	{
		private static readonly Dictionary<Player.IDType, ChessPiece> _prefab = new Dictionary<Player.IDType, ChessPiece>()
		{
			[Player.IDType.O] = null,
			[Player.IDType.X] = null
		};
		private static ChessPiece prefab(Player.IDType playerID) => _prefab[playerID] ? _prefab[playerID] : _prefab[playerID] = Resources.Load<ChessPiece>($"{typeof(ChessPiece)}.{playerID}");
		private static readonly IReadOnlyDictionary<Player.IDType, List<ChessPiece>> usedPool = new Dictionary<Player.IDType, List<ChessPiece>>()
		{
			[Player.IDType.O] = new List<ChessPiece>(),
			[Player.IDType.X] = new List<ChessPiece>()
		};
		private static readonly IReadOnlyDictionary<Player.IDType, Stack<ChessPiece>> freePool = new Dictionary<Player.IDType, Stack<ChessPiece>>()
		{
			[Player.IDType.O] = new Stack<ChessPiece>(),
			[Player.IDType.X] = new Stack<ChessPiece>()
		};



		public static ChessPiece Get(Player.IDType playerID)
		{
			var used = usedPool[playerID];
			var free = freePool[playerID];
			ChessPiece piece;
			if (free.Count != 0)
			{
				piece = free.Pop();
				used.Add(piece);
				piece.gameObject.SetActive(true);
			}
			else used.Add(piece = Instantiate(prefab(playerID)));
			return piece;
		}


		public void Recycle()
		{
			gameObject.SetActive(false);
			usedPool[playerID].Remove(this);
			freePool[playerID].Push(this);
		}


		public static void RecycleAll()
		{
			foreach (var id_list in usedPool)
				foreach (var chessPiece in id_list.Value)
				{
					chessPiece.gameObject.SetActive(false);
					freePool[id_list.Key].Push(chessPiece);
				}
			foreach (var key in usedPool.Keys) usedPool[key].Clear();
		}
	}
}