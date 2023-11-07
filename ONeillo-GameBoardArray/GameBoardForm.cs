using GameboardGUI;

namespace ONeillo_GameBoardArray
{
    public partial class GameBoardForm : Form
    {

        const int num_BoardRows = 8; // sets board rows to 8
        const int num_BoardColumns = 8; // sets board columns to 8

        GameboardImageArray _gameBoardGui; // calls the class and initialises it to the form 
        int[,] gameBoardData;
        string tilePNGpath = Directory.GetCurrentDirectory() + @"\tile images\"; 

        public GameBoardForm()
        {
            InitializeComponent();
        }

    }
}