using System.Collections.Generic;

namespace DataAccess
{
    public class Node
    {
        public string id { get; set; }
        public virtual List<Node> childs { get; set; }
    }
}
