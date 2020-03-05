using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader
{
    class DFAconnection
    {
        public char Symbol { get; private set; }
        public DFAnode ToNode { get; set; }
        
        public DFAconnection(char symbol)
        {
            this.Symbol = symbol;
        }
    }
}
