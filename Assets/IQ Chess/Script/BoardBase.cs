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
		public const int MAX_UNDO_REDO_STEP = 100;


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
			foreach (I id in Enum.GetValues(typeof(I))) playerVictoryStates[id] = false;
		}


		protected void OnDestroy()
		{
			instance = null;
		}


		//  =====================  Chức năng chính của bàn cờ  ==============================


		[Serializable]
		protected struct DataAdd
		{
			public C chessPiece;
			public Vector3Int dest;
		}

		[Serializable]
		protected struct DataMove
		{
			public Vector3Int target, dest;
			public C deadChessPice;
		}


		public bool IsWin(I playerID) => playerVictoryStates[playerID];

		public abstract void Add(C chessPiece, Vector3Int dest);

		public abstract void Move(Vector3Int target, Vector3Int dest);

		protected abstract void UndoAdd(C chessPiece, Vector3Int dest);

		protected abstract void UndoMove(Vector3Int target, Vector3Int dest);

		protected readonly LinkedList<ValueType> recentActions = new LinkedList<ValueType>();

		/// <exception cref="CannotUndoException"></exception>
		public void Undo(I playerID)
		{

		}

		/// <exception cref="CannotRedoException"></exception>
		public void Redo(I playerID)
		{

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
}