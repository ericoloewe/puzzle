using System;

namespace puzzle_logic
{
    public class PuzzleEvents
    {
        public Action<Puzzle> onFinish { get; set; }
        public Action<Puzzle> onStart { get; set; }
        public Action<Puzzle> onStateChange { get; set; }
    }
}
