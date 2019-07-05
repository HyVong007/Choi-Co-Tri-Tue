using UnityEngine;
using System;


namespace IQChess.Util
{
	/// <typeparam name="I">Kiểu của ID của người chơi.</typeparam>
	/// <typeparam name="B"></typeparam>
	[RequireComponent(typeof(SpriteRenderer))]
	public abstract class BoardDesignerBase<I, B> : MonoBehaviour, IStorable where I : Enum where B : BoardDesignerBase<I, B>
	{
		protected I[][] array;

		[SerializeField] private SpriteRenderer _spriteRenderer;
		protected SpriteRenderer spriteRenderer => _spriteRenderer;
		public static B instance { get; private set; }


		protected void Awake()
		{
			if (instance == null) instance = this as B;
			else throw new TooManyInstanceException("Không thể tạo nhiều hơn 1 Designer !");
		}



		public abstract void Load(string json);

		public abstract void Load(byte[] stream);

		public abstract string SaveToJson();

		public abstract byte[] SaveToStream();
	}
}