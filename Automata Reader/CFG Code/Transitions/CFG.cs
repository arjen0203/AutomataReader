using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader.CFG_Code.Transitions
{
    class CFG
    {
        public Dictionary<char, CFGTerminal> Terminals { get; private set; }
        public Dictionary<string, CFGVariable> AllTransitions { get; private set; }
        public CFGVariable StartVariable { get; private set; }

        public CFG(Node startLeftNode, Node startRightNode)
        {
            this.Terminals = new Dictionary<char, CFGTerminal>();
            Terminals.Add('_', new CFGTerminal('_'));
            this.AllTransitions = new Dictionary<string, CFGVariable>();
            CFGVariable startVariable = new CFGVariable(startLeftNode, startRightNode);
            AllTransitions.Add($"{startLeftNode.Name}{startRightNode.Name}", startVariable);
            this.StartVariable = startVariable;
        }

        public void PruneNotIncludedVariables(HashSet<CFGVariable> variables)
        {
            foreach (KeyValuePair<string, CFGVariable> valuePair in this.AllTransitions)
            {
                List<List<ILetterOrVariable>> pruneList = new List<List<ILetterOrVariable>>();
                foreach (List<ILetterOrVariable> transition in valuePair.Value.ToVariablesOrLetters)
                {
                    foreach (ILetterOrVariable letOrTrans in transition)
                    {
                        if (letOrTrans.IsVariable() && !variables.Contains(letOrTrans)) pruneList.Add(transition);
                    }
                }
                foreach (List<ILetterOrVariable> prune in pruneList)
                {
                    valuePair.Value.ToVariablesOrLetters.Remove(prune);
                }
            }
        }

        public void PruneNotIncludedVariablesAndSymbol(HashSet<ILetterOrVariable> varAndSymb)
        {
            List<string> pruneListVariables = new List<string>();
            foreach (KeyValuePair<string, CFGVariable> valuePair in this.AllTransitions)
            {
                if (!varAndSymb.Contains(valuePair.Value)) pruneListVariables.Add(valuePair.Key);
            }

            foreach (string pruneKey in pruneListVariables)
            {
                this.AllTransitions.Remove(pruneKey);
            }

            foreach (KeyValuePair<string, CFGVariable> valuePair in this.AllTransitions)
            {
                List<List<ILetterOrVariable>> pruneList = new List<List<ILetterOrVariable>>();
                foreach (List<ILetterOrVariable> transition in valuePair.Value.ToVariablesOrLetters)
                {
                    foreach (ILetterOrVariable letOrTrans in transition)
                    {
                        if (!varAndSymb.Contains(letOrTrans)) pruneList.Add(transition);
                    }
                }
                foreach (List<ILetterOrVariable> prune in pruneList)
                {
                    valuePair.Value.ToVariablesOrLetters.Remove(prune);
                }
            }
        }
        public int ReturnTotalTransitionAmount()
        {
            int counter = 0;
            foreach (KeyValuePair<string, CFGVariable> valuePair in this.AllTransitions)
            {
                foreach (List<ILetterOrVariable> transition in valuePair.Value.ToVariablesOrLetters)
                {
                    counter++;
                }
            }
            return counter;
        }
    }
}
