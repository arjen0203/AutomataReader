using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader
{
    class Automata
    {
        public List<char> alphabet { get; set; }
        public List<Node> nodes { get; set; }
        public List<TestWord> testWords { get; set; }
        public bool dfaTest { get; set; }
        public bool finiteTest { get; set; }
        public Automata()
        {
            this.alphabet = new List<char>();
            this.nodes = new List<Node>();
            this.testWords = new List<TestWord>();
        }
    }
}
