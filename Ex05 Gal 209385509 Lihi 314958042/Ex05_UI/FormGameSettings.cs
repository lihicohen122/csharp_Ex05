using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ex05_UI
{
    public class FormGameSettings : Form
    {
        private const int k_MinBoardSize = 4;
        private const int k_MaxBoardSize = 10;
        private const int k_DefaultBoardSize = 5;
        private const string k_ComputerDisplayName = "[Computer]";
        private const string k_ComputerPlayerName = "Computer";
        private Label m_PlayersSectionLabel;
        private Label m_Player1Label;
        private TextBox m_Player1NameTextBox;
        private CheckBox m_Player2CheckBox;
        private TextBox m_Player2NameTextBox;
        private Label m_BoardSizeSectionLabel;
        private Label m_RowsLabel;
        private NumericUpDown m_RowsNumericUpDown;
        private Label m_ColumnsLabel;
        private NumericUpDown m_ColumnsNumericUpDown;
        private Button m_PlayButton;
        private bool m_IsUpdatingBoardSize;

        public FormGameSettings()
        {
            InitializeComponent();
            initializeBoardSizeControls();
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.m_PlayersSectionLabel = new System.Windows.Forms.Label();
            this.m_Player1Label = new System.Windows.Forms.Label();
            this.m_Player1NameTextBox = new System.Windows.Forms.TextBox();
            this.m_Player2CheckBox = new System.Windows.Forms.CheckBox();
            this.m_Player2NameTextBox = new System.Windows.Forms.TextBox();
            this.m_BoardSizeSectionLabel = new System.Windows.Forms.Label();
            this.m_RowsLabel = new System.Windows.Forms.Label();
            this.m_RowsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.m_ColumnsLabel = new System.Windows.Forms.Label();
            this.m_ColumnsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.m_PlayButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.m_RowsNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_ColumnsNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // m_PlayersSectionLabel
            // 
            this.m_PlayersSectionLabel.AutoSize = true;
            this.m_PlayersSectionLabel.Location = new System.Drawing.Point(12, 12);
            this.m_PlayersSectionLabel.Name = "m_PlayersSectionLabel";
            this.m_PlayersSectionLabel.Size = new System.Drawing.Size(44, 13);
            this.m_PlayersSectionLabel.TabIndex = 10;
            this.m_PlayersSectionLabel.Text = "Players:";
            // 
            // m_Player1Label
            // 
            this.m_Player1Label.AutoSize = true;
            this.m_Player1Label.Location = new System.Drawing.Point(24, 38);
            this.m_Player1Label.Name = "m_Player1Label";
            this.m_Player1Label.Size = new System.Drawing.Size(48, 13);
            this.m_Player1Label.TabIndex = 9;
            this.m_Player1Label.Text = "Player 1:";
            // 
            // m_Player1NameTextBox
            // 
            this.m_Player1NameTextBox.Location = new System.Drawing.Point(98, 35);
            this.m_Player1NameTextBox.MaxLength = 20;
            this.m_Player1NameTextBox.Name = "m_Player1NameTextBox";
            this.m_Player1NameTextBox.Size = new System.Drawing.Size(105, 20);
            this.m_Player1NameTextBox.TabIndex = 8;
            // 
            // m_Player2CheckBox
            // 
            this.m_Player2CheckBox.AutoSize = true;
            this.m_Player2CheckBox.Location = new System.Drawing.Point(24, 66);
            this.m_Player2CheckBox.Name = "m_Player2CheckBox";
            this.m_Player2CheckBox.Size = new System.Drawing.Size(67, 17);
            this.m_Player2CheckBox.TabIndex = 7;
            this.m_Player2CheckBox.Text = "Player 2:";
            this.m_Player2CheckBox.CheckedChanged += new System.EventHandler(this.player2CheckBox_CheckedChanged);
            // 
            // m_Player2NameTextBox
            // 
            this.m_Player2NameTextBox.Enabled = false;
            this.m_Player2NameTextBox.Location = new System.Drawing.Point(98, 65);
            this.m_Player2NameTextBox.MaxLength = 20;
            this.m_Player2NameTextBox.Name = "m_Player2NameTextBox";
            this.m_Player2NameTextBox.Size = new System.Drawing.Size(105, 20);
            this.m_Player2NameTextBox.TabIndex = 6;
            this.m_Player2NameTextBox.Text = "[Computer]";
            // 
            // m_BoardSizeSectionLabel
            // 
            this.m_BoardSizeSectionLabel.AutoSize = true;
            this.m_BoardSizeSectionLabel.Location = new System.Drawing.Point(12, 110);
            this.m_BoardSizeSectionLabel.Name = "m_BoardSizeSectionLabel";
            this.m_BoardSizeSectionLabel.Size = new System.Drawing.Size(61, 13);
            this.m_BoardSizeSectionLabel.TabIndex = 5;
            this.m_BoardSizeSectionLabel.Text = "Board Size:";
            // 
            // m_RowsLabel
            // 
            this.m_RowsLabel.AutoSize = true;
            this.m_RowsLabel.Location = new System.Drawing.Point(24, 137);
            this.m_RowsLabel.Name = "m_RowsLabel";
            this.m_RowsLabel.Size = new System.Drawing.Size(37, 13);
            this.m_RowsLabel.TabIndex = 4;
            this.m_RowsLabel.Text = "Rows:";
            // 
            // m_RowsNumericUpDown
            // 
            this.m_RowsNumericUpDown.Location = new System.Drawing.Point(65, 135);
            this.m_RowsNumericUpDown.Name = "m_RowsNumericUpDown";
            this.m_RowsNumericUpDown.Size = new System.Drawing.Size(40, 20);
            this.m_RowsNumericUpDown.TabIndex = 3;
            this.m_RowsNumericUpDown.ValueChanged += new System.EventHandler(this.boardSizeNumericUpDown_ValueChanged);
            // 
            // m_ColumnsLabel
            // 
            this.m_ColumnsLabel.AutoSize = true;
            this.m_ColumnsLabel.Location = new System.Drawing.Point(118, 137);
            this.m_ColumnsLabel.Name = "m_ColumnsLabel";
            this.m_ColumnsLabel.Size = new System.Drawing.Size(30, 13);
            this.m_ColumnsLabel.TabIndex = 2;
            this.m_ColumnsLabel.Text = "Cols:";
            // 
            // m_ColumnsNumericUpDown
            // 
            this.m_ColumnsNumericUpDown.Location = new System.Drawing.Point(151, 135);
            this.m_ColumnsNumericUpDown.Name = "m_ColumnsNumericUpDown";
            this.m_ColumnsNumericUpDown.Size = new System.Drawing.Size(50, 20);
            this.m_ColumnsNumericUpDown.TabIndex = 1;
            this.m_ColumnsNumericUpDown.ValueChanged += new System.EventHandler(this.boardSizeNumericUpDown_ValueChanged);
            // 
            // m_PlayButton
            // 
            this.m_PlayButton.Location = new System.Drawing.Point(24, 175);
            this.m_PlayButton.Name = "m_PlayButton";
            this.m_PlayButton.Size = new System.Drawing.Size(179, 25);
            this.m_PlayButton.TabIndex = 0;
            this.m_PlayButton.Text = "Start!";
            this.m_PlayButton.UseVisualStyleBackColor = true;
            this.m_PlayButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // FormGameSettings
            // 
            this.ClientSize = new System.Drawing.Size(225, 210);
            this.Controls.Add(this.m_PlayButton);
            this.Controls.Add(this.m_ColumnsNumericUpDown);
            this.Controls.Add(this.m_ColumnsLabel);
            this.Controls.Add(this.m_RowsNumericUpDown);
            this.Controls.Add(this.m_RowsLabel);
            this.Controls.Add(this.m_BoardSizeSectionLabel);
            this.Controls.Add(this.m_Player2NameTextBox);
            this.Controls.Add(this.m_Player2CheckBox);
            this.Controls.Add(this.m_Player1NameTextBox);
            this.Controls.Add(this.m_Player1Label);
            this.Controls.Add(this.m_PlayersSectionLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGameSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game Settings";
            ((System.ComponentModel.ISupportInitialize)(this.m_RowsNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_ColumnsNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private void initializeBoardSizeControls()
        {
            configureBoardSizeNumericUpDownBounds(m_RowsNumericUpDown);
            configureBoardSizeNumericUpDownBounds(m_ColumnsNumericUpDown);
        }

        private void configureBoardSizeNumericUpDownBounds(NumericUpDown i_NumericUpDown)
        {
            i_NumericUpDown.Minimum = k_MinBoardSize;
            i_NumericUpDown.Maximum = k_MaxBoardSize;
            i_NumericUpDown.Value = k_DefaultBoardSize;
        }

        private void syncBoardSize(NumericUpDown i_SourceNumericUpDown, NumericUpDown i_TargetNumericUpDown)
        {
            if (!m_IsUpdatingBoardSize)
            {
                m_IsUpdatingBoardSize = true;
                i_TargetNumericUpDown.Value = i_SourceNumericUpDown.Value;
                m_IsUpdatingBoardSize = false;
            }
        }

        private void boardSizeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown sourceNumericUpDown = (NumericUpDown)sender;
            NumericUpDown targetNumericUpDown = sourceNumericUpDown == m_RowsNumericUpDown ?
                m_ColumnsNumericUpDown : m_RowsNumericUpDown;

            syncBoardSize(sourceNumericUpDown, targetNumericUpDown);
        }

        private void setPlayer2TextBoxAsComputer()
        {
            m_Player2NameTextBox.Text = k_ComputerDisplayName;
            m_Player2NameTextBox.Enabled = false;
        }

        private void player2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (m_Player2CheckBox.Checked)
            {
                m_Player2NameTextBox.Enabled = true;
                m_Player2NameTextBox.Text = string.Empty;
            }
            else
            {
                setPlayer2TextBoxAsComputer();
            }
        }

        private bool arePlayerNamesValid()
        {
            return !string.IsNullOrWhiteSpace(m_Player1NameTextBox.Text) && !string.IsNullOrWhiteSpace(m_Player2NameTextBox.Text);
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (!arePlayerNamesValid())
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
            bool isVsComputer = !m_Player2CheckBox.Checked;
            int boardSize = (int)m_RowsNumericUpDown.Value;
            string player1Name = m_Player1NameTextBox.Text.Trim();
            string player2Name = isVsComputer ? k_ComputerPlayerName : m_Player2NameTextBox.Text.Trim();

            return new GameSettings(boardSize, isVsComputer, player1Name, player2Name);
        }

        public GameSettings GetInitPackage()
        {
            GameSettings initPackage = null;

            if (ShowDialog() == DialogResult.OK)
            {
                initPackage = buildGameSettings();
            }

            return initPackage;
        }
    }
}