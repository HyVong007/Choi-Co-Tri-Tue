using UnityEngine;
using System;


public class TestGame : MonoBehaviour
{
	private void Start()
	{
		var a = new A();
		a.x = new A.S[][] { new A.S[] { new A.S() { b = true, n = true } } };


		string json = JsonUtility.ToJson(a);
		print(json);
	}






	private class A
	{
		[Serializable]
		public struct S
		{
			public bool b;
			public bool n;
		}

		public S[][] x;
	}
}


