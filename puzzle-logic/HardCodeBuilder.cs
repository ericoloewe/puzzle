using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace puzzle_logic
{
    public class HardCodeBuilder : IPuzzleBuilder
    {
        public IPuzzle Puzzle { get; private set; }
        private Task<IList<IPuzzle>> buildTask;
        private PuzzleTreeWithoutInfo<IPuzzle> tree;
        private IDictionary<string, IPuzzle> puzzleRepeatControl;

        public HardCodeBuilder()
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
            PuzzleTreeNode<IPuzzle> nodeSolution = null;
            var hasMoreItems = true;
            var foundSolution = false;
            var openedParents = new Dictionary<string, PuzzleTreeNode<IPuzzle>>();
            var closedParents = new Dictionary<string, PuzzleTreeNode<IPuzzle>>();
            var parentPuzzle = parent.Data;

            if (parentPuzzle.IsDone())
            {
                foundSolution = true;
                return parent;
            }

            openedParents.Add(parentPuzzle.ToString(), parent);

            while (hasMoreItems && !foundSolution)
            {
                foreach (var allowedMovement in parentPuzzle.AllowedMovements())
                {
                    var puzzleChild = (Puzzle)parentPuzzle.Clone();

                    puzzleChild.Move(allowedMovement);

                    if (!IsARepeatedPuzzle(puzzleChild))
                    {
                        var puzzleChildNode = tree.Insert(puzzleChild, parent);

                        if (puzzleChild.IsDone())
                        {
                            foundSolution = true;
                            return puzzleChildNode;
                        }

                        events.onStateChange.Invoke(puzzleChild);
                        openedParents.Add(puzzleChild.ToString(), puzzleChildNode);
                        AddToPuzzleRepeatedListIfNeed(puzzleChild);
                    }
                }

                var parentPuzzleString = parentPuzzle.ToString();

                openedParents.Remove(parentPuzzleString);

                if (!closedParents.ContainsKey(parentPuzzleString))
                {
                    closedParents.Add(parentPuzzleString, parent);
                }

                if (openedParents.Count == 0)
                {
                    hasMoreItems = false;
                    break;
                }

                AddToPuzzleRepeatedListIfNeed(parentPuzzle);
                parent = openedParents.First().Value;
                parentPuzzle = parent.Data;
            }

            return nodeSolution;
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
        private bool IsARepeatedPuzzle(Puzzle puzzle) => puzzleRepeatControl.ContainsKey(puzzle.ToString());
    }
}
