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
        private Business _account = new Business();
        private User user;

        Chessboard chessboard = new Chessboard();
        public ObservableCollection<Pieces> Piece { get; set; }
        Cell currentCell;

        public Game(User userPassed)
        {
            Piece = new ObservableCollection<Pieces>();
            user = userPassed;

            InitializeComponent();

            var userThemes = _account.GetUserTheme(user.UserId);
            Brush primaryColour = (SolidColorBrush)new BrushConverter().ConvertFromString(userThemes[0]);
            Brush secondaryColour = (SolidColorBrush)new BrushConverter().ConvertFromString(userThemes[1]);
            CreateButtonGrid(primaryColour, secondaryColour);


            WhiteHistory.Text = "White Moves";
            BlackHistory.Text = "Black Moves";

            NewGame();
            UpdateBoardState();

            string data = $"{user.Name}, Wins: {user.Wins}, Losses: {user.Losses}.";
            UserData.Text = data;
        }

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
            // Disable buttons

            // Check if users turn

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

                // Check if game over
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


                whitePieces = SearchForPieces(true);

                if (IsGameOver(whitePieces, blackPieces))
                {
                    NewGame();

                }

            }

            // If user wants to look for legal moves
            else if (clickedCell.IsOccupied)
            {
                chessboard.ClearMarkedLegalMoves();
                chessboard.FindLegalMoves(clickedCell.piece);
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

            if (!KingExists(whitePieces))
            {
                WhiteHistory.Text = "White Loses!";
                return true;
            }
            if (!KingExists(blackPieces))
            {
                BlackHistory.Text = "Black Loses!";
                return true;
            }
            return false;
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
                WhiteHistory.Text += message;
            }
            else
            {
                BlackHistory.Text += message;
            }
        }

        private void MovePiece(Cell beforeCell, Cell afterCell)
        {
            // move piece to new cell and set that cell to be occupied
            afterCell.piece = beforeCell.piece;
            afterCell.IsOccupied = true;
            beforeCell.piece.Position = afterCell;

            // remove piece from old cell and set it to be not occupied
            beforeCell.piece = null;
            beforeCell.IsOccupied = false;

            chessboard.ClearMarkedLegalMoves();
        }

        // If occupied add name, else clear content
        // if legal change colour to yellow, else change it back to original colour
        private void UpdateBoardState()
        {
            foreach (Button button in Chessboard.Children)
            {
                Cell cell = button.Tag as Cell;

                if (cell.IsOccupied)
                {
                    button.Content = ChessApp.Rulebook.ConvertPieceToInitial(cell.piece);
                }
                else
                {
                    button.Content = "";
                }

                if (cell.IsLegal)
                {
                    button.Background = Brushes.Yellow;
                }
                else
                {
                    if (ChessApp.Rulebook.BlackCells.Contains(button.Name))
                    {
                        button.Background = Brushes.Black;
                    }
                    else
                    {
                        button.Background = Brushes.White;
                    }
                }
            }
        }

        private void NewGame()
        {

            chessboard.ClearBoard();

            Pawn whitePawn1 = new Pawn(true, chessboard.Board[6, 0]);
            Pawn whitePawn2 = new Pawn(true, chessboard.Board[6, 1]);
            Pawn whitePawn3 = new Pawn(true, chessboard.Board[6, 2]);
            Pawn whitePawn4 = new Pawn(true, chessboard.Board[6, 3]);
            Pawn whitePawn5 = new Pawn(true, chessboard.Board[6, 4]);
            Pawn whitePawn6 = new Pawn(true, chessboard.Board[6, 5]);
            Pawn whitePawn7 = new Pawn(true, chessboard.Board[6, 6]);
            Pawn whitePawn8 = new Pawn(true, chessboard.Board[6, 7]);

            Rook whiteRook1 = new Rook(true, chessboard.Board[7, 0]);
            Knight whiteKnight1 = new Knight(true, chessboard.Board[7, 1]);
            Bishop whiteBishop1 = new Bishop(true, chessboard.Board[7, 2]);
            Queen whiteQueen = new Queen(true, chessboard.Board[7, 3]);
            King whiteKing = new King(true, chessboard.Board[7, 4]);
            Bishop whiteBishop2 = new Bishop(true, chessboard.Board[7, 5]);
            Knight whiteKnight2 = new Knight(true, chessboard.Board[7, 6]);
            Rook whiteRook2 = new Rook(true, chessboard.Board[7, 7]);

            Pawn blackPawn1 = new Pawn(false, chessboard.Board[1, 0]);
            Pawn blackPawn2 = new Pawn(false, chessboard.Board[1, 1]);
            Pawn blackPawn3 = new Pawn(false, chessboard.Board[1, 2]);
            Pawn blackPawn4 = new Pawn(false, chessboard.Board[1, 3]);
            Pawn blackPawn5 = new Pawn(false, chessboard.Board[1, 4]);
            Pawn blackPawn6 = new Pawn(false, chessboard.Board[1, 5]);
            Pawn blackPawn7 = new Pawn(false, chessboard.Board[1, 6]);
            Pawn blackPawn8 = new Pawn(false, chessboard.Board[1, 7]);

            Rook blackRook1 = new Rook(false, chessboard.Board[0, 0]);
            Knight blackKnight1 = new Knight(false, chessboard.Board[0, 1]);
            Bishop blackBishop1 = new Bishop(false, chessboard.Board[0, 2]);
            Queen blackQueen = new Queen(false, chessboard.Board[0, 3]);
            King blackKing = new King(false, chessboard.Board[0, 4]);
            Bishop blackBishop2 = new Bishop(false, chessboard.Board[0, 5]);
            Knight blackKnight2 = new Knight(false, chessboard.Board[0, 6]);
            Rook blackRook2 = new Rook(false, chessboard.Board[0, 7]);
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

        private bool KingExists(List<Pieces> pieces)
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


        // Use images instead of letters?
        public string ImageSource(Pieces piece)
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
