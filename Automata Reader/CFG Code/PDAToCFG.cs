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
            CFG cfg = new CFG(automata.nodes[0], automata.nodes[automata.nodes.Count - 1]);


            AddType1AndType2Transitions(automata, cfg);
            //cfg = testCFG();
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
            int changesCount = 1;
            while (changesCount > 0)
            {
                changesCount = 0;
                CleanCFG(cfg);
                if (CFGReduction(cfg)) changesCount++;
                CleanCFG(cfg);
                if (SubstituteEmptyTransitions(cfg)) changesCount++;
                CleanCFG(cfg);
            }
        }

        private void CleanCFG(CFG cfg)
        {
            RemoveUnneededEpsilon(cfg);
            RemoveSelfPointingTrans(cfg);
            RemoveDoubles(cfg);
        }

        private bool CFGReduction(CFG cfg)
        {
            int transAmountStartCFG = cfg.ReturnTotalTransitionAmount();

            CFGReductionPhase1(cfg);
            CFGReductionPhase2(cfg);

            if (transAmountStartCFG != cfg.ReturnTotalTransitionAmount()) return true;
            return false;
        }

        private void RemoveDoubles(CFG cfg)
        {
            foreach (KeyValuePair<string, ConvertTransition> valuePair in cfg.AllTransitions)
            {
                List<List<IConvertLetterOrTransition>> transitions = valuePair.Value.ToVariablesOrLetters;
                List<List<IConvertLetterOrTransition>> pruneList = new List<List<IConvertLetterOrTransition>>();
                int startJ = 0;
                for (int i = 0; i < transitions.Count; i++)
                {
                    startJ++;
                    for (int j = startJ; j < transitions.Count; j++)
                    {
                        if (i != j)
                        {
                            if (AreListEqual(transitions[i], transitions[j])) pruneList.Add(transitions[j]);
                        }
                    }
                }
                foreach (List<IConvertLetterOrTransition> prune in pruneList)
                {
                    valuePair.Value.ToVariablesOrLetters.Remove(prune);
                }
            }
        }
 
        private bool AreListEqual(List<IConvertLetterOrTransition> list1, List<IConvertLetterOrTransition> list2)
        {
            if (list1.Count != list2.Count) return false;
            for (int i = 0; i < list1.Count; i++) {
                if (list1[i] != list2[i]) return false;
            }
            return true;
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
                    foreach (List<IConvertLetterOrTransition> transition in valuePair.Value.ToVariablesOrLetters)
                    {
                        int counter = 0;
                        foreach (IConvertLetterOrTransition letOrVar in transition)
                        {
                            if (allNeededTransitions.Contains(letOrVar)) counter++;
                        }
                        if (counter == transition.Count) allNeededTransitions.Add(valuePair.Value);
                    }
                }
            }

            cfg.PruneNotIncludedVariables(allNeededTransitions);
        }

        private void CFGReductionPhase2(CFG cfg)
        {
            HashSet<IConvertLetterOrTransition> allReachableTransitions = new HashSet<IConvertLetterOrTransition>();
            allReachableTransitions.Add(cfg.StartVariable);
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
        }

        private void RemoveSelfPointingTrans(CFG cfg)
        {
            
            foreach (KeyValuePair<string, ConvertTransition> valuePair in cfg.AllTransitions)
            {
                List<List<IConvertLetterOrTransition>> pruneList = new List<List<IConvertLetterOrTransition>>();
                foreach (List<IConvertLetterOrTransition> transition in valuePair.Value.ToVariablesOrLetters)
                {
                    if (transition.Count == 1 && transition[0].ToString().Equals(valuePair.Value.ToString())) pruneList.Add(transition);
                }
                foreach (List<IConvertLetterOrTransition> prune in pruneList)
                {
                    valuePair.Value.ToVariablesOrLetters.Remove(prune);
                }
            }
        }

        public void RemoveUnneededEpsilon(CFG cfg)
        {
            foreach (KeyValuePair<string, ConvertTransition> valuePair in cfg.AllTransitions)
            {
                foreach (List<IConvertLetterOrTransition> transition in valuePair.Value.ToVariablesOrLetters)
                {
                    transition.RemoveAll(a => a == cfg.Terminals['_']);
                    //add 1 epsilon if would be empty
                    if (transition.Count == 0) transition.Add(cfg.Terminals['_']);
                }
            }
        }

        private bool SubstituteEmptyTransitions(CFG cfg)
        {
            int transAmountStartCFG = cfg.ReturnTotalTransitionAmount();
            foreach (KeyValuePair<string, ConvertTransition> epsilonVariable in cfg.AllTransitions)
            {
                List<List<IConvertLetterOrTransition>> pruneList = new List<List<IConvertLetterOrTransition>>();
                List<List<IConvertLetterOrTransition>> transitions = epsilonVariable.Value.ToVariablesOrLetters;
                int startCount = transitions.Count;
                for (int i = 0; i < startCount; i++) {
                    if (transitions[i].Count == 1 && transitions[i][0].ToString().Equals("_"))
                    {
                        pruneList.Add(transitions[i]);
                        foreach (KeyValuePair<string, ConvertTransition> variable in cfg.AllTransitions)
                        {
                            List<List<IConvertLetterOrTransition>> addList = new List<List<IConvertLetterOrTransition>>();
                            foreach (List<IConvertLetterOrTransition> transition in variable.Value.ToVariablesOrLetters)
                            {
                                if (transition.Contains(epsilonVariable.Value))
                                {
                                    addList.AddRange(CreateNewEpsilonTransitions(transition, epsilonVariable.Value, cfg));
                                }
                            }
                            variable.Value.ToVariablesOrLetters.AddRange(addList);
                        }
                    }
                }
                foreach (List<IConvertLetterOrTransition> prune in pruneList)
                {
                    epsilonVariable.Value.ToVariablesOrLetters.Remove(prune);
                }
            }
            int newAMount = cfg.ReturnTotalTransitionAmount();
            if (transAmountStartCFG != cfg.ReturnTotalTransitionAmount()) return true;
            return false;
        }

        private List<List<IConvertLetterOrTransition>> CreateNewEpsilonTransitions(List<IConvertLetterOrTransition> currentTransition, ConvertTransition epsilonVariable , CFG cfg)
        {
            List<List<IConvertLetterOrTransition>> createNewEpsilonTransitions = new List<List<IConvertLetterOrTransition>>();

            List<int> locations = new List<int>();
            for(int i = 0; i < currentTransition.Count; i++)
            {
                if (currentTransition[i] == epsilonVariable) locations.Add(i);
            }

            List<List<int>> combinations = PowerSet(locations);
            combinations.RemoveAt(0);
            foreach(List<int> combination in combinations)
            {
                List<IConvertLetterOrTransition> transitionCopy = new List<IConvertLetterOrTransition>(currentTransition);
                foreach (int position in combination)
                {
                    transitionCopy[position] = GetOrCreateNewSymbol('_', cfg);
                }
                createNewEpsilonTransitions.Add(transitionCopy);
            }

            bool deleteFinalTrans = true;
            foreach (IConvertLetterOrTransition letOrTrans in createNewEpsilonTransitions[createNewEpsilonTransitions.Count - 1])
            {
                if (letOrTrans != GetOrCreateNewSymbol('_', cfg)) deleteFinalTrans = false;
            }
            if (deleteFinalTrans) createNewEpsilonTransitions.RemoveAt(createNewEpsilonTransitions.Count - 1);

            return createNewEpsilonTransitions;
        }

        private static List<List<int>> PowerSet(List<int> input)
        {
            int n = input.Count;
            int powerSetCount = 1 << n;
            var ans = new List<List<int>>();

            for (int setMask = 0; setMask < powerSetCount; setMask++)
            {
                var s = new List<int>();
                for (int i = 0; i < n; i++)
                {
                    if ((setMask & (1 << i)) > 0)
                    {
                        s.Add(input[i]);
                    }
                }
                ans.Add(s);
            }

            return ans;
        }

        private bool TransitionContainsOnlyVariables(List<IConvertLetterOrTransition> transition)
        {
            foreach (IConvertLetterOrTransition letOrVar in transition)
            {
                if (!letOrVar.IsVariable()) return false;
            }
            return true;
        }

        public bool TransitionContainsOnlySymbols(List<IConvertLetterOrTransition> transition)
        {
            foreach (IConvertLetterOrTransition letOrVar in transition)
            {
                if (letOrVar.IsVariable()) return false;
            }
            return true;
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
            CFG cfg = new CFG(new Node("S"), new Node("S"));
            ConvertLetter aLet = new ConvertLetter('a');
            ConvertLetter cLet = new ConvertLetter('c');
            ConvertLetter eLet = new ConvertLetter('e');
            cfg.Terminals.Add('a', aLet);
            cfg.Terminals.Add('c', cLet);
            cfg.Terminals.Add('e', eLet);

            ConvertTransition sTrans = cfg.AllTransitions["SS"];
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
            eTrans.ToVariablesOrLetters.Add(new List<IConvertLetterOrTransition>() { eTrans });

            cfg.AllTransitions.Add("AA", aTrans);
            cfg.AllTransitions.Add("CC", cTrans);
            cfg.AllTransitions.Add("EE", eTrans);

            return cfg;
        }
    }
}
