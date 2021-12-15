using Automata_Reader.CFG_Code.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata_Reader.CFG_Code
{
    class PDAToCFG
    {
        public string ConvertPDAToCFGToString(Automata automata)
        {
            CFG cfg = new CFG();


            AddType1AndType2Transitions(automata, cfg);
            cfg = testCFG();
            SimplifyCFG(cfg);


            return PrintTransitions(cfg);
        }

        public void AddType1AndType2Transitions(Automata automata, CFG cfg)
        {
            foreach(Node fromNode in automata.nodes)
            {
                foreach (Node toNode in automata.nodes)
                {
                    ConvertTransition transition = GetOrCreateNewTransition(fromNode, toNode, cfg.AllTransitions);

                    //type 1 transitions
                    if (fromNode == toNode) transition.ToVariablesOrLetters.Add(new List<IConvertLetterOrTransition>() { GetOrCreateNewSymbol('_', cfg) });

                    //type 2 transitions
                    foreach (Node betweenNode in automata.nodes)
                    {
                        transition.ToVariablesOrLetters.Add(new List<IConvertLetterOrTransition>() {
                            GetOrCreateNewTransition(fromNode, betweenNode, cfg.AllTransitions),
                            GetOrCreateNewTransition(betweenNode, toNode, cfg.AllTransitions)
                        });
                    }

                }
                //type 3 transitions
                foreach (Connection conn in fromNode.Connections)
                {
                    if (conn.PushStack != '_')
                    {
                        foreach (Node secondConnNode in automata.nodes)
                        {
                            foreach (Connection secondConn in secondConnNode.Connections)
                            {
                                if (secondConn.PopStack == conn.PushStack)
                                {
                                    ConvertTransition type3Transition = GetOrCreateNewTransition(fromNode, secondConn.ToNode, cfg.AllTransitions);
                                    type3Transition.ToVariablesOrLetters.Add(new List<IConvertLetterOrTransition>() {
                                        GetOrCreateNewSymbol(conn.Symbol, cfg),
                                        GetOrCreateNewTransition(conn.ToNode, secondConnNode, cfg.AllTransitions),
                                        GetOrCreateNewSymbol(secondConn.Symbol, cfg),
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SimplifyCFG(CFG cfg)
        {
            bool changes = true;
            while (changes)
            {
                changes = false;

                //phase 1 changes
                changes = CFGReduction(cfg);
            }
        }

        private bool CFGReduction(CFG cfg)
        {
            int transAmountStartCFG = cfg.ReturnTotalTransitionAmount();

            CFGReductionPhase1(cfg);
            //phase 2 assuming SS (first trans variable) is start

            HashSet<IConvertLetterOrTransition> allReachableTransitions = new HashSet<IConvertLetterOrTransition>();
            allReachableTransitions.Add(cfg.AllTransitions["SS"]);
            int transitionLength = 0;
            while (transitionLength != allReachableTransitions.Count())
            {
                transitionLength = allReachableTransitions.Count();

                HashSet<IConvertLetterOrTransition> allReachableTransitionsCopy = new HashSet<IConvertLetterOrTransition>(allReachableTransitions);
                foreach (IConvertLetterOrTransition reachableTransition in allReachableTransitionsCopy)
                {
                    if (reachableTransition.IsVariable())
                    {
                        foreach (List<IConvertLetterOrTransition> varOrLetList in cfg.AllTransitions[reachableTransition.ToString()].ToVariablesOrLetters)
                        {
                            foreach (IConvertLetterOrTransition varOrLet in varOrLetList)
                            {
                                allReachableTransitions.Add(varOrLet);
                            }
                        }
                    }
                }
            }

            cfg.PruneNotIncludedVariablesAndSymbol(allReachableTransitions);

            if (transAmountStartCFG != cfg.ReturnTotalTransitionAmount()) return true;
            return false;
        }

        private void CFGReductionPhase1(CFG cfg)
        {
            //phase 1
            HashSet<ConvertTransition> allNeededTransitions = new HashSet<ConvertTransition>();

            //create first list containing terminal symbols
            foreach (KeyValuePair<string, ConvertTransition> valuePair in cfg.AllTransitions)
            {
                if (valuePair.Value.ContainsTerminals()) allNeededTransitions.Add(valuePair.Value);
            }

            //create hashset of variables that can reach a terminal
            int transitionLength = 0;
            while (transitionLength != allNeededTransitions.Count())
            {
                transitionLength = allNeededTransitions.Count();
                foreach (KeyValuePair<string, ConvertTransition> valuePair in cfg.AllTransitions)
                {
                    foreach (ConvertTransition variable in valuePair.Value.ReturnAllTransitionVariables())
                    {
                        if (allNeededTransitions.Contains(variable)) allNeededTransitions.Add(valuePair.Value);
                    }
                }
            }

            cfg.PruneNotIncludedVariables(allNeededTransitions);
        }

        private ConvertTransition GetOrCreateNewTransition(Node leftNode, Node rightNode, Dictionary<string, ConvertTransition> allTransitions)
        {
            string key = $"{leftNode.Name}{rightNode.Name}";
            if (allTransitions.ContainsKey(key)) return allTransitions[key];
            ConvertTransition newTransition = new ConvertTransition(leftNode, rightNode);
            allTransitions.Add(key, newTransition);
            return newTransition;
        }

        private ConvertLetter GetOrCreateNewSymbol(char symbol, CFG cfg)
        {
            if (cfg.Terminals.ContainsKey(symbol)) return cfg.Terminals[symbol];
            ConvertLetter newLetter = new ConvertLetter(symbol);
            cfg.Terminals.Add(symbol, newLetter);
            return newLetter;
        }

        private string PrintTransitions(CFG cfg)
        {
            string output = "";
            foreach (KeyValuePair<string, ConvertTransition> valuePair in cfg.AllTransitions)
            {
                output += $"{valuePair.Value.ReturnString()}";
            }
            return output;
        }

        private CFG testCFG()
        {
            CFG cfg = new CFG();
            ConvertLetter aLet = new ConvertLetter('a');
            ConvertLetter cLet = new ConvertLetter('c');
            ConvertLetter eLet = new ConvertLetter('e');
            cfg.Terminals.Add('a', aLet);
            cfg.Terminals.Add('c', cLet);
            cfg.Terminals.Add('e', eLet);

            ConvertTransition sTrans = new ConvertTransition(new Node("S"), new Node("S"));
            ConvertTransition aTrans = new ConvertTransition(new Node("A"), new Node("A"));
            ConvertTransition bTrans = new ConvertTransition(new Node("B"), new Node("B"));
            ConvertTransition cTrans = new ConvertTransition(new Node("C"), new Node("C"));
            ConvertTransition eTrans = new ConvertTransition(new Node("E"), new Node("E"));
            sTrans.ToVariablesOrLetters.Add(new List<IConvertLetterOrTransition>() { aTrans, cTrans });
            sTrans.ToVariablesOrLetters.Add(new List<IConvertLetterOrTransition>() { bTrans });
            aTrans.ToVariablesOrLetters.Add(new List<IConvertLetterOrTransition>() { aLet });
            cTrans.ToVariablesOrLetters.Add(new List<IConvertLetterOrTransition>() { cLet });
            cTrans.ToVariablesOrLetters.Add(new List<IConvertLetterOrTransition>() { bTrans, cTrans });
            eTrans.ToVariablesOrLetters.Add(new List<IConvertLetterOrTransition>() { aLet, aTrans });
            eTrans.ToVariablesOrLetters.Add(new List<IConvertLetterOrTransition>() { eLet });

            cfg.AllTransitions.Add("SS", sTrans);
            cfg.AllTransitions.Add("AA", aTrans);
            cfg.AllTransitions.Add("CC", cTrans);
            cfg.AllTransitions.Add("EE", eTrans);

            return cfg;
        }
    }
}
