using System.Collections.Generic;
using Guts.CustomReaders;
using Guts.Operations;

namespace Guts
{
    public class StateMachine
    {
        private readonly Dictionary<string, FunctionalOperation> _variables;
        private readonly Dictionary<string, Structure> _structures;

        public StateMachine()
        {
            _variables = new Dictionary<string, FunctionalOperation>();
            _structures = new Dictionary<string, Structure>();
        }

        public void AddVariable(string name, FunctionalOperation currentOperation)
        {
            _variables.Add(name, currentOperation);
        }

        public void AddStructure(string name, Structure structure)
        {
            _structures.Add(name, structure);
        }

        public FunctionalOperation GetVariable(string name) => _variables[name];
        
        public Structure GetStructure(string name) => _structures[name];
    }
}