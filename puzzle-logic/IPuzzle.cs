using System;
using System.Collections.Generic;

namespace puzzle_logic
{
    public interface IPuzzle : ICloneable
    {
        bool IsDone();
        IList<MovementType> AllowedMovements();
        int AmountOfPiecesOutOfOrder();
        int MovementsToFinishAllPieces();
        PuzzlePiece[][] Rows { get; }
        void Move(MovementType movement);
        void Shuffle();
    }
}