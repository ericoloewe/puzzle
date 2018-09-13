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

        private IList<IPuzzle> StartToBuildPuzzleTree(PuzzleEvents events)
        {
            var parent = tree.Insert(Puzzle);
            var puzzleNodes = StartToBuildPuzzleTree(events, parent);

            if (!puzzleNodes?.Any() ?? false)
            {
                throw new Exception("There isn't a solution for this puzzle");
            }

            var bestPuzzleTree = tree.GetNodePathToRoot(puzzleNodes.FirstOrDefault());

            foreach (var node in puzzleNodes)
            {
                var nodeTree = tree.GetNodePathToRoot(node);

                // Console.WriteLine($"nodeTree.Count: {nodeTree.Count}");

                if (bestPuzzleTree.Count > nodeTree.Count)
                {
                    bestPuzzleTree = nodeTree;
                }
            }

            return bestPuzzleTree;
        }

        private IList<PuzzleTreeNode<IPuzzle>> StartToBuildPuzzleTree(PuzzleEvents events, PuzzleTreeNode<IPuzzle> parent)
        {
            var hasMoreItems = true;
            var openedParents = new Dictionary<string, PuzzleTreeNode<IPuzzle>>();
            var closedParents = new Dictionary<string, PuzzleTreeNode<IPuzzle>>();
            var puzzleChildRepeatControl = new Dictionary<string, PuzzleTreeNode<IPuzzle>>();
            var solutions = new Dictionary<string, PuzzleTreeNode<IPuzzle>>();
            var parentPuzzle = parent.Data;
            var parentPuzzleString = parentPuzzle.ToString();

            if (parentPuzzle.IsDone())
            {
                solutions[parentPuzzleString] = parent;
            }

            openedParents[parentPuzzleString] = parent;

            while (hasMoreItems)
            {
                foreach (var allowedMovement in parentPuzzle.AllowedMovements())
                {
                    var puzzleChild = (Puzzle)parentPuzzle.Clone();

                    puzzleChild.Move(allowedMovement);

                    var puzzleChildString = puzzleChild.ToString();
                    var isARepeatedPuzzle = puzzleChildRepeatControl.ContainsKey(puzzleChildString);
                    var childWasAParent = closedParents.ContainsKey(puzzleChildString);

                    if (!childWasAParent || !isARepeatedPuzzle)
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
                                solutions[puzzleChildString] = puzzleChildNode;
                            }

                            events.onStateChange.Invoke(puzzleChild);
                            puzzleChildRepeatControl[puzzleChildString] = puzzleChildNode;
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

                puzzleChildRepeatControl[parentPuzzleString] = parent;
                parent = openedParents.First().Value;
                parentPuzzle = parent.Data;
                parentPuzzleString = parentPuzzle.ToString();
            }

            return solutions.Values.ToList();
        }
    }
}
