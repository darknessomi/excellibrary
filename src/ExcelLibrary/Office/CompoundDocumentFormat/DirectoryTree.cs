using System;
using System.Collections.Generic;
using System.Text;
using QiHe.CodeLib;

namespace ExcelLibrary.CompoundDocumentFormat
{
    public class DirectoryTree
    {
        public static void Build(DirectoryEntry rootEntry)
        {
            if (rootEntry.Members.Count > 0)
            {
                rootEntry.MembersTreeNodeDID = BuildStorageEntry(rootEntry);
            }
        }

        private static int BuildStorageEntry(DirectoryEntry storageEntry)
        {
            // direct members of each storage are organised in a separate red-black tree
            RedBlackTree<DirectoryEntry> rbTree = new RedBlackTree<DirectoryEntry>();
            foreach (DirectoryEntry entry in storageEntry.Members.Values)
            {
                rbTree.Add(entry);
            }

            foreach (RedBlackTreeNode<DirectoryEntry> node in rbTree.InorderTreeWalk(rbTree.Root))
            {
                DirectoryEntry entry = node.Data;
                entry.NodeColor = GetNodeColor(node.Color);
                entry.LeftChildDID = GetNodeID(node.Left);
                entry.RightChildDID = GetNodeID(node.Right);

                if (entry.Members.Count > 0)
                {
                    entry.EntryType = EntryType.Storage;
                    entry.MembersTreeNodeDID = BuildStorageEntry(entry);
                }
                else
                {
                    entry.EntryType = EntryType.Stream;
                    entry.MembersTreeNodeDID = -1;
                }
            }

            return rbTree.Root.Data.ID;
        }

        private static int GetNodeID(RedBlackTreeNode<DirectoryEntry> node)
        {
            if (node == RedBlackTreeNode<DirectoryEntry>.Nil)
            {
                return -1;
            }
            else
            {
                return node.Data.ID;
            }
        }

        private static byte GetNodeColor(QiHe.CodeLib.NodeColor nodeColor)
        {
            switch (nodeColor)
            {
                case QiHe.CodeLib.NodeColor.Black:
                    return NodeColor.Black;
                case QiHe.CodeLib.NodeColor.Red:
                    return NodeColor.Red;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
