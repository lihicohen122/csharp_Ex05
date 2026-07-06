using System;
using System.Drawing;
using System.Windows.Forms;
using Ex05_Logic;
using Ex05_Logic.Enums;

namespace Ex05_UI
{
    public class FormBoard : Form
    {
        private const float k_MinimumButtonFontSize = 8f;
        private const float k_ButtonHeightToFontRatio = 3.5f;
        private const int k_ButtonSize = 55;
        private const int k_ScorePanelHeight = 45;
        private const int k_FormPadding = 12;
        private const int k_MinWindowWidth = 300;
        private const int k_MinWindowHeight = 350;
        private readonly GameSettings r_GameSettings;
        private readonly Game r_Game;
        private PlayerLabel m_Player1ScoreLabel;
        private PlayerLabel m_Player2ScoreLabel;
        private Button[,] m_BoardButtons;
        private TableLayoutPanel m_BoardPanel;
        private bool m_IsPlayer1Turn;

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.m_Player1ScoreLabel = new Ex05_UI.PlayerLabel();
            this.m_Player2ScoreLabel = new Ex05_UI.PlayerLabel();
            this.SuspendLayout();

            // r_Player1ScoreLabel
            this.m_Player1ScoreLabel.Name = "m_Player1ScoreLabel";

            // r_Player2ScoreLabel
            this.m_Player2ScoreLabel.Name = "m_Player2ScoreLabel";

            // FormBoard
            this.Controls.Add(this.m_Player2ScoreLabel);
            this.Controls.Add(this.m_Player1ScoreLabel);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = false;
            this.Name = "FormBoard";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "TicTacToeMisere";

            this.Resize += new EventHandler(this.formBoard_Resize);

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
            MinimumSize = new Size(k_MinWindowWidth, k_MinWindowHeight);
            createBoardButtons(boardSize);
            initializePlayerLabels();
        }

        private void initializePlayerLabels()
        {
            m_Player1ScoreLabel.PlayerName = r_GameSettings.Player1Name;
            m_Player1ScoreLabel.Score = 0;
            m_Player1ScoreLabel.Anchor = AnchorStyles.Bottom;
            m_Player2ScoreLabel.PlayerName = r_GameSettings.Player2Name;
            m_Player2ScoreLabel.Score = 0;
            m_Player2ScoreLabel.Anchor = AnchorStyles.Bottom;
            positionScoreLabels();
        }

        private void positionScoreLabels()
        {
            const int k_Gap = 10;
            int totalWidth = m_Player1ScoreLabel.Width + k_Gap + m_Player2ScoreLabel.Width;
            int startX = (ClientSize.Width - totalWidth) / 2;
            int startY = ClientSize.Height - k_ScorePanelHeight + 10;

            m_Player1ScoreLabel.Location = new Point(startX, startY);
            m_Player2ScoreLabel.Location = new Point(startX + m_Player1ScoreLabel.Width + k_Gap, startY);
        }

        private void createBoardButtons(int i_BoardSize)
        {
            initializeBoardPanel(i_BoardSize);
            m_BoardButtons = new Button[i_BoardSize, i_BoardSize];
            for(int row = 0; row < i_BoardSize; ++row)
            {
                for(int column = 0; column < i_BoardSize; ++column)
                {
                    Button boardButton = createNewButton(row, column);

                    m_BoardPanel.Controls.Add(boardButton, column, row);
                }
            }

            Controls.Add(m_BoardPanel);
        }

