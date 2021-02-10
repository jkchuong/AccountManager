using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp
{
    // Redundant Class
    public interface IPiece
    {
        public bool IsWhite { get; set; }
        public bool IsFirstMove { get; set; }
        public Cell Position { get; set; }
        public string Name { get; set; }

        public List<Move> PossibleMoves { get; set; }
        public List<Move> LegalMoves { get; set; }

        public abstract string GetPosition();
        public abstract string GetPossibleMoves();
        public abstract string GetLegalMoves();
    }
}
