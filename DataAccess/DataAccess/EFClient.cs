using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Domain;
using DataAccess.Domain.Entity;

namespace DataAccess
{
    internal class EFClient : IDisposable
    {
        private DatabaseContext Context { get; set; }

        public EFClient()
        {
            Context = new DatabaseContext();
        }

        public void Dispose()
        {
            if (Context != null) Context.Dispose();
        }

        public void CreateTreeNode(string newNodeId, string parentNodeId)
        {
            var newNode = new TreeNode() {Id = newNodeId, Children = new List<TreeNode>()};

            if (parentNodeId != null)
            {
                var parentNode = Context.TreeNodes.Find(parentNodeId);
                newNode.Parent = parentNode;
                parentNode.Children.Add(newNode);
            }
            else
            {
                newNode.Parent = null;
            }
            Context.TreeNodes.Add(newNode);
            Context.SaveChanges();
        }

        public void UpdateTreeNode(TreeNode node, string newParentId)
        {
            node.Parent.Children.Remove(node);
            if (newParentId == null)
                node.Parent = null;
            else
            {
                var newParent = Context.TreeNodes.Find(newParentId);
                node.Parent = newParent;
                newParent.Children.Add(node);
            }
            
            Context.SaveChanges();
        }

        public void DeleteSubtree(TreeNode subTreeRootNode)
        {
            Context.TreeNodes.RemoveRange(GetNodesToRemove(subTreeRootNode));    
            Context.SaveChanges();
        }

        private List<TreeNode> GetNodesToRemove(TreeNode subTreeRootNode)
        {
            var nodesToRemove = new List<TreeNode>();
            nodesToRemove.Add(subTreeRootNode);
            foreach (var child in subTreeRootNode.Children)
            {
                nodesToRemove.AddRange(GetNodesToRemove(child));
            }
            return nodesToRemove;
        }

        public void DeleteAllNodes()        
        {
            Context.TreeNodes.RemoveRange(Context.TreeNodes);
            Context.SaveChanges();
        }

        public TreeNode GetTreeNodeById(string id)
        {
            return Context.TreeNodes.Find(id);
        }

        public List<Node> GetSubTreeByRootId(string id)
        {
            var root = GetTreeNodeById(id);

            return TreeNodeToNode(root);
        }

        private List<Node> TreeNodeToNode(TreeNode root)
        {
            var result = new List<Node>();

            if (root != null)
            {
                var newSubTreeRoot = new Node() { id = root.Id };
                result.Add(newSubTreeRoot);
                if (root.Children.Count > 0)
                    newSubTreeRoot.childs = new List<Node>();

                foreach (var child in root.Children)
                {
                    var a = TreeNodeToNode(child);
                    newSubTreeRoot.childs.AddRange(a);
                }
            }
            
            return result;
        }

        public List<Node> GetAllTree()
        {
                var rootNodes = Context.TreeNodes.Where(x => x.Parent == null).ToList();
            var result = new List<Node>();
            foreach (var node in rootNodes)
            {
                result.AddRange(TreeNodeToNode(node));
            }
            return result;
        }
    }
}