using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ParserCombinator.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void EndParserTest()
        {
            var chars = new List<char>();
            var endParser = new EndParser<char>();
            int endInput;
            Unit _;
            endParser.Parse(chars,0,out endInput, out _).IsTrue();
            endInput.Is(0);
            chars.Add('a');
            endParser.Parse(chars,0, out endInput, out _).IsFalse();
            endParser.Parse(chars, 1, out endInput, out _).IsTrue();
        }

        [TestMethod]
        public void IsParserTest()
        {
            var chars = new List<char>();
            var isParser = new IsParser<char>('a');
            int endInput;
            Unit _;
            isParser.Parse(chars, 0, out endInput, out _).IsFalse();
            endInput.Is(0);
            _.IsNull();
            chars.Add('a');
            isParser.Parse(chars, 0, out endInput, out _).IsTrue();
            endInput.Is(1);
            _.IsNull();
            chars.Add('b');
            isParser.Parse(chars, 1, out endInput, out _).IsFalse();
            endInput.Is(1);
        }

        [TestMethod]
        public void IsInParserTest()
        {
            var chars = new List<char>();
            var isInParser = new IsInParser<char>('a', 'b', 'c');
            int endInput;
            char value;
            isInParser.Parse(chars,0, out endInput, out value).IsFalse();
            endInput.Is(0);
            chars.Add('a');
            isInParser.Parse(chars,0, out endInput, out value).IsTrue();
            endInput.Is(1);
            value.Is('a');
            chars.Add('x');
            isInParser.Parse(chars,1, out endInput, out value).IsFalse();
            endInput.Is(1);
            value.Is(default(char));
        }

        [TestMethod]
        public void RangeParserTest()
        {
            var chars = new List<char>();
            var rangeParser = new RangeParser<char>('a', 'z');
            int endInput;
            char value;
            rangeParser.Parse(chars,0,out endInput, out value).IsFalse();
            endInput.Is(0);
            chars.Add('a');
            rangeParser.Parse(chars,0,out endInput, out value).IsTrue();
            endInput.Is(1);
            value.Is('a');
            chars.Add('0');
            rangeParser.Parse(chars,1,out endInput, out value).IsFalse();
            endInput.Is(1);
            value.Is(default(char));
        }

        [TestMethod]
        public void StringParserTest()
        {
            var chars = new List<char>();
            var stringParser = new StringParser("abc");
            int endInput;
            string value;
            stringParser.Parse(chars,0,out endInput, out value).IsFalse();
            endInput.Is(0);
            chars.Add('a');
            stringParser.Parse(chars, 0, out endInput, out value).IsFalse();
            endInput.Is(0);
            chars.Add('b');
            stringParser.Parse(chars, 0, out endInput, out value).IsFalse();
            endInput.Is(0);
            chars.Add('c');
            stringParser.Parse(chars, 0, out endInput, out value).IsTrue();
            endInput.Is(3);
            value.Is("abc");
        }

        [TestMethod]
        public void OrParserTest()
        {
            var chars = new List<char>();
            var p1 = new RangeParser<char>('A', 'Z');
            var p2 = new RangeParser<char>('a', 'z');
            var orParser = new OrParser<char, char, char, char>(
                p1, _ => _, 
                p2, _ => _);
            int endInput;
            char result;
            orParser.Parse(chars, 0, out endInput, out result).IsFalse();
            chars.Add('0');
            orParser.Parse(chars, 0, out endInput, out result).IsFalse();
            chars.Add('a');
            orParser.Parse(chars, 1, out endInput, out result).IsTrue();
            result.Is('a');
            chars.Add('C');
            orParser.Parse(chars, 2, out endInput, out result).IsTrue();
            result.Is('C');
            chars.Add('0');
            orParser.Parse(chars, 3, out endInput, out result).IsFalse();
        }

        [TestMethod]
        public void MapParserTest()
        {
            var chars = new List<char>();
            var p0 = new RangeParser<char>('A', 'Z');
            var mapParser = new MapParser<char, char, char, string>(
                p0, p0,
                (ch0, ch1) => new string(new[] {ch0, ch1}));
            int endInput;
            string result;
            mapParser.Parse(chars, 0, out endInput, out result).IsFalse();
            chars.Add('A');
            mapParser.Parse(chars,0, out endInput,out result).IsFalse();
            chars.Add('Z');
            mapParser.Parse(chars,0, out endInput, out result).IsTrue();
            result.Is("AZ");
        }

        [TestMethod]
        public void SimpleIntegerParseTest()
        {
            var parser = new MapParser<char, char, IList<char>, string>(
                    new RangeParser<char>('1','9'),
                    new ManyParser<char, char>( new RangeParser<char>('0','9') ), 
                    (head,tail)=>
                        {
                            var sb = new StringBuilder();
                            sb.Append(head);
                            sb.Append(tail.ToArray());
                            return sb.ToString();
                        }
                );
            string result;
            int endInput;
            parser.Parse("100".ToList(), 0,out endInput, out result).IsTrue();
            result.Is("100");
            parser.Parse("1".ToList(), 0, out endInput, out result).IsTrue();
            result.Is("1");
        }

        [TestMethod]
        public void SimpleXmlParseTest()
        {
            var nameParser = Parsers<char>.Map(
                Parsers<char>.Some(
                    Parsers<char>.Or(new RangeParser<char>('A', 'Z'), _ => _,
                                     new RangeParser<char>('a', 'z'), _ => _)),
                list =>
                    {
                        var sb = new StringBuilder();
                        sb.Append(list.ToArray());
                        return sb.ToString();
                    });

            var valueParser = new MapParser<char, Unit, IList<char>, Unit, string>(
                    new IsParser<char>('\"'), 
                    new ManyParser<char,char>( new GenericParser<char>( ch=> ch!='\"' ) ),
                    new IsParser<char>('\"'), 
                    (_,value,_1) => string.Join("",value)
                );
            var attrParser = new MapParser<char, string, Unit,string, Option<Unit>, Tuple<string, string>>(
                nameParser, new IsParser<char>('='), valueParser, new OptionParser<char,Unit>( new IsParser<char>(' ') ),
                (attrName,_,attrValue,_delim)=>Tuple.Create(attrName,attrValue));

            var innerTagParser = new MapParser<char, string, Unit, IList<Tuple<string, string>>, XElement>(
                nameParser,
                new IsParser<char>(' '),
                new ManyParser<char, Tuple<string, string>>(attrParser),
                (tagName, _, attrs) =>
                    {
                        var xe = new XElement(tagName);
                        foreach (var tuple in attrs)
                        {
                            xe.Add(new XAttribute(tuple.Item1, tuple.Item2));
                        }
                        return xe;
                    }
                );

            var tagParser = new MapParser<char, Unit, XElement, Unit, XElement>(
                new IsParser<char>('<'), 
                innerTagParser,
                new IsParser<char>('>'), 
                (_,elm,_1) => elm
                );

            const string tagText = @"<test hoge=""hage"" foo=""bar"">";
            int endInput;
            XElement result;
            tagParser.Parse(tagText.ToList(), 0, out endInput, out result).IsTrue();
            result.Name.Is("test");
            result.Attribute("hoge").Value.Is("hage");
            result.Attribute("foo").Value.Is("bar");

        }
    }

}
