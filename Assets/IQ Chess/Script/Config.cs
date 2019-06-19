using System.Collections.Generic;


namespace IQChess
{
	/// <summary>
	/// Thông số khởi tạo cho các game cờ.
	/// </summary>
	public struct Config
	{
		public static Dictionary<int, object> parameters;

		public const int FIRST_PLAYER = 0,
			MAX_TURN_TIME = 1,
			MAX_PLAYER_TIME = 2,
			MAX_GAME_TIME = 3,
			MONEY = 4,
			DIFFICULTY = 5;
	}
}