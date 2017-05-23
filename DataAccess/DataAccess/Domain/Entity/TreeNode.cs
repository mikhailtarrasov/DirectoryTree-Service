using System.Collections.Generic;

namespace DataAccess.Domain.Entity
{
    public class TreeNode
    {
        public string Id { get; set; }
        public virtual TreeNode Parent { get; set; }
        public virtual List<TreeNode> Children { get; set; }
    }
}
