using Automata_Reader.NFAToRegex;
using Force.DeepCloner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader
{
    class NFAtoRegEx
    {
        public RegexAutomata ConvertNFAToRegEx(Automata automata)
        {
            RegexAutomata regexAutomata = new RegexAutomata(automata);
            UnionMultipleTransitions(regexAutomata);
            regexAutomata = PruneNodes(regexAutomata);
            return regexAutomata;
        }

        public RegexAutomata PruneNodes(RegexAutomata automata)
        {
            int nodeCount = automata.Nodes.Count - 2;
            if (nodeCount - 2 < 5)
            {
                RegexAutomata smalllestRegexGnfa = null;
                IEnumerable<IEnumerable<int>> allPermutations = GetPermutations(Enumerable.Range(1, nodeCount), nodeCount);
                foreach (IEnumerable<int> permutation in allPermutations)
                {
                    RegexAutomata automataCopy = automata.DeepClone();
                    List<RegexNode> nodeOrder = new List<RegexNode>();
                    foreach (int nodeIndex in permutation)
                    {
                        nodeOrder.Add(automataCopy.Nodes[nodeIndex]);
                    }
                    foreach (RegexNode currentNode in nodeOrder)
                    {
                        RerouteTransitions(currentNode, automataCopy);
                        UnionMultipleTransitions(automataCopy);
                    }
                    Console.WriteLine($"Permutation {{{PermutationString(permutation)}}}: {automataCopy.Nodes[0].Connections[0].PreExpr}");
                    if (smalllestRegexGnfa == null || smalllestRegexGnfa.Nodes[0].Connections[0].PreExpr.Length > automataCopy.Nodes[0].Connections[0].PreExpr.Length)
                    {
                        smalllestRegexGnfa = automataCopy;
                    }
                }
                return smalllestRegexGnfa;
            } else
            {
                while (automata.Nodes.Count > 2)
                {
                    RegexNode currentNode = automata.Nodes[1];

                    RerouteTransitions(currentNode, automata);
                    UnionMultipleTransitions(automata);
                }
                return automata;
            }
        }

        static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public void RerouteTransitions(RegexNode toNode, RegexAutomata automata)
        {
            (List<RegexConnection>, string, string) splitTransitions = SplitSelfLoopingTransition(toNode);
            List<RegexConnection> toTransitionList = splitTransitions.Item1;
            string betweenTransition = splitTransitions.Item2;
            string betweenPreTransition = splitTransitions.Item3;

            foreach (RegexNode node in automata.Nodes)
            {
                //can assume this is only once since we union all double transitions
                RegexConnection pruneConnection = null;
                List<RegexConnection> addList = new List<RegexConnection>();
                if (node != toNode)
                {
                    foreach (RegexConnection trans in node.Connections)
                    {
                        if (trans.ToNode == toNode)
                        {
                            pruneConnection = trans;
                            foreach (RegexConnection toTransition in toTransitionList)
                            { 
                                string newExpression = CreateNewExpressionString(trans.Expression, betweenTransition, toTransition.Expression);
                                string newPreExpr = CreateNewPreExpressionString(trans.PreExpr, betweenPreTransition, toTransition.PreExpr);

                                addList.Add(new RegexConnection(newExpression, newPreExpr, toTransition.ToNode));
                            }
                        }
                    }
                }
                node.Connections.Remove(pruneConnection);
                node.Connections.AddRange(addList);
            }
            automata.Nodes.Remove(toNode);
        }

        private string CreateNewExpressionString(string fromNode, string between, string toNode)
        {
            if (fromNode.Equals("_")) fromNode = "";
            if (between.Equals("_") || between.Equals("(_)*")) between = "";
            if (toNode.Equals("_")) toNode = "";

            string newString = fromNode + between + toNode;

            if (newString.Length == 0) return "_";
            return newString;
        }

        private string CreateNewPreExpressionString(string fromNode, string between, string toNode)
        {
            if (fromNode.Equals("_")) fromNode = "";
            if (between.Equals("_") || between.Equals("*(_)")) between = "";
            if (toNode.Equals("_")) toNode = "";

            string newString;

            if (fromNode.Length > 0)
            {
                newString = fromNode;
                if (between.Length > 0)
                {
                    newString = $".({newString},{between})";
                } 
                if (toNode.Length > 0)
                {
                    newString = $".({newString},{toNode})";
                }                
            } else if (between.Length > 0)
            {
                newString = between;
                if (toNode.Length > 0)
                {
                    newString = $".({newString},{toNode})";
                }
            } else
            {
                newString = toNode;
            }

            if (newString.Length == 0) return "_";
            return newString;
        }


        public (List<RegexConnection>, string, string) SplitSelfLoopingTransition(RegexNode node)
        {
            List<RegexConnection> normalTransitions = new List<RegexConnection>();
            string selfLoopingExpression = "";
            string selfLoopingPreExpression = "";

            foreach (RegexConnection connection in node.Connections)
            {
                if (connection.ToNode == node)
                {
                    if (connection.Expression.Length > 1) selfLoopingExpression = $"({connection.Expression})*";
                    else selfLoopingExpression = $"{connection.Expression}*";
                    selfLoopingPreExpression = $"*({connection.PreExpr})";
                }
                else normalTransitions.Add(connection);
            }

            return (normalTransitions, selfLoopingExpression, selfLoopingPreExpression);
        }

        public void UnionMultipleTransitions(RegexAutomata automata)
        {
            foreach(RegexNode node in automata.Nodes)
            {
                Dictionary<RegexNode, List<RegexConnection>> transitionsPerNode = new Dictionary<RegexNode, List<RegexConnection>>();
                foreach (RegexConnection trans in node.Connections)
                {
                    if (transitionsPerNode.ContainsKey(trans.ToNode)) transitionsPerNode[trans.ToNode].Add(trans);
                    else transitionsPerNode.Add(trans.ToNode, new List<RegexConnection>() { trans });
                }
                if (transitionsPerNode.Count != node.Connections.Count)
                {
                    List<RegexConnection> newTransitions = new List<RegexConnection>();
                    foreach (KeyValuePair<RegexNode, List<RegexConnection>> valuePair in transitionsPerNode)
                    {
                        HashSet<string> uniqueExprs = new HashSet<string>();
                        HashSet<string> uniquePreExprs = new HashSet<string>();
                        foreach (RegexConnection connection in valuePair.Value)
                        {
                            uniqueExprs.Add(connection.Expression);
                            uniquePreExprs.Add(connection.PreExpr);
                        }
                        //infix expr
                        string newExpr = "";
                        foreach (string expr in uniqueExprs)
                        {
                            if (expr.Length == 1 || (expr.Substring(0, 1).Equals("(") && expr.Substring(expr.Length - 1, 1).Equals(")"))) newExpr += $"{expr}∪";
                            else newExpr += $"({expr})∪";
                        }
                        string newPreExpr = "";
                        //prefix expr
                        bool firstPreExpr = false;
                        foreach (string preExpr in uniquePreExprs)
                        {
                            if (!firstPreExpr) 
                            {
                                firstPreExpr = true;
                                newPreExpr = preExpr;
                            }
                            else
                            {
                                newPreExpr = $"|({newPreExpr},{preExpr})";
                            }
                        }
                        newTransitions.Add(new RegexConnection(newExpr.Substring(0, newExpr.Length - 1), newPreExpr, valuePair.Key));
                    }
                    node.Connections = newTransitions;
                }
            }
        }

        public string CreateRegexAutomataDotString(RegexAutomata automata)
        {
            string dotString = "";

            dotString += "digraph myAutomaton { \nrankdir=LR; \n\"\" [shape=none] \n";

            foreach (RegexNode node in automata.Nodes)
            {
                string shape = "circle";
                if (node.Final) shape = "doublecircle";
                dotString += $"\"{node.Name}\" [shape={shape}] \n";
            }

            dotString += "\n";

            
            dotString += $"\"\" -> \"{automata.Nodes[0].Name}\" \n";
            

            foreach (RegexNode node in automata.Nodes)
            {
                foreach (RegexConnection conn in node.Connections)
                {
                    string expres = conn.Expression.Replace("∪", "|");
                    dotString += $"\"{node.Name}\" -> \"{conn.ToNode.Name}\" [label=\"{expres}\"] \n";
                }
            }

            dotString += "}";

            return dotString;
        }

        public string PermutationString(IEnumerable<int> list)
        {
            string output = "";
            foreach (int i in list) output += $"{i}, ";
            return output.Substring(0, output.Length - 2);
        }
    }
}
