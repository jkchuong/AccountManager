using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
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
using System.Windows.Threading;
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



        public Game(User userPassed, bool comingFromLogin)
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

            // Load saved game if it exists and arriving from login
            if (user.SaveExist && comingFromLogin)
            {
                GetSavedGame("Save");
            }

            // Load temp save if it exists and arriving from settings
            else if (_account.TempSaveExists(user.UserId))
            {
                GetSavedGame("Temp");
            }
            else
            {
                chessboard.NewGame();
            }

            UpdateBoardState();

            // Populate user information
            string data = $"{user.Name}, Wins: {user.Wins}, Losses: {user.Losses}.";
            UserData.Text = data;

            // Populate Ranking
            var topThree = _account.GetTopThreePlayers();
            string rank1 = $"1st\r\n{topThree[0].Item1}: {topThree[0].Item2}";
            string rank2 = $"2nd\r\n{topThree[1].Item1}: {topThree[1].Item2}";
            string rank3 = $"3rd\r\n{topThree[2].Item1}: {topThree[2].Item2}";
            Ranking1.Text = rank1;
            Ranking2.Text = rank2;
            Ranking3.Text = rank3;
        }

        public void GetSavedGame(string file)
        {
            XDocument saveFile = _account.LoadSaveFile(file);
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

                if (chessboard.KingInCheckForOtherSide(whitePieces))
                {
                    Message.Text = "Black King in check";
                }

                // Start new game if it's game over, else go to next turn
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

                if (chessboard.KingInCheckForOtherSide(blackPieces))
                {
                    Message.Text = "White King in check";
                }

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

        // How to optimise? (Use goto statement?)
        private void ComputerMove(List<Pieces> blackPieces)
        {
            // Delay before computer move (doesn't work)
            Random rnd = new Random();
            Task.Delay(rnd.Next(500, 2000));

            bool hasMoved = false;

            while (!hasMoved)
            {
                List<Pieces> piecesWithAggressiveMoves = chessboard.GetPiecesWithAggressiveMoves(blackPieces);

                // Check if something can make any aggressive move
                if (user.AggressiveOn && piecesWithAggressiveMoves.Any())
                {
                    // Get a random pieces that can make an aggressive move
                    Pieces piece = piecesWithAggressiveMoves[rnd.Next(0, piecesWithAggressiveMoves.Count - 1)];
                    List<Cell> aggressivePositions = new List<Cell>();

                    chessboard.FindLegalMoves(piece);

                    foreach(Cell cell in chessboard.Board)
                    {
                        if (cell.IsLegal && cell.IsOccupied)
                        {
                            aggressivePositions.Add(cell);
                        }
                    }

                    // Print Move History
                    Cell newCell = aggressivePositions[rnd.Next(0, aggressivePositions.Count - 1)];
                    PrintMoveHistory(piece.Position, newCell);
                    chessboard.MovePiece(piece.Position, newCell);

                    // Make Promotion if pawn reaches the end
                    if (piece.Name == "Pawn" && (piece.Position.Row == 0 || piece.Position.Row == 7))
                    {
                        chessboard.Promotion(piece, "Queen");
                    }
                }

                // If user doesn't want aggressive AI or AI can't make aggressive move, make random move
                else
                {
                    List<Pieces> piecesWithLegalMoves = chessboard.GetPiecesWithLegalMoves(blackPieces);

                    Pieces piece = piecesWithLegalMoves[rnd.Next(0, piecesWithLegalMoves.Count - 1)];
                    List<Cell> legalPositions = new List<Cell>();

                    chessboard.FindLegalMoves(piece);

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

                    // Make promotion if pawn reaches the end
                    if (piece.Name == "Pawn" && (piece.Position.Row == 0 || piece.Position.Row == 7))
                    {
                        chessboard.Promotion(piece, "Queen");
                    }
                }

                hasMoved = true;
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
                    var image = new Image { Source = new BitmapImage(new Uri(ImageSource(cell.Piece), UriKind.Relative)) };
                    button.Content = image;
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
            string source = "/ChessImages/";
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
            XDocument saveFile = _account.LoadSaveFile("Temp");
            _account.DeleteUserSave(user.UserId, saveFile);

            Login login = new Login();
            this.NavigationService.Navigate(login);
        }

        // Store all piece information in SQL database
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // How to use timer?
            _account.SaveToXML(user.UserId, chessboard.Board, "Save");
            Message.Text = "Saved!";
        }

        // Go to settings page
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            _account.SaveToXML(user.UserId, chessboard.Board, "Temp");
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
