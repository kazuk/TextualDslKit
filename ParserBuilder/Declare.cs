using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserBuilder
{
    public class Declare
    {
        private readonly string _type;
        private readonly string _name;
        private readonly SyntaxNode _syntaxNode;

        public Declare(string type, string name, SyntaxNode syntaxNode)
        {
            _type = type;
            _name = name;
            _syntaxNode = syntaxNode;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Type
        {
            get { return _type; }
        }

        public SyntaxNode SyntaxNode
        {
            get { return _syntaxNode; }
        }
    }

    public class CodeAnotation : SyntaxNode
    {
        private readonly SyntaxNode _syntaxNode;
        private readonly string _code;

        public CodeAnotation(SyntaxNode syntaxNode, string code)
        {
            _syntaxNode = syntaxNode;
            _code = code;
        }

        public SyntaxNode SyntaxNode
        {
            get { return _syntaxNode; }
        }

        public string Code
        {
            get { return _code; }
        }

        public override string Describe()
        {
            return "(" + SyntaxNode.Describe() + "=>" + Code + ")";
        }

        public override void Visit(Action<SyntaxNode> nodeAction)
        {
            nodeAction(this);

            SyntaxNode.Visit(nodeAction);
        }
    }

    public abstract class CodeFlagment
    {
        public abstract string Text { get; }
        public abstract char Char { get; }
    }

    public class CodeText : CodeFlagment
    {
        private readonly string _text;

        public CodeText(string text)
        {
            _text = text;
        }

        public override string Text
        {
            get { return _text; }
        }

        public override char Char
        {
            get { throw new NotSupportedException(); }
        }
    }

    public class CodeChar : CodeFlagment
    {
        private readonly char _ch;

        public CodeChar(char ch)
        {
            _ch = ch;
        }

        public override string Text
        {
            get { throw new NotSupportedException(); }
        }

        public override char Char
        {
            get { return _ch; }
        }
    }

    public class CharSetLiteral : SyntaxNode
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

        public override void Visit(Action<SyntaxNode> nodeAction)
        {
            nodeAction(this);
        }
    }

    public class CharRangeLiteral : SyntaxNode
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

        public override void Visit(Action<SyntaxNode> nodeAction)
        {
            nodeAction(this);
        }
    }

    public class LambdaLiteral : SyntaxNode
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

        public override void Visit(Action<SyntaxNode> nodeAction)
        {
            nodeAction(this);
        }
    }

    public class TaggedSyntaxNode : SyntaxNode
    {
        private readonly SyntaxNode _syntaxNode;
        private readonly string _tagName;

        public TaggedSyntaxNode(SyntaxNode syntaxNode, string tagName)
        {
            _syntaxNode = syntaxNode;
            _tagName = tagName;
        }

        public string TagName
        {
            get { return _tagName; }
        }

        public SyntaxNode SyntaxNode
        {
            get { return _syntaxNode; }
        }

        public override string Describe()
        {
            return "(" + SyntaxNode.Describe() + "->" + TagName + ")";
        }

        public override void Visit(Action<SyntaxNode> nodeAction)
        {
            nodeAction(this);
            SyntaxNode.Visit(nodeAction);
        }
    }

    public class Alternative : SyntaxNode
    {
        private readonly IList<SyntaxNode> _terms;

        public Alternative(IList<SyntaxNode> terms)
        {
            _terms = terms;
        }

        public IList<SyntaxNode> Terms
        {
            get { return _terms; }
        }

        public override string Describe()
        {
            return "(" + string.Join("|", Terms.Select(t => t.Describe())) + ")";
        }

        public override void Visit(Action<SyntaxNode> nodeAction)
        {
            nodeAction(this);
            foreach (SyntaxNode term in Terms)
            {
                term.Visit(nodeAction);
            }
        }
    }

    public class Some : SyntaxNode
    {
        private readonly SyntaxNode _syntaxNode;

        public Some(SyntaxNode syntaxNode)
        {
            _syntaxNode = syntaxNode;
        }

        public SyntaxNode SyntaxNode
        {
            get { return _syntaxNode; }
        }

        public override string Describe()
        {
            return string.Format("({0})+", SyntaxNode.Describe());
        }

        public override void Visit(Action<SyntaxNode> nodeAction)
        {
            nodeAction(this);
            SyntaxNode.Visit(nodeAction);
        }
    }

    public class Many : SyntaxNode
    {
        private readonly SyntaxNode _syntaxNode;

        public Many(SyntaxNode syntaxNode)
        {
            _syntaxNode = syntaxNode;
        }

        public SyntaxNode SyntaxNode
        {
            get { return _syntaxNode; }
        }

        public override string Describe()
        {
            return string.Format("({0})*", SyntaxNode.Describe());
        }

        public override void Visit(Action<SyntaxNode> nodeAction)
        {
            nodeAction(this);
            SyntaxNode.Visit(nodeAction);
        }
    }

    public class Optional : SyntaxNode
    {
        private readonly SyntaxNode _syntaxNode;

        public Optional(SyntaxNode syntaxNode)
        {
            _syntaxNode = syntaxNode;
        }

        public SyntaxNode SyntaxNode
        {
            get { return _syntaxNode; }
        }

        public override string Describe()
        {
            return string.Format("({0})?", SyntaxNode.Describe());
        }

        public override void Visit(Action<SyntaxNode> nodeAction)
        {
            nodeAction(this);
            SyntaxNode.Visit(nodeAction);
        }
    }

    public class Concat : SyntaxNode
    {
        private readonly IList<SyntaxNode> _factors;

        public Concat(IList<SyntaxNode> factors)
        {
            _factors = factors;
        }

        public IList<SyntaxNode> Factors
        {
            get { return _factors; }
        }

        public override string Describe()
        {
            return string.Format("({0})", string.Join("+", Factors.Select(f => f.Describe())));
        }

        public override void Visit(Action<SyntaxNode> nodeAction)
        {
            nodeAction(this);
            foreach (SyntaxNode factor in Factors)
            {
                factor.Visit(nodeAction);
            }
        }
    }

    public class NameReference : SyntaxNode
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

        public override void Visit(Action<SyntaxNode> nodeAction)
        {
            nodeAction(this);
        }
    }

    public class StringLiteral : SyntaxNode
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

        public override void Visit(Action<SyntaxNode> nodeAction)
        {
            nodeAction(this);
        }
    }

    public class CharLiteral : SyntaxNode
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
            return string.Format("\'{0}\' /* \\x{1:x} */ ", Value, (int)Value);
        }

        public override void Visit(Action<SyntaxNode> nodeAction)
        {
            nodeAction(this);
        }
    }

    public abstract class SyntaxNode
    {
        public abstract string Describe();

        public override string ToString()
        {
            return Describe();
        }

        public abstract void Visit(Action<SyntaxNode> nodeAction);
    }

}