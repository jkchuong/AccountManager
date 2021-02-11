using System;
using System.Collections.Generic;

namespace ChessApp
{
    class Program
    {
        Chessboard chessboard = new Chessboard();

        static void Main(string[] args)
        {

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
