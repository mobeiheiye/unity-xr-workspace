using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUI;

namespace CUI.Collections
{
    public interface ITreeNode
    {
        public ITreeNode Parent { get; set; }
        public int Depth { get; }
        public ITreeNode Root { get; }
        public ITreeNode[] Ancestors { get; }

        public ITreeNode[] Children { get; set; }
        public int Height { get; }
        public ITreeNode[] Descendants { get; }
    }
    public interface ITreeNode<T>
    {
        public T Value { get; set; }

        public ITreeNode<T> Parent { get; set; }
        public int Depth { get; }
        public ITreeNode<T> Root { get; }
        public ITreeNode<T>[] Ancestors { get; }

        public ITreeNode<T>[] Children { get; set; }
        public int Height { get; }
        public ITreeNode<T>[] Descendants { get; }
    }
}