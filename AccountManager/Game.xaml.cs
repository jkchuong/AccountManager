using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            NewGame();

            UpdateBoardState();

            // Populate user information
            string data = $"{user.Name}, Wins: {user.Wins}, Losses: {user.Losses}.";
            UserData.Text = data;
        }

        #region Chess Game Mechanics

        // Create interactive Chessboard with alternating colours
        private void CreateButtonGrid(Brush primary, Brush secondary)
        {
            for (int row = 0; row < 8; row++)
            {
                bool isBlack = row % 2 == 1;
                for (int col = 0; col < 8; col++)
                {
                    var button = new Button() { Background = isBlack ? secondary : primary, Foreground = !isBlack ? secondary : primary };
                    button.Name = ChessApp.Rulebook.ArrayToCellRow[col].ToString() + ChessApp.Rulebook.ArrayToCellColumn[row].ToString();
                    button.Click += GridButton_Click;
                    button.FontSize = 50;
                    button.Tag = chessboard.Board[row, col];
                    chessboard.Board[row, col].Name = button.Name;
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
                MovePiece(currentCell, clickedCell);

                UpdateBoardState();

                // Check if game over after white move
                whitePieces = SearchForPieces(true);
                blackPieces = SearchForPieces(false);

                if (IsGameOver(whitePieces, blackPieces))
                {
                    NewGame();
                }
                else
                {
                    ComputerMove(blackPieces);
                }

                // Check if game over after black move
                blackPieces = SearchForPieces(false);
                whitePieces = SearchForPieces(true);

                if (IsGameOver(whitePieces, blackPieces))
                {
                    NewGame();
                }

            }

            
            else if (clickedCell.IsOccupied)
            {
                // Find legal moves if user clicks on white piece, do nothing if black piece that is not legal
                if (clickedCell.piece.IsWhite)
                {
                    chessboard.ClearMarkedLegalMoves();
                    chessboard.FindLegalMoves(clickedCell.piece);
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
                    MovePiece(piece.Position, newCell);

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
            // continue if king exists. Add one to win/loss otherwise
            if (KingExists(whitePieces) && KingExists(blackPieces))
            {
                return false;

            }
            else if (!KingExists(blackPieces))
            {
                newWins = _account.AddOneToWins(user.UserId);
            }
            else
            {
                newLoss = _account.AddOneToLosses(user.UserId);
            }

            //refresh data
            WhiteHistory.Content = " White Moves";
            BlackHistory.Content = " Black Moves";

            string data = $"{user.Name}, Wins: {newWins}, Losses: {newLoss}.";
            UserData.Text = data;
            return true;
        }

        private void PrintMoveHistory(Cell beforeCell, Cell afterCell)
        {
            string message = "\r\n ";
            if (beforeCell.piece.Name == "Pawn")
            {
                // Pawn moves into empty cell print only cell name
                if (!afterCell.IsOccupied)
                {
                    message += $"{afterCell.Name}";
                }
                else
                {
                    message += $"{beforeCell.Name[0]}x{afterCell.Name}";
                }
            }
            else if (!afterCell.IsOccupied)
            {
                message += $"{ChessApp.Rulebook.ConvertPieceToInitial(beforeCell.piece)}{afterCell.Name}";
            }
            else
            {
                message += $"{ChessApp.Rulebook.ConvertPieceToInitial(beforeCell.piece)}x{afterCell.Name}";
            }

            if (beforeCell.piece.IsWhite)
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

        private void MovePiece(Cell beforeCell, Cell afterCell)
        {
            // if pawn then set first move to false
            if (beforeCell.piece.IsFirstMove)
            {
                beforeCell.piece.IsFirstMove = false;
            }

            // move piece to new cell and set that cell to be occupied
            afterCell.piece = beforeCell.piece;
            afterCell.IsOccupied = true;
            beforeCell.piece.Position = afterCell;

            // remove piece from old cell and set it to be not occupied
            beforeCell.piece = null;
            beforeCell.IsOccupied = false;

            chessboard.ClearMarkedLegalMoves();
        }

        private void UpdateBoardState()
        {
            foreach (Button button in Chessboard.Children)
            {
                Cell cell = button.Tag as Cell;

                // If occupied add name, else clear content
                if (cell.IsOccupied)
                {

                    button.Content = ChessApp.Rulebook.ConvertPieceToInitial(cell.piece);
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

        private void NewGame()
        {

            chessboard.ClearBoard();

            _ = new Pawn(true, chessboard.Board[6, 0]);
            _ = new Pawn(true, chessboard.Board[6, 1]);
            _ = new Pawn(true, chessboard.Board[6, 2]);
            _ = new Pawn(true, chessboard.Board[6, 3]);
            _ = new Pawn(true, chessboard.Board[6, 4]);
            _ = new Pawn(true, chessboard.Board[6, 5]);
            _ = new Pawn(true, chessboard.Board[6, 6]);
            _ = new Pawn(true, chessboard.Board[6, 7]);

            _ = new Rook(true, chessboard.Board[7, 0]);
            _ = new Knight(true, chessboard.Board[7, 1]);
            _ = new Bishop(true, chessboard.Board[7, 2]);
            _ = new Queen(true, chessboard.Board[7, 3]);
            _ = new King(true, chessboard.Board[7, 4]);
            _ = new Bishop(true, chessboard.Board[7, 5]);
            _ = new Knight(true, chessboard.Board[7, 6]);
            _ = new Rook(true, chessboard.Board[7, 7]);

            _ = new Pawn(false, chessboard.Board[1, 0]);
            _ = new Pawn(false, chessboard.Board[1, 1]);
            _ = new Pawn(false, chessboard.Board[1, 2]);
            _ = new Pawn(false, chessboard.Board[1, 3]);
            _ = new Pawn(false, chessboard.Board[1, 4]);
            _ = new Pawn(false, chessboard.Board[1, 5]);
            _ = new Pawn(false, chessboard.Board[1, 6]);
            _ = new Pawn(false, chessboard.Board[1, 7]);
           
            _ = new Rook(false, chessboard.Board[0, 0]);
            _ = new Knight(false, chessboard.Board[0, 1]);
            _ = new Bishop(false, chessboard.Board[0, 2]);
            _ = new Queen(false, chessboard.Board[0, 3]);
            _ = new King(false, chessboard.Board[0, 4]);
            _ = new Bishop(false, chessboard.Board[0, 5]);
            _ = new Knight(false, chessboard.Board[0, 6]);
            _ = new Rook(false, chessboard.Board[0, 7]);
        }

        private List<Pieces> SearchForPieces(bool isWhite)
        {
            List<Pieces> pieces = new List<Pieces>();

            foreach (Button button in Chessboard.Children)
            {
                Cell cell = button.Tag as Cell;
                if (cell.IsOccupied)
                {
                    if (cell.piece.IsWhite == isWhite)
                    {
                        pieces.Add(cell.piece);
                    }
                }
            }

            return pieces;
        }

        private static bool KingExists(List<Pieces> pieces)
        {
            foreach (Pieces piece in pieces)
            {
                if (piece.Name == "King")
                {
                    return true;
                }
            }
            return false;
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

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            this.NavigationService.Navigate(login);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings setting = new Settings(user);
            this.NavigationService.Navigate(setting);
        }

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