        private void initializeBoardPanel(int i_BoardSize)
        {
            int panelSize = ClientSize.Width - (k_FormPadding * 2);

            m_BoardPanel = new TableLayoutPanel();
            m_BoardPanel.RowCount = i_BoardSize;
            m_BoardPanel.ColumnCount = i_BoardSize;
            m_BoardPanel.Location = new Point(k_FormPadding, k_FormPadding);
            m_BoardPanel.Size = new Size(panelSize, panelSize);
            m_BoardPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            for(int i = 0; i < i_BoardSize; ++i)
            {
                m_BoardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / i_BoardSize));
                m_BoardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / i_BoardSize));
            }
        }

        private Button createNewButton(int i_Row, int i_Column)
        {
            Button boardButton = new Button();

            boardButton.Dock = DockStyle.Fill;
            boardButton.Margin = new Padding(2);
            boardButton.Font = new Font("Microsoft Sans Serif", 11);
            boardButton.Tag = new Point(i_Row, i_Column);
            boardButton.Click += boardButton_Click;
            m_BoardButtons[i_Row, i_Column] = boardButton;

            return boardButton;
        }

        private void formBoard_Resize(object sender, EventArgs e)
        {
            positionScoreLabels();
            if(m_BoardButtons != null && m_BoardButtons[0, 0] != null)
            {
                int buttonHeight = m_BoardButtons[0, 0].Height;
                float newFontSize = Math.Max(k_MinimumButtonFontSize, buttonHeight / k_ButtonHeightToFontRatio);

                for(int row = 0; row < r_GameSettings.BoardSize; ++row)
                {
                    for(int column = 0; column < r_GameSettings.BoardSize; ++column)
                    {
                        m_BoardButtons[row, column].Font = new Font("Microsoft Sans Serif", newFontSize);
                    }
                }
            }
        }

        private void updateTurnDisplay()
        {
            m_Player1ScoreLabel.IsActiveTurn = m_IsPlayer1Turn;
            m_Player2ScoreLabel.IsActiveTurn = !m_IsPlayer1Turn;
        }

        private void updateScoreDisplay()
        {
            int[] playersScore = r_Game.GetAllPlayersScore();

            m_Player1ScoreLabel.Score = playersScore[0];
            m_Player2ScoreLabel.Score = playersScore[1];
            positionScoreLabels();
        }

        private void updateBoardDisplay()
        {
            for(int row = 0; row < r_Game.BoardSize; ++row)
            {
                for(int column = 0; column < r_Game.BoardSize; ++column)
                {
                    eCellSign cellSign = r_Game.GetCellSign(row, column);
                    bool isEmpty = cellSign == eCellSign.Empty;

                    m_BoardButtons[row, column].Text = getSignAsString(cellSign);
                    m_BoardButtons[row, column].Enabled = isEmpty;
                }
            }
        }

        private string getSignAsString(eCellSign i_CellSign)
        {
            string signAsString = string.Empty;

            if(i_CellSign == eCellSign.Cross)
            {
                signAsString = "X";
            }
            else if(i_CellSign == eCellSign.Circle)
            {
                signAsString = "O";
            }

            return signAsString;
        }

        private void handleRoundEnd()
        {
            string messageBoxText = string.Empty;
            string messageBoxTitle = string.Empty; 

            if(r_Game.GameState == eGameState.Tie)
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

            DialogResult userChoice = MessageBox.Show(messageBoxText, messageBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.None);

            if(userChoice == DialogResult.Yes)
            {
                startNewRound();
            }
            else
            {
                Close();
            }
        }

        private void processTurnFlow()
        {
            refreshDisplayState();
            if(r_Game.GameState == eGameState.Playing && r_Game.IsCurrentPlayerComputer)
            {
                r_Game.PlayComputerTurn();
                refreshDisplayState();
            }

            if(r_Game.GameState != eGameState.Playing)
            {
                handleRoundEnd();
            }
        }

        private void boardButton_Click(object sender, EventArgs e)
        {
            if(!r_Game.IsCurrentPlayerComputer)
            {
                Button clickedButton = sender as Button;

                if(clickedButton != null)
                {
                    Point cellLocation = (Point)clickedButton.Tag;

                    r_Game.PlayUserTurn(cellLocation.X, cellLocation.Y);
                    processTurnFlow();
                }
            }
        }

        private void startNewRound()
        {
            r_Game.StartNewGame();
            processTurnFlow();
        }

        private void refreshDisplayState()
        {
            updateBoardDisplay();
            updateScoreDisplay();
            updateTurnDisplay();
        }

        private void game_TurnChanged(bool i_IsPlayer1Turn)
        {
            m_IsPlayer1Turn = i_IsPlayer1Turn;
        }

        public FormBoard(GameSettings i_GameSettings)
        {
            r_GameSettings = i_GameSettings;
            r_Game = new Game(r_GameSettings.BoardSize, r_GameSettings.IsVsComputer);
            r_Game.TurnChanged += game_TurnChanged;
            InitializeComponent();
            prepareBoard();
            startNewRound();
        }
    }
}