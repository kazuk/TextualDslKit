using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParserCombinator;

namespace ParserBuilder
{
    using Lex = Parsers<char>;

    public static class ListCharExtention
    {
        public static string AsString(this IList<char> list)
        {
            return new string(list.ToArray());
        }
    }

    public class EbnfParser
    {
        private readonly Parser<char, char> _nameFirstCharParser;
        private readonly Parser<char, char> _nameLastCharParser;
        private readonly Parser<char, string> _nameParser;
        private readonly Parser<char, char> _delimiter;
        private readonly Parser<char, Unit> _delimiters;
        private readonly Parser<char, Unit> _escapeChar;
        private readonly Parser<char, char> _hexChar;
        private readonly Parser<char, char> _escapedHexChar;
        private readonly Parser<char, char> _escapedChar;
        private readonly Parser<char, char> _charLiteralChar;
        private readonly Parser<char, char> _charLiteral;
        private readonly Parser<char, string> _stringLiteral;
        private readonly Parser<char, Node> _primary;
        private readonly FowardParser<char, Node> _expression;
        private readonly Parser<char, Node> _factor;
        private readonly FowardParser<char, Node> _term;
        private readonly Parser<char, Node> _taggedFactor;
        private readonly Parser<char, char> _lambdaLiteralChars;
        private Parser<char, string> _lambdaLiteral;
        private Parser<char, Node> _charGroupLiteral;

        public EbnfParser()
        {
            _delimiter = Lex.Generic(char.IsWhiteSpace);
            _delimiters = Lex.Map(Lex.Many(_delimiter), _ => Unit.Default());

            _nameFirstCharParser = Lex.Or(
                Lex.Generic(ch => char.IsUpper(ch) || char.IsLower(ch)), _ => _,
                Lex.IsIn('_'), _ => _
                );
            _nameLastCharParser = Lex.Or(
                _nameFirstCharParser, _ => _,
                Lex.Generic(char.IsDigit), _ => _ 
                );
            _nameParser = Lex.Map(
                _nameFirstCharParser,
                Lex.Many(_nameLastCharParser),
                _delimiters,
                (first, last,_) =>
                    {
                        var sb = new StringBuilder();
                        sb.Append(first);
                        sb.Append(last.ToArray());
                        return sb.ToString();
                    }
                );
            _hexChar = Lex.Or(
                Lex.Generic(char.IsDigit), _ => _,
                Lex.Range('a', 'f'), _ => _,
                Lex.Range('A', 'F'), _ => _);
            _escapeChar = Lex.Is('\\');

            _escapedHexChar = Lex.Map(
                    _escapeChar,
                    Lex.IsIn('u','U','x','X'),
                    Lex.Some( _hexChar ),
                    (esc, uni, hexChars) =>
                        {
                            checked
                            {
                                return (char) int.Parse(hexChars.AsString(), NumberStyles.HexNumber);
                            }
                        }
                );
            _escapedChar = Lex.Or(
                    _escapedHexChar, _=>_,
                    Lex.Map(_escapeChar, Lex.IsIn('\\','\'', '\"'), (unit, ch) => ch), _ => _,
                    Lex.Map( _escapeChar, Lex.Is('a'), (unit, unit1) => '\a'), _ => _,
                    Lex.Map( _escapeChar, Lex.Is('b'), (unit, unit1) => '\b'), _ => _,
                    Lex.Map( _escapeChar, Lex.Is('f'), (unit, unit1) => '\f' ),_ => _,
                    Lex.Map( _escapeChar, Lex.Is('n'), (unit, unit1) => '\n' ),_ => _,
                    Lex.Map( _escapeChar, Lex.Is('r'), (unit, unit1) => '\r' ),_ => _,
                    Lex.Map( _escapeChar, Lex.Is('t'), (unit, unit1) => '\t' ),_ => _,
                    Lex.Map( _escapeChar, Lex.Is('v'), (unit, unit1) => '\v' ),_ => _
                );
            _charLiteralChar = Lex.Or(
                    _escapedChar, _=>_,
                    Lex.Generic( ch=> true ),_=>_
                );

            _charLiteral = Lex.Map(
                    Lex.Is('\''), 
                    _charLiteralChar, 
                    Lex.Is('\''),
                    _delimiters,
                    (n0, ch, n1,n2) => ch);

            _stringLiteral = Lex.Map(
                Lex.Is('\"'),
                Lex.Many(_charLiteralChar.Exclude(Lex.Is('\"'))),
                Lex.Is('\"'),
                _delimiters,
                (n0, chars, n1, n2) => chars.AsString());
            _lambdaLiteralChars = 
                Lex.Or(
                    Lex.Map( Lex.Is('`'),Lex.Is('`'), (unit, unit1) => '`'), _=>_,
                    Lex.Generic( ch=>ch!='`' ),_=>_
                );
            _lambdaLiteral =
                Lex.Map(Lex.Is('`'), Lex.Many(_lambdaLiteralChars), Lex.Is('`'),
                        (unit, c, arg3) => c.AsString());
            _expression = Lex.Foward<Node>();
            _charGroupLiteral = Lex.Or(
                    Lex.Map(Lex.Is('['), _charLiteralChar, Lex.Is('.'), Lex.Is('.'), _charLiteralChar, Lex.Is(']'),
                        (u0, begin, u1, u2, end, u3) => new CharRangeLiteral( begin,end ) as Node ),_=>_,
                    Lex.Map(Lex.Is('['), Lex.Some(_charLiteralChar.Exclude(Lex.Is(']'))), Lex.Is(']'),
                        (unit, c, arg3) => new CharSetLiteral(c) as Node ), _=>_
                );
            _primary = Lex.Or(
                    _charLiteral, c =>  new CharLiteral(c),
                    _stringLiteral, str => new StringLiteral(str),
                    _lambdaLiteral, s => new LambdaLiteral(s),
                    _charGroupLiteral, _ => _,
                    _nameParser, name => new NameReference(name) ,
                    Lex.Map( Lex.Is('('),  _expression, Lex.Is(')'), _delimiters, (unit, node, arg3,d) => node ), node => node
                );

            _factor = Lex.Or(
                    Lex.Map( _primary, Lex.Is('?'),_delimiters , (node, unit,u) => new Optional(node) as Node ), _=>_,
                    Lex.Map(_primary, Lex.Is('*'), _delimiters, (node, unit, u) => new Meny(node) as Node), _ => _,
                    Lex.Map(_primary, Lex.Is('+'), _delimiters, (node, unit, u) => new Some(node) as Node), _ => _,
                    _primary, _=>_
                );
            _taggedFactor = Lex.Or(
                    Lex.Map( _factor , Lex.Is(':'), _nameParser, (node, unit, tagName) => new TaggedNode(node, tagName) as Node ),_=>_,
                    _factor, _=>_
                );
            _term = Lex.Foward<Node>();
            _term.FowardTo = Lex.Map(Lex.Some(_taggedFactor), CreateConcat);
            _expression.FowardTo = Lex.Map(
                Lex.Many(Lex.Map(_term, Lex.Is('|'), (node, unit) => node)),
                _term,
                (terms, last) => CreateAlternatives(terms.Concat(new[] {last}).ToList()));
        }

