using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace puzzle_logic
{
    public class HardCodeRecursiveBuilder : IPuzzleBuilder
    {
        public IPuzzle Puzzle { get; private set; }
        private Task<IList<IPuzzle>> buildTask;
        private PuzzleTreeWithoutInfo<IPuzzle> tree;
        private IDictionary<string, IPuzzle> puzzleRepeatControl;

        public HardCodeRecursiveBuilder()
        {
            Puzzle = new Puzzle();
        }

        public async Task<IList<IPuzzle>> Build(PuzzleEvents events)
        {
            tree = new PuzzleTreeWithoutInfo<IPuzzle>();
            puzzleRepeatControl = new Dictionary<string, IPuzzle>();
            buildTask = new Task<IList<IPuzzle>>(() => StartToBuildPuzzleTree(events));
            buildTask.Start();
            buildTask.Wait();

            return buildTask.Result;
        }

        private IList<IPuzzle> StartToBuildPuzzleTree(PuzzleEvents events)
        {
            var parent = tree.Insert(Puzzle);
            var puzzleNode = StartToBuildPuzzleTree(events, parent);

            if (puzzleNode == null)
            {
                throw new Exception("There isn't a solution for this puzzle");
            }

            return tree.GetNodePathToRoot(puzzleNode);
        }

        private PuzzleTreeNode<IPuzzle> StartToBuildPuzzleTree(PuzzleEvents events, PuzzleTreeNode<IPuzzle> parent)
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
        private void AddToPuzzleRepeatedListIfNeed(IPuzzle puzzle)
        {
            var puzzleString = puzzle.ToString();

            if (!puzzleRepeatControl.ContainsKey(puzzleString))
            {
                puzzleRepeatControl.Add(puzzleString, puzzle);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private bool IsARepeatedPuzzle(IPuzzle puzzle) => puzzleRepeatControl.ContainsKey(puzzle.ToString());
    }
}
