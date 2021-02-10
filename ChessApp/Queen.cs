using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp
{
    public class Queen : Pieces, IPiece
    {
        public Queen(bool isWhite, Cell position) : base(isWhite, position)
        {
            Name = "Queen";
            PossibleMoves = Rulebook.QueenMoves;
            Position = position;
            Position.ChangeStatus();
        }
    }
}
