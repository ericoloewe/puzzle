using System;

namespace puzzle_logic
{
    public class PuzzlePiece
    {
        public bool IsHide { get; }
        public int Number { get; set; }

        public PuzzlePiece(int number, bool isHide = false)
        {
            this.Number = number;
            this.IsHide = isHide;
        }
    }
}
