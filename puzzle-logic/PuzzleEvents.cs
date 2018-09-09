using System;

namespace puzzle_logic
{
    public class PuzzleEvents
    {
        public Action<PuzzlePiece[][]> onFinish { get; set; }
        public Action<PuzzlePiece[][]> onStart { get; set; }
        public Action<PuzzlePiece[][]> onStateChange { get; set; }
    }
}
