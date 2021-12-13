using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader
{
    class CFGVariableRule
    {
        public char Variable { get; private set; }
        public List<string> TerminalTransitions { get; private set; }

        public CFGVariableRule(char variable, string initualTrans)
        {
            this.Variable = variable;
            this.TerminalTransitions = new List<string>() { initualTrans };
        }
    }
}
