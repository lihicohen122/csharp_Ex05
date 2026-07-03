using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ex05_UI
{
	public class FormGameSettings : Form
	{
		private const int k_MinBoardSize = 3;
		private const int k_MaxBoardSize = 9;
		private const int k_DefaultBoardSize = 3;
		private const string k_ComputerDisplayName = "[Computer]";
		private const string k_ComputerPlayerName = "Computer";
		private readonly Label r_PlayersSectionLabel;
		private readonly Label r_Player1Label;
		private readonly TextBox r_Player1NameTextBox;
		private readonly CheckBox r_Player2CheckBox;
		private readonly TextBox r_Player2NameTextBox;
		private readonly Label r_BoardSizeSectionLabel;
		private readonly Label r_RowsLabel;
		private readonly NumericUpDown r_RowsNumericUpDown;
		private readonly Label r_ColumnsLabel;
		private readonly NumericUpDown r_ColumnsNumericUpDown;
		private readonly Button r_PlayButton;
		private bool m_IsUpdatingBoardSize;

		private void initializeFormLayout()
		{
			initializeFormProperties();
			initializePlayersSection();
			initializeBoardSizeSection();
			initializePlayButton();
		}

		private void initializeFormProperties()
		{
			Text = "Game Settings";
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			MinimizeBox = false;
			StartPosition = FormStartPosition.CenterScreen;
			ClientSize = new Size(412, 400);
		}

		private void initializePlayersSection()
		{
			configureAutoSizeLabel(r_PlayersSectionLabel, "Players:", new Point(24, 34));
			configureAutoSizeLabel(r_Player1Label, "Player 1:", new Point(40, 74));
			configureNameTextBox(r_Player1NameTextBox, new Point(145, 68));
			configureCheckBox(r_Player2CheckBox, "Player 2:", new Point(44, 114), player2CheckBox_CheckedChanged);
			configureNameTextBox(r_Player2NameTextBox, new Point(145, 112));
			setPlayer2TextBoxAsComputer();
			addControlsToForm(r_PlayersSectionLabel, r_Player1Label, r_Player1NameTextBox,
				r_Player2CheckBox, r_Player2NameTextBox);
		}

		private void initializeBoardSizeSection()
		{
			configureAutoSizeLabel(r_BoardSizeSectionLabel, "Board Size:", new Point(24, 192));
			configureAutoSizeLabel(r_RowsLabel, "Rows:", new Point(51, 242));
			configureBoardSizeNumericUpDown(r_RowsNumericUpDown, new Point(108, 240));
			configureAutoSizeLabel(r_ColumnsLabel, "Columns:", new Point(192, 242));
			configureBoardSizeNumericUpDown(r_ColumnsNumericUpDown, new Point(273, 240));
			addControlsToForm(r_BoardSizeSectionLabel, r_RowsLabel, r_RowsNumericUpDown,
				r_ColumnsLabel, r_ColumnsNumericUpDown);
		}

		private void initializePlayButton()
		{
			configureButton(r_PlayButton, "Play", new Point(44, 305), new Size(327, 42), playButton_Click);
			addControlsToForm(r_PlayButton);
		}

		private void configureAutoSizeLabel(Label i_Label, string i_Text, Point i_Location)
		{
			i_Label.AutoSize = true;
			i_Label.Location = i_Location;
			i_Label.Text = i_Text;
		}

		private void configureCheckBox(CheckBox i_CheckBox, string i_Text, Point i_Location, EventHandler i_CheckedChangedHandler)
		{
			i_CheckBox.AutoSize = true;
			i_CheckBox.Location = i_Location;
			i_CheckBox.Text = i_Text;
			i_CheckBox.CheckedChanged += i_CheckedChangedHandler;
		}

		private void configureButton(Button i_Button, string i_Text, Point i_Location, Size i_Size, EventHandler i_ClickHandler)
		{
			i_Button.Location = i_Location;
			i_Button.Size = i_Size;
			i_Button.Text = i_Text;
			i_Button.Click += i_ClickHandler;
		}

		private void addControlsToForm(params Control[] i_Controls)
		{
			for(int controlIndex = 0; controlIndex < i_Controls.Length; ++controlIndex)
			{
				Controls.Add(i_Controls[controlIndex]);
			}
		}

		private void configureNameTextBox(TextBox i_NameTextBox, Point i_Location)
		{
			i_NameTextBox.BorderStyle = BorderStyle.FixedSingle;
			i_NameTextBox.Location = i_Location;
			i_NameTextBox.MaxLength = 10;
			i_NameTextBox.Size = new Size(163, 26);
		}

		private void configureBoardSizeNumericUpDown(NumericUpDown i_NumericUpDown, Point i_Location)
		{
			i_NumericUpDown.Location = i_Location;
			i_NumericUpDown.Size = new Size(59, 26);
			i_NumericUpDown.ValueChanged += boardSizeNumericUpDown_ValueChanged;
		}

		private void initializeBoardSizeControls()
		{
			configureBoardSizeNumericUpDownBounds(r_RowsNumericUpDown);
			configureBoardSizeNumericUpDownBounds(r_ColumnsNumericUpDown);
		}

		private void configureBoardSizeNumericUpDownBounds(NumericUpDown i_NumericUpDown)
		{
			i_NumericUpDown.Minimum = k_MinBoardSize;
			i_NumericUpDown.Maximum = k_MaxBoardSize;
			i_NumericUpDown.Value = k_DefaultBoardSize;
		}

		private void syncBoardSize(NumericUpDown i_SourceNumericUpDown, NumericUpDown i_TargetNumericUpDown)
		{
			if(!m_IsUpdatingBoardSize)
			{
				m_IsUpdatingBoardSize = true;
				i_TargetNumericUpDown.Value = i_SourceNumericUpDown.Value;
				m_IsUpdatingBoardSize = false;
			}
		}

		private void boardSizeNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			NumericUpDown sourceNumericUpDown = (NumericUpDown)sender;
			NumericUpDown targetNumericUpDown = sourceNumericUpDown == r_RowsNumericUpDown ?
				r_ColumnsNumericUpDown : r_RowsNumericUpDown;

			syncBoardSize(sourceNumericUpDown, targetNumericUpDown);
		}

		private void setPlayer2TextBoxAsComputer()
		{
			r_Player2NameTextBox.Text = k_ComputerDisplayName;
			r_Player2NameTextBox.Enabled = false;
		}

		private void player2CheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if(r_Player2CheckBox.Checked)
			{
				r_Player2NameTextBox.Enabled = true;
				r_Player2NameTextBox.Text = string.Empty;
			}
			else
			{
				setPlayer2TextBoxAsComputer();
			}
		}

		private bool arePlayerNamesValid()
		{
			return r_Player1NameTextBox.Text != string.Empty && r_Player2NameTextBox.Text != string.Empty;
		}

		private void playButton_Click(object sender, EventArgs e)
		{
			if(!arePlayerNamesValid())
			{
				MessageBox.Show("Please provide both player's names.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			else
			{
				DialogResult = DialogResult.OK;
			}
		}

		private GameSettings buildGameSettings()
		{
			bool isVsComputer = !r_Player2CheckBox.Checked;
			int boardSize = (int)r_RowsNumericUpDown.Value;
			string player1Name = r_Player1NameTextBox.Text.Trim();
			string player2Name = isVsComputer ? k_ComputerPlayerName : r_Player2NameTextBox.Text.Trim();

			return new GameSettings(boardSize, isVsComputer, player1Name, player2Name);
		}

		public FormGameSettings()
		{
			r_PlayersSectionLabel = new Label();
			r_Player1Label = new Label();
			r_Player1NameTextBox = new TextBox();
			r_Player2CheckBox = new CheckBox();
			r_Player2NameTextBox = new TextBox();
			r_BoardSizeSectionLabel = new Label();
			r_RowsLabel = new Label();
			r_RowsNumericUpDown = new NumericUpDown();
			r_ColumnsLabel = new Label();
			r_ColumnsNumericUpDown = new NumericUpDown();
			r_PlayButton = new Button();
			initializeBoardSizeControls();
			initializeFormLayout();
		}

		public GameSettings GetInitPackage()
		{
			GameSettings initPackage = null;

			if(ShowDialog() == DialogResult.OK)
			{
				initPackage = buildGameSettings();
			}

			return initPackage;
		}
	}
}
