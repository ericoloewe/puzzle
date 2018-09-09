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
            var puzzle = StartToBuildPuzzleTreeNonRecursive(events, parent);

            if (puzzle == null)
            {
                throw new Exception("There isn't a solution for this puzzle");
            }

            events.onFinish.Invoke(puzzle.Data);
        }

        #region recursive
        private TreeNode<Puzzle> StartToBuildPuzzleTree(PuzzleEvents events, TreeNode<Puzzle> parent)
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
        #endregion

        private TreeNode<Puzzle> StartToBuildPuzzleTreeNonRecursive(PuzzleEvents events, TreeNode<Puzzle> parent)
        {
            TreeNode<Puzzle> nodeSolution = null;
            var hasMoreItems = true;
            var foundSolution = false;
            var openedParents = new Dictionary<string, TreeNode<Puzzle>>();
            var closedParents = new Dictionary<string, TreeNode<Puzzle>>();
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
