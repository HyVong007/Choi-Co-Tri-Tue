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
		public I playerID;
	}
}