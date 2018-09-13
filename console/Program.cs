using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using puzzle_logic;

namespace console
{
    class Program
    {
        static Stopwatch Stopwatch = new Stopwatch();
        static IPuzzleBuilder hardCodePuzzle = new HardCodeBuilder();
        static IPuzzleBuilder puzzleBuilder = new PuzzleBuilder();

        static void Main(string[] args)
        {
            Console.WriteLine("Main start");
            Stopwatch.Start();
            Console.WriteLine("Original puzzle: ");
            PrintPuzzle();
            RunPuzzleBuild().Wait();
            Console.WriteLine("Main end");
        }

        #region hard-code
        private static async Task RunHardCodePuzzleBuild()
        {
            hardCodePuzzle.Puzzle.Shuffle();
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
        #endregion

        private static async Task RunPuzzleBuild()
        {
            IList<IPuzzle> puzzleSolutionRevertPath = new List<IPuzzle>();

            puzzleBuilder.Puzzle.Shuffle();
            LogPuzzleStart(puzzleBuilder.Puzzle);

            try
            {
                puzzleSolutionRevertPath = await puzzleBuilder.Build(new PuzzleEvents()
                {
                    onStateChange = PrintPuzzle
                });
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Não foi encontrado nenhuma solução para o quebra cabeça");
            }

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

        static void LogPuzzleStart(IPuzzle puzzle)
        {
            Stopwatch.Start();
            Console.WriteLine("Start build");
        }

        static void PrintPuzzle()
        {
            PrintPuzzle(hardCodePuzzle.Puzzle);
        }

        static void PrintPuzzle(IPuzzle puzzle)
        {
            Console.WriteLine(puzzle.ToString());
            Console.WriteLine("=========================");
        }
    }
}
