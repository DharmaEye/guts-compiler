using System;
using System.Collections.Generic;
using System.Linq;
using Guts.Excel;
using Guts.Expressions;
using Guts.Operations;

namespace Guts
{
    public class Interpreter
    {
        private readonly IEnumerable<Expression> _expressions;

        public Interpreter()
        {
            _expressions = new List<Expression>
            {
                new MakeRowExpression(),
                new MakeCellExpression(),
                new FunctionInvokeExpression(),
                new MergeExpression(),
                new DefineModelExpression()
            };
        }

        public List<FunctionalOperation> Interpret(NerpExcelBuilder nerpBuilder, ICollection<Token> tokens)
        {
            nerpBuilder.CreateSheet("Sheet1");
            var operations = new List<FunctionalOperation>();
            FunctionalOperation prevOperation = null;
            Func<Token> peek = tokens.FirstOrDefault;
            var stateMachine = new StateMachine();
            while (tokens.Any())
            {
                var token = peek();
                var checkedExpressionsCount = 0;
                var hasPrev = false;
                Console.WriteLine(token.Type);
                foreach (var expression in _expressions)
                {
                    if (token != null && token.Type == TokenType.And)
                    {
                        hasPrev = true;
                        tokens.Remove(token);
                        token = peek();
                    }
                    
                    var result = expression.Interpret(new LogicContext(stateMachine, nerpBuilder)
                    {
                        Tokens = tokens
                    });

                    if (result.Status == FunctionalOperationStatus.Failed)
                    {
                        checkedExpressionsCount++;
                        continue;
                    }
                    if (hasPrev)
                    {
                        prevOperation = prevOperation != null ? prevOperation.SetNext(result) : result;
                    }
                    else
                    {
                        operations.Add(result);
                    }

                    prevOperation = result;
                    break;
                }

                if (checkedExpressionsCount == _expressions.Count())
                {
                    throw new RuntimeException("Syntax error");
                }
            }
            return operations;
        }
    }
}