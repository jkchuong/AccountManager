using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp
{
    public class Knight : Pieces, IPiece
    {
        public Knight(bool isWhite, Cell position) : base(isWhite, position)
        {
            Name = "Knight";
            PossibleMoves = Rulebook.KnightMoves;
            Position = position;
            Position.ChangeStatus();
        }
    }
}
