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
        public TestWord(TestWord testWord)
        {
            this.word = new char[testWord.word.Length];
            Array.Copy(testWord.word, this.word, testWord.word.Length);
            this.accapted = testWord.accapted;
        }
    }
}
