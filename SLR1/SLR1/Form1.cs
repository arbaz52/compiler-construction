using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SLR1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void compute_NFA_button(object sender, EventArgs e)
        {
            extract_production_rules();

            generate_nfa();
            display_states();

            compute_first();
            compute_follow();

            generate_table();
        }

        Dictionary<char, List<String>> production_rules;
        char starting_symbol;
        char augmented_symbol = 'X'; //cannot be used by user in rules
        private void extract_production_rules()
        {
            production_rules = new Dictionary<char, List<String>>();
            String[] raw_rules = InputProductionRules.Text.Split('\n');
            starting_symbol = raw_rules[0].Split('>')[0][0];
            foreach (String raw_rule in raw_rules)
            {
                String[] pair = raw_rule.Split('>');
                char symbol = pair[0][0];
                String right_side = pair[1];
                if (right_side.Contains('|'))
                {
                    String[] rules = right_side.Split('|');
                    foreach (String rule in rules)
                    {
                        add_rule(symbol, rule);
                    }
                }
                else
                {
                    String rule = right_side;
                    add_rule(symbol, rule);
                }
            }

            //adding augmented rule
            add_rule(augmented_symbol, starting_symbol + "");

            //handling epsilon
            for (int i = 0; i < production_rules.Count; i++)
            {
                KeyValuePair<char, List<String>> pair = production_rules.ElementAt(i);
                for (int j = 0; j < pair.Value.Count; j++)
                {
                    String rule = pair.Value[j];
                    if (rule.Equals(epsilon + ""))
                    {
                        handle_epsilon(pair.Key);
                    }
                }
            }
        }
        private void add_rule(char symbol, String rule)
        {
            if (production_rules.ContainsKey(symbol))
            {
                production_rules[symbol].Add(rule);
            }
            else
            {
                production_rules.Add(symbol, new List<String>());
                add_rule(symbol, rule);
            }
        }

        List<State> states; //state containing their items i.e. state[0] = L.e
        List<int> waiting_list; //states whose movements have not been computed
        private void generate_nfa()
        {
            states = new List<State>();
            waiting_list = new List<int>();

            //add a state with augmented production rule
            states.Add(new State());
            String rule = production_rules[augmented_symbol][0];
            rule = "." + rule;
            List<String> rules_list = new List<String>();
            rules_list.Add(rule);
            states[0].rules.Add(augmented_symbol, rules_list);
            populate_state(0);

            waiting_list.Add(0);

            while (waiting_list.Count > 0)
            {
                int current = waiting_list[0];
                waiting_list.RemoveAt(0);

                foreach(KeyValuePair<char, List<String>> rules in states[current].rules)
                {
                    foreach (String temp_rule in rules.Value)
                    {
                        int dot_at = temp_rule.IndexOf('.');
                        if (dot_at == -1)
                            continue;
                        if (dot_at + 1 < temp_rule.Length)
                        {
                            char c = temp_rule[dot_at + 1];
                            String updated_rule = temp_rule.Remove(dot_at, 1);
                            updated_rule = updated_rule.Insert(dot_at + 1, ".");
                            int next_state = get_state_index_with_rule(rules.Key, updated_rule);
                            if (states[current].movements.ContainsKey(c))
                            {
                                State n_state = states[states[current].movements[c]];
                                if (!n_state.rules.ContainsKey(rules.Key))
                                    n_state.rules.Add(rules.Key, new List<String>());
                                if(!n_state.rules[rules.Key].Contains(updated_rule))
                                    n_state.rules[rules.Key].Add(updated_rule);
                            }
                            else if(next_state == -1)
                            {
                                //create a new state and add this rule and populate this
                                states.Add(new State());
                                next_state = states.Count - 1;
                                if (!states[next_state].rules.ContainsKey(rules.Key))
                                    states[next_state].rules.Add(rules.Key, new List<String>());
                                states[next_state].rules[rules.Key].Add(updated_rule);
                                populate_state(next_state);
                                waiting_list.Add(next_state);
                                //add movement!
                            }
                            if(!states[current].movements.ContainsKey(c))
                                states[current].movements.Add(c, next_state);
                        }
                    }
                }
            }
        }
        private void display_states()
        {
            OutputStates.Text = "";
            for (int i = 0; i < states.Count; i++)
            {
                OutputStates.Text += "State: " + i + "\n"+states[i].toString() + "\n";

            }
        }
        private int get_state_index_with_rule(char symbol, String rule)
        {
            int index = -1;
            for (int i = 0; i < states.Count; i++)
            {
                State state = states[i];
                if (state.has_rule(symbol, rule))
                    return i;
            }
            return index;
        }

        private void populate_state(int index)
        {
            State state = states[index];
            for(int i = 0; i < state.rules.Count; i++)
            {
                for(int j = 0; j < state.rules.ElementAt(i).Value.Count; j++)
                {
                    String rule = state.rules.ElementAt(i).Value[j];
                    int dot_at = rule.IndexOf('.');
                    if (dot_at + 1 < rule.Length && is_non_terminal(rule[dot_at + 1]))
                    {
                        char symbol = rule[dot_at + 1];
                        List<String> symbol_rules = production_rules[symbol];
                        foreach (String r in symbol_rules)
                        {
                            String rx = "." + r;
                            if (!state.rules.ContainsKey(symbol))
                            { 
                                state.rules.Add(symbol, new List<String>());
                            }
                            if(!state.rules[symbol].Contains(rx))
                                state.rules[symbol].Add(rx);
                        }
                    }
                }
            }

        }

        private bool is_non_terminal(char c)
        {
            return Char.IsUpper(c);
        }


        //previous code
        //for calculating first and follow sets

        char epsilon = '~'; 
        public void handle_epsilon(char s)
        {
            char symbol = s;
            foreach (KeyValuePair<char, List<String>> pair in production_rules)
            {
                List<String> new_rules = new List<String>();
                foreach (String rule in pair.Value)
                {
                    if (rule.Contains(symbol))
                    {
                        String modified_rule = rule.Remove(rule.IndexOf(symbol), 1);
                        if (modified_rule.Equals(""))
                            continue;
                        new_rules.Add(modified_rule);
                    }
                }
                production_rules[pair.Key].AddRange(new_rules);
            }
        }

        Dictionary<char, List<char>> first_sets;
        private void compute_first()
        {
            first_sets = new Dictionary<char, List<char>>();
            foreach (KeyValuePair<char, List<String>> productions in production_rules)
            {
                first_sets.Add(productions.Key, new List<char>());
                foreach (String rule in productions.Value)
                {
                    List<char> fs = find_first(productions.Key);
                    foreach (char c in fs)
                    {
                        if (!first_sets[productions.Key].Contains(c))
                            first_sets[productions.Key].Add(c);
                    }

                }
            }
        }
        public List<char> find_first(char c)
        {
            List<char> firsts = new List<char>();
            foreach (KeyValuePair<char, List<String>> productions in production_rules)
            {
                if (productions.Key != c)
                    continue;

                foreach (String rule in productions.Value)
                {
                    if (is_non_terminal(rule[0]))
                    {
                        List<char> first = find_first(rule[0]);
                        foreach (char cx in first)
                        {
                            firsts.Add(cx);
                        }
                    }
                    else
                    {
                        firsts.Add(rule[0]);
                    }
                }
            }
            return firsts;
        }

        Dictionary<char, List<char>> follow_sets;
        private void compute_follow()
        {
            follow_sets = new Dictionary<char, List<char>>();
            //initialise the follow sets
            foreach (KeyValuePair<char, List<String>> productions in production_rules)
            {
                follow_sets.Add(productions.Key, new List<char>());
            }




            //will hold references 
            Dictionary<char, List<List<char>>> temp_follow_sets = new Dictionary<char, List<List<char>>>();
            foreach (KeyValuePair<char, List<String>> productions in production_rules)
            {
                temp_follow_sets.Add(productions.Key, new List<List<char>>());
            }
            //add $ to starting symbol
            char starting_symbol = follow_sets.ElementAt(0).Key;
            List<char> temp_ = new List<char>();
            temp_.Add('$');
            temp_follow_sets[starting_symbol].Add(temp_);

            //rules
            /*
             * 1: add $ to starting non-terminal
             * 2: if A>pBq then follow(B) = first(q) where q is a non-terminal else follow(B) has q
             * 3: if A>pB then follow(B) = follow(A)
            */

            /*
             * check RHS of each production rule
             * if contains a non-terminal check for above stated rules and calculate followsets
             */
            foreach (KeyValuePair<char, List<String>> productions in production_rules)
            {
                //check each rule
                char lhs = productions.Key;
                foreach (String rule in productions.Value)
                {
                    //check each character to look for symbol
                    for (int i = 0; i < rule.Length; i++)
                    {
                        //symbol whose follow set we're looking for
                        char c = rule[i];
                        if (is_non_terminal(c))
                        {
                            //check for rules
                            if (i + 1 < rule.Length)
                            {
                                char nc = rule[i + 1]; //next character
                                if (is_non_terminal(nc))
                                {
                                    temp_follow_sets[c].Add(first_sets[nc]);
                                }
                                else
                                {
                                    List<char> temp = new List<char>();
                                    temp.Add(nc);
                                    temp_follow_sets[c].Add(temp);
                                }
                            }
                            else
                            {
                                //here, endless loop
                                if (c == lhs)
                                    continue;
                                for (int k = 0; k < temp_follow_sets[lhs].Count; k++)
                                {
                                    List<char> set = temp_follow_sets[lhs][k];
                                    temp_follow_sets[c].Add(set);
                                }
                            }
                        }
                    }
                }

                //merge all
                foreach (KeyValuePair<char, List<List<char>>> temp in temp_follow_sets)
                {
                    foreach (List<char> set in temp.Value)
                    {
                        foreach (char c in set)
                        {
                            if (!follow_sets[temp.Key].Contains(c))
                                follow_sets[temp.Key].Add(c);
                        }
                    }
                }

                //remove all the epsilons
                for (int i = 0; i < follow_sets.Count; i++)
                {
                    List<char> set = follow_sets.ElementAt(i).Value;
                    set.Remove(epsilon);
                    follow_sets[follow_sets.ElementAt(i).Key] = set;
                }

            }
            //fill in the gaps
            wind_up();
        }
        public void wind_up()
        {
            //this will fill in all the gaps
            //enter in the rest of the values that were missing

            foreach (KeyValuePair<char, List<String>> productions in production_rules)
            {
                //check each rule
                char lhs = productions.Key;
                foreach (String rule in productions.Value)
                {
                    //check each character to look for symbol
                    for (int i = 0; i < rule.Length; i++)
                    {
                        //symbol whose follow set we're looking for
                        char c = rule[i];
                        if (is_non_terminal(c))
                        {
                            //check for rules
                            if (i + 1 >= rule.Length)
                            {
                                //here, endless loop
                                if (c == lhs)
                                    continue;

                                List<char> set = follow_sets[lhs];
                                foreach (char cx in set)
                                {
                                    if (!follow_sets[c].Contains(cx))
                                        follow_sets[c].Add(cx);
                                }
                            }
                        }
                    }
                }
            }
        }



        //generating parsing table!
        List<char> terminals;
        List<char> non_terminals;
        String[,] table;

        int table_terminal_offset;
        int table_non_terminal_offset;
        private void generate_table()
        {
            int accepting_state = -1;
            terminals = new List<char>();
            non_terminals = new List<char>();

            terminals.Add('$');

            for(int i = 0; i < states.Count; i++)
            {
                State s = states[i];
                foreach(KeyValuePair<char, List<String>> productions in s.rules)
                {
                    foreach(String rule in productions.Value)
                    {
                        if(productions.Key == augmented_symbol
                            && rule.Equals(production_rules[augmented_symbol][0] + "."))
                        {
                            accepting_state = i;
                        }
                    }

                }
                foreach(KeyValuePair<char, int> kv in s.movements)
                {
                    if (is_non_terminal(kv.Key))
                    {
                        if (!non_terminals.Contains(kv.Key))
                            non_terminals.Add(kv.Key);
                    }
                    else
                    {
                        if (!terminals.Contains(kv.Key))
                            terminals.Add(kv.Key);
                    }
                }
            }
            int col_count = terminals.Count + non_terminals.Count + 1;
            table = new String[states.Count, col_count];
            table_terminal_offset = 1;
            table_non_terminal_offset = terminals.Count+1;
            for(int i = 0; i < states.Count; i++)
            {
                State s = states[i];
                table[i, 0] = i + "";
                //handling shift and goto statements
                foreach(KeyValuePair<char, int> m in s.movements)
                {
                    int col_index = -1;
                    if (is_non_terminal(m.Key))
                    {
                        col_index = non_terminals.IndexOf(m.Key);
                        table[i, col_index + table_non_terminal_offset] = m.Value + "";
                    }
                    else
                    {
                        col_index = terminals.IndexOf(m.Key);
                        table[i, col_index + table_terminal_offset] = "S:" + m.Value;
                    }
                }
                //now handling goto statements
                foreach(KeyValuePair<char, List<String>> productions in s.rules)
                {
                    foreach(String rule in productions.Value)
                    {
                        if(rule.IndexOf('.') == rule.Length - 1)
                        {
                            //then reduce
                            char symbol = productions.Key;
                            foreach(char c in follow_sets[symbol])
                            {
                                int col_index = terminals.IndexOf(c);
                                table[i, col_index+table_terminal_offset] = "R:" + symbol + ">" + rule;
                            }
                        }
                    }
                }
            }
            if(accepting_state != -1)
                table[accepting_state, terminals.IndexOf('$') + table_terminal_offset] = "accept";

            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = col_count;
            dataGridView1.Columns[0].Name = "States";
            for (int i = 0; i < terminals.Count; i++)
                dataGridView1.Columns[i+table_terminal_offset].Name = terminals[i]+"";

            for (int i = 0; i < non_terminals.Count; i++)
                dataGridView1.Columns[i+table_non_terminal_offset].Name = non_terminals[i] + "";

            for(int i = 0; i < states.Count; i++)
            {
                String[] row = new String[1 + terminals.Count + non_terminals.Count];
                for(int j = 0; j < row.Length; j++)
                {
                    row[j] = table[i, j];
                }
                dataGridView1.Rows.Add(row);
            }
        }

        Stack<String> stack;
        //convert stack to string and finish it tmrw, too sleepy
        //parsing using table
        private bool parse_string(String s)
        {
            String input = s;
            stack = new Stack<String>();
            bool parsed = false;
            /*
            stack.Push('$');
            stack.Push('0');
            while(stack.Count > 0)
            {
                char top = stack.Peek();
                char in_char = input[0];

                String action = get_action(int.Parse(top + ""), in_char);
                if (action.Equals("accept"))
                {
                    parsed = true;
                    break;
                }else if (action.Contains("S:"))
                {
                    stack.Push(input[0]);
                    input.Remove(0, 1);
                    stack.Push(action[2]);
                }else if (action.Contains("R:"))
                {
                    String[] pair = action.Substring(2).Split('>');
                    for(int i = 0; i < pair[1].Length; i++)
                    {
                        stack.Pop();
                        if (stack.Pop() != pair[1][i])
                        {
                            parsed = false;
                            break;
                        }
                    }
                    char next_state = get_action(stack.Peek(), pair[0][0]);
                    stack.Push(pair[0][0]);
                    stack.Push()
                }

            }
            */

            return parsed;
        }
        private String get_action(int state, char symbol)
        {
            int col_index = -1;
            if (is_non_terminal(symbol))
            {
                col_index = non_terminals.IndexOf(symbol) + table_non_terminal_offset;
            }
            else
            {
                col_index = terminals.IndexOf(symbol) + table_terminal_offset;
            }

            return table[state, col_index];
        }

        private void Parse_Click(object sender, EventArgs e)
        {
            parse_string(InputString.Text);
        }
    }
}
