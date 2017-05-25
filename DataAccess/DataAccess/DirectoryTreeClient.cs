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
                        result.Status = 200;
                        result.Note = "";
                    }
                    else if (node != null)
                    {
                        result.Status = 400;
                        result.Note = "Элемент \"" + id + "\" уже существует";
                    }
                    else                                            //if (parentId != null && parentNode == null)
                    {
                        result.Status = 400;
                        result.Note = "Нет элемента с Id \"" + parentId + "\"";
                    }
                }
                catch
                {
                    result.Status = 500;
                    result.Note = "Ошибка!";
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
                        {
                            result.Status = 400;
                            result.Note = "Петля";
                        }
                        else
                        {
                            EfClient.UpdateTreeNode(node, parentId);
                            result.Status = 200;
                            result.Note = "";
                        }
                    }
                    else
                    {
                        result.Status = 400;
                        result.Note = "Нет элемента с Id \"" + (node == null ? id : parentId) + "\"";
                    }
                }
                catch
                {
                    result.Status = 500;
                    result.Note = "Ошибка!";
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
                    result.Status = 200;   // Если что-то пойдёт не так, значение изменится
                    result.Note = ""; 
                    if (id == null)
                        EfClient.DeleteAllNodes();
                    else if (subTree == null)
                    {
                        result.Status = 400;
                        result.Note = "Нет элемента с Id \"" + id + "\"";
                    }
                    else
                        EfClient.DeleteSubtree(subTree);
                }
                catch
                {
                    result.Status = 500;
                    result.Note = "Ошибка!";
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
                    result.Status = 200; // Если что-то пойдёт не так - значение изменится
                    result.Note = "";       
                    if (id != null)
                    {
                        var subTree = EfClient.GetSubTreeByRootId(id);
                        if (subTree.Count == 0)
                        {
                            result.Status = 400;
                            result.Note = "Нет элемента с Id \"" + id + "\"";
                        }
                        else resultSubTree.Add(subTree[0]);
                    }
                    else resultSubTree = EfClient.GetAllTree();
                    
                    result.SubTree = resultSubTree;
                }
                catch
                {
                    result.Status = 500;
                    result.Note = "Ошибка!";
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