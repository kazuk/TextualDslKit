using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParserBuilder;

namespace ParserCombinator.Tests
{
    [TestClass]
    public class EbnfParserTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var ebnfParser = new EbnfParser();
            Node node;
            ebnfParser.TryParseExpression("'a'",out node).IsTrue();
            node.IsInstanceOf<CharLiteral>().Is( l=>l.Value=='a');
            ebnfParser.TryParseExpression("[a..z]", out node).IsTrue();
            node.IsInstanceOf<CharRangeLiteral>().Is(l => l.Begin=='a' && l.End=='z' );
            ebnfParser.TryParseExpression("[0123456789]", out node).IsTrue();
            node.IsInstanceOf<CharSetLiteral>().Is(l => l.Chars.Count==10 && l.Chars.Contains('5') );
            ebnfParser.TryParseExpression("a", out node).IsTrue();
            node.IsInstanceOf<NameReference>().Is(l=>l.Name=="a");
            ebnfParser.TryParseExpression("\"a\"",out node).IsTrue();
            node.IsInstanceOf<StringLiteral>().Is(l=>l.Value=="a");
            ebnfParser.TryParseExpression("`char.IsDigit`", out node).IsTrue();
            node.IsInstanceOf<LambdaLiteral>().Is(l => l.LambdaText == "char.IsDigit");
            ebnfParser.TryParseExpression("a b", out node).IsTrue();
            node.IsInstanceOf<Concat>().Is( c=>c.Describe()=="([a]+[b])");
            ebnfParser.TryParseExpression("a? b", out node).IsTrue();
            node.IsInstanceOf<Concat>().Is(c => c.Describe() == "(([a])?+[b])");
            ebnfParser.TryParseExpression("a* b", out node).IsTrue();
            node.IsInstanceOf<Concat>().Is(c => c.Describe() == "(([a])*+[b])");
            ebnfParser.TryParseExpression("a+ b", out node).IsTrue();
            node.IsInstanceOf<Concat>().Is(c => c.Describe() == "(([a])++[b])");
            ebnfParser.TryParseExpression("a|b", out node).IsTrue();
            node.IsInstanceOf<Alternative>().Is(a => a.Describe() == "([a]|[b])");
            ebnfParser.TryParseExpression("a|b|c", out node).IsTrue();
            node.IsInstanceOf<Alternative>().Is(a => a.Describe() == "([a]|[b]|[c])");
            ebnfParser.TryParseExpression("a b|c", out node).IsTrue();
            node.IsInstanceOf<Alternative>().Is(a => a.Describe() == "(([a]+[b])|[c])");
            ebnfParser.TryParseExpression("(a|b) d", out node).IsTrue();
            node.IsInstanceOf<Concat>().Is(a => a.Describe() == "(([a]|[b])+[d])");
            ebnfParser.TryParseExpression("a:first a:next a:last", out node).IsTrue();
            node.IsInstanceOf<Concat>().Is(a => a.Describe() == "(([a]->first)+([a]->next)+([a]->last))");
        }
    }
}
