using UnityEngine;


namespace IQChess.GoChess
{
	public sealed class GameManager : MonoBehaviour
	{
		private void Start()
		{
			GlobalInformations.initializedTypes.Add(GetType());
		}
	}
}