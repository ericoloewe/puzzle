using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using puzzle_logic;

namespace web.Controllers
{
    public class PuzzleController
    {
        public static PuzzleController own = new PuzzleController();
        public Stopwatch Stopwatch { get; private set; }
        public HardCodeBuilder HardCodePuzzle { get; private set; }

        public PuzzleController()
        {
            HardCodePuzzle = new HardCodeBuilder();
            Stopwatch = new Stopwatch();
        }

        public async Task<IList<IPuzzle>> BuildPuzzle(Action<IPuzzle> onChange)
        {
            return await HardCodePuzzle.Build(new PuzzleEvents()
            {
                onStateChange = onChange
            });
        }

        public void ShufflePuzzle()
        {
            HardCodePuzzle.Puzzle.Shuffle();
        }
    }
}
