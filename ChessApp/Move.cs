using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp
{
    public class Move
    {
        public int MoveRow { get; set; }
        public int MoveColumn { get; set; }

        public Move(int moveX, int moveY)
        {
            MoveRow = moveX;
            MoveColumn = moveY;
        }

        public override string ToString()
        {
            return $"({MoveRow}, {MoveColumn})";
        }
    }
}
