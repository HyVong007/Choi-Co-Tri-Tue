using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;


namespace IQChess
{
	/// <summary>
	/// Phải Reset trước khi load Scene tạo ván chơi. 
	/// </summary>
	public static class GlobalInformations
	{
		public static readonly List<Type> initializedTypes = new List<Type>();
		public static readonly List<object> allConfigs = new List<object>();



		public static void Reset()
		{
			initializedTypes.Clear(); allConfigs.Clear();
		}


		/// <summary>
		/// Kiểm tra tất cả type (hoặc sub type) đã khởi tạo xong và sẵn sàng chơi.
		/// </summary>
		/// <param name="callback"></param>
		/// <param name="types"></param>
		public static async void WaitForTypesInitialized(Action callback, params Type[] types)
		{
			var unchecks = new List<Type>(types);
			var tmp = new List<Type>();
			while (true)
			{
				foreach (var pattern in unchecks)
				{
					foreach (var current in initializedTypes) if (current == pattern || current.IsSubclassOf(pattern))
						{
							tmp.Add(pattern);
							goto CHECK_OTHER_PATTERN;
						}

					foreach (var t in tmp) unchecks.Remove(t);
					tmp.Clear();
					await Task.Delay(1); goto CONTINUE;
				CHECK_OTHER_PATTERN:;
				}
				break;
			CONTINUE:;
			}
			callback();
		}
	}
}