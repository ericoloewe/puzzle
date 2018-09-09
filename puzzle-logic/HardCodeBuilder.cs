using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace puzzle_logic
{
    public class HardCodeBuilder
    {
        public Puzzle Puzzle { get; private set; }
        private Task buildThread;
        private Tree<Puzzle> tree;
        private IDictionary<string, Puzzle> puzzleRepeatControl;

        public HardCodeBuilder()
        {
            Puzzle = new Puzzle();
        }

        public async Task Build(PuzzleEvents events)
        {
            tree = new Tree<Puzzle>();
            puzzleRepeatControl = new Dictionary<string, Puzzle>();
            buildThread = new Task(() => StartToBuildPuzzleTree(events));
            buildThread.Start();
            buildThread.Wait();
        }

        private void StartToBuildPuzzleTree(PuzzleEvents events)
        {
            events.onStart.Invoke(Puzzle.Rows);

            var parent = tree.Insert(Puzzle);
            var puzzle = StartToBuildPuzzleTree(events, parent);

            events.onFinish.Invoke(Puzzle.Rows);
        }

        private TreeNode<Puzzle> StartToBuildPuzzleTree(PuzzleEvents events, TreeNode<Puzzle> parent)
        {
            var parentPuzzle = parent.Data;

            if (IsARepeatedPuzzle(parentPuzzle))
            {
                return null;
            }

            puzzleRepeatControl.Add(parentPuzzle.ToString(), parentPuzzle);

            foreach (var allowedMovement in parentPuzzle.AllowedMovements())
            {
                var puzzleChild = (Puzzle)parentPuzzle.Clone();

                Console.WriteLine($"allowedMovement: {allowedMovement}");
                puzzleChild.Move(allowedMovement);

                var puzzleNode = tree.Insert(puzzleChild, parent);

                if (puzzleChild.IsDone())
                {
                    return puzzleNode;
                }

                events.onStateChange.Invoke(puzzleChild.Rows);
                var childrenResolution = StartToBuildPuzzleTree(events, puzzleNode);

                if (childrenResolution != null)
                {
                    return childrenResolution;
                }
            }

            return null;
        }

        private bool IsARepeatedPuzzle(Puzzle puzzle) => puzzleRepeatControl.Keys.Contains(puzzle.ToString());
    }
}
