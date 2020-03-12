using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader
{
    class Node
    {
        public bool Starting { get; private set; }
        public bool Final { get; set; }
        public string Name { get; private set; }
        public List<Connection> Connections { get; set; }

        public Node(bool starting, string name)
        {
            this.Starting = starting;
            this.Final = false;
            this.Name = name;
            this.Connections = new List<Connection>();
        }
    }
}
