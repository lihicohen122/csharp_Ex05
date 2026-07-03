using System.Drawing;
using System.Windows.Forms;

namespace Ex05_UI
{
    public class PlayerLabel : Label
    {
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
            Font = new Font("Microsoft Sans Serif", 10);
        }

        public string PlayerName
        {
            get { return m_PlayerName; }
            set
            {
                m_PlayerName = value;
                updateDisplayText();
            }
        }

        public int Score
        {
            get { return m_Score; }
            set
            {
                m_Score = value;
                updateDisplayText();
            }
        }

        public bool IsActiveTurn
        {
            get { return m_IsActiveTurn; }
            set
            {
                m_IsActiveTurn = value;
                Font = new Font(Font, m_IsActiveTurn ? FontStyle.Bold : FontStyle.Regular);
            }
        }
    }
}