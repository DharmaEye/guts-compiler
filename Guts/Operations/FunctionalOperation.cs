using System.Collections.Generic;
using System.Linq;
using Guts.Excel;
using Guts.Patterns;
using Guts.Patterns.Strategy;

namespace Guts.Operations
{
    public class FunctionalOperation
    {
        public StateMachine State { get; }
        public INerpExcelBuilder Builder { get; }
        public string Name { get; set; }

        public FunctionalOperationStatus Status { get; }
        public PatternMatchResult MatchResult { get; }

        public FunctionalOperation Next { get; private set; }
        public FunctionalOperation Parent { get; private set; }
        public FunctionalOperation Root { get; private set; }

        public FunctionalOperation(
            string name,
            FunctionalOperationStatus status,
            StateMachine state,
            INerpExcelBuilder builder,
            PatternMatchResult matchResult)
        {
            State = state;
            Builder = builder;
            Name = name;
            Status = status;
            MatchResult = matchResult;
        }

        public FunctionalOperation(FunctionalOperationStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// Event received from child
        /// </summary>
        public virtual void OnSetProperty(string property, object value)
        {
            Parent?.OnSetProperty(property, value);
        }

        public StateVariable.Value GetAttribute(string name) => MatchResult.GetVariable(name).GetFirst();
        public IEnumerable<StateVariable.Value> GetAttributes(string name) => MatchResult.GetVariable(name).Values;

        public void AddAttribute(string name, object value)
        {
            MatchResult.AddVariable($"[{name}]", value);
        }

        public virtual FunctionalOperationInvokeResult Invoke()
        {
            return Next?.Invoke();
        }

        public FunctionalOperation SetNext(FunctionalOperation next)
        {
            next.Root = Parent == null ? this : Root;
            next.Parent = this;
            Next = next;
            return Next;
        }
    }
}