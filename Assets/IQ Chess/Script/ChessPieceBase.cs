using System;
using UnityEngine;


namespace IQChess
{
	/// <summary>
	/// Phải đặt prefab với tên file là $"{typeof(I)}" vào folder Reource bất kì.
	/// </summary>
	/// <typeparam name="I"></typeparam>
	[RequireComponent(typeof(SpriteRenderer))]
	public abstract class ChessPieceBase<I> : MonoBehaviour where I : Enum
	{
		protected static ChessPieceBase<I> _prefab;

		protected static ChessPieceBase<I> prefab = _prefab ? _prefab : Resources.Load<ChessPieceBase<I>>($"{typeof(I)}");

		public abstract I playerID { get; }

		public abstract byte[] SaveToStream();

		public abstract string SaveToJson();

		protected abstract ChessPieceBase<I> Initialize(byte[] stream);

		protected abstract ChessPieceBase<I> Initialize(string json);


		protected static byte[] streamToInitialize;
		protected static string jsonToInitialize;

		protected void Awake()
		{
			if (streamToInitialize != null)
			{
				Initialize(streamToInitialize); streamToInitialize = null;
			}
			else if (jsonToInitialize != "")
			{
				Initialize(jsonToInitialize); jsonToInitialize = "";
			}
		}




		/// <exception cref="CannotLoadException"></exception>
		public static C Load<C>(byte[] stream) where C : ChessPieceBase<I>
		{
			streamToInitialize = stream;
			try
			{
				return Instantiate(prefab) as C;
			}
			catch (Exception) { throw new CannotLoadException(); }
		}


		/// <exception cref="CannotLoadException"></exception>
		public static C Load<C>(string json) where C : ChessPieceBase<I>
		{
			jsonToInitialize = json;
			try
			{
				return Instantiate(prefab) as C;
			}
			catch (Exception) { throw new CannotLoadException(); }
		}
	}
}