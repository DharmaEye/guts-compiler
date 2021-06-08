using System.Collections.Generic;
using Guts.Patterns.Strategy;

namespace Guts.Patterns
{
    public class PatternMatchResult
    {
        public PatternMatchResult(bool hasNext, bool succeeded, Dictionary<string, StateVariable> variables)
        {
            HasNext = hasNext;
            Succeeded = succeeded;
            Variables = variables;
        }

        public PatternMatchResult(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public bool TryGetVariable(string name, out StateVariable stateVariable)
        {
            if (!Variables.TryGetValue($"[{name}]", out var variableResult))
            {
                stateVariable = null;
                return false;
            }
            stateVariable = variableResult;
            return true;
        }

        public StateVariable GetVariable(string name)
        {
            return Variables[$"[{name.ToLower()}]"];
        }

        public Dictionary<string, StateVariable> Variables { get; }
        public bool HasNext { get; }
        public bool Succeeded { get; }

        public void AddVariable(string name, object value)
        {
            Variables.Add(name.ToLower(), new StateVariable(value));
        }
    }
}