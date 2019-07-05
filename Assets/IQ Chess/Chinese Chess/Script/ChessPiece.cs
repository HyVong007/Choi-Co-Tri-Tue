using UnityEngine;
using sd = RotaryHeart.Lib.SerializableDictionary;
using System;


namespace IQChess.ChineseChess
{
	public sealed class ChessPiece : ChessPieceBase<Player.IDType>
	{
		public enum Name
		{
			GENERAL, GUARD, ELEPHANT, HORSE, VEHICLE, CANNON, SOLDIER
		}
		public new Name name;
		private bool? _visible;
		public bool? visible
		{
			get => _visible;
			set => spriteRenderer.sprite = (_visible = value) == false ? hiddenSprites[playerID] : normalSprites[playerID][name];
		}

		[SerializeField] private SpriteRenderer spriteRenderer;
		[Serializable] public sealed class Name_Sprite_Dict : sd.SerializableDictionaryBase<Name, Sprite> { }
		[Serializable] public sealed class ID_Name_Sprite_Dict : sd.SerializableDictionaryBase<Player.IDType, Name_Sprite_Dict> { }
		[SerializeField] private ID_Name_Sprite_Dict normalSprites;
		[Serializable] public sealed class ID_Sprite_Dict : sd.SerializableDictionaryBase<Player.IDType, Sprite> { }
		[SerializeField] private ID_Sprite_Dict hiddenSprites;
	}
}