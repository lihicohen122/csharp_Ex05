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
		private const string k_TieMessage = "Tie!";
		private const string k_AnotherRoundQuestion = "Do you want another round?";
		private const string k_MessageBoxTitle = "TicTacToeReverse";
		private readonly GameSettings r_GameSettings;
		private readonly Game r_Game;
		private readonly Label r_Player1ScoreLabel;
		private readonly Label r_Player2ScoreLabel;
		private Button[,] m_BoardButtons;

		private void prepareBoard()
		{
			int boardSize = r_GameSettings.BoardSize;
			int boardPixelSize = boardSize * k_ButtonSize;
			int formWidth = boardPixelSize + (k_FormPadding * 2);
			int formHeight = boardPixelSize + k_ScorePanelHeight + (k_FormPadding * 2);

			Text = k_MessageBoxTitle;
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			StartPosition = FormStartPosition.CenterScreen;
			ClientSize = new Size(formWidth, formHeight);

			createBoardButtons(boardSize);
			setPlayerLabels(boardPixelSize);
		}

		private void setPlayerLabels(int i_BoardPixelSize)
		{
			int scoreLabelsTop = k_FormPadding + i_BoardPixelSize + 8;

			r_Player1ScoreLabel.AutoSize = true;
			r_Player1ScoreLabel.Font = new Font(r_Player1ScoreLabel.Font.FontFamily, 11, FontStyle.Bold);

			r_Player2ScoreLabel.AutoSize = true;
			r_Player2ScoreLabel.Font = new Font(r_Player2ScoreLabel.Font.FontFamily, 11, FontStyle.Bold);

			Controls.Add(r_Player1ScoreLabel);
			Controls.Add(r_Player2ScoreLabel);

			updateScoreLabel(r_Player1ScoreLabel, r_GameSettings.Player1Name, 0);
			updateScoreLabel(r_Player2ScoreLabel, r_GameSettings.Player2Name, 0);
			positionScoreLabels(scoreLabelsTop);
		}

		private void positionScoreLabels(int i_ScoreLabelsTop)
		{
			r_Player1ScoreLabel.Location = new Point(k_FormPadding, i_ScoreLabelsTop);
			r_Player2ScoreLabel.Location = new Point(
				ClientSize.Width - r_Player2ScoreLabel.Width - k_FormPadding, i_ScoreLabelsTop);
		}

		private void createBoardButtons(int i_BoardSize)
		{
			int boardTop = k_FormPadding;

			m_BoardButtons = new Button[i_BoardSize, i_BoardSize];
			for(int row = 0; row < i_BoardSize; ++row)
			{
				for(int column = 0; column < i_BoardSize; ++column)
				{
					Button boardButton = new Button();

					boardButton.Size = new Size(k_ButtonSize, k_ButtonSize);
					boardButton.Location = new Point(k_FormPadding + (column * k_ButtonSize), boardTop + (row * k_ButtonSize));
					boardButton.Font = new Font("Arial", 16, FontStyle.Bold);
					boardButton.Tag = new Point(row, column);
					boardButton.Click += boardButton_Click;

					m_BoardButtons[row, column] = boardButton;
					Controls.Add(boardButton);
				}
			}
		}

		private void updateScoreLabel(Label i_ScoreLabel, string i_PlayerName, int i_PlayerScore)
		{
			i_ScoreLabel.Text = $"{i_PlayerName}: {i_PlayerScore}";
		}

		private void updateScoreLabels()
		{
			int[] playersScore = r_Game.GetAllPlayersScore();
			int scoreLabelsTop = k_FormPadding + (r_Game.BoardSize * k_ButtonSize) + 8;

			updateScoreLabel(r_Player1ScoreLabel, r_GameSettings.Player1Name, playersScore[0]);
			updateScoreLabel(r_Player2ScoreLabel, r_GameSettings.Player2Name, playersScore[1]);
			positionScoreLabels(scoreLabelsTop);
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

		private void updateBoardButton(Button i_BoardButton, int i_Row, int i_Column)
		{
			eCellSign cellSign = r_Game.GetCellSign(i_Row, i_Column);

			i_BoardButton.Text = getSignAsString(cellSign);
			i_BoardButton.Enabled = cellSign == eCellSign.Empty;
		}

		private void updateBoardDisplay()
		{
			int boardSize = r_Game.BoardSize;

			for(int row = 0; row < boardSize; ++row)
			{
				for(int column = 0; column < boardSize; ++column)
				{
					updateBoardButton(m_BoardButtons[row, column], row, column);
				}
			}

			updateScoreLabels();
		}

		private string buildRoundEndMessage()
		{
			string roundEndMessage = string.Empty;

			if(r_Game.GameState == eGameState.Tie)
			{
				roundEndMessage = k_TieMessage;
			}
			else if(r_Game.GameState == eGameState.Player1Won)
			{
				roundEndMessage = $"{r_GameSettings.Player1Name} Won!";
			}
			else if(r_Game.GameState == eGameState.Player2Won)
			{
				roundEndMessage = $"{r_GameSettings.Player2Name} Won!";
			}

			return roundEndMessage;
		}

		private void handleRoundEnd()
		{
			string messageBoxText = $"{buildRoundEndMessage()}{Environment.NewLine}{k_AnotherRoundQuestion}";
			DialogResult userChoice = MessageBox.Show(
				messageBoxText, k_MessageBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if(userChoice == DialogResult.Yes)
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
			updateBoardDisplay();

			if(r_Game.GameState == eGameState.Playing && r_Game.IsCurrentPlayerComputer)
			{
				r_Game.PlayComputerTurn();
				updateBoardDisplay();
			}

			if(r_Game.GameState != eGameState.Playing)
			{
				handleRoundEnd();
			}
		}

		private void getCellLocationFromButton(Button i_Button, out int o_Row, out int o_Column)
		{
			Point cellLocation = (Point)i_Button.Tag;

			o_Row = cellLocation.X;
			o_Column = cellLocation.Y;
		}

		private void boardButton_Click(object sender, EventArgs e)
		{
			if(!r_Game.IsCurrentPlayerComputer)
			{
				getCellLocationFromButton((Button)sender, out int row, out int column);

				r_Game.PlayUserTurn(row, column);
				processTurnFlow();
			}
		}

		public FormBoard(GameSettings i_GameSettings)
		{
			r_GameSettings = i_GameSettings;
			r_Player1ScoreLabel = new Label();
			r_Player2ScoreLabel = new Label();
			r_Game = new Game(r_GameSettings.BoardSize, r_GameSettings.IsVsComputer);

			prepareBoard();
			r_Game.StartNewGame();
			processTurnFlow();
		}
	}
}
