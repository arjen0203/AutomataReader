using Automata_Reader.CFG;
using Automata_Reader.CFG.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader
{
    class PDAToCFG
    {
        public string ConvertPDAToCFGToString(Automata automata)
        {
            Dictionary<string, ConvertTransition> AllTransitions = new Dictionary<string, ConvertTransition>();

            AddType1AndType2Transitions(automata, AllTransitions);

            return PrintTransitions(AllTransitions);
        }

        public void AddType1AndType2Transitions(Automata automata, Dictionary<string, ConvertTransition> allTransitions)
        {
            foreach(Node fromNode in automata.nodes)
            {
                foreach (Node toNode in automata.nodes)
                {
                    ConvertTransition transition = GetOrCreateNewTransition(fromNode, toNode, allTransitions);

                    //type 1 transitions
                    if (fromNode == toNode) transition.ToVariablesOrLetters.Add(new List<IConvertLetterOrTransition>() { new ConvertLetter('_')});
                        
                    //type 2 transitions
                    foreach(Node betweenNode in automata.nodes)
                    {
                        transition.ToVariablesOrLetters.Add(new List<IConvertLetterOrTransition>() { 
                            GetOrCreateNewTransition(fromNode, betweenNode, allTransitions), 
                            GetOrCreateNewTransition(betweenNode, toNode, allTransitions) 
                        });
                    }
                    
                }
            }
        }
        private ConvertTransition GetOrCreateNewTransition(Node leftNode, Node rightNode, Dictionary<string, ConvertTransition> allTransitions)
        {
            string key = $"{leftNode.Name}{rightNode.Name}";
            if (allTransitions.ContainsKey(key)) return allTransitions[key];
            ConvertTransition newTransition = new ConvertTransition(leftNode, rightNode);
            allTransitions.Add(key, newTransition);
            return newTransition;
        }

        private string PrintTransitions(Dictionary<string, ConvertTransition> allTransitions)
        {
            string output = "";
            foreach (KeyValuePair<string, ConvertTransition> valuePair in allTransitions)
            {
                output += $"{valuePair.Value.ReturnString()}";
            }
            return output;
        }
    }
}
