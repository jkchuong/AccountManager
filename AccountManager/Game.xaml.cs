using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using AccountBusiness;
using AccountData;
using ChessApp;

namespace AccountManager
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    /// 

    
    public partial class Game : Page
    {
        // Declare parameters
        private Business _account = new Business();
        private User user;

        private Brush primaryColour;
        private Brush secondaryColour;

        private Chessboard chessboard = new Chessboard();
        public ObservableCollection<Pieces> Piece { get; set; }
        private Cell currentCell;



        public Game(User userPassed)
        {
            Piece = new ObservableCollection<Pieces>();
            user = userPassed;

            InitializeComponent();

            
            var userThemes = _account.GetUserTheme(user.UserId);
            primaryColour = (SolidColorBrush)new BrushConverter().ConvertFromString(userThemes[0]);
            secondaryColour = (SolidColorBrush)new BrushConverter().ConvertFromString(userThemes[1]);

            CreateButtonGrid(primaryColour, secondaryColour);

            WhiteHistory.Content = " White Moves";
            BlackHistory.Content = " Black Moves";

            if (user.SaveExist)
            {
                GetSavedGame();

            }
            else
            {
                chessboard.NewGame();
            }

            UpdateBoardState();

            // Populate user information
            string data = $"{user.Name}, Wins: {user.Wins}, Losses: {user.Losses}.";
            UserData.Text = data;
        }

        public void GetSavedGame()
        {
            XDocument saveFile = _account.LoadSaveFile();
            XElement userSave =
                saveFile.Descendants("User")
                .Where(s => (string)s.Attribute("UserId") == user.UserId)
                .FirstOrDefault();

            foreach (XElement pieceInSave in userSave.Elements())
            {
                string pieceName = pieceInSave.Attribute("Name").Value;
                int pieceRow = Int32.Parse(pieceInSave.Element("Row").Value);
                int pieceCol = Int32.Parse(pieceInSave.Element("Column").Value);
                bool isWhite = Convert.ToBoolean(pieceInSave.Element("IsWhite").Value);
                bool isFirstMove = Convert.ToBoolean(pieceInSave.Element("IsFirstMove").Value);

                chessboard.AddPieceToBoard(pieceName, pieceRow, pieceCol, isWhite, isFirstMove);
            }
        }

        #region Chess Game Mechanics

        // Create interactive Chessboard using user theme colours
        private void CreateButtonGrid(Brush primary, Brush secondary)
        {
            for (int row = 0; row < 8; row++)
            {
                bool isBlack = row % 2 == 1;
                for (int col = 0; col < 8; col++)
                {
                    var button = new Button() { Background = isBlack ? secondary : primary, Foreground = !isBlack ? secondary : primary };

                    // Name buttons with algebraic notation and connect to chessboard
                    button.Name = ChessApp.Rulebook.ArrayToCellRow[col].ToString() + ChessApp.Rulebook.ArrayToCellColumn[row].ToString(); 
                    button.Click += GridButton_Click;
                    button.FontSize = 50;
                    button.Tag = chessboard.Board[row, col];
                    chessboard.Board[row, col].Name = button.Name;

                    // Add the buttons to uniform grid
                    Chessboard.Children.Add(button);
                    isBlack = !isBlack;
                }
            }
        }

        private void GridButton_Click(object sender, RoutedEventArgs e)
        {

            // Get info about Cell
            Button clickedButton = (sender as Button);
            Cell clickedCell = clickedButton.Tag as Cell;

            List<Pieces> whitePieces;
            List<Pieces> blackPieces;

            // If user intends to move
            if (clickedCell.IsLegal)
            {
                // Move the piece and update the board
                PrintMoveHistory(currentCell, clickedCell);
                chessboard.MovePiece(currentCell, clickedCell);

                if (clickedCell.Piece.Name == "Pawn" && (clickedCell.Row == 7 || clickedCell.Row == 0))
                {
                    PromotionBackground.Visibility = Visibility.Visible;
                    Promotion.Visibility = Visibility.Visible;
                }

                UpdateBoardState();

                // Check if game over after white move
                whitePieces = chessboard.SearchForPieces(true);
                blackPieces = chessboard.SearchForPieces(false);

                if (IsGameOver(whitePieces, blackPieces))
                {
                    chessboard.NewGame();
                }
                else
                {
                    ComputerMove(blackPieces);
                }

                // Check if game over after black move
                blackPieces = chessboard.SearchForPieces(false);
                whitePieces = chessboard.SearchForPieces(true);

                if (IsGameOver(whitePieces, blackPieces))
                {
                    chessboard.NewGame();
                }

            }

            else if (clickedCell.IsOccupied)
            {
                // Find legal moves if user clicks on white piece, do nothing if black piece that is not legal
                if (clickedCell.Piece.IsWhite)
                {
                    chessboard.ClearMarkedLegalMoves();
                    chessboard.FindLegalMoves(clickedCell.Piece);
                }
                else
                {
                    return;
                }
            }

            // If user didn't click on legal or occupied
            else
            {
                chessboard.ClearMarkedLegalMoves();
            }

            currentCell = clickedCell;

            UpdateBoardState();

        }

        private void Promotion_Click(object sender, RoutedEventArgs e)
        {
            PromotionBackground.Visibility = Visibility.Collapsed;
            Promotion.Visibility = Visibility.Collapsed;

            string promotedPiece = (sender as Button).Content.ToString();

            chessboard.Promotion(currentCell.Piece, promotedPiece);

            UpdateBoardState();
        }

        private void ComputerMove(List<Pieces> blackPieces)
        {
            // Delay before computer move
            Random rnd = new Random();
            Task.Delay(rnd.Next(500, 2000));

            bool hasNotMoved = true;
            while (hasNotMoved)
            {
                Pieces piece = blackPieces[rnd.Next(0, blackPieces.Count - 1)];
                if (chessboard.HasLegalMove(piece))
                {
                    // Get list of legal moves for that piece
                    List<Cell> legalPositions = new List<Cell>();

                    foreach (Cell cell in chessboard.Board)
                    {
                        if (cell.IsLegal)
                        {
                            legalPositions.Add(cell);
                        }
                    }

                    // Print Move History
                    Cell newCell = legalPositions[rnd.Next(0, legalPositions.Count - 1)];
                    PrintMoveHistory(piece.Position, newCell);

                    chessboard.MovePiece(piece.Position, newCell);
                    hasNotMoved = false;
                }
                else
                {
                    blackPieces.Remove(piece);
                }
            }
            chessboard.ClearMarkedLegalMoves();
        }

        private bool IsGameOver(List<Pieces> whitePieces, List<Pieces> blackPieces)
        {
            int newWins = 0, newLoss = 0;

            // Continue if king exists. Add one to win/loss otherwise
            if (chessboard.KingExists(whitePieces) && chessboard.KingExists(blackPieces))
            {
                return false;

            }
            else if (!chessboard.KingExists(blackPieces))
            {
                newWins = _account.AddOneToWins(user.UserId);
            }
            else
            {
                newLoss = _account.AddOneToLosses(user.UserId);
            }

            //Refresh data
            WhiteHistory.Content = " White Moves";
            BlackHistory.Content = " Black Moves";

            string data = $"{user.Name}, Wins: {newWins}, Losses: {newLoss}.";
            UserData.Text = data;
            return true;
        }

        private void PrintMoveHistory(Cell beforeCell, Cell afterCell)
        {
            // Put each move on new line
            string message = "\r\n ";
            if (beforeCell.Piece.Name == "Pawn")
            {
                // Pawn moves into empty cell print only cell name, if occupied add column value and x
                if (!afterCell.IsOccupied)
                {
                    message += $"{afterCell.Name}";
                }
                else
                {
                    message += $"{beforeCell.Name[0]}x{afterCell.Name}";
                }
            }

            // Print piece initial and cell name if moving into empty cell, otherwise add x
            else if (!afterCell.IsOccupied)
            {
                message += $"{ChessApp.Rulebook.ConvertPieceToInitial(beforeCell.Piece)}{afterCell.Name}";
            }
            else
            {
                message += $"{ChessApp.Rulebook.ConvertPieceToInitial(beforeCell.Piece)}x{afterCell.Name}";
            }

            if (beforeCell.Piece.IsWhite)
            {
                WhiteHistory.Content += message;
                WhiteHistory.ScrollToBottom();
            }
            else
            {
                BlackHistory.Content += message;
                BlackHistory.ScrollToBottom();
            }
        }

        private void UpdateBoardState()
        {
            foreach (Button button in Chessboard.Children)
            {
                Cell cell = button.Tag as Cell;

                // If occupied add name, else clear content
                if (cell.IsOccupied)
                {

                    button.Content = ChessApp.Rulebook.ConvertPieceToInitial(cell.Piece);
                }
                else
                {
                    button.Content = "";
                    button.IsEnabled = true;
                }

                // If legal change colour to yellow, else change it back to original colour
                if (cell.IsLegal)
                {
                    button.Background = Brushes.Yellow;
                }
                else
                {
                    if (ChessApp.Rulebook.BlackCells.Contains(button.Name))
                    {
                        button.Background = secondaryColour;
                    }
                    else
                    {
                        button.Background = primaryColour;
                    }
                }
            }
        }

        // Use images instead of letters? (not implemented)
        public static string ImageSource(Pieces piece)
        {
            string source = "pack://application:,,,/ChessImages/";
            if (piece.Name == "BlackPawn" || piece.Name == "WhitePawn")
            {
                source += piece.Name + ".png";
            }
            else
            {
                if (piece.IsWhite)
                {
                    source += "White" + piece.Name + ".png";
                }
                else
                {
                    source += "Black" + piece.Name + ".png";
                }
            }
            return source;
        }

        #endregion

        // Go back to login page
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            this.NavigationService.Navigate(login);
        }

        // Store all piece information in SQL database
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _account.SaveToXML(user.UserId, chessboard.Board);
        }


        // Go to settings page
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings setting = new Settings(user);
            this.NavigationService.Navigate(setting);
        }

        // Toggle rules 
        private void Rules_Click(object sender, RoutedEventArgs e)
        {
            if (Rulebook.Visibility == Visibility.Collapsed)
            {
                Rulebook.Visibility = Visibility.Visible;
                Rules.Background = Brushes.Yellow;
            }
            else
            {
                Rulebook.Visibility = Visibility.Collapsed;
                Rules.Background = Brushes.LightGoldenrodYellow;
            }
        }
    }
}
