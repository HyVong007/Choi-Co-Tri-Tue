using UnityEngine;
using System;


namespace IQChess
{
	///<summary>Trong scene cần có sẵn 1 instance.</summary>
	/// <typeparam name="C">Kiểu của quân cờ: struct? hoặc enum?</typeparam>
	public abstract class BoardBase<C, B> : MonoBehaviour where B : BoardBase<C, B>
	{
		[Serializable]
		public class Config
		{
			public static Config instance;
			public Vector2Int arraySize;
		}

		public static B instance { get; private set; }

		public C[][] array { get; protected set; }



		protected void Awake()
		{
			if (instance == null) instance = this as B;
			else throw new Exception("Không thể tạo nhiều hơn 1 BoardBase !");

			var c = Config.instance;
			array = new C[c.arraySize.x][];
			for (int i = 0; i < array.Length; ++i) array[i] = new C[c.arraySize.y];
		}


		protected void OnDestroy()
		{
			instance = null;
		}


		public abstract bool IsWin(params Vector3Int[] pos);
	}
}