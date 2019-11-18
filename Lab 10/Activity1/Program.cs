using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Activity1
{
    class Program
    {
        const string EPSILON = "\u03B5";

        static Regex terminalParser = new Regex(@"[^A-Z\u03B5\#]");
        static Regex nonterminalParser = new Regex(@"[A-Z][']*");
        static Regex epsilonReplacer = new Regex(@"#");
        static Regex dotReplacer = new Regex(@"\.");

        // 1st `string`: Current state
        // 2nd `string`: Transition symbol
        // 3rd `string`: Transition state
        static Dictionary<string, Dictionary<string, List<string>>> NFATransitions = new Dictionary<string, Dictionary<string, List<string>>>();

        // 1st `int`: State index
        // 2nd `string`: DFA State items
        static Dictionary<int, List<string>> DFAStates = new Dictionary<int, List<string>>();

        static List<int> ShiftStates = new List<int>();
        static List<int> ReduceStates = new List<int>();

        // 1st `int`: State index
        // 2nd `string`: Transition symbol
        // 3rd `int`: Transition state index
        static Dictionary<int, Dictionary<string, int>> DFATransitions = new Dictionary<int, Dictionary<string, int>>();

        
        static string itemToProduction(string item)
        {
            return dotReplacer.Replace(item, "");
        }

        static string formatAsProduction(string symbol, string production)
        {
            return symbol + " -> " + production;
        }


        static string productionListToString(List<string> productions)
        {
            string productionsStr = "";
            for (int i = 0, len = productions.Count; i < len; i++)
            {
                productionsStr += productions[i];
                productionsStr += i < len - 1 ? "|" : "";
            }
            return productionsStr;
        } 

        static int searchState(List<string> stateSet)
        {
            foreach (int index in DFAStates.Keys)
            {
                if (statesMatch(stateSet, DFAStates[index]))
                {
                    return index;
                }
            }
            return -1;
        }

        static bool statesMatch(List<string> state1Set, List<string> state2Set)
        {
            if (state1Set.Count != state2Set.Count) { return false; }
            foreach (string state in state1Set)
            {
                if (!state2Set.Contains(state))
                {
                    return false;
                }
            }
            return true;
        }

        static string getProductionNonTerminal(string production)
        {
            return production.Split(' ')[0];
        }

        static string getProductionOutput(string production)
        {
            return production.Split(' ')[2];
        }

        static bool itemIsComplete(string item)
        {
            if (getProductionOutput(item).Equals("."))
            {
                return false;
            }
            return item.IndexOf('.') == item.Length - 1;
        }

        static bool isShiftState(int stateIndex)
        {
            return ShiftStates.Contains(stateIndex);
        }

        static bool isReduceState(int stateIndex)
        {
            return ReduceStates.Contains(stateIndex);
        }

        static string getStackContents(Stack<string> stack)
        {
            string[] contents = stack.ToArray();
            string outputStr = "";
            for(int len = contents.Length, i = len - 1; i >= 0; i--)
            {
                outputStr += contents[i] + " ";
            }
            return outputStr;
        }

        static string getQueueContents(Queue<string> queue)
        {
            string[] contents = queue.ToArray();
            string outputStr = "";
            foreach (string item in contents)
            {
                outputStr += item + " ";
            }
            return outputStr;
        }

        static List<string> getEpsilonClosure(string currentNFAState)
        {
            return recursiveClosureRetreival(currentNFAState, new List<string>());
        }

        static List<string> recursiveClosureRetreival(string currentNFAState, List<string> computedClosure)
        {
            if (NFATransitions.ContainsKey(currentNFAState) && NFATransitions[currentNFAState].ContainsKey(EPSILON))
            {
                foreach (string transitionNFAState in NFATransitions[currentNFAState][EPSILON])
                {
                    if (!computedClosure.Contains(transitionNFAState))
                    {
                        computedClosure.Add(transitionNFAState);
                    }
                    if (!computedClosure.Contains(transitionNFAState))
                    {
                        List<string> nextEpsilonClosure = recursiveClosureRetreival(transitionNFAState, computedClosure);
                        foreach (string nextTransitionNFAState in nextEpsilonClosure)
                        {
                            if (!computedClosure.Contains(transitionNFAState))
                            {
                                computedClosure.Add(nextTransitionNFAState);
                            }
                        }
                    }
                }
            }
            return computedClosure;
        }

        static void Main(string[] args)
        {

            Dictionary<string, List<string>> grammar = new Dictionary<string, List<string>>();

            Queue<string> symbolInputQueue = new Queue<string>();
            // Input the grammar from the user
            Console.WriteLine("Enter production rules for the grammar (Use `" + EPSILON + "` or `#` for epsilon):");

            Console.Write("Enter letter for starting symbol: ");
            string startingSymbol = Console.ReadLine();
            symbolInputQueue.Enqueue(startingSymbol);

            while (symbolInputQueue.Count > 0)
            {
                // Get the current symbol
                string symbol = symbolInputQueue.Dequeue();
                // Input the rule for the current symbol
                Console.Write(symbol + " -> ");
                string rule = Console.ReadLine();

                rule = epsilonReplacer.Replace(rule, EPSILON);

                // Initialize a new production list
                grammar[symbol] = new List<String>();

                // Parse all different productions available
                string[] productions = rule.Split('|');
                if (symbol.Equals(startingSymbol) && productions.Length > 1)
                {
                    Console.WriteLine("Error: Augmented rule can have only 1 production!");
                    Console.ReadLine();
                    return;
                }
                foreach (string production in productions)
                {
                    // Add production at symbol index
                    if (!grammar[symbol].Contains(production))
                    {
                        grammar[symbol].Add(production);
                    }
                }

                // Parse all other non-terminals from the given rule and add them to input queue
                MatchCollection matches = nonterminalParser.Matches(rule);
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        if (!symbolInputQueue.Contains(match.Value) && !grammar.ContainsKey(match.Value))
                        {
                            symbolInputQueue.Enqueue(match.Value);
                        }
                    }
                }
            }

            // Print the grammar
            Console.WriteLine("\nGrammar obtained: ");
            foreach(KeyValuePair<string, List<string>> entry in grammar)
            {
                Console.WriteLine(formatAsProduction(entry.Key, productionListToString(grammar[entry.Key])));
            }

            Console.WriteLine("\nPress enter to extract all the items from grammar...");
            Console.ReadLine();

            // Compute the the items
            Dictionary<string, List<string>> items = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> initialItems = new Dictionary<string, List<string>>();
            foreach (string symbol in grammar.Keys)
            {
                initialItems.Add(symbol, new List<string>());
                List<string> productions = grammar[symbol];
                foreach (string production in productions)
                {
                    string itemKey = formatAsProduction(symbol, production);
                    items.Add(itemKey, new List<string>());
                    if (production.Equals(EPSILON))
                    {
                        initialItems[symbol].Add(formatAsProduction(symbol, "."));
                        items[itemKey].Add(formatAsProduction(symbol, "."));
                        continue;
                    }
                    string itemRightStr = production;
                    for (int i = 0, len = production.Length; i <= len; i++)
                    {
                        string finalItemStr = formatAsProduction(symbol, itemRightStr.Insert(i, "."));
                        if (i == 0)
                        {
                            initialItems[symbol].Add(finalItemStr);
                        }
                        if (!items[itemKey].Contains(finalItemStr))
                        {
                            items[itemKey].Add(finalItemStr);
                        }
                        itemRightStr = production;
                    }
                }
            }

            // Print the items
            foreach (string production in items.Keys)
            {
                Console.WriteLine(production);
                foreach (string item in items[production])
                {
                    Console.WriteLine("\t" + item);
                }
                Console.WriteLine();
            }

            foreach (string symbol in initialItems.Keys)
            {
                Console.WriteLine("Initial items for " + symbol);
                foreach (string item in initialItems[symbol])
                {
                    Console.WriteLine("\t" + item);
                }
                Console.WriteLine();
            }

            Console.WriteLine("\nPress enter to make the NFA for the items...");
            Console.ReadLine();


            // Initialize the NFA
            foreach (string production in items.Keys)
            {
                Console.WriteLine(production);
                foreach (string item in items[production])
                {
                    if (!NFATransitions.ContainsKey(item))
                    {
                        NFATransitions[item] = new Dictionary<string, List<string>>();
                        Console.WriteLine("Created state for: " + item);
                    }
                }
                Console.WriteLine();
            }

            // iterate over sets of items for each production
            foreach (string production in items.Keys)
            {
                Console.WriteLine("\nProduction: " + production);
                // iterate over every single item and compare it with all other items in set
                foreach (string item in items[production])
                {
                    // comparing `item` to all other items for the production
                    foreach (string nextItem in items[production])
                    {
                        // skip if item is current item
                        if (item.Equals(nextItem))
                        {
                            continue;
                        }

                        // check if the item is an adjacent shift or parse
                        if ((nextItem.IndexOf('.') - item.IndexOf('.')) == 1)
                        {
                            // get shifted or parsed token to infer what to do
                            string nextToken = item[item.IndexOf('.') + 1].ToString();
                            Console.WriteLine("Adjacent items: " + item + " " + nextItem + "\tShifted/Parsed token: " + nextToken);
                            // if terminal, add a simple transition with the terminal as the transition symbol
                            if (terminalParser.IsMatch(nextToken.ToString()))
                            {
                                if (!NFATransitions[item].ContainsKey(nextToken))
                                {
                                    NFATransitions[item].Add(nextToken, new List<string>());
                                }
                                NFATransitions[item][nextToken].Add(nextItem);
                                Console.WriteLine("Added transistion from " + item + " to " + nextItem + " using " + nextToken);
                            }
                            // if non-terminal, add ε-transitions from current state to initial items of the non-terminal
                            // add a transition with the non-terminal to the next item
                            else if (nonterminalParser.IsMatch(nextToken.ToString()))
                            {
                                nextToken = nonterminalParser.Match(item.Substring(item.IndexOf('.') + 1)).Value;
                                foreach (string nonTerminalItem in initialItems[nextToken])
                                {
                                    if (!NFATransitions[item].ContainsKey(EPSILON))
                                    {
                                        NFATransitions[item].Add(EPSILON, new List<string>());
                                    }
                                    NFATransitions[item][EPSILON].Add(nonTerminalItem);
                                    Console.WriteLine("Added transistion from " + item + " to " + nonTerminalItem + " using " + EPSILON);
                                }
                                if (!NFATransitions[item].ContainsKey(nextToken))
                                {
                                    NFATransitions[item].Add(nextToken, new List<string>());
                                }
                                NFATransitions[item][nextToken].Add(nextItem);
                                Console.WriteLine("Added transistion from " + item + " to " + nextItem + " using " + nextToken);
                            }
                        }
                    }
                }
            }

            Console.WriteLine("\n\n");
            foreach (string state in NFATransitions.Keys)
            {
                Console.WriteLine("State " + state);
                foreach (KeyValuePair<string, List<string>> transitions in NFATransitions[state])
                {
                    string transitionSymbol = transitions.Key;
                    Console.WriteLine("\tTransition Symbol:" + transitionSymbol);
                    foreach (string transitionState in transitions.Value)
                    {
                        Console.WriteLine("\t\t" + transitionState);
                    }
                }
                if (NFATransitions[state].Keys.Count == 0)
                {
                    Console.WriteLine("No transitions for this state!");
                }
                Console.WriteLine("\n");

            }

            string NFAStartingState = items[formatAsProduction(startingSymbol, grammar[startingSymbol][0])][0];

            Console.WriteLine("\nPress enter to make the DFA for the sets...");
            Console.ReadLine();

            // Construct the DFA using subset construction
            // Create the first DFA state set
            int startingIndex = 0;
            List<string> DFAStateSet = new List<string>();
            DFAStateSet.Add(NFAStartingState);
            // Add the NFA closure items inside the ε-closure (closure items) to the DFA states set
            List<string> epsilonClosure = getEpsilonClosure(NFAStartingState);
            foreach (string NFAClosureItem in epsilonClosure)
            {
                if (!DFAStateSet.Contains(NFAClosureItem))
                {
                    DFAStateSet.Add(NFAClosureItem);
                }
            }

            // Add the DFA state set to the DFA States
            DFAStates.Add(startingIndex, new List<string>());
            Console.WriteLine("Created DFA state " + startingIndex);
            foreach (string stateItem in DFAStateSet)
            {
                DFAStates[startingIndex].Add(stateItem);
                Console.WriteLine("\tIn current state set, added item: " + stateItem);
            }
            ShiftStates.Add(startingIndex);
            DFATransitions.Add(startingIndex, new Dictionary<string, int>());

            Stack<int> stateExpandStack = new Stack<int>();
            stateExpandStack.Push(startingIndex);
            while (stateExpandStack.Count > 0)
            {
                int stateIndex = stateExpandStack.Pop();
                Console.WriteLine("Selected state " + stateIndex + " for further expansion");
                int nextStateIndex = stateIndex;
                DFAStateSet = DFAStates[stateIndex];

                // Get all the transition symbols making up transitions for kernel items
                Console.WriteLine("Finding all the kernel symbols...");
                List<string> kernelSymbols = new List<string>();
                foreach (string NFAStateItem in DFAStateSet)
                {
                    foreach (string NFATransitionSymbol in NFATransitions[NFAStateItem].Keys)
                    {
                        if (!NFATransitionSymbol.Equals(EPSILON))
                        {
                            Console.WriteLine("\tFound kernel symbol: " + NFATransitionSymbol);
                            kernelSymbols.Add(NFATransitionSymbol);
                        }
                    }
                }
                if (kernelSymbols.Count > 0)
                {
                    Console.WriteLine("Finding next DFA state set for each transition symbol...");
                }
                else
                {
                    Console.WriteLine("No kernel symbols found");
                }

                // Iterate over each kernel symbol to get the next DFA state set
                foreach (string symbol in kernelSymbols)
                {
                    List<string> NextDFAStateSet = new List<string>();
                    // Get all transitions for current symbol in current DFA state set
                    foreach (string NFAStateItem in DFAStateSet)
                    {
                        // Check if the current NFA state item has a transition for the symbol
                        if (NFATransitions[NFAStateItem].ContainsKey(symbol))
                        {
                            List<string> NFATransitionItems = NFATransitions[NFAStateItem][symbol];
                            // Add the transition items to the current next DFA state set
                            
                            foreach (string NFATransitionItem in NFATransitionItems)
                            {
                                if (!NextDFAStateSet.Contains(NFATransitionItem))
                                {
                                    NextDFAStateSet.Add(NFATransitionItem);
                                }
                                // Get and add the epsilon closure of each item to the next DFA state set
                                epsilonClosure = getEpsilonClosure(NFATransitionItem);
                                foreach (string nextClosureItem in epsilonClosure)
                                {
                                    NextDFAStateSet.Add(nextClosureItem);
                                }
                            }
                        }
                    }

                    // if there are any transitions from the current state at all
                    if (NextDFAStateSet.Count > 0)
                    {
                        int searchKey = searchState(NextDFAStateSet);
                        if (searchKey != -1)
                        {
                            nextStateIndex = searchKey;
                        }
                        else
                        {
                            do
                            {
                                nextStateIndex++;
                            } while (DFAStates.ContainsKey(nextStateIndex));
                        }
                        if (!stateExpandStack.Contains(nextStateIndex) && !DFAStates.ContainsKey(nextStateIndex))
                        {
                            stateExpandStack.Push(nextStateIndex);

                            DFAStates.Add(nextStateIndex, new List<string>());
                            Console.WriteLine("\nCreated DFA state: " + nextStateIndex);

                            bool isReduceState = false;
                            bool isShiftState = false;
                            // Add the next DFA state set and the respective transition symbol to the DFA states
                            foreach (string nextDFAStateItem in NextDFAStateSet)
                            {
                                if (!DFAStates[nextStateIndex].Contains(nextDFAStateItem))
                                {
                                    if (itemIsComplete(nextDFAStateItem))
                                    {
                                        isReduceState = true;
                                    }
                                    else
                                    {
                                        isShiftState = true;
                                    }
                                    DFAStates[nextStateIndex].Add(nextDFAStateItem);
                                    Console.WriteLine("\tIn next state set, added item: " + nextDFAStateItem);
                                }
                            }

                            if (!isShiftState && isReduceState)
                            {
                                if (!ReduceStates.Contains(nextStateIndex))
                                {
                                    ReduceStates.Add(nextStateIndex);
                                }
                            }
                            else if (isShiftState && !isReduceState)
                            {
                                if (!ShiftStates.Contains(nextStateIndex))
                                {
                                    ShiftStates.Add(nextStateIndex);
                                }
                            }
                            else if (isShiftState && isReduceState)
                            {
                                throw new Exception("Logical Error: Incorrect computation asserted!");
                            }

                        }

                        if (!DFATransitions.ContainsKey(stateIndex))
                        {
                            DFATransitions.Add(stateIndex, new Dictionary<string, int>());
                        }
                        DFATransitions[stateIndex].Add(symbol, nextStateIndex);
                        Console.WriteLine("\tAdded transition from state " + stateIndex + " to state " + nextStateIndex + " for the symbol: " + symbol);
                    }
                    
                }
            }

            foreach (int stateIndex in DFAStates.Keys)
            {
                Console.WriteLine("State " + stateIndex + (isShiftState(stateIndex) ? " shifting" : " reducing"));
                foreach (string item in DFAStates[stateIndex])
                {
                    Console.WriteLine("\t" + item);
                }
                if (DFATransitions.ContainsKey(stateIndex))
                {
                    foreach (KeyValuePair<string, int> entry in DFATransitions[stateIndex])
                    {
                        Console.WriteLine("Transition for " + entry.Key + " to state " + entry.Value);
                    }
                }
                else
                {
                    Console.WriteLine("No transitions for this state!");
                }
                Console.WriteLine("\n\n");

            }

            Console.WriteLine("\nPress enter to input and parse a string...");
            Console.ReadLine();


            // Get the input and parse it
            Console.Write("Input a string: ");
            string inputString = Console.ReadLine();
            Queue<string> inputTape = new Queue<string>();
            for (int i = 0, len = inputString.Length; i < len; i++)
            {
                inputTape.Enqueue(inputString[i].ToString());
            }
            inputTape.Enqueue("$");

            Stack<string> parseStack = new Stack<string>();
            parseStack.Push("$");
            parseStack.Push("0");
            Console.WriteLine("Parsing Stack\t\t\tInput\t\t\tAction");
            bool isParsing = true;
            while (inputTape.Count > 0 && isParsing)
            {
                Console.Write(getStackContents(parseStack) + "\t\t\t" + getQueueContents(inputTape) + "\t\t\t");
                int currentStateIndex = Int32.Parse(parseStack.Peek());
                if (isShiftState(currentStateIndex))
                {
                    string shiftingSymbol = inputTape.Peek();
                    bool shiftPossible = true;
                    if (!DFATransitions[currentStateIndex].ContainsKey(shiftingSymbol))
                    {
                        shiftPossible = false;
                        // If the shifting symbol is a non terminal, check wh
                        
                        bool foundNullable = false;
                        foreach (KeyValuePair<string, int> entry in DFATransitions[currentStateIndex])
                        {
                            string nullableSymbol = entry.Key;
                            if (nonterminalParser.IsMatch(nullableSymbol) && grammar[nullableSymbol].Contains(EPSILON))
                            {
                                shiftingSymbol = nullableSymbol;
                                foundNullable = true;
                                break;
                            }
                        }
                        if (!foundNullable)
                        {
                            Console.WriteLine("\n\nDFA state " + currentStateIndex + " does not contain a transition for symbol " + shiftingSymbol);
                            isParsing = false;
                            break;
                        }
                    }
                    if (shiftPossible)
                    {
                        inputTape.Dequeue();
                    }
                    Console.Write("shift\n");
                    parseStack.Push(shiftingSymbol);
                    int nextState = DFATransitions[currentStateIndex][shiftingSymbol];
                    parseStack.Push(nextState.ToString());

                }
                else if (isReduceState(currentStateIndex))
                {
                    string production = itemToProduction(DFAStates[currentStateIndex][0]);
                    string productionOutput = getProductionOutput(production);
                    foreach (char symbol in productionOutput)
                    {
                        parseStack.Pop();
                        parseStack.Pop();
                    }
                    currentStateIndex = Int32.Parse(parseStack.Peek());
                    string reducingSymbol = getProductionNonTerminal(production);
                    if (reducingSymbol.Equals(startingSymbol))
                    {
                        Console.Write("accept\n");
                        break;
                    }
                    Console.Write("reduce " + production + "\n");
                    parseStack.Push(reducingSymbol);
                    int nextState = DFATransitions[currentStateIndex][reducingSymbol];
                    parseStack.Push(nextState.ToString());
                }
            }

            Console.WriteLine("");

            



            Console.Write("\n\nPress enter to exit...");
            Console.ReadLine();
        }
    }
}
