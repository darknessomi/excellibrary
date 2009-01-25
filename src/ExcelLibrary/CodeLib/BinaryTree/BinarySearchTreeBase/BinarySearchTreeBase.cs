using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeLib
{
    public class BinarySearchTreeBase<TItem, TTreeNode>
        where TItem : IComparable<TItem>
        where TTreeNode : BinaryTreeNodeBase<TItem, TTreeNode>, new()
    {
        public TTreeNode Root;

        private int count;

        public BinarySearchTreeBase()
        {
            Root = Nil;
        }

        public virtual TTreeNode Nil
        {
            get { return null; }
        }

        /// <summary>
        /// The number of nodes contained in the tree.
        /// </summary>
        public int Size
        {
            get { return count; }
        }

        public IEnumerable<TItem> InorderTreeWalk()
        {
            foreach (TTreeNode node in InorderTreeWalk(Root))
            {
                yield return node.Data;
            }
        }

        public bool Contains(TItem item)
        {
            return Search(Root, item) != Nil;
        }

        public TTreeNode Find(TItem item)
        {
            return Search(Root, item);
        }

        public void Add(TItem item)
        {
            TTreeNode node = new TTreeNode();
            node.Data = item;
            Insert(node);
        }

        public void Remove(TItem item)
        {
            TTreeNode node = Search(Root, item);
            if (node != Nil)
            {
                Delete(node);
            }
        }

        public void Clear()
        {
            Root = Nil;
            count = 0;
        }

        public IEnumerable<TTreeNode> InorderTreeWalk(TTreeNode node)
        {
            if (node != Nil)
            {
                foreach (TTreeNode subnode in InorderTreeWalk(node.Left))
                {
                    yield return subnode;
                }
                yield return node;

                foreach (TTreeNode subnode in InorderTreeWalk(node.Right))
                {
                    yield return subnode;
                }
            }
        }

        public TTreeNode Search(TTreeNode node, TItem item)
        {
            if (node == Nil || node.Data.CompareTo(item) == 0)
            {
                return node;
            }
            if (item.CompareTo(node.Data) < 0)
            {
                return Search(node.Left, item);
            }
            else
            {
                return Search(node.Right, item);
            }
        }

        public TTreeNode Minimum(TTreeNode node)
        {
            if (node == Nil) return Nil;
            TTreeNode left = node;
            while (left.Left != Nil)
            {
                left = left.Left;
            }
            return left;
        }

        public TTreeNode Maximum(TTreeNode node)
        {
            if (node == Nil) return Nil;
            TTreeNode right = node;
            while (right.Right != Nil)
            {
                right = right.Right;
            }
            return right;
        }

        protected TTreeNode Successor(TTreeNode node)
        {
            if (node.Right != Nil)
            {
                return Minimum(node.Right);
            }
            TTreeNode parent = node.Parent;
            while (parent != Nil && node == parent.Right)
            {
                node = parent;
                parent = parent.Parent;
            }
            return parent;
        }

        protected TTreeNode Predecessor(TTreeNode node)
        {
            if (node.Left != Nil)
            {
                return Maximum(node.Left);
            }
            TTreeNode parent = node.Parent;
            while (parent != Nil && node == parent.Left)
            {
                node = parent;
                parent = parent.Parent;
            }
            return parent;
        }

        ///<summary>
        /// Rebalance the tree by rotating the nodes to the left.
        ///</summary>        
        protected void RotateLeft(TTreeNode x)
        {
            // pushing node x down and to the Left to balance the tree. x's Right child (y)
            // replaces x (since y > x), and y's Left child becomes x's Right child 
            // (since it's < y but > x).

            TTreeNode y = x.Right;

            // Turn y's left subtree into x's right subtree
            x.Right = y.Left;

            // modify parents
            if (y.Left != Nil)
            {
                y.Left.Parent = x;
            }
            if (y != Nil)
            {
                y.Parent = x.Parent;
            }

            if (x.Parent != Nil)
            {
                // determine which side of it's Parent x was on
                if (x.IsLeftChild)
                {
                    x.Parent.Left = y;
                }
                else
                {
                    x.Parent.Right = y;
                }
            }
            else
            {
                // at rbTree, set it to y
                Root = y;
            }

            // link x and y
            // put x on y's Left
            y.Left = x;
            if (x != Nil)
            {
                x.Parent = y;
            }
        }

        ///<summary>
        /// Rebalance the tree by rotating the nodes to the right.
        ///</summary>
        protected void RotateRight(TTreeNode x)
        {
            // pushing node x down and to the Right to balance the tree. x's Left child (y)
            // replaces x (since x < y), and y's Right child becomes x's Left child 
            // (since it's < x but > y).

            // get x's Left node, this becomes y
            TTreeNode y = x.Left;

            // y's Right child becomes x's Left child
            x.Left = y.Right;

            // modify parents
            if (y.Right != Nil)
            {
                y.Right.Parent = x;
            }

            if (y != Nil)
            {
                y.Parent = x.Parent;
            }

            // Nil=rbTree, could also have used rbTree
            if (x.Parent != Nil)
            {
                // determine which side of it's Parent x was on
                if (x.IsRightChild)
                {
                    x.Parent.Right = y;
                }
                else
                {
                    x.Parent.Left = y;
                }
            }
            else
            {
                // at rbTree, set it to y
                Root = y;
            }

            // link x and y
            // put x on y's Right
            y.Right = x;
            if (x != Nil)
            {
                x.Parent = y;
            }
        }

        public virtual void Insert(TTreeNode node)
        {
            TTreeNode parent = Nil;
            TTreeNode current = Root;

            while (current != Nil)
            {
                parent = current;

                if (node.Data.CompareTo(current.Data) < 0)
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }

            node.Parent = parent;

            if (parent == Nil)
            {
                Root = node;
            }
            else
            {
                if (node.Data.CompareTo(parent.Data) < 0)
                {
                    parent.Left = node;
                }
                else
                {
                    parent.Right = node;
                }
            }
            count++;
        }

        public virtual TTreeNode Delete(TTreeNode node)
        {
            TTreeNode tobeDeleted;
            if (node.Left == Nil || node.Right == Nil)
            {
                tobeDeleted = node;
            }
            else
            {
                tobeDeleted = Successor(node);
            }

            TTreeNode childNode;
            if (tobeDeleted.Left != Nil)
            {
                childNode = tobeDeleted.Left;
            }
            else
            {
                childNode = tobeDeleted.Right;
            }

            if (childNode != Nil)
            {
                childNode.Parent = tobeDeleted.Parent;
            }

            TTreeNode parent = tobeDeleted.Parent;
            if (parent == Nil)
            {
                Root = childNode;
            }
            else
            {
                if (tobeDeleted.IsLeftChild)
                {
                    parent.Left = childNode;
                }
                else
                {
                    parent.Right = childNode;
                }
            }

            if (tobeDeleted != node)
            {
                node.Data = tobeDeleted.Data;
            }
            count--;
            return tobeDeleted;
        }
    }
}
