using System.Drawing;
using System.Windows.Forms;

namespace Ex05_UI
{
    public class PlayerLabel : Label
    {
        private string m_PlayerName = string.Empty;
        private int m_Score = 0;
        private bool m_IsActiveTurn = false;

        public string PlayerName
        {
            get { return m_PlayerName; }
            set
            {
                m_PlayerName = value;
                UpdateDisplayText();
            }
        }

        public int Score
        {
            get { return m_Score; }
            set
            {
                m_Score = value;
                UpdateDisplayText();
            }
        }

        public bool IsActiveTurn
        {
            get { return m_IsActiveTurn; }
            set
            {
                m_IsActiveTurn = value;
                this.Font = new Font(this.Font, m_IsActiveTurn ? FontStyle.Bold : FontStyle.Regular);
            }
        }

        public PlayerLabel()
        {
            this.AutoSize = true;
            this.Font = new Font("Microsoft Sans Serif", 10);
        }

        private void UpdateDisplayText()
        {
            this.Text = $"{m_PlayerName}: {m_Score}";
        }
    }
}