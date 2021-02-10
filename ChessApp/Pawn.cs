using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp
{
    public class Pawn : Pieces, IPiece
    {
        public Pawn(bool isWhite, Cell position) : base(isWhite, position)
        {
            Name = "Pawn";

            if (isWhite)
            {
                Name = "Pawn";
                PossibleMoves = Rulebook.WhitePawnMoves;
            }
            else
            {
                PossibleMoves = Rulebook.BlackPawnMoves;
            }

            Position = position;
            Position.ChangeStatus();
        }
    }
}
