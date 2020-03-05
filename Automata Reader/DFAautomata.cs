using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader
{
    class DFAautomata
    {
        public List<char> alphabet { get; set; }
        public List<DFAnode> dfaNodes { get; set; }

        public DFAautomata()
        {
            this.alphabet = new List<char>();
            this.dfaNodes = new List<DFAnode>();
        }
    }
}
