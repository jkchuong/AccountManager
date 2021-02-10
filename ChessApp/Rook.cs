using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp
{
    public class Rook : Pieces, IPiece
    {
        public Rook(bool isWhite, Cell position) : base(isWhite, position)
        {
            Name = "Rook";
            PossibleMoves = Rulebook.RookMoves;
            Position = position;
            Position.ChangeStatus();
        }
    }
}
