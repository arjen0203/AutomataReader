using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader
{
    class TestWord
    {
        public char[] word { get; set; }
        public bool accapted { get; set;}
        public TestWord(string wrd, bool acc)
        {
            this.word = wrd.ToCharArray();
            this.accapted = acc;
        }
    }
}
