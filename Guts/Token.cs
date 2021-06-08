namespace Guts
{
    public class Token
    {
        public TokenType Type { get; }
        public int Line { get; }
        public object Literal { get; }

        public Token(TokenType type, int line, object literal)
        {
            Type = type;
            Line = line;
            Literal = literal;
        }

        public static implicit operator bool(Token token)
        {
            return token != null;
        }
    }
}