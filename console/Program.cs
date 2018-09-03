using System;
using System.Linq;
using puzzle_logic;

namespace console
{
    class Program
    {
        static Puzzle puzzle = new Puzzle();

        static void Main(string[] args)
        {
            PrintPuzzle();
            puzzle.Shuffle();
            PrintPuzzle();
        }

        static void PrintPuzzle()
        {
            var columns = puzzle.Columns;

            for (int i = 0; i < columns.Length; i++)
            {
                for (int j = 0; j < columns[i].Length; j++)
                {
                    Console.WriteLine($"i: {i}; j: {j}; piece: {columns[i][j].Number};");
                }
            }

            Console.WriteLine("=========================");
        }
    }
}
