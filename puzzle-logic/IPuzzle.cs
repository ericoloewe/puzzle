using System;
using System.Collections.Generic;

namespace puzzle_logic
{
    public interface IPuzzle : ICloneable
    {
        PuzzlePiece[][] Rows { get; }
        IList<MovementType> AllowedMovements();
        bool IsDone();
        void Move(MovementType movement);
        void Shuffle();
    }
}