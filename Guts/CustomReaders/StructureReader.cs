using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;

namespace Guts.CustomReaders
{
    internal class StructureIndexTable
    {
        private readonly Dictionary<string, Structure> _structures;

        public StructureIndexTable()
        {
            _structures = new Dictionary<string, Structure>();
        }

        public void Add(string name, Structure structure)
        {
            _structures.Add(name, structure);
        }

        public Structure Get(string name)
        {
            return _structures[name];
        }
    }

    public class Structure
    {
        public string _propertyName;
        private object _value;
        private bool _empty;

        private Structure _next;
        private Structure _prev;
        private Structure _child;
        private Structure _parent;
        private Structure _root;

        private StructureIndexTable _indexTable;

        private Structure()
        {
            _indexTable ??= new StructureIndexTable();
        }

        public static Structure Create(string propertyName)
        {
            Guard.Against.NullOrEmpty(propertyName, nameof(propertyName));
            return new Structure
            {
                _propertyName = propertyName
            };
        }

        public Structure GetChild()
        {
            return _child ??= new Structure
            {
                _empty = true,
                _parent = this,
                _root = this,
                _indexTable = _indexTable
            };
        }

        public Structure GetParent()
        {
            return _parent;
        }

        public Structure AddNext(Structure structure)
        {
            Guard.Against.Null(structure, nameof(structure));
            structure._prev = this;
            structure._root = _root;
            structure._parent = _parent;
            structure._indexTable = _indexTable;
            if (_next == null)
            {
                _next = structure;
            }
            else
            {
                _next?.AddNext(structure);
            }
            return structure;
        }

        public Structure GetFirst()
        {
            var structure = _prev;
            while (true)
            {
                if (structure._prev == null)
                {
                    break;
                }
                structure = structure._prev;
            }
            return structure;
        }

        private void ToIndex(IEnumerable<string> parents, List<string> properties)
        {
            var newParents = parents.Concat(new[] {_propertyName});
            _child?.ToIndex(newParents, properties);
            _next?.ToIndex(parents, properties);
            if (!_empty && _child == null)
            {
                _indexTable.Add(string.Join('.', newParents), this);
            }
        }

        public Structure IndexFields()
        {
            var structure = GetFirst();
            structure.ToIndex(new List<string>(), new List<string>());
            return structure;
        }

        public Structure Get(string what)
        {
            return _indexTable.Get(what);
        }
    }

    public class StructureReader
    {
        private readonly ICollection<Token> _tokens;
        private int _current = 0;
        private Structure _structure;
        private bool _hasFinished = false;

        public StructureReader(ICollection<Token> tokens)
        {
            _tokens = tokens;
        }

        public Structure ReadStructure()
        {
            var proceededTokens = new List<Token>();
            var brackets = 0;
            while (!IsAtEnd())
            {
                var token = Peek();
                if (!token)
                {
                    break;
                }

                proceededTokens.Add(token);
                switch (token.Type)
                {
                    case TokenType.Comma:
                        Advance();
                        continue;
                    case TokenType.CurlyBracketsOpen:
                        brackets++;
                        EnterParent();
                        Advance();
                        continue;
                    case TokenType.CurlyBracketsClose:
                        brackets--;
                        LeaveParent();
                        Advance();
                        continue;
                    default:
                        AddProperty(token.Literal.ToString());
                        Advance();
                        continue;
                }
            }
            foreach (var proceededToken in proceededTokens)
            {
                _tokens.Remove(proceededToken);
            }
            if (brackets > 0)
            {
                throw new RuntimeException("Unexpected token");
            }
            return _structure.IndexFields();
        }

        private void EnterParent()
        {
            if (_structure == null)
            {
                return;
            }
            _structure = _structure.GetChild();
        }

        private void AddProperty(string propName)
        {
            if (_structure == null)
            {
                _structure = Structure.Create(propName);
                return;
            }
            _structure = _structure.AddNext(Structure.Create(propName));
        }

        private void LeaveParent()
        {
            if (_structure == null)
            {
                throw new RuntimeException("Unexpected token");
            }

            var parent = _structure.GetParent();
            if (parent == null)
            {
                _hasFinished = true;
            }
            _structure = parent ?? _structure;
        }

        private void Advance()
        {
            _current++;
        }

        private bool IsAtEnd()
        {
            return _current > _tokens.Count - 1 || _hasFinished;
        }

        private Token Peek()
        {
            if (IsAtEnd())
            {
                return null;
            }

            return _tokens.ElementAt(_current);
        }
    }
}