using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using puzzle_logic;

namespace web.Controllers
{
    public class PuzzleController
    {
        public Puzzle Current { get; private set; }
        public PuzzlePiece[][] CurrentPuzzleColumns { get { return Current.Columns; } }
        public static PuzzleController own = new PuzzleController();

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
    }
}
