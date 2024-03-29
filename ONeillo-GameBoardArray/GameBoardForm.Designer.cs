﻿namespace ONeillo_GameBoardArray
{
    partial class GameBoardForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameBoardForm));
            menuStrip = new MenuStrip();
            menuToolStripMenuItem = new ToolStripMenuItem();
            saveGameToolStripMenuItem = new ToolStripMenuItem();
            newGameToolStripMenuItem = new ToolStripMenuItem();
            restoreGameToolStripMenuItem = new ToolStripMenuItem();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            informationPanelToolStripMenuItem = new ToolStripMenuItem();
            gameToSpeechToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem1 = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            PlayerPanel = new Panel();
            currentPlayerInd = new Label();
            player2NameBox = new TextBox();
            player1NameBox = new TextBox();
            pictureBox2 = new PictureBox();
            pictureBox1 = new PictureBox();
            whitePieceCounter = new Label();
            blackPieceCounter = new Label();
            fileSystemWatcher1 = new FileSystemWatcher();
            menuStrip.SuspendLayout();
            PlayerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).BeginInit();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(24, 24);
            menuStrip.Items.AddRange(new ToolStripItem[] { menuToolStripMenuItem, optionsToolStripMenuItem, helpToolStripMenuItem1, exitToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(878, 33);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            menuToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveGameToolStripMenuItem, newGameToolStripMenuItem, restoreGameToolStripMenuItem });
            menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            menuToolStripMenuItem.Size = new Size(74, 29);
            menuToolStripMenuItem.Text = "Game";
            // 
            // saveGameToolStripMenuItem
            // 
            saveGameToolStripMenuItem.Name = "saveGameToolStripMenuItem";
            saveGameToolStripMenuItem.Size = new Size(224, 34);
            saveGameToolStripMenuItem.Text = "Save Game ";
            saveGameToolStripMenuItem.Click += saveGameToolStripMenuItem_Click;
            // 
            // newGameToolStripMenuItem
            // 
            newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            newGameToolStripMenuItem.Size = new Size(224, 34);
            newGameToolStripMenuItem.Text = "New Game";
            newGameToolStripMenuItem.Click += newGameToolStripMenuItem_Click;
            // 
            // restoreGameToolStripMenuItem
            // 
            restoreGameToolStripMenuItem.Name = "restoreGameToolStripMenuItem";
            restoreGameToolStripMenuItem.Size = new Size(224, 34);
            restoreGameToolStripMenuItem.Text = "Restore Game";
            restoreGameToolStripMenuItem.Click += restoreGameToolStripMenuItem_Click;
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { informationPanelToolStripMenuItem, gameToSpeechToolStripMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(92, 29);
            optionsToolStripMenuItem.Text = "Settings";
            // 
            // informationPanelToolStripMenuItem
            // 
            informationPanelToolStripMenuItem.Checked = true;
            informationPanelToolStripMenuItem.CheckOnClick = true;
            informationPanelToolStripMenuItem.CheckState = CheckState.Checked;
            informationPanelToolStripMenuItem.Name = "informationPanelToolStripMenuItem";
            informationPanelToolStripMenuItem.Size = new Size(254, 34);
            informationPanelToolStripMenuItem.Text = "Information Panel";
            informationPanelToolStripMenuItem.Click += informationPanelToolStripMenuItem_Click;
            // 
            // gameToSpeechToolStripMenuItem
            // 
            gameToSpeechToolStripMenuItem.CheckOnClick = true;
            gameToSpeechToolStripMenuItem.Name = "gameToSpeechToolStripMenuItem";
            gameToSpeechToolStripMenuItem.Size = new Size(254, 34);
            gameToSpeechToolStripMenuItem.Text = "Game-To-Speech";
            gameToSpeechToolStripMenuItem.Click += gameToSpeechToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem1
            // 
            helpToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            helpToolStripMenuItem1.Size = new Size(65, 29);
            helpToolStripMenuItem1.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(164, 34);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(55, 29);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // PlayerPanel
            // 
            PlayerPanel.BackColor = SystemColors.ControlDark;
            PlayerPanel.Controls.Add(currentPlayerInd);
            PlayerPanel.Controls.Add(player2NameBox);
            PlayerPanel.Controls.Add(player1NameBox);
            PlayerPanel.Controls.Add(pictureBox2);
            PlayerPanel.Controls.Add(pictureBox1);
            PlayerPanel.Controls.Add(whitePieceCounter);
            PlayerPanel.Controls.Add(blackPieceCounter);
            PlayerPanel.Location = new Point(12, 814);
            PlayerPanel.Name = "PlayerPanel";
            PlayerPanel.Size = new Size(854, 102);
            PlayerPanel.TabIndex = 1;
            // 
            // currentPlayerInd
            // 
            currentPlayerInd.AutoSize = true;
            currentPlayerInd.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            currentPlayerInd.ForeColor = Color.Green;
            currentPlayerInd.Location = new Point(18, 0);
            currentPlayerInd.Name = "currentPlayerInd";
            currentPlayerInd.Size = new Size(154, 28);
            currentPlayerInd.TabIndex = 5;
            currentPlayerInd.Text = "Current Player:";
            // 
            // player2NameBox
            // 
            player2NameBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            player2NameBox.Location = new Point(747, 46);
            player2NameBox.Name = "player2NameBox";
            player2NameBox.Size = new Size(88, 34);
            player2NameBox.TabIndex = 4;
            player2NameBox.Text = "Player #2";
            // 
            // player1NameBox
            // 
            player1NameBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            player1NameBox.Location = new Point(18, 46);
            player1NameBox.Name = "player1NameBox";
            player1NameBox.Size = new Size(94, 34);
            player1NameBox.TabIndex = 4;
            player1NameBox.Text = "Player #1";
            // 
            // pictureBox2
            // 
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(668, 31);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(61, 58);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 3;
            pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(128, 31);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(61, 58);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // whitePieceCounter
            // 
            whitePieceCounter.AutoSize = true;
            whitePieceCounter.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            whitePieceCounter.ForeColor = SystemColors.ActiveCaptionText;
            whitePieceCounter.Location = new Point(609, 41);
            whitePieceCounter.Name = "whitePieceCounter";
            whitePieceCounter.Size = new Size(53, 38);
            whitePieceCounter.TabIndex = 1;
            whitePieceCounter.Text = "x 2";
            // 
            // blackPieceCounter
            // 
            blackPieceCounter.AutoSize = true;
            blackPieceCounter.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            blackPieceCounter.ForeColor = SystemColors.ActiveCaptionText;
            blackPieceCounter.Location = new Point(195, 41);
            blackPieceCounter.Name = "blackPieceCounter";
            blackPieceCounter.Size = new Size(53, 38);
            blackPieceCounter.TabIndex = 1;
            blackPieceCounter.Text = "x 2";
            // 
            // fileSystemWatcher1
            // 
            fileSystemWatcher1.EnableRaisingEvents = true;
            fileSystemWatcher1.SynchronizingObject = this;
            // 
            // GameBoardForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Desktop;
            ClientSize = new Size(878, 928);
            Controls.Add(PlayerPanel);
            Controls.Add(menuStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GameBoardForm";
            Text = "O'Neillo";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            PlayerPanel.ResumeLayout(false);
            PlayerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem menuToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem newGameToolStripMenuItem;
        private ToolStripMenuItem saveGameToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem1;
        private ToolStripMenuItem gameToSpeechToolStripMenuItem;
        private Panel PlayerPanel;
        private Label blackPieceCounter;
        private Label whitePieceCounter;
        private FileSystemWatcher fileSystemWatcher1;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem informationPanelToolStripMenuItem;
        private TextBox player2NameBox;
        private TextBox player1NameBox;
        private Label currentPlayerInd;
        private ToolStripMenuItem restoreGameToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
    }
}