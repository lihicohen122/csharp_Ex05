using System.Windows.Forms;

namespace Ex05_UI
{
	public class UIManager
	{
		public UIManager()
		{
            const bool v_UseCompatibleTextRendering = true;

            Application.SetCompatibleTextRenderingDefault(!v_UseCompatibleTextRendering);
			GameSettings gameSettings = new FormGameSettings().ShowAndGetSettings();

			if(gameSettings != null)
			{
				Application.Run(new FormBoard(gameSettings));
			}
		}
	}
}