using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader.CFG_Code.Transitions
{
    class ConvertLetter : IConvertLetterOrTransition
    {
        public char Symbol { get; private set; }

        public ConvertLetter(char symbol)
        {
            this.Symbol = symbol;
        }

        public bool IsVariable()
        {
            return false;
        }

        public override string ToString()
        {
            return Symbol.ToString();
        }
    }
}
