using System.Collections.Generic;
using System;
using System.Threading.Tasks;


namespace IQChess
{
	/// <summary>
	/// Phải Reset trước khi load Scene tạo ván chơi. 
	/// </summary>
	public static class GlobalInformations
	{
		public static readonly List<Type> initializedTypes = new List<Type>();


		public static async void WaitForTypesInitialized(Action callback, params Type[] types)
		{
			while (true)
			{
				foreach (var type in types) if (!initializedTypes.Contains(type)) await Task.Delay(1);
				break;
			}
			callback();
		}
	}
}