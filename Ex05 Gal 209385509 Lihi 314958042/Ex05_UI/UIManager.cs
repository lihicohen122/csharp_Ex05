using System.Windows.Forms;

namespace Ex05_UI
{
	public class UIManager
	{
		public UIManager()
		{
			Application.SetCompatibleTextRenderingDefault(false);
			GameSettings gameSettings = new FormGameSettings().ShowAndGetSettings();

			if(gameSettings != null)
			{
				Application.Run(new FormBoard(gameSettings));
			}
		}
	}
}