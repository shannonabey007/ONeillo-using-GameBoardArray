using GameboardGUI;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Drawing2D;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Windows.Forms;
using System.Speech.Synthesis;
using Newtonsoft.Json;


namespace ONeillo_GameBoardArray
{
    public partial class GameBoardForm : Form
    {

        const int num_BoardRows = 8; // sets board rows to 8
        const int num_BoardColumns = 8; // sets board columns to 8
        const int BlankPiece = 10; // constant for empty squares 
        const int WhitePiece = 1; // white pieces = 1 because thats what the png is named
        const int BlackPiece = 0; // black = 0 because ^

        bool gamestarted = false;
        bool gameSaved = false;
        bool gameover = false;

        int currentPlayer = BlackPiece;
        int oppositePlayer = WhitePiece;

        GameboardImageArray _gameBoardGui; // calls the class and initialises it to the form 
        int[,] gameBoardData; // array for gameboard 
        string tilePNGpath = Directory.GetCurrentDirectory() + @"\tile images\"; // uses the files in the directory for the gameboard tiles

        /// <summary>
        /// windows form for actual game window, draws game board on with array data corresponding
        /// </summary>
        public GameBoardForm()
        {
            InitializeComponent();

            Point topLC = new Point(100, 100);
            Point bottomRC = new Point(100, 150);

            gameBoardData = this.CreateBoardArray();


            try
            {
                _gameBoardGui = new GameboardImageArray(this, gameBoardData, topLC, bottomRC, 0, tilePNGpath);
                _gameBoardGui.TileClicked += new GameboardImageArray.TileClickedEventDelegate(GameTileClicked);
            }
            catch (Exception ex)
            {
                DialogResult result = MessageBox.Show(ex.ToString(), "Game board size error", MessageBoxButtons.OK);
                this.Close();
            }

        }


        /// <summary>
        /// creates the board array and populates it with blank pieces and also sets the board up in starting positions
        /// sets all rows and columns to empty board pieces - green = 10
        /// </summary>
        /// <returns></returns>
        private int[,] CreateBoardArray()
        {
            int[,] boardArray = new int[num_BoardRows, num_BoardColumns];

            for (int row = 0; row < num_BoardRows; row++) // iterates over the rows 
            {
                for (int col = 0; col < num_BoardColumns; col++) // iterates over the columns
                {
                    boardArray[row, col] = BlankPiece; // sets them all to 10 which is what the empty game tile is set to 
                }

            }

            // sets up the board with the initial pieces, 1 being white pieces and 0 being black pieces 
            boardArray[3, 3] = 1;
            boardArray[3, 4] = 0;
            boardArray[4, 3] = 0;
            boardArray[4, 4] = 1;

            return boardArray;
        }

        /// <summary>
        /// swaps the player pieces and indicates on the informtion panel who is currently playing
        /// </summary>
        private void SwapPlayPieces()
        {

            int temp = currentPlayer;
            currentPlayer = oppositePlayer;
            oppositePlayer = temp;

            // moves the current player indicator 
            if (currentPlayer == 0)
            {
                currentPlayerInd.Location = new Point(18, 0);
            }
            else
            {
                currentPlayerInd.Location = new Point(689, 0);
            }

        }

        /// <summary>
        /// Checks if any of the game tiles on the board have been clicked, if so checks if it is a valid move, otherwise displays error message
        /// also makes it so players cant change their names once gameplay has started 
        /// displays when no valid moves left 
        /// displays game over and game winner 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GameTileClicked(object sender, EventArgs e)
        {
            int clickedRow = _gameBoardGui.GetCurrentRowIndex(sender);
            int clickedColumn = _gameBoardGui.GetCurrentColumnIndex(sender);

            // stops players from changing their in-game name after the game has started (if gamestarted = true then)
            if (!gamestarted)
            {
                gamestarted = true;
                player1NameBox.ReadOnly = true;
                player2NameBox.ReadOnly = true;
            }


            if (ValidMove(clickedRow, clickedColumn, currentPlayer, oppositePlayer))
            {
                gameBoardData[clickedRow, clickedColumn] = currentPlayer; // applies the move 
                FlipPieces(clickedRow, clickedColumn, currentPlayer, oppositePlayer); // flips pieces 
                _gameBoardGui.UpdateBoardGui(gameBoardData); // updates gui 
                SwapPlayPieces(); // calls swap method 

                if (gameToSpeechToolStripMenuItem.Checked) // when the menu item is checked the pieces will be spoken and placed asynchronously
                {
                    SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                    string gameState = GetGameState();
                    await Task.Run(() => synthesizer.SpeakAsync(gameState));
                }
            }
            else
            {
                MessageBox.Show("Invalid Move, try again"); // displays when an invalid move is made during gameplay
            }

            // shows the number of black pieces on the gui panel
            int blackPieces = 0;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (gameBoardData[row, col] == 0) // finds the black pieces on the board/ in the array
                    {
                        blackPieces++; // adds it all to the variable, incrementing the number each time a piece is found
                        blackPieceCounter.Text = "x" + Convert.ToString(blackPieces); // displays it on the gui
                    }
                }
            }