        private static Node CreateConcat(IList<Node> factors)
        {
            if (factors.Count == 1) return factors[0];
            return new Concat(factors);
        }

        private static Node CreateAlternatives(IList<Node> nodes)
        {
            if (nodes.Count == 1) return nodes[0];
            return new Alternative(nodes);
        }

        public bool TryParseExpression(string ebnfText, out Node result)
        {
            var input = ebnfText.ToList();
            int endInput;
            bool tryParseExpression = _expression.Parse(input, 0, out endInput, out result);
            return tryParseExpression && endInput==input.Count;
        }
    }

    public class CharSetLiteral : Node
    {
        private readonly IList<char> _chars;

        public CharSetLiteral(IList<char> chars)
        {
            _chars = chars;
        }

        public IList<char> Chars
        {
            get { return _chars; }
        }

        public override string Describe()
        {
            return string.Format("[{0}]",
                string.Join(",", Chars.Select(ch => string.Format("'{0}'", ch))));
        }
    }

    public class CharRangeLiteral : Node
    {
        private readonly char _begin;
        private readonly char _end;

        public CharRangeLiteral(char begin, char end)
        {
            _begin = begin;
            _end = end;
        }

        public char Begin
        {
            get { return _begin; }
        }

        public char End
        {
            get { return _end; }
        }

        public override string Describe()
        {
            return string.Format("[\'{0}\'..\'{1}\']", Begin, End);
        }
    }

    public class LambdaLiteral : Node
    {
        private readonly string _lambdaText;

        public LambdaLiteral(string lambdaText)
        {
            _lambdaText = lambdaText;
        }

        public string LambdaText
        {
            get { return _lambdaText; }
        }

        public override string Describe()
        {
            return string.Format("{{{0}}}", LambdaText);
        }
    }

    public class TaggedNode : Node
    {
        private readonly Node _node;
        private readonly string _tagName;

        public TaggedNode(Node node, string tagName)
        {
            _node = node;
            _tagName = tagName;
        }

        public override string Describe()
        {
            return "("+ _node.Describe() + "->" + _tagName +")";
        }
    }

    public class Alternative : Node
    {
        private readonly IList<Node> _terms;

        public Alternative(IList<Node> terms)
        {
            _terms = terms;
        }

        public override string Describe()
        {
            return "(" + string.Join("|", _terms.Select(t => t.Describe())) + ")";
        }
    }

    public class Some : Node
    {
        private readonly Node _node;

        public Some(Node node)
        {
            _node = node;
        }

        public override string Describe()
        {
            return string.Format("({0})+", _node.Describe());
        }
    }

    public class Meny : Node
    {
        private readonly Node _node;

        public Meny(Node node)
        {
            _node = node;
        }

        public override string Describe()
        {
            return string.Format("({0})*", _node.Describe());
        }
    }

    public class Optional : Node
    {
        private readonly Node _node;

        public Optional(Node node)
        {
            _node = node;
        }

        public override string Describe()
        {
            return string.Format("({0})?", _node.Describe());
        }
    }

    public class Concat : Node
    {
        private readonly IList<Node> _factors;

        public Concat(IList<Node> factors)
        {
            _factors = factors;
        }

        public override string Describe()
        {
            return string.Format("({0})", string.Join("+", _factors.Select(f => f.Describe())));
        }
    }

    public class NameReference : Node
    {
        private readonly string _name;

        public NameReference(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        public override string Describe()
        {
            return string.Format("[{0}]", Name);
        }
    }

    public class StringLiteral : Node
    {
        private readonly string _str;

        public StringLiteral(string str)
        {
            _str = str;
        }

        public string Value
        {
            get { return _str; }
        }

        public override string Describe()
        {
            return string.Format("\"{0}\"", Value);
        }
    }

    public class CharLiteral : Node
    {
        private readonly char _value;

        public CharLiteral(char value)
        {
            _value = value;
        }

        public char Value
        {
            get { return _value; }
        }

        public override string Describe()
        {
            return string.Format("\'{0}\' /* \\x{1:x} */ ", Value, (int)Value );
        }
    }

    public abstract class Node
    {
        public abstract string Describe();

        public override string ToString()
        {
            return Describe();
        }
    }
}
