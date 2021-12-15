using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader.CFG_Code.Transitions
{
    class CFG
    {
        public Dictionary<char, ConvertLetter> Terminals { get; private set; }
        public Dictionary<string, ConvertTransition> AllTransitions { get; private set; }

        public CFG()
        {
            this.Terminals = new Dictionary<char, ConvertLetter>();
            Terminals.Add('_', new ConvertLetter('_'));
            this.AllTransitions = new Dictionary<string, ConvertTransition>();
        }

        public void PruneNotIncludedVariables(HashSet<ConvertTransition> variables)
        {
            foreach (KeyValuePair<string, ConvertTransition> valuePair in this.AllTransitions)
            {
                List<List<IConvertLetterOrTransition>> pruneList = new List<List<IConvertLetterOrTransition>>();
                foreach (List<IConvertLetterOrTransition> transition in valuePair.Value.ToVariablesOrLetters)
                {
                    foreach (IConvertLetterOrTransition letOrTrans in transition)
                    {
                        if (letOrTrans.IsVariable() && !variables.Contains(letOrTrans)) pruneList.Add(transition);
                    }
                }
                foreach (List<IConvertLetterOrTransition> prune in pruneList)
                {
                    valuePair.Value.ToVariablesOrLetters.Remove(prune);
                }
            }
        }

        public void PruneNotIncludedVariablesAndSymbol(HashSet<IConvertLetterOrTransition> varAndSymb)
        {
            List<string> pruneListVariables = new List<string>();
            foreach (KeyValuePair<string, ConvertTransition> valuePair in this.AllTransitions)
            {
                if (!varAndSymb.Contains(valuePair.Value)) pruneListVariables.Add(valuePair.Key);
            }

            foreach (string pruneKey in pruneListVariables)
            {
                this.AllTransitions.Remove(pruneKey);
            }

            foreach (KeyValuePair<string, ConvertTransition> valuePair in this.AllTransitions)
            {
                List<List<IConvertLetterOrTransition>> pruneList = new List<List<IConvertLetterOrTransition>>();
                foreach (List<IConvertLetterOrTransition> transition in valuePair.Value.ToVariablesOrLetters)
                {
                    foreach (IConvertLetterOrTransition letOrTrans in transition)
                    {
                        if (!varAndSymb.Contains(letOrTrans)) pruneList.Add(transition);
                    }
                }
                foreach (List<IConvertLetterOrTransition> prune in pruneList)
                {
                    valuePair.Value.ToVariablesOrLetters.Remove(prune);
                }
            }
        }
        public int ReturnTotalTransitionAmount()
        {
            int counter = 0;
            foreach (KeyValuePair<string, ConvertTransition> valuePair in this.AllTransitions)
            {
                foreach (List<IConvertLetterOrTransition> transition in valuePair.Value.ToVariablesOrLetters)
                {
                    counter++;
                }
            }
            return counter;
        }
    }
}
