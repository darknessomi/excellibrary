using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeLib
{
    ///<summary>
    ///A red-black tree must satisfy these properties:
    ///1. The root is black. 
    ///2. All leaves are black. 
    ///3. Red nodes can only have black children. 
    ///4. All paths from a node to its leaves contain the same number of black nodes.
    ///</summary>
    public class RedBlackTree<TItem> : BinarySearchTreeBase<TItem, RedBlackTreeNode<TItem>>
        where TItem : IComparable<TItem>
    {
        public override RedBlackTreeNode<TItem> Nil
        {
            get { return RedBlackTreeNode<TItem>.Nil; }
        }

        public override void Insert(RedBlackTreeNode<TItem> node)
        {
            base.Insert(node);
            node.Color = NodeColor.Red;
            FixupAfterInsert(node);
        }

        /// <summary>
        /// restores the red-black properties to the search tree
        /// </summary>
        /// <param name="node"></param>
        private void FixupAfterInsert(RedBlackTreeNode<TItem> node)
        {
            while (node.Parent.Color == NodeColor.Red)
            {
                if (node.Parent.IsLeftChild)
                {
                    RedBlackTreeNode<TItem> uncle = node.Parent.Parent.Right;
                    // case 1
                    if (uncle.Color == NodeColor.Red)
                    {
                        uncle.Color = NodeColor.Black;
                        node.Parent.Color = NodeColor.Black;
                        node.Parent.Parent.Color = NodeColor.Red;
                        node = node.Parent.Parent;
                    }
                    else
                    {
                        // case 2
                        if (node.IsRightChild)
                        {
                            node = node.Parent;
                            RotateLeft(node);
                        }
                        // case 3
                        node.Parent.Color = NodeColor.Black;
                        node.Parent.Parent.Color = NodeColor.Red;
                        RotateRight(node.Parent.Parent);
                    }
                }
                else // "right" and "left" exchanged
                {
                    RedBlackTreeNode<TItem> uncle = node.Parent.Parent.Left;
                    // case 1
                    if (uncle.Color == NodeColor.Red)
                    {
                        uncle.Color = NodeColor.Black;
                        node.Parent.Color = NodeColor.Black;
                        node.Parent.Parent.Color = NodeColor.Red;
                        node = node.Parent.Parent;
                    }
                    else
                    {
                        // case 2
                        if (node.IsLeftChild)
                        {
                            node = node.Parent;
                            RotateRight(node);
                        }
                        // case 3
                        node.Parent.Color = NodeColor.Black;
                        node.Parent.Parent.Color = NodeColor.Red;
                        RotateLeft(node.Parent.Parent);
                    }
                }
            }
            Root.Color = NodeColor.Black;
        }

        public override RedBlackTreeNode<TItem> Delete(RedBlackTreeNode<TItem> node)
        {
            RedBlackTreeNode<TItem> deletedNode = base.Delete(node);

            RedBlackTreeNode<TItem> child = Nil;

            RedBlackTreeNode<TItem> parent = deletedNode.Parent;
            if (parent != Nil)
            {
                child = deletedNode.IsLeftChild ? parent.Left : parent.Right;
            }

            if (deletedNode.Color == NodeColor.Black)
            {
                FixupAfterDelete(child);
            }
            return deletedNode;
        }


        /// <summary>
        /// restores the red-black properties to the search tree
        /// </summary>
        /// <param name="node"></param>
        private void FixupAfterDelete(RedBlackTreeNode<TItem> node)
        {
            while (node != Root && node.Color == NodeColor.Black)
            {
                if (node.IsLeftChild)
                {
                    RedBlackTreeNode<TItem> sibling = node.Parent.Right;
                    if (sibling.Color == NodeColor.Red)
                    {
                        sibling.Color = NodeColor.Black;
                        node.Parent.Color = NodeColor.Red;
                        RotateLeft(node.Parent);
                        sibling = node.Parent.Right;
                    }
                    if (sibling.Left.Color == NodeColor.Black && sibling.Right.Color == NodeColor.Black)
                    {
                        sibling.Color = NodeColor.Red;
                        node = node.Parent;
                    }
                    else
                    {
                        if (sibling.Right.Color == NodeColor.Black)
                        {
                            sibling.Left.Color = NodeColor.Black;
                            sibling.Color = NodeColor.Red;
                            RotateRight(sibling);
                            sibling = node.Parent.Right;
                        }

                        sibling.Color = node.Parent.Color;
                        node.Parent.Color = NodeColor.Black;
                        sibling.Right.Color = NodeColor.Black;
                        RotateLeft(node.Parent);
                        node = Root;
                    }
                }
                else // same as code above with right and left swapped
                {
                    RedBlackTreeNode<TItem> sibling = node.Parent.Left;
                    if (sibling.Color == NodeColor.Red)
                    {
                        sibling.Color = NodeColor.Black;
                        node.Parent.Color = NodeColor.Red;
                        RotateRight(node.Parent);
                        sibling = node.Parent.Left;
                    }
                    if (sibling.Left.Color == NodeColor.Black && sibling.Right.Color == NodeColor.Black)
                    {
                        sibling.Color = NodeColor.Red;
                        node = node.Parent;
                    }
                    else
                    {
                        if (sibling.Left.Color == NodeColor.Black)
                        {
                            sibling.Right.Color = NodeColor.Black;
                            sibling.Color = NodeColor.Red;
                            RotateLeft(sibling);
                            sibling = node.Parent.Left;
                        }

                        sibling.Color = node.Parent.Color;
                        node.Parent.Color = NodeColor.Black;
                        sibling.Left.Color = NodeColor.Black;
                        RotateRight(node.Parent);
                        node = Root;
                    }
                }
            }
            node.Color = NodeColor.Black;
        }
    }
}
