using System.Collections.Generic;

namespace Guts
{
    public class Operation
    {
        public string Name { get; }
        public Dictionary<string, object> Attributes { get; }
        public object Result { get; set; }

        private Operation _nextOperation;
        private Operation _prevOperation;
        private Operation _parentOperation;

        public Operation(string name)
        {
            Name = name;
            Attributes = new Dictionary<string, object>();
        }

        public void SetParent(Operation parent)
        {
            _parentOperation = parent;
        }

        public void SetPrev(Operation prev)
        {
            _prevOperation = prev;
        }

        public void SetNext(Operation next)
        {
            _nextOperation = next;
        }

        public Operation GetParent()
        {
            return _parentOperation;
        }

        public Operation GetPrev()
        {
            return _prevOperation;
        }

        public Operation GetNext()
        {
            return _nextOperation;
        }
    }
}