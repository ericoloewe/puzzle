using System;

namespace puzzle_logic
{
    public class PuzzlePiece : ICloneable
    {
        public bool IsHide { get; }
        public int Number { get; }
        public PiecePosition Position { get; set; }

        public PuzzlePiece(int number, PiecePosition position, bool isHide = false)
        {
            this.Number = number;
            this.Position = position;
            this.IsHide = isHide;
        }

        public object Clone()
        {
            var newPiece = (PuzzlePiece)this.MemberwiseClone();

            newPiece.Position = (PiecePosition)this.Position.Clone();

            return newPiece;
        }
    }
}
