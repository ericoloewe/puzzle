using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace puzzle_logic
{
    public class PuzzleBuilder : IPuzzleBuilder
    {
        public IPuzzle Puzzle { get; private set; }
        private Task<IList<IPuzzle>> buildTask;
        private PuzzleTreeWithInfo tree;

        public PuzzleBuilder()
        {
            Puzzle = new Puzzle();
        }

        public async Task<IList<IPuzzle>> Build(PuzzleEvents events)
        {
            tree = new PuzzleTreeWithInfo();
            buildTask = new Task<IList<IPuzzle>>(() => StartToBuildPuzzleTree(events));
            buildTask.Start();
            buildTask.Wait();

            return buildTask.Result;
        }

        private PuzzleTreeNode<IPuzzle> GetBestNodeByHeuristic(Dictionary<string, PuzzleTreeNode<IPuzzle>> puzzles)
        {
            var min = puzzles.Values.Min(p => p.HeuristicValue);

            return puzzles.Values.FirstOrDefault(p => p.HeuristicValue.Equals(min));
        }

        private IList<IPuzzle> StartToBuildPuzzleTree(PuzzleEvents events)
        {
            var parent = tree.Insert(Puzzle);
            var puzzleNode = StartToBuildPuzzleTree(events, parent);

            if (puzzleNode == null)
            {
                throw new ArgumentException("There isn't a solution for this puzzle");
            }

            return tree.GetNodePathToRoot(puzzleNode);
        }

        private PuzzleTreeNode<IPuzzle> StartToBuildPuzzleTree(PuzzleEvents events, PuzzleTreeNode<IPuzzle> parent)
        {
            PuzzleTreeNode<IPuzzle> solution = null;
            var hasMoreItems = true;
            var foundSolution = false;
            var openedParents = new Dictionary<string, PuzzleTreeNode<IPuzzle>>();
            var closedParents = new Dictionary<string, PuzzleTreeNode<IPuzzle>>();
            var childRepeatControl = new Dictionary<string, PuzzleTreeNode<IPuzzle>>();
            var parentPuzzle = parent.Data;
            var parentPuzzleString = parentPuzzle.ToString();

            if (parentPuzzle.IsDone())
            {
                solution = parent;

                return solution;
            }

            openedParents[parentPuzzleString] = parent;

            while (hasMoreItems && !foundSolution)
            {
                foreach (var allowedMovement in parentPuzzle.AllowedMovements())
                {
                    var puzzleChild = (Puzzle)parentPuzzle.Clone();

                    puzzleChild.Move(allowedMovement);

                    var puzzleChildString = puzzleChild.ToString();
                    var isARepeatedPuzzle = childRepeatControl.ContainsKey(puzzleChildString);
                    var childWasAParent = closedParents.ContainsKey(puzzleChildString);
                    var childWillBeAParent = openedParents.ContainsKey(puzzleChildString);

                    if (!childWasAParent || !childWillBeAParent || !isARepeatedPuzzle)
                    {
                        var puzzleChildNode = tree.Insert(puzzleChild, parent);

                        if (!childWasAParent)
                        {
                            openedParents[puzzleChildString] = puzzleChildNode;
                        }

                        if (!isARepeatedPuzzle)
                        {
                            if (puzzleChild.IsDone())
                            {
                                solution = puzzleChildNode;
                                foundSolution = true;

                                return puzzleChildNode;
                            }

                            events.onStateChange.Invoke(puzzleChild);
                            childRepeatControl[puzzleChildString] = puzzleChildNode;
                        }
                    }
                }

                openedParents.Remove(parentPuzzleString);
                closedParents[parentPuzzleString] = parent;

                if (!openedParents.Any())
                {
                    hasMoreItems = false;
                    break;
                }

                childRepeatControl[parentPuzzleString] = parent;
                parent = GetBestNodeByHeuristic(openedParents);
                parentPuzzle = parent.Data;
                parentPuzzleString = parentPuzzle.ToString();
            }

            return solution;
        }
    }
}
