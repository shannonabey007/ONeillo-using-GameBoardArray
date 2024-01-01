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

        int currentPlayer = BlackPiece;
        int oppositePlayer = WhitePiece;

        GameboardImageArray _gameBoardGui; // calls the class and initialises it to the form 
        int[,] gameBoardData; // array for gameboard 
        string tilePNGpath = Directory.GetCurrentDirectory() + @"\tile images\"; // uses the files in the directory for the gameboard tiles

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

        // creates the board array and setting all rows and columns to empty board pieces - green = 10

        /// <summary>
        /// creates the board array and populates it with blank pieces and also sets the board up in starting positions
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

        // swaps the player each time a move is made
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
        /// 
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
            }

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
        /// 
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


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Owner = this;
            about.Show();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gameBoardData = this.CreateBoardArray();
            _gameBoardGui.UpdateBoardGui(gameBoardData);

            gamestarted = false; // sets this back to false as its a new game so the game has not started yet

            // resets the name boxes and allows players to change names again
            player1NameBox.Text = "Player 1";
            player2NameBox.Text = "Player 2";
            player1NameBox.ReadOnly = false;
            player2NameBox.ReadOnly = false;

        }

        /// <summary>
        /// this method allows the game data to be saved to the file game_data.json in the same directory as the program is running in.
        /// This is done when the "Save Game" menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string serializedGameData = JsonConvert.SerializeObject(gameBoardData);
            string saveGamePath = Path.Combine(Directory.GetCurrentDirectory(), "game_data.json");

            System.IO.File.WriteAllText(saveGamePath, serializedGameData);

            MessageBox.Show("Game saved!");

        }

        /// <summary>
        /// Click restore game menu item to restore a saved game from the game_data.json file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restoreGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string readGameData = System.IO.File.ReadAllText("game_data.json");
            gameBoardData = JsonConvert.DeserializeObject<int[,]>(readGameData);
            _gameBoardGui.UpdateBoardGui(gameBoardData);

            MessageBox.Show("Game restored!");

        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetGameState() // turns gameBoardArray into a string to be spoken by iterating over the board and speaking the pievces and their positions
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
        /// 
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

    }
}

