using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using puzzle_logic;

namespace console
{
    class Program
    {
        static Stopwatch Stopwatch = new Stopwatch();
        static HardCodeBuilder hardCodePuzzle = new HardCodeBuilder();

        static void Main(string[] args)
        {
            Stopwatch.Start();
            Console.WriteLine("Original puzzle: ");
            PrintPuzzle();
            hardCodePuzzle.Puzzle.Shuffle();
            RunHardCodePuzzleBuild().Wait();
            Console.WriteLine("Main end");
        }

        private static async Task RunHardCodePuzzleBuild()
        {
            LogPuzzleStart(hardCodePuzzle.Puzzle);

            var puzzleSolutionRevertPath = await hardCodePuzzle.Build(new PuzzleEvents()
            {
                onStateChange = PrintPuzzle
            });

            LogPuzzleFinish();

            foreach (var puzzle in puzzleSolutionRevertPath.Reverse())
            {
                PrintPuzzle(puzzle);
            }
        }

        static void LogPuzzleFinish()
        {
            Stopwatch.Stop();
            Console.WriteLine($"Elapsed: {Stopwatch.Elapsed}");
        }

        static void LogPuzzleStart(Puzzle puzzle)
        {
            Stopwatch.Start();
            Console.WriteLine("Start build");
        }

        static void PrintPuzzle()
        {
            PrintPuzzle(hardCodePuzzle.Puzzle);
        }

        static void PrintPuzzle(Puzzle puzzle)
        {
            Console.WriteLine(puzzle.ToString());
            Console.WriteLine("=========================");
        }
    }
}
