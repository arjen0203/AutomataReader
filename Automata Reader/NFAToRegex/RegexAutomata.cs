using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader.NFAToRegex
{
    class RegexAutomata
    {
        public List<char> Alphabet { get; private set; }
        public List<RegexNode> Nodes { get; private set; }
        public RegexNode StartNode { get; private set; }
        public RegexNode FinalNode { get; private set; }

        public RegexAutomata(Automata automata)
        {
            this.Alphabet = new List<char>(automata.alphabet);
            this.StartNode = new RegexNode("qStart");
            this.FinalNode = new RegexNode("qFinal", true);
            CreateRegexNodes(automata.nodes);
        }

        public RegexAutomata(RegexAutomata automata)
        {
            this.Alphabet = new List<char>(automata.Alphabet);
            this.StartNode = new RegexNode(automata.StartNode);
            this.FinalNode = new RegexNode(automata.FinalNode);
            //fullNodeListHere pls
        }

        public void CreateRegexNodes(List<Node> automataNodes)
        {
            this.Nodes = new List<RegexNode>();
            
            this.Nodes.Add(this.StartNode);
            foreach (Node node in automataNodes) this.Nodes.Add(new RegexNode(node));
            this.Nodes.Add(this.FinalNode);

            foreach (Node node in automataNodes)
            {
                RegexNode regexNode = GetCorrespondingNode(node.Name);
                foreach(Connection connection in node.Connections)
                {
                    if (!(connection.ToNode == node && connection.Symbol == '_'))
                    {
                        regexNode.Connections.Add(new RegexConnection(connection.Symbol.ToString(), GetCorrespondingNode(connection.ToNode.Name)));
                    }
                }
                if (node.Final) regexNode.Connections.Add(new RegexConnection("_", this.FinalNode));
            }
            StartNode.Connections.Add(new RegexConnection("_", this.Nodes[1]));
        }

        public RegexNode GetCorrespondingNode(string name)
        {
            foreach (RegexNode node in this.Nodes)
            {
                if (node.Name == name) return node;
            }
            return null;
        }
    }
}
