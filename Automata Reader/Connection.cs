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
        public char PushStack { get; private set; }
        public char PopStack { get; private set; }


        public Connection(char symbol, Node toNode)
        {
            this.Symbol = symbol;
            this.ToNode = toNode;
        }

        public Connection(char symbol, Node toNode, char pushStack, char popStack)
        {
            this.Symbol = symbol;
            this.ToNode = toNode;
            this.PushStack = pushStack;
            this.PopStack = popStack;
        }
    }
}
