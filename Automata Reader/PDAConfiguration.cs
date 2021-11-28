using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader
{
    class PDAConfiguration
    {
        public Stack<char> Stack { get; private set; }
        public Node Node { get; private set; }
        public Queue<char> Word { get; private set; }
        public int Depth;

        public PDAConfiguration(Stack<char> stack, Node node, Queue<char> word, int depth)
        {
            this.Stack = stack;
            this.Node = node;
            this.Word = word;
            this.Depth = depth;
        }
        public void print()
        {
            Console.WriteLine($"Depth: {this.Depth}, Node: {this.Node.Name}, Stack:{returnStackString()}, word: {returnWordString()}");
        }
        public string CreateHashString()
        {
            return $"{this.Node.Name},{returnStackString()},{returnWordString()}";
        }
        private string returnStackString()
        {
            string stackString = "";
            foreach (char letter in this.Stack)
            {
                stackString += letter;
            }
            return stackString;
        }
        private string returnWordString()
        {
            string wordString = "";
            foreach (char letter in this.Word)
            {
                wordString += letter;
            }
            return wordString;
        }
    }
}
