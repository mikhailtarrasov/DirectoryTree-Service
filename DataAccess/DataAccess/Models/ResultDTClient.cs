using System.Collections.Generic;

namespace DataAccess
{
    public class ResultDTClient
    {
        public KeyValuePair<int, string> CodeNotePair { get; set; }
        public List<Node> SubTree { get; set; }
    }
}
