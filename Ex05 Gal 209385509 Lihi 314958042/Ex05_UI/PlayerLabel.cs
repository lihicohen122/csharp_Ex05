using System.Drawing;
using System.Windows.Forms;

namespace Ex05_UI
{
    public class PlayerLabel : Label
    {
        private const int k_DefaultPlayerLabelFontSize = 10;
        private string m_PlayerName = string.Empty;
        private int m_Score = 0;
        private bool m_IsActiveTurn = false;

        private void updateDisplayText()
        {
            Text = $"{m_PlayerName}: {m_Score}";
        }

        public PlayerLabel()
        {
            AutoSize = true;
            Font = new Font("Microsoft Sans Serif", k_DefaultPlayerLabelFontSize);
        }

        public string PlayerName
        {
            set
            {
                m_PlayerName = value;
                updateDisplayText();
            }
        }

        public int Score
        {
            set
            {
                m_Score = value;
                updateDisplayText();
            }
        }

        public bool IsActiveTurn
        {
            set
            {
                m_IsActiveTurn = value;
                Font = new Font(Font, m_IsActiveTurn ? FontStyle.Bold : FontStyle.Regular);
            }
        }
    }
}