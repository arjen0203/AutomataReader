using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automata_Reader
{
    class Logic
    {
        public Automata automata { get; private set; }
        public DFAautomata dFAautomata { get; private set; }
        public void ReadLines(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

            StreamReader reader = new StreamReader(fileStream);

            automata = new Automata();

            string text = reader.ReadLine();
            while (text != null)
            {
                if (text.Equals("") || text.StartsWith("#"))
                {
                    text = reader.ReadLine();
                    continue;
                }

                if (text.StartsWith("alphabet:"))
                {
                    CreateAlphabet(text);
                    text = reader.ReadLine();
                    continue;
                }

                if (text.StartsWith("states:"))
                {
                    CreateNodes(text);
                    text = reader.ReadLine();
                    continue;
                }

                if (text.StartsWith("final:"))
                {
                    SetFinalNodes(text);
                    text = reader.ReadLine();
                    continue;
                }

                if (text.StartsWith("transitions:"))
                {
                    SetTransitions(reader);
                    text = reader.ReadLine();
                    continue;
                }

                text = reader.ReadLine();
            }
        }

        public void CreateAlphabet(string text)
        {
            string newText = text.Substring(9).Trim();

            foreach (char s in newText)
            {
                automata.alphabet.Add(s);
            }
        }

        public void CreateNodes(string text)
        {
            string newText = text.Substring(7).Trim();
            string[] nodeNames = newText.Split(',');

            bool firstNode = true;

            foreach (string name in nodeNames)
            {
                if (!name.Equals("")) automata.nodes.Add(new Node(firstNode, name));
                if (firstNode) firstNode = false;
            }
        }

        public void SetFinalNodes(string text)
        {
            string newText = text.Substring(6).Trim();
            string[] finalNodeNames = newText.Split(',');

            foreach (string name in finalNodeNames)
            {
                foreach (Node node in automata.nodes)
                {
                    if (name.Equals(node.Name))
                    {
                        node.Final = true;
                        break;
                    }
                }
            }
        }

        public void SetTransitions(StreamReader reader)
        {
            string text = reader.ReadLine();

            //split for A,a --> B to A & a --> B
            string[] wichState;

            //split for a --> B to a & B
            string[] wichConnection;

            while (!text.Trim().Equals("end."))
            {
                wichState = text.Trim().Split(',');
                wichConnection = wichState[1].Trim().Split(new string[] { "-->" }, StringSplitOptions.None);

                foreach (Node node in automata.nodes)
                {
                    if (wichState[0].Equals(node.Name))
                    {
                        foreach (Node connNode in automata.nodes)
                        {
                            if (wichConnection[1].Trim().Equals(connNode.Name))
                            {
                                node.Connections.Add(new Connection(wichConnection[0].Trim().ToCharArray()[0], connNode));
                                break;
                            }
                        }
                        break;
                    }
                }

                text = reader.ReadLine();
            }
        }

        public void CreateAutomatePicture()
        {
            string pathAndName;

            pathAndName = "C:\\Users\\20182942\\Documents\\Fontys\\S4 AUT\\AutomataReader\\Automate Picture\\" + "AutomataPicture.dot";

            FileStream fileStream = new FileStream(pathAndName, FileMode.Create, FileAccess.Write);

            StreamWriter writer = new StreamWriter(fileStream);

            string outputDot = GetDotString();

            writer.WriteLine(outputDot);

            writer.Close();
            fileStream.Close();
        }

        public string GetDotString()
        {
            string dotString = "";

            dotString += "digraph myAutomaton { \nrankdir=LR; \n\"\" [shape=none] \n";

            foreach (Node node in automata.nodes)
            {
                string shape = "circle";
                if (node.Final) shape = "doublecircle";
                dotString += $"\"{node.Name}\" [shape={shape}] \n";
            }

            dotString += "\n";

            foreach (Node node in automata.nodes)
            {
                if (node.Starting) dotString += $"\"\" -> \"{node.Name}\" \n";
            }

            foreach (Node node in automata.nodes)
            {
                foreach (Connection conn in node.Connections)
                {
                    dotString += $"\"{node.Name}\" -> \"{conn.ToNode.Name}\" [label=\"{conn.Symbol}\"] \n";
                }
            }

            dotString += "}";

            return dotString;
        }

        public void RunGraphicviz(PictureBox pictureBox)
        {
            Process dot = new Process();

            dot.StartInfo.FileName = "dot.exe";

            dot.StartInfo.WorkingDirectory = "C:\\Users\\20182942\\Documents\\Fontys\\S4 AUT\\AutomataReader\\Automate Picture";

            dot.StartInfo.Arguments = "-Tpng -O AutomataPicture.dot";

            dot.Start();

            dot.WaitForExit();

            pictureBox.ImageLocation = "C:\\Users\\20182942\\Documents\\Fontys\\S4 AUT\\AutomataReader\\Automate Picture\\AutomataPicture.dot.png";
        }

        public bool GraphIsDFA()
        {
            int connectionsNeeded = automata.alphabet.Count;
            int startNodes = 0;

            foreach (Node node in automata.nodes)
            {
                if (node.Starting) startNodes++;
                int connectionsAmount = 0;
                foreach (Connection conn in node.Connections)
                {
                    if (conn.Symbol == '_') return false;
                    connectionsAmount++;

                    foreach (Connection conn2 in node.Connections)
                    {
                        if (conn != conn2 && conn.Symbol == conn2.Symbol) return false;
                    }
                }
                if (connectionsAmount != connectionsNeeded) return false;
            }
            if (startNodes != 1) return false;

            return true;
        }

        public void ConvertNFAtoDFA()
        {
            List<SymbolConnection> symbolConnections = new List<SymbolConnection>();
            Node sinkNode = new Node(false, "SINK");

            GetAllTransitionsNFA(symbolConnections, sinkNode);

            dFAautomata = new DFAautomata();
            dFAautomata.alphabet = new List<char>(automata.alphabet);

            //starting node
            List<Node> startingNode = new List<Node>() { automata.nodes[0] };

            for (int i = 0; i < startingNode.Count; i++)
            {
                foreach (SymbolConnection symbConn in symbolConnections)
                {
                    if (symbConn.symbol == '_' && symbConn.startingNode == automata.nodes[0])
                    {
                        foreach (Node node in symbConn.toPossibleNodes)
                        {
                            if (!startingNode.Contains(node)) startingNode.Add(node);
                        }
                    }
                }
            }

            dFAautomata.dfaNodes.Add(new DFAnode(startingNode.OrderBy(a => a.Name).ToList()));
            dFAautomata.dfaNodes[0].Starting = true;
            dFAautomata.dfaNodes[0].CreateName();
            

            for (int i = 0; i < dFAautomata.dfaNodes.Count; i++)
            {
                foreach (char symbol in dFAautomata.alphabet)
                {
                    List<Node> newNode = new List<Node>();
                    DFAconnection newConnection = new DFAconnection(symbol);

                    foreach (Node node in dFAautomata.dfaNodes[i].nodesTogether)
                    {
                        foreach (SymbolConnection symbConn in symbolConnections)
                        {
                            if (node == symbConn.startingNode && symbConn.symbol == symbol && !newNode.Contains(node))
                            {
                                foreach (Node toNode in symbConn.toPossibleNodes)
                                {
                                    if (!newNode.Contains(toNode)) newNode.Add(toNode);
                                }
                            }
                        }
                    }

                    if (newNode.Contains(sinkNode) && newNode.Count > 1) newNode.Remove(sinkNode);

                    foreach (Node node in newNode)
                    {
                        foreach (SymbolConnection symbConn in symbolConnections)
                        {
                            if (node == symbConn.startingNode && symbConn.symbol == '_' && !newNode.Contains(node))
                            {
                                foreach (Node toNode in symbConn.toPossibleNodes)
                                {
                                    if (!newNode.Contains(toNode)) newNode.Add(toNode);
                                }
                            }
                        }
                    }

                    DFAnode tempNode = new DFAnode(newNode.OrderBy(a => a.Name).ToList());
                    bool nodeExists = false;
                    foreach (DFAnode node in dFAautomata.dfaNodes)
                    {
                        if (node.Name.Equals(tempNode.Name))
                        {
                            newConnection.ToNode = node;
                            dFAautomata.dfaNodes[i].Connections.Add(newConnection);
                            nodeExists = true;
                        }
                    }
                    if (!nodeExists)
                    {
                        newConnection.ToNode = tempNode;
                        dFAautomata.dfaNodes[i].Connections.Add(newConnection);
                        dFAautomata.dfaNodes.Add(tempNode);
                    }
                }
            }

            List<Node> acceptingNodes = new List<Node>();
            foreach (Node node in automata.nodes)
            {
                if (node.Final) acceptingNodes.Add(node);
            }

            foreach (DFAnode node in dFAautomata.dfaNodes)
            {
                foreach (Node finalNode in acceptingNodes)
                {
                    if (node.nodesTogether.Contains(finalNode)) node.Final = true;
                }
            }
        }

        public void GetAllTransitionsNFA(List<SymbolConnection> symbolConnections, Node sinkNode)
        {
            List<char> alphabetWithEpsilon = new List<char>(automata.alphabet);
            alphabetWithEpsilon.Add('_');
            bool containsSink = false;

            foreach (Node node in automata.nodes)
            {
                foreach (char symbol in alphabetWithEpsilon)
                {
                    SymbolConnection symbolConn = new SymbolConnection();
                    symbolConn.startingNode = node;
                    symbolConn.symbol = symbol;

                    foreach (Connection connection in node.Connections)
                    {
                        if (symbol == connection.Symbol)
                        {
                            if (symbolConn.toPossibleNodes == null) symbolConn.toPossibleNodes = new List<Node>();
                            symbolConn.toPossibleNodes.Add(connection.ToNode);
                            AddEpsilonTransitions(symbolConn.toPossibleNodes);
                        }
                    }
                    if (symbolConn.toPossibleNodes == null)
                    {
                        symbolConn.toPossibleNodes = new List<Node>() { sinkNode };
                        containsSink = true;
                    }

                    symbolConnections.Add(symbolConn);
                }
            }
            if (containsSink == true)
            {
                foreach (char symbol in automata.alphabet)
                {
                    SymbolConnection sinkConn = new SymbolConnection();
                    sinkConn.startingNode = sinkNode;
                    sinkConn.symbol = symbol;
                    sinkConn.toPossibleNodes = new List<Node>() { sinkNode };
                    symbolConnections.Add(sinkConn);
                }
            }
        }

        private void AddEpsilonTransitions(List<Node> possibleNodesList)
        {
            int startCount = possibleNodesList.Count;

            for (int i = 0; i < possibleNodesList.Count; i++)
            {
                foreach (Connection connection in possibleNodesList[i].Connections)
                {
                    if (connection.Symbol == '_' && !possibleNodesList.Contains(connection.ToNode)) possibleNodesList.Add(connection.ToNode);
                }
            }
        }
        public void RunGraphicvizDFA(PictureBox pictureBox)
        {
            Process dot = new Process();

            dot.StartInfo.FileName = "dot.exe";

            dot.StartInfo.WorkingDirectory = "C:\\Users\\20182942\\Documents\\Fontys\\S4 AUT\\AutomataReader\\Automate Picture";

            dot.StartInfo.Arguments = "-Tpng -O AutomataConvertedDFAPicture.dot";

            dot.Start();

            dot.WaitForExit();

            pictureBox.ImageLocation = "C:\\Users\\20182942\\Documents\\Fontys\\S4 AUT\\AutomataReader\\Automate Picture\\AutomataConvertedDFAPicture.dot.png";
        }

        public void CreateDFAAutomatePicture()
        {
            string pathAndName;

            pathAndName = "C:\\Users\\20182942\\Documents\\Fontys\\S4 AUT\\AutomataReader\\Automate Picture\\" + "AutomataConvertedDFAPicture.dot";

            FileStream fileStream = new FileStream(pathAndName, FileMode.Create, FileAccess.Write);

            StreamWriter writer = new StreamWriter(fileStream);

            string outputDot = GetDFADotString();

            writer.WriteLine(outputDot);

            writer.Close();
            fileStream.Close();
        }

        public string GetDFADotString()
        {
            string dotString = "";

            dotString += "digraph myAutomatonDFA { \nrankdir=LR; \n\"\" [shape=none] \n";

            foreach (DFAnode node in dFAautomata.dfaNodes)
            {
                string shape = "circle";
                if (node.Final) shape = "doublecircle";
                dotString += $"\"{node.Name}\" [shape={shape}] \n";
            }

            dotString += "\n";

            foreach (DFAnode node in dFAautomata.dfaNodes)
            {
                if (node.Starting) dotString += $"\"\" -> \"{node.Name}\" \n";
            }

            foreach (DFAnode node in dFAautomata.dfaNodes)
            {
                foreach (DFAconnection conn in node.Connections)
                {
                    dotString += $"\"{node.Name}\" -> \"{conn.ToNode.Name}\" [label=\"{conn.Symbol}\"] \n";
                }
            }

            dotString += "}";

            return dotString;
        }
    }
}
