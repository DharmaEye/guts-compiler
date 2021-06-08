using System.Collections.Generic;
using System.Linq;

namespace Guts.Patterns.Strategy
{
    public class StateVariable
    {
        public class Value
        {
            public TokenType Type { get; set; }
            public object Object { get; set; }
        }

        public TokenType Token { get; }
        public IList<Value> Values { get; }

        public StateVariable()
        {
            Values = new List<Value>();
        }

        public StateVariable(object variable) : this()
        {
            Values.Add(new Value
            {
                Object = variable,
                Type = TokenType.None
            });
        }

        public StateVariable(object variable, TokenType token) : this()
        {
            Token = token;
            Values.Add(new Value
            {
                Object = variable,
                Type = token
            });
        }

        public void AddVariable(object variable, TokenType token)
        {
            Values.Add(new Value
            {
                Object = variable,
                Type = token
            });
        }

        public Value GetFirst()
        {
            return Values.FirstOrDefault();
        }
    }
}