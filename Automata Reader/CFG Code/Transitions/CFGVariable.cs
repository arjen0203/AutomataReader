using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader.CFG_Code.Transitions
{
    class CFGVariable : ILetterOrVariable
    {
        private static readonly char[] stateAlphabet = new char[26] { 'S', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        public Node FromNode { get; private set; }
        public Node ToNode { get; private set; }
       
        public List<List<ILetterOrVariable>> ToVariablesOrLetters { get; private set; }

        public CFGVariable(Node fromNode, Node toNode)
        {
            this.FromNode = fromNode;
            this.ToNode = toNode;
            this.ToVariablesOrLetters = new List<List<ILetterOrVariable>>();
        }

        public bool IsVariable()
        {
            return true;
        }

        public List<CFGVariable> ReturnAllTransitionVariables()
        {
            List<CFGVariable> AllVariables = new List<CFGVariable>();
            foreach (List<ILetterOrVariable> outputList in ToVariablesOrLetters)
            {
                foreach (ILetterOrVariable letterOrTrans in outputList)
                {
                    if (letterOrTrans.IsVariable()) AllVariables.Add((CFGVariable)letterOrTrans);
                }
            }
            return AllVariables;
        }

        public bool ContainsTerminals()
        {
            foreach (List<ILetterOrVariable> outputList in ToVariablesOrLetters)
            {
                foreach (ILetterOrVariable letterOrTrans in outputList)
                {
                    if (!letterOrTrans.IsVariable()) return true;
                }
            }
            return false;
        }



        public string ReturnString(Dictionary<string, char> charStates)
        {
            string fromVariable = $"{this.FromNode.Name}{this.ToNode.Name}";
            string output = "";
            foreach (List<ILetterOrVariable> outputList in ToVariablesOrLetters)
            {
                output += $"{GetNewStateChar(fromVariable, charStates)} : ";
                foreach (ILetterOrVariable letterOrTrans in outputList)
                {
                    if (!letterOrTrans.IsVariable()) output += $"{letterOrTrans} ";
                    else output += $"{GetNewStateChar(letterOrTrans.ToString(), charStates)} ";
                }
                output += "\r\n";
            }
            return output;
        }
        private char GetNewStateChar(string inputState, Dictionary<string, char> charStates)
        {
            
            if (!charStates.ContainsKey(inputState))
            {
                charStates.Add(inputState, stateAlphabet[charStates.Count]);
                return stateAlphabet[charStates.Count - 1];
            }
            return charStates[inputState];
        }
        public override string ToString()
        {
            return $"{this.FromNode.Name}{this.ToNode.Name}";
        }
    }
}
