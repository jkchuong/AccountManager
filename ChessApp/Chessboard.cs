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

        // Create grid of Cells
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

        // Set all cells IsLegal property to false
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
                cell.Piece = null;
            }
        }

        // Check if K is in check, if so next move must move piece out of check (i.e check if K is on same cell as legal move)
        // Check if K is in mate, if so end the game (check all K legal and occupied positions are in opponents legal move)
        // Castling, en passant?
        // Promotion, when pawn reaches either side of the board, give choice on which piece to choose (B R Q N)

        public void FindLegalMoves(Pieces piece)
        {
            if (piece.Name == "Pawn")
            {
                CheckPawnLegals(piece);
            }

            else if (piece.Name == "King" || piece.Name == "Knight")
            {
                CheckKnightLegals(piece);
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

        private void CheckKnightLegals(Pieces piece)
        {
            foreach (Move move in piece.PossibleMoves)
            {
                int desitinationRow = piece.Position.Row + move.MoveRow;
                int desitnationColumn = piece.Position.Column + move.MoveColumn;

                // Can move anywhere as long as it is of opposite colour or unoccupied
                if (IsOnBoard(desitinationRow, desitnationColumn))
                {
                    if (Board[desitinationRow, desitnationColumn].Piece == null)
                    {
                        Board[desitinationRow, desitnationColumn].IsLegal = true;
                    }
                    else
                    {
                        if (IsOppositeColour(piece.IsWhite, Board[desitinationRow, desitnationColumn].Piece.IsWhite))
                        {
                            Board[desitinationRow, desitnationColumn].IsLegal = true;
                        }
                    }
                }
            }
        }

        private void CheckPawnLegals(Pieces piece)
        {
            foreach (Move move in piece.PossibleMoves)
            {
                int desitinationRow = piece.Position.Row + move.MoveRow;
                int destinationColumn = piece.Position.Column + move.MoveColumn;

                if (IsOnBoard(desitinationRow, destinationColumn))
                {
                    // move diagonally only if it's occupied by opposite colour
                    if (desitinationRow != piece.Position.Row && destinationColumn != piece.Position.Column)
                    {
                        if (Board[desitinationRow, destinationColumn].IsOccupied
                            && IsOppositeColour(piece.IsWhite, Board[desitinationRow, destinationColumn].Piece.IsWhite))
                        {
                            Board[desitinationRow, destinationColumn].IsLegal = true;
                        }
                    }

                    // Moving forward
                    else
                    {
                        // if unobstructed is legal
                        if (!Board[desitinationRow, destinationColumn].IsOccupied)
                        {
                            Board[desitinationRow, destinationColumn].IsLegal = true;

                            // if first move, can move two steps
                            if (piece.IsFirstMove)
                            {
                                Board[desitinationRow + move.MoveRow, destinationColumn + move.MoveColumn].IsLegal = true;
                            }
                        }
                    }
                }
            }
        }

        // Use for... continue loop?
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
                    if (Board[desitinationRow, piece.Position.Column].Piece == null)
                    {
                        Board[desitinationRow, piece.Position.Column].IsLegal = true;
                    }
                    else 
                    {
                        if (IsOppositeColour(piece.IsWhite, Board[desitinationRow, piece.Position.Column].Piece.IsWhite))
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
                    if (Board[desitinationRow, piece.Position.Column].Piece == null)
                    {
                        Board[desitinationRow, piece.Position.Column].IsLegal = true;
                    }
                    else 
                    {
                        if (IsOppositeColour(piece.IsWhite, Board[desitinationRow, piece.Position.Column].Piece.IsWhite))
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
                    if (Board[piece.Position.Row, destinationColumn].Piece == null)
                    {
                        Board[piece.Position.Row, destinationColumn].IsLegal = true;
                    }
                    else 
                    {
                        if (IsOppositeColour(piece.IsWhite, Board[piece.Position.Row, destinationColumn].Piece.IsWhite))
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
                    if (Board[piece.Position.Row, destinationColumn].Piece == null)
                    {
                        Board[piece.Position.Row, destinationColumn].IsLegal = true;
                    }
                    else 
                    {
                        if (IsOppositeColour(piece.IsWhite, Board[piece.Position.Row, destinationColumn].Piece.IsWhite))
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
            // backward right movement
            bool Unobstructed = true;
            int movement = 1;
            do
            {
                int desitinationRow = piece.Position.Row + movement;
                int destinationColumn = piece.Position.Column + movement;
                if (IsOnBoard(desitinationRow, destinationColumn))
                {
                    if (Board[desitinationRow, destinationColumn].Piece == null)
                    {
                        Board[desitinationRow, destinationColumn].IsLegal = true;
                    }
                    else 
                    {
                        if (IsOppositeColour(piece.IsWhite, Board[desitinationRow, destinationColumn].Piece.IsWhite))
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

            // forward right movement
            Unobstructed = true;
            movement = 1;
            do
            {
                int desitinationRow = piece.Position.Row - movement;
                int destinationColumn = piece.Position.Column + movement;

                if (IsOnBoard(desitinationRow, destinationColumn))
                {
                    if (Board[desitinationRow, destinationColumn].Piece == null)
                    {
                        Board[desitinationRow, destinationColumn].IsLegal = true;
                    }
                    else 
                    {
                        if (IsOppositeColour(piece.IsWhite, Board[desitinationRow, destinationColumn].Piece.IsWhite))
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

            // backward left movement
            Unobstructed = true;
            movement = 1;
            do
            {
                int desitinationRow = piece.Position.Row + movement;
                int destinationColumn = piece.Position.Column - movement;

                if (IsOnBoard(desitinationRow, destinationColumn))
                {
                    if (Board[desitinationRow, destinationColumn].Piece == null)
                    {
                        Board[desitinationRow, destinationColumn].IsLegal = true;
                    }
                    else 
                    {
                        if (IsOppositeColour(piece.IsWhite, Board[desitinationRow, destinationColumn].Piece.IsWhite))
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

            // forward left movement
            Unobstructed = true;
            movement = 1;
            do
            {
                int desitinationRow = piece.Position.Row - movement;
                int destinationColumn = piece.Position.Column - movement;

                if (IsOnBoard(desitinationRow, destinationColumn))
                {
                    if (Board[desitinationRow, destinationColumn].Piece == null)
                    {
                        Board[desitinationRow, destinationColumn].IsLegal = true;
                    }
                    else 
                    {
                        if (IsOppositeColour(piece.IsWhite, Board[desitinationRow, destinationColumn].Piece.IsWhite))
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

        // Check if piece has legal moves
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

        public bool KingExists(List<Pieces> pieces)
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

        public void MovePiece(Cell beforeCell, Cell afterCell)
        {
            // first move to false if first move
            if (beforeCell.Piece.IsFirstMove)
            {
                beforeCell.Piece.IsFirstMove = false;
            }

            // move piece to new cell and set that cell to be occupied
            afterCell.Piece = beforeCell.Piece;
            afterCell.IsOccupied = true;
            beforeCell.Piece.Position = afterCell;

            // remove piece from old cell and set it to be not occupied
            beforeCell.Piece = null;
            beforeCell.IsOccupied = false;

            ClearMarkedLegalMoves();
        }

        public void Promotion(Pieces piece, string newPiece)
        {
            switch (newPiece)
            {
                case "Rook":
                    piece.Position.Piece = new Rook(piece.IsWhite, piece.Position);
                    break;

                case "Bishop":
                    piece.Position.Piece = new Bishop(piece.IsWhite, piece.Position);
                    break;

                case "Knight":
                    piece.Position.Piece = new Knight(piece.IsWhite, piece.Position);
                    break;

                case "Queen":
                    piece.Position.Piece = new Queen(piece.IsWhite, piece.Position);
                    break;
            }

            // Prevent ChangeStatus() from occuring
            piece.Position.IsOccupied = true;
        }

        // Get list of all pieces of one colour on board
        public List<Pieces> SearchForPieces(bool isWhite)
        {
            List<Pieces> pieces = new List<Pieces>();

            foreach (Cell cell in Board)
            {
                if (cell.IsOccupied)
                {
                    if (cell.Piece.IsWhite == isWhite)
                    {
                        pieces.Add(cell.Piece);
                    }
                }
            }

            return pieces;
        }

        public void NewGame()
        {
            ClearBoard();

            _ = new Pawn(true, Board[6, 0]);
            _ = new Pawn(true, Board[6, 1]);
            _ = new Pawn(true, Board[6, 2]);
            _ = new Pawn(true, Board[6, 3]);
            _ = new Pawn(true, Board[6, 4]);
            _ = new Pawn(true, Board[6, 5]);
            _ = new Pawn(true, Board[6, 6]);
            _ = new Pawn(true, Board[6, 7]);

            _ = new Rook(true, Board[7, 0]);
            _ = new Knight(true, Board[7, 1]);
            _ = new Bishop(true, Board[7, 2]);
            _ = new Queen(true, Board[7, 3]);
            _ = new King(true, Board[7, 4]);
            _ = new Bishop(true, Board[7, 5]);
            _ = new Knight(true, Board[7, 6]);
            _ = new Rook(true, Board[7, 7]);

            _ = new Pawn(false, Board[1, 0]);
            _ = new Pawn(false, Board[1, 1]);
            _ = new Pawn(false, Board[1, 2]);
            _ = new Pawn(false, Board[1, 3]);
            _ = new Pawn(false, Board[1, 4]);
            _ = new Pawn(false, Board[1, 5]);
            _ = new Pawn(false, Board[1, 6]);
            _ = new Pawn(false, Board[1, 7]);

            _ = new Rook(false, Board[0, 0]);
            _ = new Knight(false, Board[0, 1]);
            _ = new Bishop(false, Board[0, 2]);
            _ = new Queen(false, Board[0, 3]);
            _ = new King(false, Board[0, 4]);
            _ = new Bishop(false, Board[0, 5]);
            _ = new Knight(false, Board[0, 6]);
            _ = new Rook(false, Board[0, 7]);

        }

        public void AddPieceToBoard(string pieceName, int pieceRow, int pieceCol, bool isWhite, bool isFirstMove)
        {
            switch (pieceName)
            {
                case "Pawn":
                    _ = new Pawn(isWhite, Board[pieceRow, pieceCol]) { IsFirstMove = isFirstMove };
                    break;
                case "Knight":
                    _ = new Knight(isWhite, Board[pieceRow, pieceCol]) { IsFirstMove = isFirstMove };
                    break;
                case "King":
                    _ = new King(isWhite, Board[pieceRow, pieceCol]) { IsFirstMove = isFirstMove };
                    break;
                case "Rook":
                    _ = new Rook(isWhite, Board[pieceRow, pieceCol]) { IsFirstMove = isFirstMove };
                    break;
                case "Bishop":
                    _ = new Bishop(isWhite, Board[pieceRow, pieceCol]) { IsFirstMove = isFirstMove };
                    break;
                case "Queen":
                    _ = new Queen(isWhite, Board[pieceRow, pieceCol]) { IsFirstMove = isFirstMove };
                    break;
            }
        }

    }
}
