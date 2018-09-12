using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace puzzle_logic
{
    #region puzzle-tree-without-info
    public class PuzzleTreeWithoutInfo<T>
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

        public IList<T> GetNodePathToRoot(PuzzleTreeNode<T> puzzleNode)
        {
            IList<T> pathToRoot = new List<T>();

            do
            {
                pathToRoot.Add(puzzleNode.Data);
                puzzleNode = puzzleNode.Parent;
            } while (puzzleNode != null);

            return pathToRoot;
        }
    }
    #endregion

    #region puzzle-tree-with-info
    public class PuzzleTreeWithInfo
    {
        public PuzzleTreeNode<IPuzzle> Root { get; private set; }

        public PuzzleTreeNode<IPuzzle> Insert(IPuzzle data)
        {
            Root = new PuzzleTreeNode<IPuzzle>(data);

            AddInfoTo(Root);

            return Root;
        }

        public PuzzleTreeNode<IPuzzle> Insert(IPuzzle data, PuzzleTreeNode<IPuzzle> parent)
        {
            var newNode = new PuzzleTreeNode<IPuzzle>(data);

            newNode.Parent = parent;
            parent.Children.Add(newNode);

            return newNode;
        }

        public IList<IPuzzle> GetNodePathToRoot(PuzzleTreeNode<IPuzzle> puzzleNode)
        {
            IList<IPuzzle> pathToRoot = new List<IPuzzle>();

            do
            {
                pathToRoot.Add(puzzleNode.Data);
                puzzleNode = puzzleNode.Parent;
            } while (puzzleNode != null);

            return pathToRoot;
        }

        private void AddInfoTo(PuzzleTreeNode<IPuzzle> node)
        {
        }
    }
    #endregion

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
