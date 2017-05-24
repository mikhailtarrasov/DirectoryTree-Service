using System.Collections.Generic;
using DataAccess.Domain.Entity;

namespace DataAccess
{
    public class DirectoryTreeClient
    {
        private EFClient EfClient { get; set; }

        public ResultDTClient CreateNode(string id, string parentId)
        {
            ResultDTClient result = new ResultDTClient();
            using (EfClient = new EFClient())
            {
                try
                {   
                    var node = EfClient.GetTreeNodeById(id);
                    var parentNode = EfClient.GetTreeNodeById(parentId);

                    if (node == null && (parentId == null || parentNode != null))
                    {
                        EfClient.CreateTreeNode(id, parentId);
                        result.CodeNotePair = new KeyValuePair<int, string>(200, "");
                    }
                    else if (node != null)
                    {
                        result.CodeNotePair = new KeyValuePair<int, string>(400, "Элемент \"" + id + "\" уже существует");
                    }
                    else                                            //if (parentId != null && parentNode == null)
                    {
                        result.CodeNotePair = new KeyValuePair<int, string>(400, "Нет элемента с Id \"" + parentId + "\"");
                    }
                }
                catch
                {
                    result.CodeNotePair = new KeyValuePair<int, string>(500, "Ошибка!");
                }
            }
            return result;
        }

        public ResultDTClient UpdateNode(string id, string parentId)
        {
            ResultDTClient result = new ResultDTClient();
            using (EfClient = new EFClient())
            {
                try
                {
                    var node = EfClient.GetTreeNodeById(id);
                    var newParentNode = EfClient.GetTreeNodeById(parentId);

                    if (node != null && (parentId == null || newParentNode != null))
                    {
                        var loop = IsLoopExist(node, parentId);
                        if (loop)
                            result.CodeNotePair = new KeyValuePair<int, string>(400, "Петля");
                        else
                        {
                            EfClient.UpdateTreeNode(node, parentId);
                            result.CodeNotePair = new KeyValuePair<int, string>(200, "");
                        }
                    }
                    else
                    {
                        result.CodeNotePair = new KeyValuePair<int, string>(400, "Нет элемента с Id \"" + (node == null ? id : parentId) + "\"");
                    }
                }
                catch
                {
                    result.CodeNotePair = new KeyValuePair<int, string>(500, "Ошибка!");
                }
            }
            return result;
        }

        public ResultDTClient DeleteSubtree(string id)
        {
            ResultDTClient result = new ResultDTClient();
            using (EfClient = new EFClient())
            {
                try
                {
                    var subTree = EfClient.GetTreeNodeById(id);
                    result.CodeNotePair = new KeyValuePair<int, string>(200, "");   // Если что-то пойдёт не так, значение изменится
                    if (id == null)
                        EfClient.DeleteAllNodes();
                    else if (subTree == null)
                        result.CodeNotePair = new KeyValuePair<int, string>(400, "Нет элемента с Id \"" + id + "\"");
                    else
                        EfClient.DeleteSubtree(subTree);
                }
                catch
                {
                    result.CodeNotePair = new KeyValuePair<int, string>(500, "Ошибка!");
                }
            }
            return result;
        }

        public ResultDTClient GetSubTree(string id)
        {
            ResultDTClient result = new ResultDTClient();
            using (EfClient = new EFClient())
            {
                try
                {
                    var resultSubTree = new List<Node>();
                    if (id != null)
                    {
                        var subTree = EfClient.GetSubTreeByRootId(id);
                        if (subTree.Count == 0)
                            result.CodeNotePair = new KeyValuePair<int, string>(400, "Нет элемента с Id \"" + id + "\"");
                        else resultSubTree.Add(subTree[0]);
                    }
                    else resultSubTree = EfClient.GetAllTree();
                    if (resultSubTree.Count >= 0) result.CodeNotePair = new KeyValuePair<int, string>(200, "");
                    result.SubTree = resultSubTree;
                }
                catch
                {
                    result.CodeNotePair = new KeyValuePair<int, string>(500, "Ошибка!");
                    throw;
                }
            }
            return result;
        }

        private bool IsLoopExist(TreeNode node, string newParentId)
        {
            var result = false;
            if (newParentId == null)
                return result;

            foreach (var child in node.Children)
            {
                if (child.Id == newParentId)
                {
                    result = true;
                    return result;
                }
                var loop = IsLoopExist(child, newParentId);
                result = loop == false ? result : true;
                if (result) return result;
            }
            return result;
        }
    }
}