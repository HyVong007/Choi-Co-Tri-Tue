using System;
using UnityEngine;


namespace IQChess
{
	/// <summary>
	/// Phải đặt prefab với tên file là $"{typeof(I)}" vào folder Resource bất kì.
	/// </summary>
	/// <exception cref="CannotCreateInstanceException"></exception>
	/// <typeparam name="I"></typeparam>
	[RequireComponent(typeof(SpriteRenderer))]
	public abstract class ChessPieceBase<I> : MonoBehaviour where I : Enum
	{
		public class Config
		{
			public static Config instance;
			public I playerID;
		}

		protected static ChessPieceBase<I> _prefab;

		protected static ChessPieceBase<I> prefab = _prefab ? _prefab : Resources.Load<ChessPieceBase<I>>($"{typeof(I)}");

		public I playerID { get; private set; }

		public abstract byte[] SaveToStream();

		public abstract string SaveToJson();

		protected abstract ChessPieceBase<I> Load(byte[] stream);

		protected abstract ChessPieceBase<I> Load(string json);

		protected static byte[] streamToInitialize;
		protected static string jsonToInitialize;


		//  ======================================================================


		protected void Awake()
		{
			if (streamToInitialize != null)
			{
				Load(streamToInitialize); streamToInitialize = null;
			}
			else if (jsonToInitialize != "")
			{
				Load(jsonToInitialize); jsonToInitialize = "";
			}
			else if (Config.instance != null)
			{

			}
			else throw new CannotCreateInstanceException("Không thể tạo instance vì thiếu config hoặc json hoặc stream.");
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