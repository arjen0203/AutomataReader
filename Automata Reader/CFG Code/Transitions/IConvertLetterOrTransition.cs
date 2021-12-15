using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader.CFG_Code.Transitions
{
    interface IConvertLetterOrTransition
    {
        bool IsVariable();
    }
}
