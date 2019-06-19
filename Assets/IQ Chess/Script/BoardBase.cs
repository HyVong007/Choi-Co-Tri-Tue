using UnityEngine;
using System;


namespace IQChess
{
	/// <typeparam name="C">Kiểu của quân cờ: struct? hoặc enum?</typeparam>
	public abstract class BoardBase<C, B> : MonoBehaviour where B : BoardBase<C, B>
	{
		public static B instance { get; private set; }

		public C[][] array { get; protected set; }



		protected void Awake()
		{
			if (instance == null) instance = this as B;
			else if (instance != this) throw new Exception("Không thể tạo 2 Board !");
		}


		protected void OnDestroy()
		{
			if (instance == this) instance = null;
		}


		public abstract bool IsWin(Vector3Int pos1, Vector3Int? pos2 = null);
	}
}