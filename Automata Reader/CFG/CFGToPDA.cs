using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Automata_Reader
{
    class CFGToPDA
    {
        int stateCounter = 1;
        public Automata CreatePDAFromCFG(StreamReader reader)
        {
            List<CFGVariableRule> variableRules = new List<CFGVariableRule>();
            string rule = reader.ReadLine();
            stateCounter = 1;

            while (!rule.Trim().Equals("end."))
            {
                splitAndSaveTransitions(variableRules, rule);
                rule = reader.ReadLine();
            }

            Automata automata = createStartAutomata();

            addCFGTransitions(automata, variableRules);

            return automata;
        }

        private void splitAndSaveTransitions(List<CFGVariableRule> variableRules, string rule)
        {
            rule = Regex.Replace(rule, @"\s+", "");
            string[] splitup = rule.Split(':');

            for (int i = 0; i < variableRules.Count; i++)
            {
                if (variableRules[i].Variable == splitup[0][0])
                {
                    variableRules[i].TerminalTransitions.Add(splitup[1]);
                    return;
                }
            }

            variableRules.Add(new CFGVariableRule(splitup[0][0], splitup[1]));
        }

        private Automata createStartAutomata()
        {
            Automata automata = new Automata();
            Node startNode = new Node(true, "Qstart");
            Node dollarNode = new Node(false, "Qdollar");
            Node mainCFGNode = new Node(false, "Qloop");
            Node endNode = new Node(false, "Qend", true);

            startNode.Connections.Add(new Connection('_', dollarNode, '$', '_'));
            dollarNode.Connections.Add(new Connection('_', mainCFGNode, 'S', '_'));
            mainCFGNode.Connections.Add(new Connection('_', endNode, '_', '$'));

            automata.stack = new List<char>();
            addStackSymbol('$', automata);
            addStackSymbol('S', automata);

            automata.nodes.Add(startNode);
            automata.nodes.Add(dollarNode);
            automata.nodes.Add(mainCFGNode);
            automata.nodes.Add(endNode);

            return automata;
        }

        public void addCFGTransitions(Automata automata, List<CFGVariableRule> rules)
        {
            foreach (CFGVariableRule variableRule in rules)
            {
                addStackSymbol(variableRule.Variable, automata);
                foreach (string terminals in variableRule.TerminalTransitions)
                {
                    Node currentNode = automata.nodes[2];
                    for (int i = terminals.Length - 1; i > -1; i--)
                    {
                        Node connectionNode;
                        if (0 == i) connectionNode = automata.nodes[2];
                        else
                        {
                            connectionNode = new Node(false, stateCounter++.ToString());
                            automata.nodes.Add(connectionNode);
                        }
                        currentNode.Connections.Add(new Connection('_', connectionNode, terminals[i], (i == terminals.Length - 1 ? variableRule.Variable : '_')));
                        addStackSymbol(terminals[i], automata);
                        addAlphabetSymbol(terminals[i], automata);
                        currentNode = connectionNode;
                    }
                }
            }

            foreach (char symbol in automata.alphabet)
            {
                automata.nodes[2].Connections.Add(new Connection(symbol, automata.nodes[2], '_', symbol));
            }
        }

        public void addStackSymbol(char symbol, Automata automata)
        {
            if (!automata.stack.Contains(symbol) && symbol != '_') automata.stack.Add(symbol);
        }
        public void addAlphabetSymbol(char symbol, Automata automata)
        {
            if (char.IsLower(symbol) && !automata.alphabet.Contains(symbol)) automata.alphabet.Add(symbol);
        }
    }
}
