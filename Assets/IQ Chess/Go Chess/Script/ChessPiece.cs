using System.Collections.Generic;
using UnityEngine;


namespace IQChess.GoChess
{
	public sealed class ChessPiece : ChessPieceBase<Player.IDType>
	{
		public sealed class Land
		{
			public int airHole;
			public readonly List<Vector3Int> positions = new List<Vector3Int>();
		}
		public Land land;

		private static readonly Dictionary<Player.IDType, ChessPiece> _prefab = new Dictionary<Player.IDType, ChessPiece>()
		{
			[Player.IDType.BLACK] = null,
			[Player.IDType.WHITE] = null
		};
		private static ChessPiece prefab(Player.IDType playerID) => _prefab[playerID] ? _prefab[playerID] : _prefab[playerID] = Resources.Load<ChessPiece>($"{typeof(ChessPiece)}.{playerID}");
		private static readonly IReadOnlyDictionary<Player.IDType, List<ChessPiece>> usedPool = new Dictionary<Player.IDType, List<ChessPiece>>()
		{
			[Player.IDType.BLACK] = new List<ChessPiece>(),
			[Player.IDType.WHITE] = new List<ChessPiece>()
		};
		private static readonly IReadOnlyDictionary<Player.IDType, Stack<ChessPiece>> freePool = new Dictionary<Player.IDType, Stack<ChessPiece>>()
		{
			[Player.IDType.BLACK] = new Stack<ChessPiece>(),
			[Player.IDType.WHITE] = new Stack<ChessPiece>()
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
			land = null;
			gameObject.SetActive(false);
			usedPool[playerID].Remove(this);
			freePool[playerID].Push(this);
		}
	}
}