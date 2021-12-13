﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader.CFG.Transitions
{
    class ConvertTransition : IConvertLetterOrTransition
    {
        public Node FromNode { get; private set; }
        public Node ToNode { get; private set; }
       
        public List<List<IConvertLetterOrTransition>> ToVariablesOrLetters { get; private set; }

        public ConvertTransition(Node fromNode, Node toNode)
        {
            this.FromNode = fromNode;
            this.ToNode = toNode;
            this.ToVariablesOrLetters = new List<List<IConvertLetterOrTransition>>();
        }

        public bool IsVariable()
        {
            return true;
        }

        public string ReturnString()
        {
            string fromVariable = $"{this.FromNode.Name}{this.ToNode.Name}";
            string output = "";
            foreach (List<IConvertLetterOrTransition> outputList in ToVariablesOrLetters)
            {
                output += $"{fromVariable} : ";
                foreach (IConvertLetterOrTransition letterOrTrans in outputList)
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
