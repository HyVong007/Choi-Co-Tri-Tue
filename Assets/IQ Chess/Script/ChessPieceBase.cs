using System;
using UnityEngine;


namespace IQChess
{
	[RequireComponent(typeof(SpriteRenderer))]
	public abstract class ChessPieceBase<I> : MonoBehaviour where I : Enum
	{
		public I playerID;
	}
}