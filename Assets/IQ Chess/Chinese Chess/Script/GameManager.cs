using UnityEngine;


namespace IQChess.ChineseChess
{
	public sealed class GameManager : MonoBehaviour
	{
		private void Start()
		{
			GlobalInformations.initializedTypes.Add(GetType());
		}
	}
}