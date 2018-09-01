using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using puzzle_logic;

namespace web.Controllers
{
    public class PuzzleController
    {
        public static PuzzleController own = new PuzzleController();
        public Puzzle Current { get; private set; }

        public void BuildPuzzle()
        {
            Current.Build();
        }

        public void CreatePuzzle()
        {
            Current = new Puzzle();
        }

        public void ShufflePuzzle()
        {
            Current.Shuffle();
        }

        public PuzzlePiece[][] GetCurrentPuzzleColumns()
        {
            return Current.Columns.Select(c => c.ToArray()).ToArray();
        }
    }
}
