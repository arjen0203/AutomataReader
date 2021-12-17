using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader.CFG_Code.Transitions
{
    class CFGTerminal : ILetterOrVariable
    {
        public char Symbol { get; private set; }

        public CFGTerminal(char symbol)
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
