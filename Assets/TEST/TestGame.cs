using UnityEngine;


public class TestGame : MonoBehaviour
{
	private void Start()
	{
		var rect = new RectInt();
		rect.min = new Vector2Int(0, 0);
		rect.max = new Vector2Int(8, 4);
		print(rect.Contains(rect.max));
	}
}