using System;
using Guts.Operations;
using Guts.Patterns;

namespace Guts.Expressions
{
    public abstract class Expression
    {
        public abstract FunctionalOperation Interpret(LogicContext context);

        protected FunctionalOperation CreateByPattern<T>(string pattern, LogicContext context) 
            where T : FunctionalOperation
        {
            var patternProvider = new PatternProvider(context.Tokens, pattern);
            var matchResult = patternProvider.Match();
            if (!matchResult.Succeeded)
            {
                return new FunctionalOperation(FunctionalOperationStatus.Failed);
            }

            var args = new object[]
            {
                FunctionalOperationStatus.Succeeded,
                context.State,
                context.Builder,
                matchResult
            };
            return Activator.CreateInstance(typeof(T), args) as T;
        }
    }
}