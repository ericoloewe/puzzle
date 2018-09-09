using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace puzzle_logic
{
    public class Tree<T>
    {
        public TreeNode<T> Root { get; set; }

        public TreeNode<T> Insert(T data)
        {
            return Root = new TreeNode<T>(data);
        }

        public TreeNode<T> Insert(T data, TreeNode<T> parent)
        {
            var newNode = new TreeNode<T>(data);

            newNode.Parent = parent;
            parent.Children.Add(newNode);

            return newNode;
        }
    }

    public class TreeNode<T>
    {
        public IList<TreeNode<T>> Children { get; set; }
        public T Data { get; set; }
        public TreeNode<T> Parent { get; set; }

        public TreeNode(T data)
        {
            this.Data = data;
            this.Children = new List<TreeNode<T>>();
        }
    }
}
