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
        public Automata automata { get; set; }
        public DFAautomata dFAautomata { get; private set; }

        public CFGToPDA CFGToPDA { get; private set; }
        public PDAToCFG PDAToCFG { get; private set; }

        public Logic()
        {
            this.CFGToPDA = new CFGToPDA();
            this.PDAToCFG = new PDAToCFG();
        }
        public void ReadLines(string path)
        {
            FileStream fileStream;
            try
            {
                fileStream = new FileStream($"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\Automata input files\\{path}.txt", FileMode.Open, FileAccess.Read);
            } catch (Exception)
            {
                Console.WriteLine("ERROR: File not found");
                return;
            }
            

            StreamReader reader = new StreamReader(fileStream);

            automata = new Automata();

            string text = reader.ReadLine();
            while (text != null)
            {
                if (text.StartsWith("alphabet:"))
                {
                    CreateAlphabet(text);
                } 
                else if (text.StartsWith("states:"))
                {
                    CreateNodes(text);
                }
                else if (text.StartsWith("stack:"))
                {
                    SetStack(text);
                }
                else if (text.StartsWith("final:"))
                {
                    SetFinalNodes(text);
                }
                else if (text.StartsWith("transitions:"))
                {
                    SetTransitions(reader, isPDA());
                }
                else if (text.StartsWith("dfa:"))
                {
                    SetDFATest(text);
                }
                else if (text.StartsWith("finite:"))
                {
                    SetFiniteTest(text);
                }
                else if (text.StartsWith("words:"))
                {
                    SetTestWords(reader);
                }
                else if (text.StartsWith("grammar:"))
                {
                    automata = CFGToPDA.CreatePDAFromCFG(reader);
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

        public void SetTransitions(StreamReader reader, bool isPDA)
        {
            string text = reader.ReadLine();

            while (!text.Trim().Equals("end."))
            {
                //split for A,a --> B to A & a --> B
                string[] commaSplit = text.Trim().Split(new[] { ',' }, 2);

                //split for a --> B to a & B
                string[] arrowSplit = commaSplit[1].Trim().Split(new string[] { "-->" }, StringSplitOptions.None);

                //split for a [_,_] to to a
                char charachter = char.Parse(arrowSplit[0].Substring(0, 1));

                foreach (Node node in automata.nodes)
                {
                    if (commaSplit[0].Equals(node.Name))
                    {
                        foreach (Node connNode in automata.nodes)
                        {
                            if (arrowSplit[1].Trim().Equals(connNode.Name))
                            {
                                if (this.isPDA())
                                {
                                    string[] pushAndPop = arrowSplit[0].Substring(1, arrowSplit[0].Length - 1).Trim().Split(',');
                                    if (pushAndPop.Length == 2)
                                    {
                                        char popChar = char.Parse(pushAndPop[0].Substring(pushAndPop[0].Length - 1, 1));
                                        char pushChar = char.Parse(pushAndPop[1].Substring(0, 1));
                                        node.Connections.Add(new Connection(charachter, connNode, pushChar, popChar));
                                    } else
                                    {
                                        node.Connections.Add(new Connection(charachter, connNode));
                                    }

                                    
                                } else
                                {
                                    node.Connections.Add(new Connection(charachter, connNode));
                                }
                                break;
                            }
                        }
                        break;
                    }
                }

                text = reader.ReadLine();
            }
        }

        public void SetStack(string text)
        {
            automata.stack = new List<char>();

            string newText = text.Substring(6).Trim();

            foreach (char s in newText)
            {
                automata.stack.Add(s);
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

        public string TestOutputString(bool automataIsDFA, bool automataIsFinite, bool automataIsPDA)
        {
            string temp = "";

            temp += $"dfa: {(automata.dfaTest ? 'y' : 'n')} == {automata.dfaTest == automataIsDFA}\r\n";
            temp += $"finite: {(automata.finiteTest ? 'y' : 'n')} == {automata.finiteTest == automataIsFinite}\r\n\r\n";

            foreach (TestWord word in automata.testWords)
            {
                temp += TestWordAccaptence(word.word, word.accapted, automataIsDFA) + Environment.NewLine;
            }

            return temp;
        }

        public string TestWordAccaptence(char[] word, bool testOutcome, bool automataIsDfa)
        {
            bool accepted = false;

            if (this.isPDA())
            {
                accepted = wordAcceptancePDA(word);
            }
            else if (automataIsDfa)
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

            return $"{new string(word)},{(testOutcome ? 'y' : 'n')} == {accepted == testOutcome}";
        }

        public bool wordAcceptancePDA(char[] word)
        {
            Queue<PDAConfiguration> configurations = new Queue<PDAConfiguration>();
            configurations.Enqueue(new PDAConfiguration(new Stack<char>(), automata.nodes[0], new Queue<char>(word), 0));
            HashSet<string> oldConfigs = new HashSet<string>();

            while(configurations.Count != 0)
            {
                PDAConfiguration configuration = configurations.Dequeue();
                if (configuration.Word.Count == 0 & configuration.Stack.Count == 0 && configuration.Node.Final) return true;

                foreach (Connection trans in configuration.Node.Connections)
                {
                    PDAConfiguration newConfig = checkTransitionAndCreateNewConfig(configuration, trans);
                    if (newConfig != null && newConfig.Depth < 50)
                    {
                        string newConfigHashString = newConfig.CreateHashString();
                        if (!oldConfigs.Contains(newConfigHashString))
                        {
                            configurations.Enqueue(newConfig);
                            oldConfigs.Add(newConfigHashString);
                        }
                    }
                }
                //outcomment this for big performence increase
                configuration.print();
            }

            return false;
        }

        public PDAConfiguration checkStackAndCreateNewConfig(PDAConfiguration currentConfig, Queue<char> newWord, Connection transition)
        {
            Stack<char> newStack = new Stack<char>(new Stack<char>(currentConfig.Stack));
            Node newNode = transition.ToNode;

            if (currentConfig.Stack.Count > 0)
            {
                if (transition.PopStack == currentConfig.Stack.Peek())
                {
                    newStack.Pop();
                    if (transition.PushStack != '_' && transition.PushStack != '\0') newStack.Push(transition.PushStack);
                    return new PDAConfiguration(newStack, newNode, newWord, currentConfig.Depth + 1);
                }
            }

            if (transition.PopStack == '_')
            {
                if (transition.PushStack != '_' && transition.PushStack != '\0') newStack.Push(transition.PushStack);
                return new PDAConfiguration(newStack, newNode, newWord, currentConfig.Depth + 1);
            }
            else if (transition.PopStack == '\0')
            {
                return new PDAConfiguration(newStack, newNode, newWord, currentConfig.Depth + 1);
            }
            return null;
        }
        
        public PDAConfiguration checkTransitionAndCreateNewConfig(PDAConfiguration currentConfig, Connection transition)
        {
            Queue<char> newWord = new Queue<char>(currentConfig.Word);
            if (currentConfig.Word.Count > 0)
            {
                if (transition.Symbol == currentConfig.Word.Peek())
                {
                    newWord.Dequeue();
                    return checkStackAndCreateNewConfig(currentConfig, newWord, transition);
                }
            }

            if (transition.Symbol == '_')
            {
                return checkStackAndCreateNewConfig(currentConfig, newWord, transition);
            }
            return null;
        }

        public void CreateAutomatePicture()
        {
            string pathAndName = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\Automate Picture\\AutomataPicture.dot";

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
                    string label = conn.Symbol.ToString();
                    if (conn.PopStack != '\0') label = $"{conn.Symbol} [{conn.PopStack},{conn.PushStack}]";
                    dotString += $"\"{node.Name}\" -> \"{conn.ToNode.Name}\" [label=\"{label}\"] \n";
                }
            }

            dotString += "}";

            return dotString;
        }

        public void RunGraphicviz(PictureBox pictureBox)
        {
            Process dot = new Process();

            dot.StartInfo.FileName = "dot.exe";
            
            dot.StartInfo.WorkingDirectory = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\Automate Picture";

            dot.StartInfo.Arguments = "-Tpng -O AutomataPicture.dot";

            dot.Start();

            dot.WaitForExit();

            pictureBox.ImageLocation = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\Automate Picture\\AutomataPicture.dot.png";
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

            dot.StartInfo.WorkingDirectory = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\Automate Picture";

            dot.StartInfo.Arguments = "-Tpng -O AutomataConvertedDFAPicture.dot";

            dot.Start();

            dot.WaitForExit();

            pictureBox.ImageLocation = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\Automate Picture\\AutomataConvertedDFAPicture.dot.png";
        }

        public void CreateDFAAutomatePicture()
        {
            string pathAndName = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\Automate Picture\\AutomataConvertedDFAPicture.dot";

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
            string pathAndName = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\DFAoutput\\DFAoutput.txt";

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

        public string addTestWord(string word, bool acceptence, bool automataIsDFA)
        {
            if (automata == null) return "";
            automata.testWords.Add(new TestWord(word, acceptence));
            return TestWordAccaptence(word.ToCharArray(), acceptence, automataIsDFA) + Environment.NewLine;
        }

        public bool isPDA()
        {
            if (automata.stack != null) return true;
            return false;
        }

        public void ConvertPDAToCFG()
        {
            string outputString = PDAToCFG.ConvertPDAToCFGToString(automata);

            string pathAndName = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\CFGoutput\\CFGoutput.txt";

            FileStream fileStream = new FileStream(pathAndName, FileMode.Create, FileAccess.Write);

            StreamWriter writer = new StreamWriter(fileStream);

            writer.WriteLine(outputString);

            writer.Close();
            fileStream.Close();
        }
    }
}
