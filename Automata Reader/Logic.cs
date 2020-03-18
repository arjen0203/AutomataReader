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
                if (text.StartsWith("dfa:"))
                {
                    SetDFATest(text);
                    text = reader.ReadLine();
                    continue;
                }
                if (text.StartsWith("finite:"))
                {
                    SetFiniteTest(text);
                    text = reader.ReadLine();
                    continue;
                }
                if (text.StartsWith("words:"))
                {
                    SetTestWords(reader);
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

        public void SetDFATest(string text)
        {
            string[] awnsersplit = text.Trim().Split(':');

            if (awnsersplit[1].Trim().Equals("y")) automata.dfaTest = true;
            else automata.dfaTest = false;
        }

        public void SetFiniteTest(string text)
        {
            string[] awnsersplit = text.Trim().Split(':');

            if (awnsersplit[1].Trim().Equals("y")) automata.finiteTest = true;
            else automata.finiteTest = false;
        }

        public void SetTestWords(StreamReader reader)
        {
            string text = reader.ReadLine();

            //split for accaptence and word
            string[] wordAndAcc;

            while (!text.Trim().Equals("end."))
            {
                wordAndAcc = text.Trim().Split(',');

                bool acceptence = false;
                if (wordAndAcc[1].Trim().Equals("y")) acceptence = true;

                automata.testWords.Add(new TestWord(wordAndAcc[0], acceptence));

                text = reader.ReadLine();
            }
        }

        public bool IsFinite(bool automataIsDfa)
        {
            bool finite = true;

            if (automataIsDfa)
            {
                foreach (Node node in automata.nodes)
                {
                    if (node.Final)
                    {
                        foreach (Connection con in node.Connections)
                        {
                            if (con.ToNode == node) finite = false;
                        }
                    }
                }
            }
            else
            {
                foreach (DFAnode node in dFAautomata.dfaNodes)
                {
                    if (node.Final)
                    {
                        foreach (DFAconnection con in node.Connections)
                        {
                            if (con.ToNode == node) finite = false;
                        }
                    }
                }
            }

            return finite;
        }

        public string TestOutputString(bool automataIsDFA, bool automataIsFinite)
        {
            string temp = "";

            if (automata.dfaTest == true && automataIsDFA == true) temp += "dfa: y == true" + Environment.NewLine;
            else if (automata.dfaTest == true && automataIsDFA == false) temp += "dfa: y == false" + Environment.NewLine;
            else if (automata.dfaTest == false && automataIsDFA == false) temp += "dfa: n == true" + Environment.NewLine;
            else if (automata.dfaTest == false && automataIsDFA == true) temp += "dfa: n == false" + Environment.NewLine;

            if (automata.finiteTest == true && automataIsFinite == true) temp += "finite: y == true" + Environment.NewLine;
            else if (automata.finiteTest == true && automataIsFinite == false) temp += "finite: y == false" + Environment.NewLine;
            else if (automata.finiteTest == false && automataIsFinite == false) temp += "finite: n == true" + Environment.NewLine;
            else if (automata.finiteTest == false && automataIsFinite == true) temp += "finite: n == false" + Environment.NewLine;

            temp += Environment.NewLine;

            foreach (TestWord word in automata.testWords)
            {
                temp += TestWordAccaptence(word.word, word.accapted, automataIsDFA) + Environment.NewLine;
            }

            return temp;
        }

        public string TestWordAccaptence(char[] word, bool testOutcome, bool automataIsDfa)
        {
            string temp = "";
            bool accepted = false;

            if (automataIsDfa)
            {
                Node currentNode = automata.nodes[0];

                for (int i = 0; i < word.Length; i++)
                {
                    Connection thisConn = new Connection('q', null);

                    foreach (Connection conn in currentNode.Connections)
                    {
                        if (conn.Symbol == word[i]) thisConn = conn;
                    }

                    currentNode = thisConn.ToNode;
                }
                if (currentNode.Final) accepted = true;
            }
            else
            {
                DFAnode currentNode = dFAautomata.dfaNodes[0];

                for (int i = 0; i < word.Length; i++)
                {
                    DFAconnection thisConn = new DFAconnection('q');

                    foreach (DFAconnection conn in currentNode.Connections)
                    {
                        if (conn.Symbol == word[i]) thisConn = conn;
                    }

                    currentNode = thisConn.ToNode;
                }
                if (currentNode.Final) accepted = true;
            }


            if (accepted == true && testOutcome == true) temp = new string(word) + ",y == true";
            else if (accepted == false && testOutcome == true) temp = new string(word) + ",y == false";
            else if (accepted == true && testOutcome == false) temp = new string(word) + ",n == false";
            else if (accepted == false && testOutcome == false) temp = new string(word) + ",n == true";

            return temp;
        }

        public void CreateAutomatePicture()
        {
            string pathAndName;

            pathAndName = "C:\\Users\\arjen\\Documents\\Fontys\\S4 AUT\\AutomataReader\\Automate Picture\\" + "AutomataPicture.dot";

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

            dot.StartInfo.WorkingDirectory = "C:\\Users\\arjen\\Documents\\Fontys\\S4 AUT\\AutomataReader\\Automate Picture";

            dot.StartInfo.Arguments = "-Tpng -O AutomataPicture.dot";

            dot.Start();

            dot.WaitForExit();

            pictureBox.ImageLocation = "C:\\Users\\arjen\\Documents\\Fontys\\S4 AUT\\AutomataReader\\Automate Picture\\AutomataPicture.dot.png";
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

            dot.StartInfo.WorkingDirectory = "C:\\Users\\arjen\\Documents\\Fontys\\S4 AUT\\AutomataReader\\Automate Picture";

            dot.StartInfo.Arguments = "-Tpng -O AutomataConvertedDFAPicture.dot";

            dot.Start();

            dot.WaitForExit();

            pictureBox.ImageLocation = "C:\\Users\\arjen\\Documents\\Fontys\\S4 AUT\\AutomataReader\\Automate Picture\\AutomataConvertedDFAPicture.dot.png";
        }

        public void CreateDFAAutomatePicture()
        {
            string pathAndName;

            pathAndName = "C:\\Users\\arjen\\Documents\\Fontys\\S4 AUT\\AutomataReader\\Automate Picture\\" + "AutomataConvertedDFAPicture.dot";

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
        public void CreateDFAfile()
        {
            string pathAndName;

            pathAndName = "C:\\Users\\arjen\\Documents\\Fontys\\S4 AUT\\AutomataReader\\DFAoutput\\" + "DFAoutput.txt";

            FileStream fileStream = new FileStream(pathAndName, FileMode.Create, FileAccess.Write);

            StreamWriter writer = new StreamWriter(fileStream);

            string outputDot = GetDFAString();

            writer.WriteLine(outputDot);

            writer.Close();
            fileStream.Close();
        }
        public string GetDFAString()
        {
            string temp = "alphabet: " + new string(automata.alphabet.ToArray()) + Environment.NewLine + "states: ";

            for (int i = 0; i < dFAautomata.dfaNodes.Count; i++)
            {
                temp += dFAautomata.dfaNodes[i].Name;
                if (i != dFAautomata.dfaNodes.Count - 1) temp += ",";
            }

            temp += Environment.NewLine + "final: ";
            for (int i = 0; i < dFAautomata.dfaNodes.Count; i++)
            {
                if (dFAautomata.dfaNodes[i].Final)
                {
                    temp += dFAautomata.dfaNodes[i].Name + ",";
                    
                }
                
            }
            temp += Environment.NewLine + Environment.NewLine + "transitions:" + Environment.NewLine;

            foreach (DFAnode node in dFAautomata.dfaNodes)
            {
                foreach (DFAconnection conn in node.Connections)
                {
                    temp += node.Name + "," + conn.Symbol + " --> " + conn.ToNode.Name + Environment.NewLine;
                }
            }
            temp += "end." + Environment.NewLine + Environment.NewLine;

            string yOrN;
            if (automata.dfaTest) yOrN = "y";
            else yOrN = "n";
            temp += "dfa: " + yOrN + Environment.NewLine;

            if (automata.finiteTest) yOrN = "y";
            else yOrN = "n";
            temp += "finite: " + yOrN + Environment.NewLine + Environment.NewLine;

            temp += "words:" + Environment.NewLine;

            foreach (TestWord word in automata.testWords)
            {
                if (word.accapted) yOrN = "y";
                else yOrN = "n";
                temp += new string(word.word) + ", " + yOrN + Environment.NewLine;
                
            }

            temp += "end.";

            return temp;
        }
    }
}
