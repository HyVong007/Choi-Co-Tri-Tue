using UnityEngine;
using System;
using System.Collections.Generic;


namespace IQChess
{
	///<typeparam name="I">Kiểu của ID của người chơi.</typeparam>
	///<summary>Trong scene cần có sẵn 1 instance.
	///<para>Lớp con phải thông báo sẵn sàng chơi.</para></summary>
	/// <exception cref="TooManyInstanceException"></exception>
	public abstract class BoardBase<I, C, B, P> : MonoBehaviour, IStorable where I : Enum where C : ChessPieceBase<I> where B : BoardBase<I, C, B, P> where P : PlayerBase<I, P>
	{
		public const int MAX_STEP = 100;

		[Serializable]
		public class Config
		{
			public static Config instance;
			public Vector2Int arraySize;
			public string json = "";
			public byte[] stream;
		}

		public static B instance { get; private set; }
		protected C[][] array;
		protected readonly Dictionary<I, bool> playerVictoryStates = new Dictionary<I, bool>();


		//  =========================================================================


		protected void Awake()
		{
			if (!instance) instance = this as B;
			else throw new TooManyInstanceException("Không thể tạo nhiều hơn 1 BoardBase !");

			var c = Config.instance;
			array = new C[c.arraySize.x][];
			for (int x = 0; x < array.Length; ++x) array[x] = new C[c.arraySize.y];
			Conversion.origin = new Vector3(-array.Length / 2f, -array[0].Length / 2f, 0);
			Conversion.arraySize = c.arraySize;
			foreach (I id in Enum.GetValues(typeof(I))) playerVictoryStates[id] = false;
		}


		protected void Start()
		{
			GlobalInformations.initializedTypes.Add(GetType());
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
			public int turn;
			public I playerID;
			public Vector3Int[] pos;
			public object customData;
		}


		public bool IsWin(I playerID) => playerVictoryStates[playerID];

		protected abstract void _Play(ref ActionData data, bool undo = false);
		protected readonly LinkedList<ActionData> recentActions = new LinkedList<ActionData>();
		protected readonly LinkedList<ActionData> undoneActions = new LinkedList<ActionData>();
		private int turn;


		public void Play(I playerID, params Vector3Int[] pos)
		{
			var data = new ActionData() { turn = turn++, playerID = playerID, pos = pos };
			_Play(ref data);
			recentActions.AddLast(data);
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
				_Play(ref value, undo: true);
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
				id = v.playerID;
				_Play(ref v);
				int order = nodeUndo.Value.turn - 1;
				while (recentActions.Count != 0)
				{
					if (recentActions.Last.Value.turn == order) break;
					var value = recentActions.Last.Value;
					_Play(ref value, undo: true);
					recentActions.RemoveLast();
				}

				recentActions.AddLast(nodeUndo);
			} while (id.CompareTo(playerID) != 0);
		}

		/// <summary>
		/// Sự kiện khi game kết thúc: có người thắng hay Hòa.
		/// parameter: người chơi thắng. Nếu null: Hòa.
		/// </summary>
		public static event Action<P> endGame
		{
			add { _endGame += value; }
			remove { _endGame -= value; }
		}
		protected static Action<P> _endGame;

		public abstract string SaveToJson();

		public abstract byte[] SaveToStream();
		public abstract void Load(string json);
		public abstract void Load(byte[] stream);
	}



	public interface IStorable
	{
		string SaveToJson();

		byte[] SaveToStream();

		void Load(string json);

		void Load(byte[] stream);
	}



	public static class Conversion
	{
		internal static Vector3 origin;
		internal static Vector2Int arraySize;
		public static readonly Vector3 ZERO_DOT_FIVE = new Vector3(0.5f, 0.5f, 0);


		public static bool IsValidArray(this Vector3Int a) => 0 <= a.x && a.x < arraySize.x && 0 <= a.y && a.y < arraySize.y;

		public static Vector3 ArrayToWorld(this Vector3Int array) => array + origin + ZERO_DOT_FIVE;


		public static Vector3Int WorldToArray(this Vector3 world)
		{
			var result = Vector3Int.FloorToInt(world - ZERO_DOT_FIVE - origin);
			result.z = 0;
			return result;
		}


		public static Vector3Int ScreenToArray(this Vector3 screen)
		{
			var result = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(screen) - origin);
			result.z = 0;
			return result;
		}


		public static Vector3 ScreenToWorld(this Vector3 screen)
		{
			var result = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(screen) - origin) + origin + ZERO_DOT_FIVE;
			result.z = 0;
			return result;
		}
	}
}