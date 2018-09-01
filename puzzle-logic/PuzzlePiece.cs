using System;

namespace puzzle_logic
{
    public class PuzzlePiece
    {
        public bool IsHide { get; }
        public int Column { get; set; }
        public int Number { get; set; }
        public int Row { get; set; }

        public PuzzlePiece(int number, int column, int row, bool isHide = false)
        {
            this.Number = number;
            this.Column = column;
            this.Row = row;
            this.IsHide = isHide;
        }
    }
}
