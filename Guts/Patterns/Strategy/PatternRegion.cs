using System.Collections.Generic;
using System.Linq;

namespace Guts.Patterns.Strategy
{
    public abstract class PatternRegion
    {
        protected IReadOnlyDictionary<string, TokenType> Keywords { get; }
        public PatternRegionContext Context { get; }

        public List<StrategyCommand> OriginalCommands { get; set; }
        public Queue<StrategyCommand> Commands { get; set; }
        public PatternRegion Neighbor;
        public PatternRegion Parent;
        public PatternRegion Child;

        private readonly Dictionary<string, StateVariable> _variables = new Dictionary<string, StateVariable>();

        public PatternRegion State { get; set; }

        protected PatternRegion(IReadOnlyDictionary<string, TokenType> keywords, PatternRegionContext context)
        {
            Keywords = keywords;
            Context = context;
            OriginalCommands = new List<StrategyCommand>();
            Commands = new Queue<StrategyCommand>();
            State = this;
        }

        protected virtual PatternRegionResult EnterChildRegion(Token token)
        {
            Context.SetState(Child);
            return Child.Handle(token);
        }

        protected virtual PatternRegionResult EnterNeighborRegion(Token token)
        {
            Context.SetState(Neighbor);
            return Neighbor.Handle(token);
        }

        protected virtual PatternRegionResult LeaveRegion(Token token)
        {
            Context.SetState(Parent);
            return Parent.Handle(token);
        }

        public void AddChild(PatternRegion region)
        {
            region.Parent = this;
            if (Child != null)
            {
                EnqueueCommand(new StrategyCommand(RegionConsts.EnterNeighborRegion));
                AddNeighbor(region);
            }
            else
            {
                EnqueueCommand(new StrategyCommand(RegionConsts.EnterChildRegion));
                Child = region;
            }
        }

        public StrategyCommand DequeueCommand(Token token)
        {
            var command = Commands.Dequeue();
            if (command.HasVariable)
            {
                AddVariable(command.VariableName, token.Literal, token.Type);
            }
            return command;
        }

        protected virtual void AddVariable(string name, object value, TokenType type)
        {
            if (!_variables.TryGetValue(name, out var variable))
            {
                _variables.Add(name.ToLower(), new StateVariable(value, type));
                return;
            }

            var @var = variable;
            @var.AddVariable(value, type);
        }

        public void AddNeighbor(PatternRegion region)
        {
            Neighbor = region;
        }

        public void EnqueueCommand(StrategyCommand command)
        {
            OriginalCommands.Add(command);
            Commands.Enqueue(command);
        }

        public void SetVariable(string name)
        {
            var command = Commands.Last();
            command.SetVariable(name);
        }

        public void RestoreSubStates()
        {
            Commands = OriginalCommands.ToQueue();
            Child?.RestoreSubStates();
            Neighbor?.RestoreSubStates();
        }

        public abstract PatternRegionResult Handle(Token token);

        private Dictionary<string, StateVariable> GetVariables(IDictionary<string, StateVariable> dictionary)
        {
            foreach (var (key, value) in _variables)
            {
                dictionary.Add(key, value);
            }
            Child?.GetVariables(dictionary);
            Neighbor?.GetVariables(dictionary);
            return dictionary.ToDictionary(s => s.Key, s => s.Value);
        }

        public Dictionary<string, StateVariable> GetVariables()
        {
            return GetVariables(new Dictionary<string, StateVariable>());
        }

        public PatternRegionResult CheckToken(string type, TokenType expected)
        {
            var isSameToken = Keywords.TryGetValue(type, out var tokenType) && tokenType == expected;
            return isSameToken ? Succeeded : Failed;
        }

        public PatternRegionResult Succeeded => PatternRegionResult.Succeeded;
        public PatternRegionResult Failed => PatternRegionResult.Failed;
        /// <summary>
        /// Operation hasn't had enough data to proceed being that it's marked as finished
        /// </summary>
        public PatternRegionResult Finished => PatternRegionResult.Finished;

        public bool IsFinished()
        {
            return !Commands.Any();
        }
    }
}