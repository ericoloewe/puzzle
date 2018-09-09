using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            events.onStart.Invoke(Puzzle);

            var parent = tree.Insert(Puzzle);
            var puzzle = StartToBuildPuzzleTree(events, parent);

            if (puzzle == null)
            {
                throw new Exception("There isn't a solution for this puzzle");
            }

            events.onFinish.Invoke(puzzle.Data);
        }

        private TreeNode<Puzzle> StartToBuildPuzzleTree(PuzzleEvents events, TreeNode<Puzzle> parent)
        {
            var parentPuzzle = parent.Data;

            if (IsARepeatedPuzzle(parentPuzzle))
            {
                return null;
            }

            AddPuzzleRepeatedList(parentPuzzle);

            foreach (var allowedMovement in parentPuzzle.AllowedMovements())
            {
                var puzzleChild = (Puzzle)parentPuzzle.Clone();

                Console.WriteLine($"allowedMovement: {allowedMovement}");
                puzzleChild.Move(allowedMovement);

                if (!IsARepeatedPuzzle(puzzleChild))
                {
                    var puzzleChildNode = tree.Insert(puzzleChild, parent);

                    if (puzzleChild.IsDone())
                    {
                        return puzzleChildNode;
                    }

                    events.onStateChange.Invoke(puzzleChild);
                    var childrenResolution = StartToBuildPuzzleTree(events, puzzleChildNode);

                    if (childrenResolution != null)
                    {
                        return childrenResolution;
                    }
                }
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddPuzzleRepeatedList(Puzzle puzzle) => puzzleRepeatControl.Add(puzzle.ToString(), puzzle);

        [MethodImpl(MethodImplOptions.Synchronized)]
        private bool IsARepeatedPuzzle(Puzzle puzzle) => puzzleRepeatControl.ContainsKey(puzzle.ToString());
    }
}
