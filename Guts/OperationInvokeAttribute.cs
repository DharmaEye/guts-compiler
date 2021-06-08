using System;

namespace Guts
{
    public class OperationInvokeAttribute : Attribute
    {
        public string Name { get; }

        public OperationInvokeAttribute(string name)
        {
            Name = name;
        }
    }
}