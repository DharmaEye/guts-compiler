using System.Collections.Generic;
using Guts.Excel;

namespace Guts
{
    public class LogicContext
    {
        public ICollection<Token> Tokens { get; set; }
        public StateMachine State { get; }
        public INerpExcelBuilder Builder { get; }

        public LogicContext(StateMachine statemachine, INerpExcelBuilder builder)
        {
            State = statemachine;
            Builder = builder;
            Tokens = new List<Token>();
        }

        public void Add(Token token)
        {
            Tokens.Add(token);
        }
    }
}