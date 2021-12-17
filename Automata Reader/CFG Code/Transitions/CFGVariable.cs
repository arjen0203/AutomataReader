using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader.CFG_Code.Transitions
{
    class CFGVariable : ILetterOrVariable
    {
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



        public string ReturnString()
        {
            string fromVariable = $"{this.FromNode.Name}{this.ToNode.Name}";
            string output = "";
            foreach (List<ILetterOrVariable> outputList in ToVariablesOrLetters)
            {
                output += $"{fromVariable} : ";
                foreach (ILetterOrVariable letterOrTrans in outputList)
                {
                    output += $"{letterOrTrans} ";
                }
                output += "\r\n";
            }
            return output;
        }
        public override string ToString()
        {
            return $"{this.FromNode.Name}{this.ToNode.Name}";
        }
    }
}
