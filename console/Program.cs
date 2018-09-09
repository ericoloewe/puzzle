using System;
using System.Diagnostics;
using System.Linq;
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
            hardCodePuzzle.Build(new PuzzleEvents()
            {
                onFinish = LogPuzzleFinish,
                onStart = LogPuzzleStart,
                onStateChange = PrintPuzzle
            });
        }

        static void LogPuzzleFinish(PuzzlePiece[][] rows)
        {
            Stopwatch.Stop();
            Console.WriteLine($"Elapsed: {Stopwatch.Elapsed}");
        }

        static void LogPuzzleStart(PuzzlePiece[][] obj)
        {
            Stopwatch.Start();
            Console.WriteLine("Start build");
            Console.WriteLine("Shuffled puzzle: ");
            PrintPuzzle();
        }

        static void PrintPuzzle()
        {
            PrintPuzzle(hardCodePuzzle.Puzzle.Rows);
        }

        static void PrintPuzzle(PuzzlePiece[][] rows)
        {
            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < rows[i].Length; j++)
                {
                    Console.Write($"{{i: {i}, j: {j}, piece: {rows[i][j].Number}}} ");
                }

                Console.Write("\n");
            }

            Console.WriteLine("=========================");
        }
    }
}
