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
            SimplifyCFG(cfg);


            return PrintTransitions(cfg, automata);
        }

        public void AddType1AndType2Transitions(Automata automata, CFG cfg)
        {
            foreach(Node fromNode in automata.nodes)
            {
                foreach (Node toNode in automata.nodes)
                {
                    CFGVariable transition = GetOrCreateNewTransition(fromNode, toNode, cfg.AllTransitions);

                    //type 1 transitions
                    if (fromNode == toNode) transition.ToVariablesOrLetters.Add(new List<ILetterOrVariable>() { GetOrCreateNewSymbol('_', cfg) });

                    //type 2 transitions
                    foreach (Node betweenNode in automata.nodes)
                    {
                        if (betweenNode != fromNode && betweenNode != toNode)
                        {
                            transition.ToVariablesOrLetters.Add(new List<ILetterOrVariable>() {
                                GetOrCreateNewTransition(fromNode, betweenNode, cfg.AllTransitions),
                                GetOrCreateNewTransition(betweenNode, toNode, cfg.AllTransitions)
                            });
                        }
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
                                    CFGVariable type3Transition = GetOrCreateNewTransition(fromNode, secondConn.ToNode, cfg.AllTransitions);
                                    type3Transition.ToVariablesOrLetters.Add(new List<ILetterOrVariable>() {
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
            foreach (KeyValuePair<string, CFGVariable> valuePair in cfg.AllTransitions)
            {
                List<List<ILetterOrVariable>> transitions = valuePair.Value.ToVariablesOrLetters;
                List<List<ILetterOrVariable>> pruneList = new List<List<ILetterOrVariable>>();
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
                foreach (List<ILetterOrVariable> prune in pruneList)
                {
                    valuePair.Value.ToVariablesOrLetters.Remove(prune);
                }
            }
        }
 
        private bool AreListEqual(List<ILetterOrVariable> list1, List<ILetterOrVariable> list2)
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
            HashSet<CFGVariable> allNeededTransitions = new HashSet<CFGVariable>();

            //create first list containing terminal symbols
            foreach (KeyValuePair<string, CFGVariable> valuePair in cfg.AllTransitions)
            {
                if (valuePair.Value.ContainsTerminals()) allNeededTransitions.Add(valuePair.Value);
            }

            //create hashset of variables that can reach a terminal
            int transitionLength = 0;
            while (transitionLength != allNeededTransitions.Count())
            {
                transitionLength = allNeededTransitions.Count();
                foreach (KeyValuePair<string, CFGVariable> valuePair in cfg.AllTransitions)
                {
                    foreach (List<ILetterOrVariable> transition in valuePair.Value.ToVariablesOrLetters)
                    {
                        int counter = 0;
                        foreach (ILetterOrVariable letOrVar in transition)
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
            HashSet<ILetterOrVariable> allReachableTransitions = new HashSet<ILetterOrVariable>();
            allReachableTransitions.Add(cfg.StartVariable);
            int transitionLength = 0;
            while (transitionLength != allReachableTransitions.Count())
            {
                transitionLength = allReachableTransitions.Count();

                HashSet<ILetterOrVariable> allReachableTransitionsCopy = new HashSet<ILetterOrVariable>(allReachableTransitions);
                foreach (ILetterOrVariable reachableTransition in allReachableTransitionsCopy)
                {
                    if (reachableTransition.IsVariable())
                    {
                        foreach (List<ILetterOrVariable> varOrLetList in cfg.AllTransitions[reachableTransition.ToString()].ToVariablesOrLetters)
                        {
                            foreach (ILetterOrVariable varOrLet in varOrLetList)
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
            
            foreach (KeyValuePair<string, CFGVariable> valuePair in cfg.AllTransitions)
            {
                List<List<ILetterOrVariable>> pruneList = new List<List<ILetterOrVariable>>();
                foreach (List<ILetterOrVariable> transition in valuePair.Value.ToVariablesOrLetters)
                {
                    if (transition.Count == 1 && transition[0].ToString().Equals(valuePair.Value.ToString())) pruneList.Add(transition);
                }
                foreach (List<ILetterOrVariable> prune in pruneList)
                {
                    valuePair.Value.ToVariablesOrLetters.Remove(prune);
                }
            }
        }

        public void RemoveUnneededEpsilon(CFG cfg)
        {
            foreach (KeyValuePair<string, CFGVariable> valuePair in cfg.AllTransitions)
            {
                foreach (List<ILetterOrVariable> transition in valuePair.Value.ToVariablesOrLetters)
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
            foreach (KeyValuePair<string, CFGVariable> epsilonVariable in cfg.AllTransitions)
            {
                List<List<ILetterOrVariable>> pruneList = new List<List<ILetterOrVariable>>();
                List<List<ILetterOrVariable>> transitions = epsilonVariable.Value.ToVariablesOrLetters;
                bool blockDeletion = false;
                int startCount = transitions.Count;
                for (int i = 0; i < startCount; i++) {
                    if (transitions[i].Count == 1 && transitions[i][0].ToString().Equals("_"))
                    {
                        pruneList.Add(transitions[i]);
                        foreach (KeyValuePair<string, CFGVariable> variable in cfg.AllTransitions)
                        {
                            List<List<ILetterOrVariable>> addList = new List<List<ILetterOrVariable>>();
                            foreach (List<ILetterOrVariable> transition in variable.Value.ToVariablesOrLetters)
                            {
                                if (transition.Contains(epsilonVariable.Value))
                                {
                                    if (variable.Value == epsilonVariable.Value) blockDeletion = true;
                                    addList.AddRange(CreateNewEpsilonTransitions(transition, epsilonVariable.Value, cfg));
                                }
                            }
                            variable.Value.ToVariablesOrLetters.AddRange(addList);
                        }
                    }
                }
                foreach (List<ILetterOrVariable> prune in pruneList)
                {
                    if (!blockDeletion) epsilonVariable.Value.ToVariablesOrLetters.Remove(prune);
                }
            }
            CleanCFG(cfg);
            int newAMount = cfg.ReturnTotalTransitionAmount();

            if (transAmountStartCFG != cfg.ReturnTotalTransitionAmount()) return true;
            return false;
        }

        private List<List<ILetterOrVariable>> CreateNewEpsilonTransitions(List<ILetterOrVariable> currentTransition, CFGVariable epsilonVariable , CFG cfg)
        {
            List<List<ILetterOrVariable>> createNewEpsilonTransitions = new List<List<ILetterOrVariable>>();

            List<int> locations = new List<int>();
            for(int i = 0; i < currentTransition.Count; i++)
            {
                if (currentTransition[i] == epsilonVariable) locations.Add(i);
            }

            List<List<int>> combinations = PowerSet(locations);
            foreach(List<int> combination in combinations)
            {
                List<ILetterOrVariable> transitionCopy = new List<ILetterOrVariable>(currentTransition);
                foreach (int position in combination)
                {
                    transitionCopy[position] = GetOrCreateNewSymbol('_', cfg);
                }
                createNewEpsilonTransitions.Add(transitionCopy);
            }

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

        private bool TransitionContainsOnlyVariables(List<ILetterOrVariable> transition)
        {
            foreach (ILetterOrVariable letOrVar in transition)
            {
                if (!letOrVar.IsVariable()) return false;
            }
            return true;
        }

        public bool TransitionContainsOnlySymbols(List<ILetterOrVariable> transition)
        {
            foreach (ILetterOrVariable letOrVar in transition)
            {
                if (letOrVar.IsVariable()) return false;
            }
            return true;
        }

        private CFGVariable GetOrCreateNewTransition(Node leftNode, Node rightNode, Dictionary<string, CFGVariable> allTransitions)
        {
            string key = $"{leftNode.Name}{rightNode.Name}";
            if (allTransitions.ContainsKey(key)) return allTransitions[key];
            CFGVariable newTransition = new CFGVariable(leftNode, rightNode);
            allTransitions.Add(key, newTransition);
            return newTransition;
        }

        private CFGTerminal GetOrCreateNewSymbol(char symbol, CFG cfg)
        {
            if (cfg.Terminals.ContainsKey(symbol)) return cfg.Terminals[symbol];
            CFGTerminal newLetter = new CFGTerminal(symbol);
            cfg.Terminals.Add(symbol, newLetter);
            return newLetter;
        }

        private string PrintTransitions(CFG cfg, Automata automata)
        {
            Dictionary<string, char> charStates = new Dictionary<string, char>();
            string output = "grammar:\r\n";
            foreach (KeyValuePair<string, CFGVariable> valuePair in cfg.AllTransitions)
            {
                output += $"{valuePair.Value.ReturnString(charStates)}";
            }
            output += "end. \r\n\r\ndfa: n\r\nfinite:n\r\n\r\nwords:\r\n";

            foreach (TestWord word in automata.testWords)
            {
                output += $"{new string(word.word)}, {(word.accapted ? 'y' : 'n')} \r\n";
            }

            output += "end.";
            return output;
        }

    }
}
