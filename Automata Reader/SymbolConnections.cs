using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader
{
    class SymbolConnections
    {
        public Node startingNode { get; set; }
        public char symbol { get; set; }
        public List<Node> toPossibleNodes { get; set; }
    }
}
