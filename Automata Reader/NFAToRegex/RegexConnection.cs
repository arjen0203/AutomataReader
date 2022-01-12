using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader.NFAToRegex
{
    class RegexConnection
    {
        public string Expression { get; set; }
        public string PreExpr { get; set; }
        public RegexNode ToNode { get; private set; }


        public RegexConnection(string expression, RegexNode toNode)
        {
            this.Expression = expression;
            this.PreExpr = expression;
            this.ToNode = toNode;
        }
        public RegexConnection(string expression, string preExpr, RegexNode toNode)
        {
            this.Expression = expression;
            this.PreExpr = preExpr;
            this.ToNode = toNode;
        }
    }
}
