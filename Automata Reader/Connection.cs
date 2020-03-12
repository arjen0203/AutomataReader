using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader
{
    class Connection
    {
        public char Symbol { get; private set; }
        public Node ToNode { get; private set; }
        
        public Connection(char symbol, Node toNode)
        {
            this.Symbol = symbol;
            this.ToNode = toNode;
        }
    }
}
