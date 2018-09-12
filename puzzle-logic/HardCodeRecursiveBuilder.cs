using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace puzzle_logic
{
    public class HardCodeRecursiveBuilder : IPuzzleBuilder
    {
        public Puzzle Puzzle { get; private set; }
        private Task<IList<Puzzle>> buildTask;
        private PuzzleTree<Puzzle> tree;
        private IDictionary<string, Puzzle> puzzleRepeatControl;

        public HardCodeRecursiveBuilder()
        {
            Puzzle = new Puzzle();
        }

        public async Task<IList<Puzzle>> Build(PuzzleEvents events)
        {
            tree = new PuzzleTree<Puzzle>();
            puzzleRepeatControl = new Dictionary<string, Puzzle>();
            buildTask = new Task<IList<Puzzle>>(() => StartToBuildPuzzleTree(events));
            buildTask.Start();
            buildTask.Wait();

            return buildTask.Result;
        }

        private IList<Puzzle> StartToBuildPuzzleTree(PuzzleEvents events)
        {
            var parent = tree.Insert(Puzzle);
            var puzzleNode = StartToBuildPuzzleTree(events, parent);

            if (puzzleNode == null)
            {
                throw new Exception("There isn't a solution for this puzzle");
            }

            return tree.GetNodePathToRoot(puzzleNode);
        }

        private PuzzleTreeNode<Puzzle> StartToBuildPuzzleTree(PuzzleEvents events, PuzzleTreeNode<Puzzle> parent)
        {
            var parentPuzzle = parent.Data;

            if (IsARepeatedPuzzle(parentPuzzle))
            {
                return null;
            }

            AddToPuzzleRepeatedListIfNeed(parentPuzzle);

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
        private void AddToPuzzleRepeatedListIfNeed(Puzzle puzzle)
        {
            var puzzleString = puzzle.ToString();

            if (!puzzleRepeatControl.ContainsKey(puzzleString))
            {
                puzzleRepeatControl.Add(puzzleString, puzzle);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private bool IsARepeatedPuzzle(Puzzle puzzle) => puzzleRepeatControl.ContainsKey(puzzle.ToString());
    }
}
