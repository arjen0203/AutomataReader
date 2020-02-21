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
        public string ToNodeName { get; private set; }
        
        public Connection(char symbol, string toNodeName)
        {
            this.Symbol = symbol;
            this.ToNodeName = toNodeName;
        }
    }
}
