using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLR1
{
    class State
    {
        public Dictionary<char, int> movements;
        public Dictionary<char, List<String>> rules;

        public State()
        {
            this.rules = new Dictionary<char, List<String>>();
            this.movements = new Dictionary<char, int>();
        }
        public bool has_rule(char symbol, String rule)
        {
            if (rules.ContainsKey(symbol))
            {
                return rules[symbol].Contains(rule);
            }
            else
            {
                return false;
            }
        }
        public String toString()
        {
            String response = "";
            response += "Rules: \n";
            foreach(KeyValuePair<char, List<String>> rule in rules)
            {
                foreach(String r in rule.Value)
                {
                    response += rule.Key + ">" + r + "\n";
                }
            }
            response += "Movements: \n";
            foreach(KeyValuePair<char, int> movement in movements)
            {
                response += movement.Key + ": " + movement.Value + "\n";
            }

            return response;
        }
    }
}
