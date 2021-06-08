using System.Collections.Generic;

namespace Guts
{
    public class Variable
    {
        public static string Generate(IEnumerable<string> variables)
        {
            return string.Join('.', variables);
        }

        public static string[] Parse(string var)
        {
            return var.Split('.');
        }

        public static string[] Parse(object var)
        {
            if (!(var is string str))
            {
                throw new RuntimeException("Unexpected error happened");
            }

            return str.Split('.');
        }
    }
}