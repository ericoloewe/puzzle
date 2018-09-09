using System;

namespace puzzle_logic
{
    public class PiecePosition : ICloneable
    {
        public int Column { get; set; }
        public int Row { get; set; }

        public PiecePosition(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}