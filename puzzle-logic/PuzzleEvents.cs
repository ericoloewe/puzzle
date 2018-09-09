using System;

namespace puzzle_logic
{
    public class PuzzleEvents
    {
        public Action<Puzzle> onStateChange { get; set; }
    }
}
