using System;
using System.Linq;
using puzzle_logic;

namespace console
{
    class Program
    {
        static HardCodeBuilder hardCodePuzzle = new HardCodeBuilder();

        static void Main(string[] args)
        {
            Console.WriteLine("Original puzzle: ");
            PrintPuzzle();
            hardCodePuzzle.Puzzle.Shuffle();
            Console.WriteLine("Shuffled puzzle: ");
            PrintPuzzle();
            hardCodePuzzle.Build(c => PrintPuzzle(c));
        }

        static void PrintPuzzle()
        {
            PrintPuzzle(hardCodePuzzle.Puzzle.Columns);
        }

        static void PrintPuzzle(PuzzlePiece[][] columns)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                for (int j = 0; j < columns[i].Length; j++)
                {
                    Console.WriteLine($"i: {i}; j: {j}; piece: {columns[i][j].Number}; ");
                }

                Console.Write("\n");
            }

            Console.WriteLine("=========================");
        }
    }
}
