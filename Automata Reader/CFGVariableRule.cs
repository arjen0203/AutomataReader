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
        public List<string> TerminalTransition { get; private set; }

        public CFGVariableRule(char variable, string initualTrans)
        {
            this.Variable = variable;
            this.TerminalTransition = new List<string>() { initualTrans };
        }
    }
}