            // shows the number of white pieces on the gui panel
            int whitePieces = 0;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (gameBoardData[row, col] == 1) // finds the white pieces on the board/ in the array
                    {
                        whitePieces++; // adds it all to the variable, incrementing the variable number each time a piece is found
                        whitePieceCounter.Text = "x" + Convert.ToString(whitePieces); // displays it on the gui 
                    }
                }
            }


            if (HasValidMovesLeft(currentPlayer, oppositePlayer) == false)
            {
                MessageBox.Show("No Valid Moves left.");

                if (currentPlayer == 1)
                {
                    currentPlayer = 0;
                    oppositePlayer = 1;
                    currentPlayerInd.Location = new Point(689, 0);
                }

                else
                {
                    currentPlayer = 1;
                    oppositePlayer = 0;
                    currentPlayerInd.Location = new Point(18, 0);
                }

                if(!HasValidMovesLeft(currentPlayer,oppositePlayer) && !HasValidMovesLeft(oppositePlayer, currentPlayer))
                {
                    gameover = true;
                    ShowGameOverMessage(); 
                }
            }

        }

        /// <summary>
        /// displays the winner by appending the result of the winnercalculation to game over message 
        /// </summary>
        private void ShowGameOverMessage()
        {
            string winner = WinnerCalculator();
            MessageBox.Show($"Game Over! {winner}"); 
           
        }

        /// <summary>
        /// function for the game logic -
        /// checks if there is a blank piece - if the tile isnt blank then the move is not valid
        /// if the function finds the opposite players piece(s) then the current player pieces then the move is valid
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="currentPlayer"></param>
        /// <param name="oppositePlayer"></param>
        /// <returns></returns>#
        /// 
        private bool ValidMove(int row, int col, int currentPlayer, int oppositePlayer)
        {
            if (gameBoardData[row, col] != BlankPiece)
            {
                MessageBox.Show("Invalid Move: Tile Occupied");
                return false;
            }


            // these will be used to check the directions (incl diagonals) 
            int[] checkRows = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] checkHorizontal = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < 8; i++) // i is used to iterate over the tiles - i is anything between 0 and 8 
            {
                int r = row + checkRows[i]; // checks the rows around the tile clicked
                int c = col + checkHorizontal[i]; // checks the horizontal directions from the tile clicked

                while (r >= 0 && r < num_BoardRows && c >= 0 && c < num_BoardColumns) // while the tile clicked is in the board 
                {
                    if (gameBoardData[r, c] == oppositePlayer) // if the opposite player pieces are found when iterating around the clicked tile
                    {
                        r += checkRows[i];
                        c += checkHorizontal[i];
                        if (r >= 0 && r < num_BoardRows && c >= 0 && c < num_BoardColumns && gameBoardData[r, c] == currentPlayer) // if the current player's piece is also found after find other players piece(s):
                        {
                            return true; // the move is valid 
                        }
                    }

                    else if (gameBoardData[r, c] == BlankPiece || gameBoardData[r, c] == currentPlayer) // breaks if 
                    {

                        break;

                    }
                }

            }

            return false;
        }

        /// <summary>
        /// Checks if there are any valid moves left on the board, if there are none the game is over. 
        /// </summary>
        /// <param name="currentPlayer"></param>
        /// <param name="oppositePlayer"></param>
        /// <returns></returns>
        private bool HasValidMovesLeft(int currentPlayer, int oppositePlayer)
        {
            for (int row = 0; row < num_BoardRows; row++)
            {
                for (int col = 0; col < num_BoardColumns; col++)
                {
                    if (gameBoardData[row, col] == 10)
                    {
                        if (ValidMove(row, col, currentPlayer, oppositePlayer) == true)
                        {
                            return true;
                        }
                    }
                }
            }
            return false; // No valid moves left
        }

        /// <summary>
        /// Flips the pieces when a valid move is made, checks the board for the opposite player's pieces and checks for the current players pieces around it 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="currentPlayer"></param>
        /// <param name="oppositePlayer"></param>
        /// 
        // function to flip pieces once a valid move is made 
        private void FlipPieces(int row, int col, int currentPlayer, int oppositePlayer)
        {
            // these will be used to check the directions (incl diagonals)
            int[] checkRows = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] checkHorizontal = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < 8; i++) // i is used to iterate over the tiles - i is anything between 0 and 8 
            {
                int r = row + checkRows[i];
                int c = col + checkHorizontal[i];

                List<Point> tilesToFlip = new List<Point>(); // stores tiles to be flipped

                while (r >= 0 && r < num_BoardRows && c >= 0 && c < num_BoardColumns)
                {
                    if (gameBoardData[r, c] == oppositePlayer)
                    {
                        tilesToFlip.Add(new Point(r, c)); // iterates over opposite players pieces and stores them as points to be flipped
                        r += checkRows[i];
                        c += checkHorizontal[i];

                        if (r >= 0 && r < num_BoardRows && c >= 0 && c < num_BoardColumns && gameBoardData[r, c] == currentPlayer)
                        {
                            foreach (Point p in tilesToFlip) // flips all stored points 
                            {
                                gameBoardData[p.X, p.Y] = currentPlayer; // to the current player's pieces 
                            }
                            break;
                        }
                    }
                    else if (gameBoardData[r, c] == BlankPiece || gameBoardData[r, c] == currentPlayer) // if there are blank pieces or current player pieces, does not flip them 
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the winner by seeing which player has the most pieces 
        /// </summary>
        /// <returns></returns>
        private string WinnerCalculator()
        {
            int numOfBlackPieces = 0;
            int numOfWhitePieces = 0;

            for (int row = 0; row < num_BoardRows; row++)
            {
                for (int col = 0; col < num_BoardColumns; col++)
                {
                    if (gameBoardData[row, col] == 0)
                    {
                        numOfBlackPieces++;
                    }
                    else if (gameBoardData[row, col] == 1)
                    {
                        numOfWhitePieces++;
                    }
                }
            }

            if (numOfBlackPieces > numOfWhitePieces)
            {
                return player1NameBox.Text + " wins!";
            }
            else if (numOfWhitePieces > numOfBlackPieces)
            {
                return player2NameBox.Text + " wins!";
            }
            else
            {
                return "It's a draw!";
            }
        }

        /// <summary>
        /// When clicked, pop up window shows the about page for the game. improvised slightly for humour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Owner = this;
            about.Show();
        }

        /// <summary>
        /// new game, restores the board to starting state 
        /// asks players if they want to save if not saved already
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gamestarted && !gameSaved)
            {
                DialogResult result = MessageBox.Show("Do you want to save the current game before starting a new one?", "Save Game", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    saveGameToolStripMenuItem_Click(sender, e); // Save the game
                }
                else if (result == DialogResult.Cancel)
                {
                    return; // Cancel the operation if the user selects Cancel
                }

            }

            gameBoardData = this.CreateBoardArray();
            _gameBoardGui.UpdateBoardGui(gameBoardData);

            gamestarted = false; // sets this back to false as its a new game so the game has not started yet

            // resets the name boxes and allows players to change names again
            player1NameBox.Text = "Player #1";
            player2NameBox.Text = "Player #2";
            player1NameBox.ReadOnly = false;
            player2NameBox.ReadOnly = false;

        }

        /// <summary>
        /// gets and sets the player names from text boxes as well as game board data, speech settings and panel information settings
        /// </summary>
        public class GameData
        {

            public string Player1Name { get; set; }
            public string Player2Name { get; set; }
            public int[,] GameBoardData { get; set; }
            public bool SpeechEnabled { get; set; }

            public bool ShowingPanel { get; set; }
            public GameData(int[,] gameBoardData, string player1Name, string player2Name, bool speechEnabled, bool showingPanel)
            {
                Player1Name = player1Name;
                Player2Name = player2Name;
                GameBoardData = gameBoardData;
                SpeechEnabled = speechEnabled;
                ShowingPanel = showingPanel;
            }
        }

        /// <summary>
        /// allows the game to be saved when menu item is clicked, saves all settings, board state and names
        /// creates an instance of GameData and calls the variables needed as arguments to be serialized 
        /// sets gameSaved to true when a game state has been saved 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string saveGamePath = Path.Combine(Directory.GetCurrentDirectory(), "game_data.json");

            try
            {
                GameData gameData = new GameData(gameBoardData, player1NameBox.Text, player2NameBox.Text, gameToSpeechToolStripMenuItem.Checked, informationPanelToolStripMenuItem.Checked);
                string serializedGameData = JsonConvert.SerializeObject(gameData);
                System.IO.File.WriteAllText(saveGamePath, serializedGameData);

                gameSaved = true;

                MessageBox.Show("Game saved!");
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error saving game: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }


        /// <summary>
        /// Click restore game menu item to restore a saved game from the game_data.json file
        /// restores all settings (speech and panel information)
        /// restores the original variables to their saved instances 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restoreGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string readGameData = System.IO.File.ReadAllText("game_data.json");

            GameData data = JsonConvert.DeserializeObject<GameData>(readGameData);
            gameBoardData = data.GameBoardData;
            player1NameBox.Text = data.Player1Name;
            player2NameBox.Text = data.Player2Name;
            gameToSpeechToolStripMenuItem.Checked = data.SpeechEnabled;
            informationPanelToolStripMenuItem.Checked = data.ShowingPanel;

            _gameBoardGui.UpdateBoardGui(gameBoardData);

            MessageBox.Show("Game restored!");

        }

        /// <summary>
        /// Default set to checked - showing
        /// when clicked, unchecks and does not show the informations panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void informationPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (informationPanelToolStripMenuItem.Checked == false) // it is checked by default so if it is clicked then the panel and player indicator are not visable 
            {
                PlayerPanel.Visible = false;
                currentPlayerInd.Visible = false;

            }
            else
            {
                PlayerPanel.Visible = true;
                currentPlayerInd.Visible = true;
            }
        }

        /// <summary>
        /// turns gameBoardArray into a string to be spoken by iterating over the board and speaking the pievces and their positions
        /// </summary>
        /// <returns></returns>
        private string GetGameState()
        {
            // implementation to generate game state text for text to speech fnction
            string gameState = "Current game state: \n";

            for (int row = 0; row < num_BoardRows; row++)
            {
                for (int col = 0; col < num_BoardColumns; col++)
                {
                    if (gameBoardData[row, col] != BlankPiece)
                    {
                        gameState += $"Row {row},Column {col}: ";

                        if (gameBoardData[row, col] == BlackPiece)
                        {
                            gameState += "Black Piece \n";
                        }
                        else if (gameBoardData[row, col] == WhitePiece)
                        {
                            gameState += "White Piece \n";
                        }
                    }
                }
            }

            return gameState;
        }

        /// <summary>
        /// when clicked, checks the menu item
        /// allows gameplay of the entire board to be dictated, speaks colour of pieces and their positions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gameToSpeechToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gameToSpeechToolStripMenuItem.Checked == true)
            {
                SpeechSynthesizer synthesizer = new SpeechSynthesizer(); // creates synthesizer 
                string gameState = GetGameState(); //gets the game state into a string which can be spoken using the GetGameState function
                synthesizer.Speak(gameState);// speaks
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gamestarted && !gameSaved)
            {
                DialogResult result = MessageBox.Show("Do you want to save the current game before starting a new one?", "Save Game", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    saveGameToolStripMenuItem_Click(sender, e); // Save the game
                }
                else if (result == DialogResult.Cancel)
                {
                    return; // Cancel the operation if the user selects Cancel
                }

            }

            this.Close();

        }
    }
}

