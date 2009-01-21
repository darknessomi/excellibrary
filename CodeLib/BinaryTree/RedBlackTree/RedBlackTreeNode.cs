using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeLib
{
    public enum NodeColor { Red, Black }

    public class RedBlackTreeNode<TItem> : BinaryTreeNodeBase<TItem, RedBlackTreeNode<TItem>>
    {
        public NodeColor Color;

        public RedBlackTreeNode()
        {
            Parent = Nil;
            Left = Nil;
            Right = Nil;
            Color = NodeColor.Black;
        }

        public static readonly RedBlackTreeNode<TItem> Nil = new RedBlackTreeNode<TItem>();
    }
}
