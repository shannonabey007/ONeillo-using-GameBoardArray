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
            /*
            MenuStrip menuStrip = new MenuStrip(); // creates the menu strip
            ToolStripMenuItem fileMenuItem = new ToolStripMenuItem("File"); // adds the 'file' option to the menu strip
            ToolStripMenuItem optionsMenuItem = new ToolStripMenuItem("Options"); // adds the 'options' option to the menu strip
            menuStrip.Items.Add(fileMenuItem); // adds all the drop downs to the 'file' option to the menu strip
            menuStrip.Items.Add(optionsMenuItem); // ass all the drop downs to the 'options' option to the menu strip 
            this.Controls.Add(menuStrip);

            // drop downs for the file option
            fileMenuItem.DropDownItems.Add("New Game");
            fileMenuItem.DropDownItems.Add("Exit Game");

            // drop downs for options option
            optionsMenuItem.DropDownItems.Add("Settings");
            optionsMenuItem.DropDownItems.Add("Help");

            */

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

        }

        /// <summary>
        /// 
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

            int[] checkStraight = { -1, -1, -1, 0, 0, 1, 1, 1 }; // these will be used to check the directions (incl diagonals) 
            int[] checkHorizontal = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < 8; i++) // i is anything between 0 and
            {
                int r = row + checkStraight[i];
                int c = col + checkHorizontal[i];

                while (r >= 0 && r < num_BoardRows && c >= 0 && c < num_BoardColumns)
                {
                    if (gameBoardData[r, c] == oppositePlayer)
                    {
                        r += checkStraight[i];
                        c += checkHorizontal[i];
                        if (r >= 0 && r < num_BoardRows && c >= 0 && c < num_BoardColumns && gameBoardData[r, c] == currentPlayer)
                        {
                            MessageBox.Show("Valid Move");
                            return true;
                        }
                    }
                    else if (gameBoardData[r, c] == BlankPiece || gameBoardData[r, c] == currentPlayer)
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
        private void FlipPieces(int row, int col, int currentPlayer, int oppositePlayer)
        {
            int[] checkStraight = { -1, -1, -1, 0, 0, 1, 1, 1 }; // these will be used to check the directions (incl diagonals) 
            int[] checkHorizontal = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < 8; i++)
            {
                int r = row + checkStraight[i];
                int c = col + checkHorizontal[i];

                List<Point> tilesToFlip = new List<Point>(); // To store tiles to be flipped

                while (r >= 0 && r < num_BoardRows && c >= 0 && c < num_BoardColumns)
                {
                    if (gameBoardData[r, c] == oppositePlayer)
                    {
                        tilesToFlip.Add(new Point(r, c));
                        r += checkStraight[i];
                        c += checkHorizontal[i];

                        if (r >= 0 && r < num_BoardRows && c >= 0 && c < num_BoardColumns && gameBoardData[r, c] == currentPlayer)
                        {
                            foreach (Point p in tilesToFlip)
                            {
                                gameBoardData[p.X, p.Y] = currentPlayer;
                            }
                            break;
                        }
                    }
                    else if (gameBoardData[r, c] == BlankPiece || gameBoardData[r, c] == currentPlayer)
                    {
                        break;
                    }
                }
            }
        }

    }
}

