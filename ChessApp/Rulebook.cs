using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp
{
    public static class Rulebook
    {
        public static string ConvertPieceToInitial(Pieces piece)
        {
            string pieceInitial = "";
            switch (piece.Name)
            {
                case "Pawn":
                    pieceInitial = "P";
                    break;
                case "Knight":
                    pieceInitial = "N";
                    break;
                case "King":
                    pieceInitial = "K";
                    break;
                case "Bishop":
                    pieceInitial = "B";
                    break;
                case "Rook":
                    pieceInitial = "R";
                    break;
                case "Queen":
                    pieceInitial = "Q";
                    break;
            }

            if (!piece.IsWhite)
            {
                pieceInitial = pieceInitial.ToLower();
            }

            return pieceInitial;
        }

        // Cells that are the secondary colour
        public static readonly List<string> BlackCells = new List<string>
        {
            "b8", "d8", "f8", "h8",
            "a7", "c7", "e7", "g7",
            "b6", "d6", "f6", "h6",
            "a5", "c5", "e5", "g5",
            "b4", "d4", "f4", "h4",
            "a3", "c3", "e3", "g3",
            "b2", "d2", "f2", "h2",
            "a1", "c1", "e1", "g1"
        };

        // Column in Array to Column Cell
        public static readonly Dictionary<int, int> ArrayToCellColumn = new Dictionary<int, int>
        {
            {0, 8 },
            {1, 7 },
            {2, 6 },
            {3, 5 },
            {4, 4 },
            {5, 3 },
            {6, 2 },
            {7, 1 }
        };

        // Row in Array to Row Cell
        public static readonly Dictionary<int, char> ArrayToCellRow = new Dictionary<int, char>
        {
            {0, 'a' },
            {1, 'b' },
            {2, 'c' },
            {3, 'd' },
            {4, 'e' },
            {5, 'f' },
            {6, 'g' },
            {7, 'h' }
        };

        // Can be condensed? Make one for each direction and have it be multiplied?
        public static readonly List<Move> WhitePawnMoves = new List<Move>
        {
            new Move(-1, 0),
            new Move(-1, 1),
            new Move(-1, -1)
        };

        public static readonly List<Move> BlackPawnMoves = new List<Move>
        {
            new Move(1, 0),
            new Move(1, 1),
            new Move(1, -1)
        };

        public static readonly List<Move> KnightMoves = new List<Move>
        {
            new Move(2, 1),
            new Move(1, 2),

            new Move(2, -1),
            new Move(-1, 2),

            new Move(-2, 1),
            new Move(1, -2),

            new Move(-2, -1),
            new Move(-1, -2)
        };

        public static readonly List<Move> KingMoves = new List<Move>
        {
            new Move(1, 0),
            new Move(1, 1),
            new Move(1, -1),

            new Move(-1, 0),
            new Move(-1, 1),
            new Move(-1, -1),

            new Move(0, -1),
            new Move(0, 1)
        };


        // Rook Bishop and Queen move list are redundant
        public static readonly List<Move> RookMoves = new List<Move>
        {
            new Move(1, 0),
            new Move(2, 0),
            new Move(3, 0),
            new Move(4, 0),
            new Move(5, 0),
            new Move(6, 0),
            new Move(7, 0),

            new Move(-1, 0),
            new Move(-2, 0),
            new Move(-3, 0),
            new Move(-4, 0),
            new Move(-5, 0),
            new Move(-6, 0),
            new Move(-7, 0),

            new Move(0, 1),
            new Move(0, 2),
            new Move(0, 3),
            new Move(0, 4),
            new Move(0, 5),
            new Move(0, 6),
            new Move(0, 7),

            new Move(0, -1),
            new Move(0, -2),
            new Move(0, -3),
            new Move(0, -4),
            new Move(0, -5),
            new Move(0, -6),
            new Move(0, -7),
        };

        public static readonly List<Move> BishopMoves = new List<Move>
        {
            new Move(1, 1),
            new Move(2, 2),
            new Move(3, 3),
            new Move(4, 4),
            new Move(5, 5),
            new Move(6, 6),
            new Move(7, 7),

            new Move(-1, -1),
            new Move(-2, -2),
            new Move(-3, -3),
            new Move(-4, -4),
            new Move(-5, -5),
            new Move(-6, -6),
            new Move(-7, -7),

            new Move(-1, 1),
            new Move(-2, 2),
            new Move(-3, 3),
            new Move(-4, 4),
            new Move(-5, 5),
            new Move(-6, 6),
            new Move(-7, 7),

            new Move(1, -1),
            new Move(2, -2),
            new Move(3, -3),
            new Move(4, -4),
            new Move(5, -5),
            new Move(6, -6),
            new Move(7, -7),
        };

        public static readonly List<Move> QueenMoves = new List<Move>
        {
            // Rook Moves
            new Move(1, 0),
            new Move(2, 0),
            new Move(3, 0),
            new Move(4, 0),
            new Move(5, 0),
            new Move(6, 0),
            new Move(7, 0),

            new Move(-1, 0),
            new Move(-2, 0),
            new Move(-3, 0),
            new Move(-4, 0),
            new Move(-5, 0),
            new Move(-6, 0),
            new Move(-7, 0),

            new Move(0, 1),
            new Move(0, 2),
            new Move(0, 3),
            new Move(0, 4),
            new Move(0, 5),
            new Move(0, 6),
            new Move(0, 7),

            new Move(0, -1),
            new Move(0, -2),
            new Move(0, -3),
            new Move(0, -4),
            new Move(0, -5),
            new Move(0, -6),
            new Move(0, -7),

            //Bishop Moves
            new Move(1, 1),
            new Move(2, 2),
            new Move(3, 3),
            new Move(4, 4),
            new Move(5, 5),
            new Move(6, 6),
            new Move(7, 7),

            new Move(-1, -1),
            new Move(-2, -2),
            new Move(-3, -3),
            new Move(-4, -4),
            new Move(-5, -5),
            new Move(-6, -6),
            new Move(-7, -7),

            new Move(-1, 1),
            new Move(-2, 2),
            new Move(-3, 3),
            new Move(-4, 4),
            new Move(-5, 5),
            new Move(-6, 6),
            new Move(-7, 7),

            new Move(1, -1),
            new Move(2, -2),
            new Move(3, -3),
            new Move(4, -4),
            new Move(5, -5),
            new Move(6, -6),
            new Move(7, -7),
        };

    }
}
