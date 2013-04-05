using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
        private readonly Parser<char, Unit> _delimiter;
        private readonly Parser<char, Unit> _delimiters;
        private readonly Parser<char, Unit> _escapeChar;
        private readonly Parser<char, char> _hexChar;
        private readonly Parser<char, char> _escapedHexChar;
        private readonly Parser<char, char> _escapedChar;
        private readonly Parser<char, char> _charLiteralChar;
        private readonly Parser<char, char> _charLiteral;
        private readonly Parser<char, string> _stringLiteral;
        private readonly Parser<char, SyntaxNode> _primary;
        private readonly FowardParser<char, SyntaxNode> _expression;
        private readonly Parser<char, SyntaxNode> _factor;
        private readonly FowardParser<char, SyntaxNode> _term;
        private readonly Parser<char, SyntaxNode> _taggedFactor;
        private readonly Parser<char, char> _lambdaLiteralChars;
        private readonly Parser<char, string> _lambdaLiteral;
        private readonly Parser<char, SyntaxNode> _charGroupLiteral;
        private readonly FowardParser<char, CodeFlagment> _codeBlock;
        private readonly Parser<char, Unit> _endComment;
        private readonly Parser<char, string> _codeCommentBlock;
        private readonly Parser<char, string> _codeLineComment;
        private readonly Parser<char, string> _codeStringLiteral;
        private readonly Parser<char, SyntaxNode> _codedTerm;
        private readonly Parser<char, SyntaxNode> _memoTerm;
        private readonly Parser<char, SyntaxNode> _memoCodedTerm;
        private readonly Parser<char, SyntaxNode> _memoPrimary;
        private readonly Parser<char, SyntaxNode> _memoFactor;
        private readonly FowardParser<char, string> _clrTypeName;
        private readonly Parser<char, string> _clrNameComponent;
        private readonly Parser<char, string> _genericParameterList;
        private readonly Parser<char, Declare> _declareSyntax;
        private readonly Parser<char, Dictionary<string, Declare>> _declFile;
        private readonly Parser<char, Unit> _comments;

        public EbnfParser()
        {
            _comments = Lex.Or(
                    Lex.Map( Lex.Is('/'), Lex.Is('*'), Lex.Many( Lex.Generic(ch=>true).Exclude( new StringParser("*/") ) ), new StringParser("*/"), (unit, unit1, arg3, arg4) => Unit.Default() ),_=>_,
                    Lex.Map( Lex.Is('/'), Lex.Is('/'), Lex.Many( Lex.Generic(ch=>true).Exclude( Lex.Generic(ch=>ch=='\n') )), (unit, unit1, arg3) => Unit.Default() ),_=>_
                );
            _delimiter = Lex.Or( Lex.Generic(char.IsWhiteSpace),_=>Unit.Default() , _comments, _=>_ );
            _delimiters = Lex.Memoize( Lex.Map(Lex.Many(_delimiter), _ => Unit.Default()) );

            _nameFirstCharParser = Lex.Or(
                Lex.Generic(ch => char.IsUpper(ch) || char.IsLower(ch)), _ => _,
                Lex.IsIn('_'), _ => _
                );
            _nameLastCharParser = Lex.Or(
                _nameFirstCharParser, _ => _,
                Lex.Generic(char.IsDigit), _ => _ 
                );
            _nameParser = Lex.Memoize( Lex.Map(
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
                ));
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

            _stringLiteral =Lex.Memoize( Lex.Map(
                Lex.Is('\"'),
                Lex.Many(_charLiteralChar.Exclude(Lex.Is('\"'))),
                Lex.Is('\"'),
                _delimiters,
                (n0, chars, n1, n2) => chars.AsString()) );
            _lambdaLiteralChars = 
                Lex.Or(
                    Lex.Map( Lex.Is('`'),Lex.Is('`'), (unit, unit1) => '`'), _=>_,
                    Lex.Generic( ch=>ch!='`' ),_=>_
                );
            _lambdaLiteral =
                Lex.Map(Lex.Is('`'), Lex.Many(_lambdaLiteralChars), Lex.Is('`'),
                        (unit, c, arg3) => c.AsString());

            _endComment = Lex.Map(Lex.Is('*'), Lex.Is('/'),
                (unit, unit1) => Unit.Default());
            _codeCommentBlock = Lex.Map(
                    Lex.Is('/'), Lex.Is('*'), Lex.Many(Lex.Generic(ch => true).Exclude(_endComment)), _endComment,
                        (u0, u1, chars, u2) => "/*" + chars.AsString() + "*/"
                );
            _codeLineComment = Lex.Map(
                Lex.Is('/'), Lex.Is('/'), Lex.Many(Lex.Generic(ch => true).Exclude(Lex.Is('\n'))),
                (u0, u1, chars) => "//" + chars.AsString()
                );
            _codeStringLiteral = Lex.Map(
                    Lex.Is('\"'), Lex.Many(_charLiteralChar.Exclude(Lex.Is('\"'))), Lex.Is('\"'),
                        (u0, chars, u1) => "\"" + Escape(chars.AsString()) + "\""
                );
            _codeBlock = Lex.Foward<CodeFlagment>();
            _codeBlock.FowardTo = Lex.Memoize( Lex.Map(
                Lex.Is('{'), 
                Lex.Many(Lex.Or(
                    _codeBlock, _ => _,
                    _codeCommentBlock, _ => new CodeText(_),
                    _codeLineComment, _ => new CodeText(_),
                    _codeStringLiteral, _ => new CodeText(_),
                    Lex.Generic(ch => true).Exclude(Lex.Is('}')), ch => new CodeChar(ch))), 
                Lex.Is('}'),
                (u0, list, u1) =>
                {
                    var sb = new StringBuilder();
                    foreach (var item in list)
                    {
                        if (item is CodeText)
                        {
                            sb.Append(item.Text);
                        }
                        if (item is CodeChar)
                        {
                            sb.Append(item.Char);
                        }
                    }
                    return new CodeText("{" + sb.ToString() + "}") as CodeFlagment;
                }) );

            _expression = Lex.Foward<SyntaxNode>();
            _charGroupLiteral = Lex.Or(
                    Lex.Map(Lex.Is('['), _charLiteralChar, Lex.Is('.'), Lex.Is('.'), _charLiteralChar, Lex.Is(']'),
                        (u0, begin, u1, u2, end, u3) => new CharRangeLiteral( begin,end ) as SyntaxNode ),_=>_,
                    Lex.Map(Lex.Is('['), Lex.Some(_charLiteralChar.Exclude(Lex.Is(']'))), Lex.Is(']'),
                        (unit, c, arg3) => new CharSetLiteral(c) as SyntaxNode ), _=>_
                );
            _primary = Lex.Or(
                    _charLiteral, c =>  new CharLiteral(c),
                    _stringLiteral, str => new StringLiteral(str),
                    _lambdaLiteral, s => new LambdaLiteral(s),
                    _charGroupLiteral, _ => _,
                    _nameParser, name => new NameReference(name) ,
                    Lex.Map( Lex.Is('('),  _expression, Lex.Is(')'), _delimiters, (unit, node, arg3,d) => node ), node => node
                );

            _memoPrimary = Lex.Memoize(_primary);

            _factor = Lex.Or(
                    Lex.Map(_memoPrimary, Lex.IsIn('?', '*', '+'), _delimiters, (node, op, u) =>
                        {
                            switch (op)
                            {
                                case '?':
                                    return new Optional(node) as SyntaxNode;
                                case '*':
                                    return new Many(node) as SyntaxNode;
                                case '+':
                                    return new Some(node) as SyntaxNode;
                                default:
                                    throw new NotSupportedException();
                            }
                        }), _ => _,
                    _memoPrimary, _ => _
                );
            _memoFactor = Lex.Memoize(_factor);

            _taggedFactor = Lex.Or(
                    Lex.Map(_memoFactor, Lex.Is(':'), _nameParser, (node, unit, tagName) => new TaggedSyntaxNode(node, tagName) as SyntaxNode), _ => _,
                    _memoFactor, _ => _
                );
            _term = Lex.Foward<SyntaxNode>();
            _term.FowardTo = Lex.Map(Lex.Some(_taggedFactor), CreateConcat);

            _memoTerm = Lex.Memoize(_term);

            _codedTerm = Lex.Or(
                    Lex.Map(_memoTerm, _codeBlock, (node, flagment) => new CodeAnotation(node, flagment.Text) as SyntaxNode), _ => _,
                    _memoTerm, _ => _
                );
            _memoCodedTerm = Lex.Memoize(_codedTerm);

            _expression.FowardTo = Lex.Memoize(
                Lex.Map(
                Lex.Many(Lex.Map(_memoCodedTerm, Lex.Is('|'), (node, unit) => node)),
                _memoCodedTerm,
                (terms, last) => CreateAlternatives(terms.Concat(new[] {last}).ToList())) );

            _clrNameComponent = Lex.Map(Lex.Some(Lex.Generic(char.IsLetterOrDigit)), (chars) => chars.AsString());
            _genericParameterList = Lex.Map(
                Lex.Is('<'), Lex.Many(Lex.Map(_clrTypeName, Lex.Is(','), (s, unit) => s)),Lex.Optional(_clrTypeName), Lex.Is('>'),
                                            (unit, args, lastArg, u) =>
                                                {
                                                    List<string> emitArgs = new List<string>();
                                                    emitArgs.AddRange(args);
                                                    if (lastArg.HasValue)
                                                    {
                                                        emitArgs.Add(lastArg.Value);
                                                    }
                                                    return "<" + string.Join(", ", emitArgs) + ">";
                                                });
            _clrTypeName = Lex.Foward<string>();
            _clrTypeName.FowardTo = Lex.Map(
                _clrNameComponent, Lex.Many( Lex.Map( Lex.Is('.'), _clrNameComponent, (unit, s) => s  )), Lex.Optional(_genericParameterList),
                    (s, list, genParam) => s + (list.Count != 0 ? "." : "") + (genParam.HasValue ? genParam.Value : ""))
                    .ReportFail("clrTypeName parseFailed", OnParseFail);


            _declareSyntax = Lex.Memoize(
                    Lex.Map(
                        _clrTypeName.ReportFail("CLR型名が取得できませんでした",OnParseFail) ,
                        _delimiters.ReportFail("デリミタが正しくありません",OnParseFail) , 
                        _nameParser.ReportFail("名前が正しくありません",OnParseFail) , 
                        Lex.Is('=').ReportFail("=が必要です",OnParseFail) , 
                        _delimiters.ReportFail("デリミタが正しくありません",OnParseFail) , 
                        _expression.ReportFail("式が正しくありません",OnParseFail) ,
                        (type, u, name, arg3, arg4, node) => new Declare(type, name, node)).ReportFail("declareSyntax parse failed", OnParseFail)
                );

            _declFile =
                Lex.Map(
                    Lex.Many(
                        Lex.Map(
                            _declareSyntax.ReportFail("定義の取得に失敗しました",OnParseFail) ,
                            _delimiters.ReportFail("デリミタが不正です",OnParseFail),
                            Lex.Is(';').ReportFail(";が必要です", OnParseFail),
                            _delimiters.ReportFail("デリミタが不正です", OnParseFail),
                                     (declare, u0, u1, u2) => declare).Exclude( new EndParser<char>() )
                    ),
                    new EndParser<char>().ReportFail("ファイル終端に達しませんでした",OnParseFail)
                    , (list,u) => list.ToDictionary(item => item.Name))
                .ReportFail( "declFile parse failer", OnParseFail ) ;
        }

        private void OnParseFail(int index, string message)
        {
            Console.WriteLine( "{0} at {1}", message,index);
        }

        private static string Escape(string asString)
        {
            var sb = new StringBuilder();
            foreach (var ch in asString)
            {
                switch (ch)
                {
                    default:
                        sb.Append(ch);
                        break;
                    case '\a':
                        sb.Append("\\a");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\v':
                        sb.Append("\\v");
                        break;
                    case '\\':
                        sb.Append("\\");
                        break;
                    case '\'':
                        sb.Append("\\'");
                        break;
                    case '\"':
                        sb.Append("\\\"");
                        break;
                }
            }
            return sb.ToString();
        }

        private static SyntaxNode CreateConcat(IList<SyntaxNode> factors)
        {
            return factors.Count == 1 ? factors[0] : new Concat(factors);
        }

        private static SyntaxNode CreateAlternatives(IList<SyntaxNode> nodes)
        {
            return nodes.Count == 1 ? nodes[0] : new Alternative(nodes);
        }

        public bool TryParseExpression(string ebnfText, out SyntaxNode result)
        {
            var input = ebnfText.ToList();
            int endInput;
            var tryParseExpression = _expression.Parse(input, 0, out endInput, out result);
            return tryParseExpression && endInput==input.Count;
        }

        public bool TryParseDeclFile(string content, out Dictionary<string, Declare> declares)
        {
            var input = content.ToList();
            int endInput;
            var retVal = _declFile.Parse(input,0, out endInput, out declares);
            return retVal && endInput==input.Count;
        }
    }

}
