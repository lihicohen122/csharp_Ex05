namespace Ex05_UI
{
	public class GameSettings
	{
		private readonly int r_BoardSize;
		private readonly bool r_IsVsComputer;
		private readonly string r_Player1Name;
		private readonly string r_Player2Name;

		public GameSettings(int i_BoardSize, bool i_IsVsComputer, string i_Player1Name, string i_Player2Name)
		{
			r_BoardSize = i_BoardSize;
			r_IsVsComputer = i_IsVsComputer;
			r_Player1Name = i_Player1Name;
			r_Player2Name = i_Player2Name;
		}

		public int BoardSize
		{
			get
			{
				return r_BoardSize;
			}
		}

		public bool IsVsComputer
		{
			get
			{
				return r_IsVsComputer;
			}
		}

		public string Player1Name
		{
			get
			{
				return r_Player1Name;
			}
		}

		public string Player2Name
		{
			get
			{
				return r_Player2Name;
			}
		}
	}
}
