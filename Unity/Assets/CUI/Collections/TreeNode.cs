using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUI.Collections
{
    [Serializable]
    public class TreeNode<T>
    {
        public T Value { get; private set; }

        public TreeNode<T> Parent { get; private set; }

        public int Depth
        {
            get
            {
                int depth = 0;
                TreeNode<T> node = this;
                while (node.Parent != null)
                {
                    node = node.Parent;
                    depth++;
                }
                return depth;
            }
        }

        public TreeNode<T> Root
        {
            get
            {
                TreeNode<T> node = this;
                while (node.Parent != null)
                {
                    node = node.Parent;
                }
                return node;
            }
        }


        public TreeNode<T>[] Ancestors
        {
            get
            {
                List<TreeNode<T>> ancestors = new List<TreeNode<T>>();
                TreeNode<T> node = this;
                while (node.Parent != null)
                {
                    ancestors.Add(node.Parent);
                    node = node.Parent;
                }
                return ancestors.ToArray();
            }
        }

        public List<TreeNode<T>> Children { get; private set; }

        public bool AddChild(TreeNode<T> child)
        {
            if (Children.Contains(child))
            {
                return false;
            }
            Children.Add(child);
            child.Parent = this;
            return true;
        }
        public bool AddChild(TreeNode<T> child, int index)
        {
            if (Children.Contains(child))
            {
                return false;
            }
            Children.Add(child);
            Children.Insert(index, child);
            child.Parent = this;
            return true;
        }

        public int Height
        {
            get
            {
                return GetHeight(this, 0);
            }
        }
        private int GetHeight(TreeNode<T> node, int height)
        {
            int currentHeight = height;
            if (node.Children != null)
            {
                foreach (var item in node.Children)
                {
                    currentHeight = Mathf.Max(GetHeight(item, height + 1), currentHeight);
                }
            }
            return currentHeight;
        }

        public TreeNode<T>[] Descendants
        {
            get
            {
                return GetDescendants(this);
            }
        }

        private TreeNode<T>[] GetDescendants(TreeNode<T> node)
        {
            List<TreeNode<T>> ancestors = new List<TreeNode<T>>();
            if (node.Children != null)
            {
                foreach (var item in node.Children)
                {
                    ancestors.Add(item);
                    ancestors.AddRange(GetDescendants(item));
                }
            }
            return ancestors.ToArray();
        }


        public TreeNode()
        {

        }
        public TreeNode(T value)
        {
            Value = value;
        }
        public TreeNode(T _value, TreeNode<T> _parent)
        {
            Value = _value;
            Parent = _parent;
            _parent.AddChild(this);
        }
    }
}