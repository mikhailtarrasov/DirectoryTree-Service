using System.Collections.Generic;

namespace DataAccess
{
    public class ResultDTClient
    {
        //public KeyValuePair<int, string> CodeNotePair { get; set; }
        public int Status { get; set; }
        public string Note { get; set; }
        public List<Node> SubTree { get; set; }
    }
}
