using System;
using System.Collections.Generic;

namespace ChessApp
{
    class Program
    {
        Chessboard chessboard = new Chessboard();

        static void Main(string[] args)
        {
            Chessboard chessboard = new Chessboard();

            // IPiece interface? Wanna be able to get it all in a list.
            Pawn whitePawn = new Pawn(true, chessboard.Board[4, 3]);
            Pawn blackPawn = new Pawn(false, chessboard.Board[6, 0]);
            Knight blackKnight = new Knight(false, chessboard.Board[2, 2]);
            King whiteKing = new King(false, chessboard.Board[7, 3]);
            Rook whiteRook = new Rook(true, chessboard.Board[3, 6]);
            Bishop blackBishop = new Bishop(false, chessboard.Board[2, 4]);
            Queen whiteQueen = new Queen(true, chessboard.Board[1, 5]);

            List<Pieces> pieces = new List<Pieces>
            {
                whitePawn,
                blackPawn,
                blackKnight,
                whiteKing,
                whiteRook,
                blackBishop,
                whiteQueen
            };

            Console.WriteLine("The Pieces");
            chessboard.ClearMarkedLegalMoves();
            PrintBoardOccupiedAndLegal(chessboard);

            foreach(Pieces piece in pieces)
            {
                Console.WriteLine("=====================================================");
                Console.WriteLine(piece.Name);
                chessboard.ClearMarkedLegalMoves();
                chessboard.FindLegalMoves(piece);
                PrintBoardOccupiedAndLegal(chessboard);
            }

            Console.WriteLine("=====================================================");

            Console.WriteLine("Before Move");
            chessboard.MovePiece(whiteQueen, chessboard.Board[2, 4]);
            chessboard.ClearMarkedLegalMoves();
            PrintBoardOccupiedAndLegal(chessboard);

            Console.WriteLine("=====================================================");

            Console.WriteLine("Clear Board");
            chessboard.ClearBoard();
            PrintBoardOccupiedAndLegal(chessboard);

            Console.WriteLine("=====================================================");

            Console.WriteLine("New Game");
            //chessboard.NewGame();
            PrintBoardOccupiedAndLegal(chessboard);

            Console.WriteLine("=====================================================");

            List<Pieces> blackPieces = SearchForPieces(chessboard, false);
            List<Pieces> whitePieces = SearchForPieces(chessboard, true);
            

        }

        private static void PrintBoardOccupiedAndLegal(Chessboard chessboard)
        {
            int rowLength = chessboard.Board.GetLength(0);
            int colLength = chessboard.Board.GetLength(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    if (chessboard.Board[i, j].IsOccupied)
                    {
                        if (chessboard.Board[i, j].IsLegal)
                        {
                            Console.Write("X    ");
                        }
                        else
                        {
                            switch (chessboard.Board[i, j].piece.Name)
                            {
                                case "Pawn":
                                    if(chessboard.Board[i, j].piece.IsWhite)
                                    Console.Write("P    ");
                                    else
                                    {
                                        Console.WriteLine("p    ");
                                    }
                                    break;

                                case "Knight":
                                    if (chessboard.Board[i, j].piece.IsWhite)
                                        Console.Write("N   ");
                                    else
                                    {
                                        Console.WriteLine("n    ");
                                    }
                                    break;

                                case "King":
                                    if (chessboard.Board[i, j].piece.IsWhite)
                                        Console.Write("K    ");
                                    else
                                    {
                                        Console.WriteLine("k    ");
                                    }
                                    break;

                                case "Queen":
                                    if (chessboard.Board[i, j].piece.IsWhite)
                                        Console.Write("Q    ");
                                    else
                                    {
                                        Console.WriteLine("q    ");
                                    }
                                    break;

                                case "Rook":
                                    if (chessboard.Board[i, j].piece.IsWhite)
                                        Console.Write("R    ");
                                    else
                                    {
                                        Console.WriteLine("r    ");
                                    }
                                    break;

                                case "Bishop":
                                    if (chessboard.Board[i, j].piece.IsWhite)
                                        Console.Write("B    ");
                                    else
                                    {
                                        Console.WriteLine("b    ");
                                    }
                                    break;
                            }
                        }
                    }

                    else if (chessboard.Board[i, j].IsLegal)
                    {
                        Console.Write("X    ");
                    }
                    else
                    {
                        Console.Write("-    ");
                    }
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
        }

        private static List<Pieces> SearchForPieces(Chessboard chessboard, bool isWhite)
        {
            List<Pieces> pieces = new List<Pieces>();

            foreach (Cell cell in chessboard.Board)
            {
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

    }
}
