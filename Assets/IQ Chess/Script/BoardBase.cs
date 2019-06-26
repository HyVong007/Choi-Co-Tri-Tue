using UnityEngine;
using System;
using System.Collections.Generic;


namespace IQChess
{
	///<typeparam name="I">Kiểu của ID của người chơi.</typeparam>
	///<summary>Trong scene cần có sẵn 1 instance.</summary>
	/// <exception cref="TooManyInstanceException"></exception>
	[RequireComponent(typeof(SpriteRenderer))]
	public abstract class BoardBase<I, C, B> : MonoBehaviour where I : Enum where C : ChessPieceBase<I> where B : BoardBase<I, C, B>
	{
		public const int MAX_STEP = 100;

		[Serializable]
		public class Config
		{
			public static Config instance;
			public Vector2Int arraySize;
		}

		public static B instance { get; private set; }
		protected C[][] array;
		protected readonly Dictionary<I, bool> playerVictoryStates = new Dictionary<I, bool>();


		//  =========================================================================


		protected void Awake()
		{
			if (instance == null) instance = this as B;
			else throw new TooManyInstanceException("Không thể tạo nhiều hơn 1 BoardBase !");

			var c = Config.instance;
			array = new C[c.arraySize.x][];
			for (int i = 0; i < array.Length; ++i) array[i] = new C[c.arraySize.y];
			Conversion.origin = new Vector3(-array.Length / 2f, -array[0].Length / 2f, 0);
			Conversion.arraySize = c.arraySize;
			foreach (I id in Enum.GetValues(typeof(I))) playerVictoryStates[id] = false;
		}


		protected void OnDestroy()
		{
			instance = null;
		}


		//  =====================  Chức năng chính của bàn cờ  ==============================


		public C this[int x, int y] => array[x][y];

		[Serializable]
		protected struct ActionData
		{
			public I playerID;
			public int turn;
			public C chessPiece;
			public Vector3Int[] pos;
			public object customData;
		}


		public bool IsWin(I playerID) => playerVictoryStates[playerID];

		protected abstract object _Play(C chessPiece, bool undo, params Vector3Int[] pos);
		protected readonly LinkedList<ActionData> recentActions = new LinkedList<ActionData>();
		protected readonly LinkedList<ActionData> undoneActions = new LinkedList<ActionData>();


		private int turn;

		public void Play(I playerID, C chessPiece, params Vector3Int[] pos)
		{
			recentActions.AddLast(new ActionData()
			{
				playerID = playerID,
				turn = turn++,
				chessPiece = chessPiece,
				pos = pos,
				customData = _Play(chessPiece, undo: false, pos)
			});

			if (recentActions.Count > MAX_STEP)
			{
				if (recentActions.First.Value.turn == undoneActions.Last?.Value.turn) undoneActions.RemoveLast();
				recentActions.RemoveFirst();
			}
		}


		public bool CanUndo(I playerID)
		{
			var node = recentActions.Last;
			while (node != null)
			{
				if (node.Value.playerID.CompareTo(playerID) == 0) return true;
				node = node.Previous;
			}
			return false;
		}


		public bool CanRedo(I playerID)
		{
			foreach (var data in undoneActions) if (data.playerID.CompareTo(playerID) == 0) return true;
			return false;
		}


		public void Undo(I playerID)
		{
			I id;
			LinkedListNode<ActionData> node;
			do
			{
				--turn;
				node = recentActions.Last;
				recentActions.RemoveLast();
				var value = node.Value;
				id = value.playerID;
				_Play(value.chessPiece, undo: true, value.pos);
				undoneActions.AddFirst(node);
				if (undoneActions.Count > MAX_STEP) undoneActions.RemoveLast();
			} while (id.CompareTo(playerID) != 0);
		}


		public void Redo(I playerID)
		{
			I id;
			do
			{
				var nodeUndo = undoneActions.First;
				undoneActions.RemoveFirst();
				var v = nodeUndo.Value;
				_Play(v.chessPiece, undo: false, v.pos);
				id = v.playerID;
				int order = nodeUndo.Value.turn - 1;
				while (recentActions.Count != 0)
				{
					if (recentActions.Last.Value.turn == order) break;
					var value = recentActions.Last.Value;
					_Play(value.chessPiece, undo: true, value.pos);
					recentActions.RemoveLast();
				}

				recentActions.AddLast(nodeUndo);
			} while (id.CompareTo(playerID) != 0);
		}


		public string SaveToJson()
		{
			throw new NotImplementedException();
		}


		public byte[] SaveToStream()
		{
			throw new NotImplementedException();
		}


		/// <exception cref="CannotLoadException"></exception>
		public static BoardBase<I, C, B> Load(string json)
		{
			throw new NotImplementedException();
		}


		/// <exception cref="CannotLoadException"></exception>
		public static BoardBase<I, C, B> Load(byte[] stream)
		{
			throw new NotImplementedException();
		}
	}



	public static class Conversion
	{
		internal static Vector3 origin;
		internal static Vector2Int arraySize;


		public static Vector3 ArrayToWorld(this Vector3Int a) { throw new NotImplementedException(); }

		public static Vector3Int WorldToArray(this Vector3 w) => Vector3Int.FloorToInt(w - origin);

		public static Vector3Int ScreenToArray(this Vector3Int s) => s.ScreenToWorld().WorldToArray();

		public static Vector3Int ArrayToScreen(this Vector3Int a) => a.ArrayToWorld().WorldToScreen();

		public static Vector3 ScreenToWorld(this Vector3Int s) => Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(s));

		public static Vector3Int WorldToScreen(this Vector3 w) => Vector3Int.FloorToInt(Camera.main.WorldToScreenPoint(w));

		public static bool IsValidArray(this Vector3Int a) => 0 <= a.x && a.x < arraySize.x && 0 <= a.y && a.y < arraySize.y;
	}
}