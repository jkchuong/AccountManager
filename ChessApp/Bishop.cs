using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp
{
    public class Bishop : Pieces, IPiece
    {
        public Bishop(bool isWhite, Cell position) : base(isWhite, position)
        {
            Name = "Bishop";
            PossibleMoves = Rulebook.BishopMoves;
            Position = position;
            Position.ChangeStatus();
        }
    }
}
