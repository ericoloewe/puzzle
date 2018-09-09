using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace puzzle_logic
{
    public class HardCodeBuilder
    {
        public Puzzle Puzzle { get; private set; }
        private Thread buildThread;

        public HardCodeBuilder()
        {
            Puzzle = new Puzzle();
        }

        public async void Build(PuzzleEvents events)
        {
            buildThread = new Thread(() => StartToBuild(events));
            buildThread.Start();
        }

        private void StartToBuild(PuzzleEvents events)
        {
            events.onStart.Invoke(Puzzle.Rows);

            while (!Puzzle.IsDone())
            {
                var allowedMovements = Puzzle.AllowedMovements();

                foreach (var allowedMovement in allowedMovements)
                {
                    Console.WriteLine($"allowedMovement: {allowedMovement}");
                }

                break;
            }

            events.onFinish.Invoke(Puzzle.Rows);
        }
    }
}
