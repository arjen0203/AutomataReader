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
            automata.alphabet = new List<char>();

            string newText = text.Substring(9).Trim();

            foreach (char s in newText)
            {
                automata.alphabet.Add(s);
            }
        }

        public void CreateNodes(string text)
        {
            automata.nodes = new List<Node>();

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
    }
}
