using System;
using System.Collections.Generic;

namespace puzzle_logic
{
    public class HardCodeBuilder
    {
        public Puzzle Puzzle { get; private set; }

        public HardCodeBuilder()
        {
            Puzzle = new Puzzle();
        }

        public async void Build(Action<PuzzlePiece[][]> onStateChange)
        {
            throw new NotImplementedException();
        }
    }
}
