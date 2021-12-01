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

            return null;
        }

        private Automata ThompsonStep(string regex)
        {
            switch (regex[0])
            {
                case '*':
                    return KleeneStep(regex);
                case '.':
                    break;
                case '|':
                    return UnionStep(regex);
                case '_':
                    break;
                case '(':
                    break;
                case '\0':
                    break;
                //default in this case means any char that is not specified thus it must be a letter
                default:
                    break;
            }
        }
        private Automata KleeneStep(string regex)
        {
            if (regex[1] == '(') regex = regex.Substring(2, regex.Length - 3);
            else regex = regex.Substring(1, regex.Length - 1);
            Automata automata = ThompsonStep(regex);

            Automata kleenAutomata = new Automata();
            kleenAutomata.alphabet = automata.alphabet;

            Node newStartNode = new Node(false, StateCounter.ToString());
            List<Node> subNodes = automata.nodes;
            Node newFinalNode = new Node(false, StateCounter.ToString());

            newStartNode.Connections.Add(new Connection('_', subNodes[0]));
            subNodes[subNodes.Count - 1].Connections.Add(new Connection('_', subNodes[0]));
            subNodes[subNodes.Count - 1].Connections.Add(new Connection('_', newFinalNode));

            kleenAutomata.nodes.Add(newStartNode);
            kleenAutomata.nodes.AddRange(automata.nodes);
            kleenAutomata.nodes.Add(newFinalNode);

            return automata;
        }
        private Automata ContentinationStep(string regex)
        {
            regex = trimRegex(regex);
            Automata automata = ThompsonStep(regex);

            (string, string) splitup = splitByComma(regex);

            return automata;
        }

        private Automata UnionStep(string regex)
        {
            regex = trimRegex(regex);
            

            (string, string) splitup = splitByComma(regex);
            Automata automataLeft = ThompsonStep(splitup.Item1);
            Automata automataRight = ThompsonStep(splitup.Item2);

            Automata unionAutomata = new Automata();
            unionAutomata.alphabet = combineAlphabets(automataLeft.alphabet, automataRight.alphabet);

            Node newStartNode = new Node(false, StateCounter.ToString());
            List<Node> subNodesLeft = automataLeft.nodes;
            List<Node> subNodesRight = automataRight.nodes;
            Node newFinalNode = new Node(false, StateCounter.ToString());

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
            Automata automata = ThompsonStep(regex);

            //do empty

            return automata;
        }

        private Automata LetterStep(string regex)
        {
            Automata automata = ThompsonStep(regex);

            //do Contentination

            return automata;
        }

        private string trimRegex(string regex)
        {
            if (regex[1] == '(') regex = regex.Substring(2, regex.Length - 3);
            else regex = regex.Substring(1, regex.Length - 1);
            return regex;
        }

        private (string, string) splitByComma(string regex)
        {
            int commaPos = -1;

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
