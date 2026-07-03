using System;
using System.Drawing;
using System.Windows.Forms;
using Ex05_Logic;
using Ex05_Logic.Enums;

namespace Ex05_UI
{
    public class FormBoard : Form
    {
        private const int k_ButtonSize = 55;
        private const int k_ScorePanelHeight = 45;
        private const int k_FormPadding = 12;

        private readonly GameSettings r_GameSettings;
        private readonly Game r_Game;

        private PlayerLabel r_Player1ScoreLabel;
        private PlayerLabel r_Player2ScoreLabel;
        private Button[,] m_BoardButtons;
        private TableLayoutPanel m_BoardPanel;

        public FormBoard(GameSettings i_GameSettings)
        {
            r_GameSettings = i_GameSettings;
            r_Game = new Game(r_GameSettings.BoardSize, r_GameSettings.IsVsComputer);

            r_Game.TurnChanged += game_TurnChanged;
            InitializeComponent();
            prepareBoard();

            r_Game.StartNewGame();
            processTurnFlow();
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.r_Player1ScoreLabel = new Ex05_UI.PlayerLabel();
            this.r_Player1ScoreLabel.IsActiveTurn = true;
            this.r_Player2ScoreLabel = new Ex05_UI.PlayerLabel();
            this.r_Player2ScoreLabel.IsActiveTurn = false;
            this.SuspendLayout();

            // r_Player1ScoreLabel
            this.r_Player1ScoreLabel.Name = "r_Player1ScoreLabel";

            // r_Player2ScoreLabel
            this.r_Player2ScoreLabel.Name = "r_Player2ScoreLabel";

            // FormBoard
            this.Controls.Add(this.r_Player2ScoreLabel);
            this.Controls.Add(this.r_Player1ScoreLabel);
            this.FormBorderStyle = FormBorderStyle.Sizable; // 2. Resizable Form Layout
            this.MaximizeBox = false;
            this.Name = "FormBoard";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "TicTacToeMisere";

            // 3. Responsive Form Integration - Event Hook
            this.Resize += new EventHandler(this.FormBoard_Resize);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private void prepareBoard()
        {
            int boardSize = r_GameSettings.BoardSize;
            int boardPixelSize = boardSize * k_ButtonSize;
            int formWidth = boardPixelSize + (k_FormPadding * 2);
            int formHeight = boardPixelSize + k_ScorePanelHeight + (k_FormPadding * 2);

            ClientSize = new Size(formWidth, formHeight);
            MinimumSize = new Size(300, 350); // Prevent collapsing

            createBoardButtons(boardSize);
            initializePlayerLabels();
        }

        private void initializePlayerLabels()
        {
            r_Player1ScoreLabel.PlayerName = r_GameSettings.Player1Name;
            r_Player1ScoreLabel.Score = 0;
            r_Player1ScoreLabel.Anchor = AnchorStyles.Bottom;

            r_Player2ScoreLabel.PlayerName = r_GameSettings.Player2Name;
            r_Player2ScoreLabel.Score = 0;
            r_Player2ScoreLabel.Anchor = AnchorStyles.Bottom;

            positionScoreLabels();
        }

        // 1. Player Labels Alignment (Centered horizontally, side-by-side)
        private void positionScoreLabels()
        {
            const int k_Gap = 10;
            int totalWidth = r_Player1ScoreLabel.Width + k_Gap + r_Player2ScoreLabel.Width;
            int startX = (this.ClientSize.Width - totalWidth) / 2;
            int startY = this.ClientSize.Height - k_ScorePanelHeight + 10;

            r_Player1ScoreLabel.Location = new Point(startX, startY);
            r_Player2ScoreLabel.Location = new Point(startX + r_Player1ScoreLabel.Width + k_Gap, startY);
        }

        private void createBoardButtons(int i_BoardSize)
        {
            m_BoardPanel = new TableLayoutPanel();
            m_BoardPanel.RowCount = i_BoardSize;
            m_BoardPanel.ColumnCount = i_BoardSize;
            m_BoardPanel.Location = new Point(k_FormPadding, k_FormPadding);

            int panelSize = ClientSize.Width - (k_FormPadding * 2);
            m_BoardPanel.Size = new Size(panelSize, panelSize);
            m_BoardPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Define equal percentage sizing for rows and columns
            for (int i = 0; i < i_BoardSize; i++)
            {
                m_BoardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / i_BoardSize));
                m_BoardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / i_BoardSize));
            }

            m_BoardButtons = new Button[i_BoardSize, i_BoardSize];

            for (int row = 0; row < i_BoardSize; ++row)
            {
                for (int column = 0; column < i_BoardSize; ++column)
                {
                    Button boardButton = new Button();
                    boardButton.Dock = DockStyle.Fill; // Scales with TableLayoutPanel
                    boardButton.Margin = new Padding(2); // 4. Button Grid Spacing (Gap)
                    boardButton.Font = new Font("Arial", 16, FontStyle.Regular); // 5. Normal Font Weight
                    boardButton.Tag = new Point(row, column);

                    boardButton.FlatStyle = FlatStyle.Flat;
                    boardButton.BackColor = SystemColors.ControlLight;
                    boardButton.FlatAppearance.BorderColor = Color.DarkGray;

                    boardButton.Click += boardButton_Click;

                    m_BoardButtons[row, column] = boardButton;
                    m_BoardPanel.Controls.Add(boardButton, column, row);
                }
            }

            Controls.Add(m_BoardPanel);
        }

        // 3. Responsive Form Integration - Dynamic Resizing
        private void FormBoard_Resize(object sender, EventArgs e)
        {
            positionScoreLabels();

            if (m_BoardButtons != null && m_BoardButtons[0, 0] != null)
            {
                // Calculate dynamic font size based on current button height
                int buttonHeight = m_BoardButtons[0, 0].Height;
                float newFontSize = Math.Max(8f, buttonHeight / 3f);

                for (int row = 0; row < r_GameSettings.BoardSize; ++row)
                {
                    for (int column = 0; column < r_GameSettings.BoardSize; ++column)
                    {
                        m_BoardButtons[row, column].Font = new Font("Arial", newFontSize, FontStyle.Regular);
                    }
                }
            }
        }

        private void OnTurnChanged()
        {
            r_Player1ScoreLabel.Font = new Font(r_Player1ScoreLabel.Font, r_Player1ScoreLabel.IsActiveTurn ? FontStyle.Bold : FontStyle.Regular);
            r_Player2ScoreLabel.Font = new Font(r_Player2ScoreLabel.Font, r_Player2ScoreLabel.IsActiveTurn ? FontStyle.Bold : FontStyle.Regular);
        }

        private void OnScoreUpdated()
        {
            int[] playersScore = r_Game.GetAllPlayersScore();
            r_Player1ScoreLabel.Score = playersScore[0];
            r_Player2ScoreLabel.Score = playersScore[1];

            positionScoreLabels();
        }

        private void OnBoardUpdated()
        {
            for (int row = 0; row < r_Game.BoardSize; ++row)
            {
                for (int column = 0; column < r_Game.BoardSize; ++column)
                {
                    eCellSign cellSign = r_Game.GetCellSign(row, column);
                    bool isEmpty = cellSign == eCellSign.Empty;

                    m_BoardButtons[row, column].Text = getSignAsString(cellSign);
                    m_BoardButtons[row, column].Enabled = isEmpty;

                    // 5. Disabled Cell State Modernization
                    if (!isEmpty)
                    {
                        m_BoardButtons[row, column].BackColor = Color.WhiteSmoke;
                        m_BoardButtons[row, column].ForeColor = Color.DimGray;
                    }
                    else
                    {
                        m_BoardButtons[row, column].BackColor = SystemColors.ControlLight;
                        m_BoardButtons[row, column].ForeColor = Control.DefaultForeColor;
                    }
                }
            }
        }

        private string getSignAsString(eCellSign i_CellSign)
        {
            string signAsString = string.Empty;
            if (i_CellSign == eCellSign.Cross)
            {
                signAsString = "X";
            }
            else if (i_CellSign == eCellSign.Circle)
            {
                signAsString = "O";
            }

            return signAsString;
        }

        private void handleRoundEnd()
        {
            string messageBoxText = string.Empty;
            string messageBoxTitle = string.Empty;

            if (r_Game.GameState == eGameState.Tie)
            {
                messageBoxTitle = "A Tie!";
                messageBoxText = $"Tie!{Environment.NewLine}Would you like to play another round?";
            }
            else
            {
                messageBoxTitle = "A Win!";
                string winner = (r_Game.GameState == eGameState.Player1Won) ? r_GameSettings.Player1Name : r_GameSettings.Player2Name;
                messageBoxText = $"The winner is {winner}!{Environment.NewLine}Would you like to play another round?";
            }

            DialogResult userChoice = MessageBox.Show(
                messageBoxText,
                messageBoxTitle,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.None);

            if (userChoice == DialogResult.Yes)
            {
                r_Game.StartNewGame();
                processTurnFlow();
            }
            else
            {
                Close();
            }
        }

        private void processTurnFlow()
        {
            OnBoardUpdated();
            OnScoreUpdated();
            OnTurnChanged();

            if (r_Game.GameState == eGameState.Playing && r_Game.IsCurrentPlayerComputer)
            {
                r_Game.PlayComputerTurn();

                OnBoardUpdated();
                OnScoreUpdated();
                OnTurnChanged();
            }

            if (r_Game.GameState != eGameState.Playing)
            {
                handleRoundEnd();
            }
        }

        private void boardButton_Click(object sender, EventArgs e)
        {
            if (!r_Game.IsCurrentPlayerComputer)
            {
                Point cellLocation = (Point)((Button)sender).Tag;
                r_Game.PlayUserTurn(cellLocation.X, cellLocation.Y);
                processTurnFlow();
            }
        }

        private void game_TurnChanged(bool i_IsPlayer1Turn)
        {
            r_Player1ScoreLabel.IsActiveTurn = i_IsPlayer1Turn;
            r_Player2ScoreLabel.IsActiveTurn = !i_IsPlayer1Turn;
        }
    }
}