using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Automata_Reader
{
    class RegexLogic
    {
        private int StateCounter = 1;
        public Automata processRegex(string regex)
        {
            regex = Regex.Replace(regex, @"\s+", "");
            StateCounter = 1;

            Automata regexAutomata;

            regexAutomata = ThompsonStep(regex);

            if (regexAutomata.nodes.Count > 0)
            {
                regexAutomata.nodes[0].Starting = true;
                regexAutomata.nodes[regexAutomata.nodes.Count - 1].Final = true;
            }

            return regexAutomata;
        }

        private Automata ThompsonStep(string regex)
        {
            switch (regex[0])
            {
                case '*':
                    return KleeneStep(regex);
                case '.':
                    return ContentinationStep(regex);
                case '|':
                    return UnionStep(regex);
                case '_':
                    return EmptyStep(regex);
                //default in this case means any char that is not specified thus it must be a letter
                default:
                    return LetterStep(regex);
            }
        }
        private Automata KleeneStep(string regex)
        {
            if (regex[1] == '(') regex = regex.Substring(2, regex.Length - 3);
            else regex = regex.Substring(1, regex.Length - 1);
            Automata automata = ThompsonStep(regex);

            Automata kleenAutomata = new Automata();
            kleenAutomata.alphabet = automata.alphabet;

            Node newStartNode = new Node(false, StateCounter++.ToString());
            List<Node> subNodes = automata.nodes;
            Node newFinalNode = new Node(false, StateCounter++.ToString());

            newStartNode.Connections.Add(new Connection('_', subNodes[0]));
            newStartNode.Connections.Add(new Connection('_', newFinalNode));
            subNodes[subNodes.Count - 1].Connections.Add(new Connection('_', subNodes[0]));
            subNodes[subNodes.Count - 1].Connections.Add(new Connection('_', newFinalNode));

            kleenAutomata.nodes.Add(newStartNode);
            kleenAutomata.nodes.AddRange(automata.nodes);
            kleenAutomata.nodes.Add(newFinalNode);

            return kleenAutomata;
        }
        private Automata ContentinationStep(string regex)
        {
            (string, string) splitup = trimAndSplitByComma(regex);

            Automata automataLeft = ThompsonStep(splitup.Item1);
            Automata automataRight = ThompsonStep(splitup.Item2);

            Automata contentinationAutomata = new Automata();
            contentinationAutomata.alphabet = combineAlphabets(automataLeft.alphabet, automataRight.alphabet);

            List<Node> subNodesLeft = automataLeft.nodes;
            List<Node> subNodesRight = automataRight.nodes;

            Node betweenNode = subNodesLeft[subNodesLeft.Count - 1];
            betweenNode.Connections.AddRange(subNodesRight[0].Connections);
            subNodesRight.RemoveAt(0);

            contentinationAutomata.nodes.AddRange(automataLeft.nodes);
            contentinationAutomata.nodes.AddRange(automataRight.nodes);

            return contentinationAutomata;
        }

        private Automata UnionStep(string regex)
        {
            (string, string) splitup = trimAndSplitByComma(regex);
            Automata automataLeft = ThompsonStep(splitup.Item1);
            Automata automataRight = ThompsonStep(splitup.Item2);

            Automata unionAutomata = new Automata();
            unionAutomata.alphabet = combineAlphabets(automataLeft.alphabet, automataRight.alphabet);

            Node newStartNode = new Node(false, StateCounter++.ToString());
            List<Node> subNodesLeft = automataLeft.nodes;
            List<Node> subNodesRight = automataRight.nodes;
            Node newFinalNode = new Node(false, StateCounter++.ToString());

            newStartNode.Connections.Add(new Connection('_', subNodesLeft[0]));
            newStartNode.Connections.Add(new Connection('_', subNodesRight[0]));
            subNodesLeft[subNodesLeft.Count - 1].Connections.Add(new Connection('_', newFinalNode));
            subNodesRight[subNodesRight.Count - 1].Connections.Add(new Connection('_', newFinalNode));

            unionAutomata.nodes.Add(newStartNode);
            unionAutomata.nodes.AddRange(automataLeft.nodes);
            unionAutomata.nodes.AddRange(automataRight.nodes);
            unionAutomata.nodes.Add(newFinalNode);

            return unionAutomata;
        }

        private Automata EmptyStep(string regex)
        {
            return CreateSymbolAutomata('_');
        }

        private Automata LetterStep(string regex)
        {
            return CreateSymbolAutomata(regex[0]);
        }

        public Automata CreateSymbolAutomata(char symbol)
        {
            Automata automata = new Automata();

            Node startNode = new Node(false, StateCounter++.ToString());
            Node endNode = new Node(false, StateCounter++.ToString());
            startNode.Connections.Add(new Connection(symbol, endNode));

            automata.nodes.Add(startNode);
            automata.nodes.Add(endNode);

            if (symbol != '_') automata.alphabet.Add(symbol);

            return automata;
        }

        private string trimRegex(string regex)
        {
            if (regex[1] == '(') regex = regex.Substring(2, regex.Length - 3);
            else regex = regex.Substring(1, regex.Length - 1);
            return regex;
        }

        private (string, string) trimAndSplitByComma(string regex)
        {
            int commaPos = -1;
            regex = trimRegex(regex);

            int counter = 0;
            for (int i = 0; i < regex.Length; i++)
            {
                if (regex[i] == '(') counter++;
                else if (regex[i] == ')') counter--;
                else if (regex[i] == ',' && counter == 0) commaPos = i;
            }

            return (regex.Substring(0, commaPos), regex.Substring(commaPos + 1, regex.Length - commaPos - 1));
        }

        private List<char> combineAlphabets(List<char> leftChars, List<char> rightChars)
        {
            var combined = new List<char>(leftChars);
            foreach (char chr in rightChars)
            {
                if (!combined.Contains(chr)) combined.Add(chr);
            }
            return combined;
        }
    }
}
