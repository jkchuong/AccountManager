using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp
{
    public class Chessboard
    {

        public Cell[,] Board { get; }

        public Chessboard()
        {
            Board = new Cell[8, 8];
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Board[x, y] = new Cell(x, y);
                }
            }
        }

        public bool IsOnBoard(int row, int column)
        {
            if (row >= 8 || column >= 8 || row < 0 || column < 0)
            {
                return false;
            }
            return true;
        }

        public bool IsOppositeColour(bool isWhite, bool otherPieceWhite)
        {
            if((isWhite && otherPieceWhite) || (!isWhite && !otherPieceWhite))
            {
                return false;
            }
            return true;
        }

        public void ClearMarkedLegalMoves()
        {
            foreach (Cell cell in Board)
            {
                cell.IsLegal = false;
            }
        }

        public void ClearBoard()
        {
            foreach (Cell cell in Board)
            {
                cell.IsLegal = false;
                cell.IsOccupied = false;
                cell.piece = null;
            }
        }

        // Can make this shorter? (DRY)
        // Create method to check each of these and reuse?
        // Check if K is in check, if so next move must move piece out of check (i.e check if K is on same cell as legal move)
        // Check if K is in mate, if so end the game (check all K legal and occupied positions are in opponents legal move)
        // Castling, en passant?
        // Promotion, when pawn reaches other side of the board, give choice on which piece to choose (B R Q N)

        public void FindLegalMoves(Pieces piece)
        {
            if (piece.Name == "Pawn")
            {
                foreach (Move move in piece.PossibleMoves)
                {
                    int desitinationRow = piece.Position.Row + move.MoveRow;
                    int desitnationColumn = piece.Position.Column + move.MoveColumn;

                    if (IsOnBoard(desitinationRow, desitnationColumn))
                    {
                        // move 2 only if first move and empty or occupied by opposite colour and it's the first move
                        if (Math.Abs(move.MoveRow) == 2)
                        {
                            if (piece.IsFirstMove &&
                                (Board[desitinationRow, desitnationColumn].piece == null
                                || IsOppositeColour(piece.IsWhite, Board[desitinationRow, desitnationColumn].piece.IsWhite)))
                            {
                                Board[desitinationRow, desitnationColumn].IsLegal = true;
                                piece.IsFirstMove = false;
                            }
                        }

                        // move diagonally only if it's occupied by opposite colour
                        else if (desitinationRow != piece.Position.Row && desitnationColumn != piece.Position.Column)
                        {
                            if (Board[desitinationRow, desitnationColumn].IsOccupied
                                && IsOppositeColour(piece.IsWhite, Board[desitinationRow, desitnationColumn].piece.IsWhite))
                            {
                                Board[desitinationRow, desitnationColumn].IsLegal = true;
                            }
                        }

                        // Moving forward by 1
                        else
                        {
                            if (!Board[desitinationRow, desitnationColumn].IsOccupied)
                            {
                                Board[desitinationRow, desitnationColumn].IsLegal = true;
                            }
                        }
                    }
                }
            }

            else if (piece.Name == "King" || piece.Name == "Knight")
            {
                foreach (Move move in piece.PossibleMoves)
                {
                    int desitinationRow = piece.Position.Row + move.MoveRow;
                    int desitnationColumn = piece.Position.Column + move.MoveColumn;
                    if (IsOnBoard(desitinationRow, desitnationColumn))
                    {
                        if (Board[desitinationRow, desitnationColumn].piece == null)
                        {
                            Board[desitinationRow, desitnationColumn].IsLegal = true;
                        }
                        else
                        {
                            if (IsOppositeColour(piece.IsWhite, Board[desitinationRow, desitnationColumn].piece.IsWhite))
                            {
                                Board[desitinationRow, desitnationColumn].IsLegal = true;
                            }
                        }
                    }
                }
            }
            
            else if (piece.Name == "Rook")
            {
                CheckRookObstruction(piece);
            }

            else if (piece.Name == "Bishop")
            {
                CheckBishopObstruction(piece);
            }

            else if (piece.Name == "Queen")
            {
                CheckRookObstruction(piece);
                CheckBishopObstruction(piece);
            }
        }

        public void NewGame()
        {
            Pawn whitePawn1 = new Pawn(true, Board[6, 0]);
            Pawn whitePawn2 = new Pawn(true, Board[6, 1]);
            Pawn whitePawn3 = new Pawn(true, Board[6, 2]);
            Pawn whitePawn4 = new Pawn(true, Board[6, 3]);
            Pawn whitePawn5 = new Pawn(true, Board[6, 4]);
            Pawn whitePawn6 = new Pawn(true, Board[6, 5]);
            Pawn whitePawn7 = new Pawn(true, Board[6, 6]);
            Pawn whitePawn8 = new Pawn(true, Board[6, 7]);

            Rook whiteRook1 = new Rook(true, Board[7, 0]);
            Knight whiteKnight1 = new Knight(true, Board[7, 1]);
            Bishop whiteBishop1 = new Bishop(true, Board[7, 2]);
            Queen whiteQueen = new Queen(true, Board[7, 3]);
            King whiteKing = new King(true, Board[7, 4]);
            Bishop whiteBishop2 = new Bishop(true, Board[7, 5]);
            Knight whiteKnight2 = new Knight(true, Board[7, 6]);
            Rook whiteRook2 = new Rook(true, Board[7, 7]);

            Pawn blackPawn1 = new Pawn(false, Board[1, 0]);
            Pawn blackPawn2 = new Pawn(false, Board[1, 1]);
            Pawn blackPawn3 = new Pawn(false, Board[1, 2]);
            Pawn blackPawn4 = new Pawn(false, Board[1, 3]);
            Pawn blackPawn5 = new Pawn(false, Board[1, 4]);
            Pawn blackPawn6 = new Pawn(false, Board[1, 5]);
            Pawn blackPawn7 = new Pawn(false, Board[1, 6]);
            Pawn blackPawn8 = new Pawn(false, Board[1, 7]);

            Rook blackRook1 = new Rook(false, Board[0, 0]);
            Knight blackKnight1 = new Knight(false, Board[0, 1]);
            Bishop blackBishop1 = new Bishop(false, Board[0, 2]);
            Queen blackQueen = new Queen(false, Board[0, 3]);
            King blackKing = new King(false, Board[0, 4]);
            Bishop blackBishop2 = new Bishop(false, Board[0, 5]);
            Knight blackKnight2 = new Knight(false, Board[0, 6]);
            Rook blackRook2 = new Rook(false, Board[0, 7]);

        }

        public void MovePiece(Pieces piece, Cell cell)
        {
            ClearMarkedLegalMoves();
            FindLegalMoves(piece);

            if (cell.IsLegal)
            {
                piece.Position.ChangeStatus();
                piece.Position = cell;
                cell.piece = piece;

                if (!cell.IsOccupied)
                {
                    cell.ChangeStatus();
                }
            }
            
        }

        public void CheckRookObstruction(Pieces piece)
        {
            // backward movement
            bool Unobstructed = true;
            int movement = 1;
            do
            {
                int desitinationRow = piece.Position.Row + movement;
                if (IsOnBoard(desitinationRow, piece.Position.Column))
                {
                    if (Board[desitinationRow, piece.Position.Column].piece == null)
                    {
                        Board[desitinationRow, piece.Position.Column].IsLegal = true;
                    }
                    else 
                    {
                        if (IsOppositeColour(piece.IsWhite, Board[desitinationRow, piece.Position.Column].piece.IsWhite))
                        {
                            Board[desitinationRow, piece.Position.Column].IsLegal = true;
                        }
                        Unobstructed = false;
                    }
                    ++movement;
                }
                else
                {
                    Unobstructed = false;
                }
            }
            while (Unobstructed);

            // forward movement
            Unobstructed = true;
            movement = 1;
            do
            {
                int desitinationRow = piece.Position.Row - movement;
                if (IsOnBoard(desitinationRow, piece.Position.Column))
                {
                    if (Board[desitinationRow, piece.Position.Column].piece == null)
                    {
                        Board[desitinationRow, piece.Position.Column].IsLegal = true;
                    }
                    else 
                    {
                        if (IsOppositeColour(piece.IsWhite, Board[desitinationRow, piece.Position.Column].piece.IsWhite))
                        {
                            Board[desitinationRow, piece.Position.Column].IsLegal = true;
                        }
                        Unobstructed = false;
                    }
                    ++movement;
                }
                else
                {
                    Unobstructed = false;
                }
            }
            while (Unobstructed);

            // left movement
            Unobstructed = true;
            movement = 1;
            do
            {
                int destinationColumn = piece.Position.Column - movement;
                if (IsOnBoard(piece.Position.Row, destinationColumn))
                {
                    if (Board[piece.Position.Row, destinationColumn].piece == null)
                    {
                        Board[piece.Position.Row, destinationColumn].IsLegal = true;
                    }
                    else 
                    {
                        if (IsOppositeColour(piece.IsWhite, Board[piece.Position.Row, destinationColumn].piece.IsWhite))
                        {
                            Board[piece.Position.Row, destinationColumn].IsLegal = true;
                        }
                        Unobstructed = false;
                    }
                    ++movement;
                }
                else
                {
                    Unobstructed = false;
                }
            }
            while (Unobstructed);

            // right movement
            Unobstructed = true;
            movement = 1;
            do
            {
                int destinationColumn = piece.Position.Column + movement;
                if (IsOnBoard(piece.Position.Row, destinationColumn))
                {
                    if (Board[piece.Position.Row, destinationColumn].piece == null)
                    {
                        Board[piece.Position.Row, destinationColumn].IsLegal = true;
                    }
                    else 
                    {
                        if (IsOppositeColour(piece.IsWhite, Board[piece.Position.Row, destinationColumn].piece.IsWhite))
                        {
                            Board[piece.Position.Row, destinationColumn].IsLegal = true;
                        }
                        Unobstructed = false;
                    }
                    ++movement;
                }
                else
                {
                    Unobstructed = false;
                }
            }
            while (Unobstructed);
        }

        private void CheckBishopObstruction(Pieces piece)
        {
            // backward movement
            bool Unobstructed = true;
            int movement = 1;
            do
            {
                int desitinationRow = piece.Position.Row + movement;
                int destinationColumn = piece.Position.Column + movement;
                if (IsOnBoard(desitinationRow, destinationColumn))
                {
                    if (Board[desitinationRow, destinationColumn].piece == null)
                    {
                        Board[desitinationRow, destinationColumn].IsLegal = true;
                    }
                    else 
                    {
                        if (IsOppositeColour(piece.IsWhite, Board[desitinationRow, destinationColumn].piece.IsWhite))
                        {
                            Board[desitinationRow, destinationColumn].IsLegal = true;
                        }
                        Unobstructed = false;
                    }
                    ++movement;
                }
                else
                {
                    Unobstructed = false;
                }
            }
            while (Unobstructed);

            // forward movement
            Unobstructed = true;
            movement = 1;
            do
            {
                int desitinationRow = piece.Position.Row - movement;
                int destinationColumn = piece.Position.Column + movement;

                if (IsOnBoard(desitinationRow, destinationColumn))
                {
                    if (Board[desitinationRow, destinationColumn].piece == null)
                    {
                        Board[desitinationRow, destinationColumn].IsLegal = true;
                    }
                    else 
                    {
                        if (IsOppositeColour(piece.IsWhite, Board[desitinationRow, destinationColumn].piece.IsWhite))
                        {
                            Board[desitinationRow, destinationColumn].IsLegal = true;
                        }
                        Unobstructed = false;
                    }
                    ++movement;
                }
                else
                {
                    Unobstructed = false;
                }
            }
            while (Unobstructed);

            // left movement
            Unobstructed = true;
            movement = 1;
            do
            {
                int desitinationRow = piece.Position.Row + movement;
                int destinationColumn = piece.Position.Column - movement;

                if (IsOnBoard(desitinationRow, destinationColumn))
                {
                    if (Board[desitinationRow, destinationColumn].piece == null)
                    {
                        Board[desitinationRow, destinationColumn].IsLegal = true;
                    }
                    else 
                    {
                        if (IsOppositeColour(piece.IsWhite, Board[desitinationRow, destinationColumn].piece.IsWhite))
                        {
                            Board[desitinationRow, destinationColumn].IsLegal = true;
                        }
                        Unobstructed = false;
                    }
                    ++movement;
                }
                else
                {
                    Unobstructed = false;
                }
            }
            while (Unobstructed);

            // right movement
            Unobstructed = true;
            movement = 1;
            do
            {
                int desitinationRow = piece.Position.Row - movement;
                int destinationColumn = piece.Position.Column - movement;

                if (IsOnBoard(desitinationRow, destinationColumn))
                {
                    if (Board[desitinationRow, destinationColumn].piece == null)
                    {
                        Board[desitinationRow, destinationColumn].IsLegal = true;
                    }
                    else 
                    {
                        if (IsOppositeColour(piece.IsWhite, Board[desitinationRow, destinationColumn].piece.IsWhite))
                        {
                            Board[desitinationRow, destinationColumn].IsLegal = true;
                        }
                        Unobstructed = false;
                    }
                    ++movement;
                }
                else
                {
                    Unobstructed = false;
                }
            }
            while (Unobstructed);
        }

        public bool HasLegalMove(Pieces piece)
        {
            FindLegalMoves(piece);
            foreach (Cell cell in Board)
            {
                if (cell.IsLegal)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
