using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader
{
    class DFAnode
    {
        public bool Starting { get; set; }
        public bool Final { get; set; }
        public string Name { get; set; }
        public List<Node> nodesTogether { get; set; }
        public List<DFAconnection> Connections { get; set; }

        public DFAnode()
        {
            this.nodesTogether = new List<Node>();
            this.Connections = new List<DFAconnection>();
        }
        public DFAnode(List<Node> nodes)
        {
            this.nodesTogether = nodes;
            CreateName();
            this.Connections = new List<DFAconnection>();
        }
        public void CreateName()
        {
            string nodesName = "";

            foreach (Node node in nodesTogether)
            {
                nodesName += node.Name.Trim() + ",";
            }

            //this.Name = nodesName.Substring(0, nodesName.Length - 1);
            this.Name = nodesName;
        }
    }
}
