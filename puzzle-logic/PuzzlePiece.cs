using System;

namespace puzzle_logic
{
    public class PuzzlePiece
    {
        public bool IsHide { get; }
        public int Number { get; set; }
        public PiecePosition Position { get; set; }

        public PuzzlePiece(int number, PiecePosition position, bool isHide = false)
        {
            this.Number = number;
            this.Position = position;
            this.IsHide = isHide;
        }
    }
}
