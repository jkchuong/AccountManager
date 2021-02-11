using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp
{
    public class Cell
    {
        public bool IsOccupied { get; set; } = false;
        public bool IsLegal { get; set; } = false;
        public Pieces Piece { get; set; } = null;
        public int Row { get; set; }
        public int Column { get; set; }
        public string Name { get; set; }

        // Properties for storing data
        public string PieceName { get; set; }
        public bool PieceColour { get; set; }

        public Cell()
        {

        }

        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public (int x, int y) GetPoint()
        {
            return (Row, Column);
        }

        public void ChangeStatus()
        {
            IsOccupied = !IsOccupied;
        }

        public override string ToString()
        {
            return $"({Row}, {Column})";
        }
    }
}
