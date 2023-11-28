using GameboardGUI;
using System.Drawing.Drawing2D;
using System.Linq.Expressions;

namespace ONeillo_GameBoardArray
{
    public partial class GameBoardForm : Form
    {

        const int num_BoardRows = 8; // sets board rows to 8
        const int num_BoardColumns = 8; // sets board columns to 8
        const int BlankPiece = 10; // constant for empty squares 
        const int WhitePiece = 1;
        const int BlackPiece = 0;


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
        }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
        private void GameTileClicked(object sender, EventArgs e)
        {
            int clickedRow = _gameBoardGui.GetCurrentRowIndex(sender);
            int clickedColumn = _gameBoardGui.GetCurrentColumnIndex(sender);


            if (ValidMove(clickedRow, clickedColumn, currentPlayer, oppositePlayer))
            {
                gameBoardData[clickedRow, clickedColumn] = currentPlayer; // applies the move 
                FlipPieces(clickedRow, clickedColumn, currentPlayer, oppositePlayer); // flips pieces 
                _gameBoardGui.UpdateBoardGui(gameBoardData);
                SwapPlayPieces();
            }
            /*
            else
            {
                MessageBox.Show("Invalid move: tile occupied");
            }
            */

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

            MessageBox.Show("Invalid Move");
            return false;
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
        }

        private void saveGameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}

