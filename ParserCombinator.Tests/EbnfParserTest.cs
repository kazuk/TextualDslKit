using System;
using System.Collections.Generic;
using System.IO;
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
            SyntaxNode syntaxNode;
            ebnfParser.TryParseExpression("'a'",out syntaxNode).IsTrue();
            syntaxNode.IsInstanceOf<CharLiteral>().Is( l=>l.Value=='a');
            ebnfParser.TryParseExpression("[a..z]", out syntaxNode).IsTrue();
            syntaxNode.IsInstanceOf<CharRangeLiteral>().Is(l => l.Begin=='a' && l.End=='z' );
            ebnfParser.TryParseExpression("[0123456789]", out syntaxNode).IsTrue();
            syntaxNode.IsInstanceOf<CharSetLiteral>().Is(l => l.Chars.Count==10 && l.Chars.Contains('5') );
            ebnfParser.TryParseExpression("a", out syntaxNode).IsTrue();
            syntaxNode.IsInstanceOf<NameReference>().Is(l=>l.Name=="a");
            ebnfParser.TryParseExpression("\"a\"",out syntaxNode).IsTrue();
            syntaxNode.IsInstanceOf<StringLiteral>().Is(l=>l.Value=="a");
            ebnfParser.TryParseExpression("`char.IsDigit`", out syntaxNode).IsTrue();
            syntaxNode.IsInstanceOf<LambdaLiteral>().Is(l => l.LambdaText == "char.IsDigit");
            ebnfParser.TryParseExpression("a b", out syntaxNode).IsTrue();
            syntaxNode.IsInstanceOf<Concat>().Is( c=>c.Describe()=="([a]+[b])");
            ebnfParser.TryParseExpression("a? b", out syntaxNode).IsTrue();
            syntaxNode.IsInstanceOf<Concat>().Is(c => c.Describe() == "(([a])?+[b])");
            ebnfParser.TryParseExpression("a* b", out syntaxNode).IsTrue();
            syntaxNode.IsInstanceOf<Concat>().Is(c => c.Describe() == "(([a])*+[b])");
            ebnfParser.TryParseExpression("a+ b", out syntaxNode).IsTrue();
            syntaxNode.IsInstanceOf<Concat>().Is(c => c.Describe() == "(([a])++[b])");
            ebnfParser.TryParseExpression("a|b", out syntaxNode).IsTrue();
            syntaxNode.IsInstanceOf<Alternative>().Is(a => a.Describe() == "([a]|[b])");
            ebnfParser.TryParseExpression("a|b|c", out syntaxNode).IsTrue();
            syntaxNode.IsInstanceOf<Alternative>().Is(a => a.Describe() == "([a]|[b]|[c])");
            ebnfParser.TryParseExpression("a b|c", out syntaxNode).IsTrue();
            syntaxNode.IsInstanceOf<Alternative>().Is(a => a.Describe() == "(([a]+[b])|[c])");
            ebnfParser.TryParseExpression("(a|b) d", out syntaxNode).IsTrue();
            syntaxNode.IsInstanceOf<Concat>().Is(a => a.Describe() == "(([a]|[b])+[d])");
            ebnfParser.TryParseExpression("a:first a:next a:last", out syntaxNode).IsTrue();
            syntaxNode.IsInstanceOf<Concat>().Is(a => a.Describe() == "(([a]->first)+([a]->next)+([a]->last))");

            ebnfParser.TryParseExpression("a:first a:next a:last { this is code }", out syntaxNode).IsTrue();
            syntaxNode.IsInstanceOf<CodeAnotation>().Is(a => a.Describe() == "((([a]->first)+([a]->next)+([a]->last))=>{ this is code })");
        }

        [TestMethod]
        public void ParseDeclFile()
        {
            var ebnfParser = new EbnfParser();
            var content = File.ReadAllText("TestParserDef\\ValidParserDef.txt");
            Dictionary<string, Declare> decls;
            ebnfParser.TryParseDeclFile(content, out decls).IsTrue();

        }
    }
}
