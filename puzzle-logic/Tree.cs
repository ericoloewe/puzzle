using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace puzzle_logic
{
    public class PuzzleTree<T>
    {
        public PuzzleTreeNode<T> Root { get; set; }

        public PuzzleTreeNode<T> Insert(T data)
        {
            return Root = new PuzzleTreeNode<T>(data);
        }

        public PuzzleTreeNode<T> Insert(T data, PuzzleTreeNode<T> parent)
        {
            var newNode = new PuzzleTreeNode<T>(data);

            newNode.Parent = parent;
            parent.Children.Add(newNode);

            return newNode;
        }

        public IList<Puzzle> GetNodePathToRoot(PuzzleTreeNode<Puzzle> puzzleNode)
        {
            IList<Puzzle> pathToRoot = new List<Puzzle>();

            do
            {
                pathToRoot.Add(puzzleNode.Data);
                puzzleNode = puzzleNode.Parent;
            } while (puzzleNode != null);

            return pathToRoot;
        }
    }

    public class PuzzleTreeNode<T>
    {
        public IList<PuzzleTreeNode<T>> Children { get; set; }
        public T Data { get; set; }
        public PuzzleTreeNode<T> Parent { get; set; }

        public PuzzleTreeNode(T data)
        {
            this.Data = data;
            this.Children = new List<PuzzleTreeNode<T>>();
        }
    }
}
