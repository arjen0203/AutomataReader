using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader.NFAToRegex
{
    class RegexNode
    {
        public bool Final { get; set; }
        public string Name { get; private set; }
        public List<RegexConnection> Connections { get; set; }

        public RegexNode(string name)
        {
            this.Final = false;
            this.Name = name;
            this.Connections = new List<RegexConnection>();
        }

        public RegexNode(string name, bool final)
        {
            this.Final = final;
            this.Name = name;
            this.Connections = new List<RegexConnection>();
        }
        public RegexNode(Node node)
        {
            this.Final = node.Final;
            this.Name = node.Name;
            this.Connections = new List<RegexConnection>();
        }
        public RegexNode(RegexNode node)
        {
            this.Final = node.Final;
            this.Name = node.Name;
            this.Connections = NewConnections(node.Connections);
        }

        public List<RegexConnection> NewConnections(List<RegexConnection> connections)
        {
            List<RegexConnection> newConns = new List<RegexConnection>();
            //foreach (RegexConnection conn in connections) newConns.Add(new RegexConnection(conn));
            return newConns;
        }
    }
}
