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

		/// <summary>
		/// Chưa biết Offline hay Online !
		/// </summary>
		public static MonoBehaviour turnManager;


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
			while (true)
			{
				foreach (var pattern in types)
				{
					foreach (var current in initializedTypes) if (current == pattern || current.IsSubclassOf(pattern)) goto CONTINUE_LOOP_PATTERN;
					goto CONTINUE_WHILE_TRUE;
				CONTINUE_LOOP_PATTERN:;
				}
				break;
			CONTINUE_WHILE_TRUE: await Task.Delay(1);
			}
			callback();
		}
	}
}