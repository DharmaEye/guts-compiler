using System.Collections.Generic;
using System.Linq;
using Guts.Excel;
using Guts.Patterns;

namespace Guts.Operations
{
    public class MethodFunctionalOperation : FunctionalOperation
    {
        public MethodFunctionalOperation(FunctionalOperationStatus status,
            StateMachine state,
            INerpExcelBuilder builder,
            PatternMatchResult result)
            : base("method", status, state, builder, result)
        {
        }

        public MethodFunctionalOperation(FunctionalOperationStatus status) : base(status)
        {
        }

        public override FunctionalOperationInvokeResult Invoke()
        {
            var names = GetAttributes("name").ToList();
            var indexes = GetAttributes("value").ToList();
            for (int i = 0; i < names.Count(); i++)
            {
                var name = names[i].Object;
                var variable = indexes[i];
                if (Parent == null)
                {
                    throw new RuntimeException($"{Name} doesn't have parent");
                }
                if (variable.Type == TokenType.Variable)
                {
                    var properties = new Queue<string>(Variable.Parse(variable.Object));
                    var structureName = properties.Dequeue();
                    var structure = State.GetStructure(structureName).Get(Variable.Generate(properties));
                    Parent.OnSetProperty(name.ToString(), structure);
                    continue;
                }
                Parent.OnSetProperty(name.ToString(), variable.Object);
            }
            return base.Invoke();
        }
    }
}