using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserBuilder
{
    public static class SemanticProcessor
    {
        public static SemanticDeclare ToSemanticDeclare(this Declare decl)
        {
            var node = ToSemanticNode(decl.SyntaxNode);

            return new SemanticDeclare( decl.Name, decl.Type,node );
        }

        private static SemanticNode ToSemanticNode(this SyntaxNode currentNode)
        {
            string codeText = null;
            string tagName = null;
            SemanticNode node = null;
            var codeNode = currentNode as CodeAnotation;
            if (codeNode != null)
            {
                codeText = codeNode.Code;
                currentNode = codeNode.SyntaxNode;
            }
            var taggedNode = currentNode as TaggedSyntaxNode;
            if (taggedNode != null)
            {
                tagName = taggedNode.TagName;
                currentNode = taggedNode.SyntaxNode;
            }

            var altNode = currentNode as Alternative;
            if (altNode != null)
            {
                node = altNode.ToSemanticNode();
            }
            var concatNode = currentNode as Concat;
            if (concatNode != null)
            {
                node = concatNode.ToSemanticNode();
            }
            var charSetLiteral = currentNode as CharSetLiteral;
            if (charSetLiteral != null)
            {
                node = charSetLiteral.ToSemanticNode();
            }
            var charRangeLiteral = currentNode as CharRangeLiteral;
            if (charRangeLiteral != null)
            {
                node = charRangeLiteral.ToSemanticNode();
            }
            var lambda = currentNode as LambdaLiteral;
            if (lambda != null)
            {
                node = lambda.ToSemanticNode();
            }
            var some = currentNode as Some;
            if (some != null)
            {
                node = some.ToSemanticNode();
            }
            var many = currentNode as Many;
            if (many != null)
            {
                node = many.ToSemanticNode();
            }
            var opt = currentNode as Optional;
            if (opt != null)
            {
                node = opt.ToSemanticNode();
            }
            var nameRef = currentNode as NameReference;
            if (nameRef != null)
            {
                node = nameRef.ToSemanticNode();
            }
            if (node == null)
            {
                throw new ApplicationException("意味ノードが生成されませんでした。");
            }
            node.CodeText = codeText;
            node.TagName = tagName;
            return node;
        }

        private static IDictionary<string, Declare> _declares;

        public static void Initialize(IDictionary<string, Declare> declares)
        {
            _declares = declares;
        }

        public static SemanticNameRef ToSemanticNode(this NameReference nameRef)
        {
            Declare decl;
            if (!_declares.TryGetValue(nameRef.Name, out decl))
            {
                Warning(nameRef.Name + " で定義される文法がありません。", nameRef);
                return new SemanticNameRef(string.Format("SyntaxNotFound<{0}>", nameRef.Name));
            }
            return new SemanticNameRef(decl.Name) { Type = decl.Type};
        }

        public static SemanticAlternative ToSemanticNode(this Alternative alt)
        {
            var semTerms = alt.Terms.Select(ToSemanticNode).ToArray();
            int unnamedParamIndex = 0;
            foreach (var node in semTerms.Where(node => node.TagName == null))
            {
                node.TagName = string.Format("p{0}", unnamedParamIndex);
                unnamedParamIndex++;
            }
            var clrType = semTerms.First().Type;
            if (semTerms.Any(t => t.Type != clrType))
            {
                Warning( "選択肢の出力型がマッチしません",alt );
                foreach (var t in semTerms)
                {
                    Info(t + " results " + t.Type);
                }
            }
            return new SemanticAlternative(semTerms) { Type = clrType };
        }

        private static void Info(string message)
        {
            throw new NotImplementedException();
        }

        public static SemanticConcat ToSemanticNode(this Concat concat)
        {
            List<SemanticNode> semanticNodes =
                concat.Factors.Select(node => node.ToSemanticNode()).ToList();
            var tagNames = semanticNodes.Where(n => n.TagName != null).Select(n => n.TagName).ToArray();
            if (tagNames.Distinct().Count() != tagNames.Length)
            {
                Warning("タグ名が重複しています。",concat);
                foreach (var semanticNode in semanticNodes)
                {
                    if (semanticNode.TagName != null)
                    {
                        Info( semanticNode + " tag:"+ semanticNode.TagName);
                    }
                    else
                    {
                        Info( semanticNode + " tag: <none> ");
                    }
                }
            }
            int tagIndex = 0;
            foreach (var semanticNode in semanticNodes.Where(semanticNode => semanticNode.TagName == null))
            {
                semanticNode.TagName = string.Format("p{0}", tagIndex);
                tagIndex++;
            }
            return new SemanticConcat(semanticNodes);
        }

        public static SemanticSome ToSemanticNode(this Some some)
        {
            return new SemanticSome( some.SyntaxNode.ToSemanticNode() );
        }

        public static SemanticMeny ToSemanticNode(this Many many)
        {
            return new SemanticMeny(many.SyntaxNode.ToSemanticNode());
        }

        public static SemanticOptional ToSemanticNode(this Optional optional)
        {
            return new SemanticOptional( optional.SyntaxNode.ToSemanticNode() );
        }

        public static SemanticLambda ToSemanticNode(this LambdaLiteral lambda)
        {
            return new SemanticLambda( lambda.LambdaText );
        }

        public static SemanticCharSet ToSemanticNode(this CharSetLiteral set)
        {
            var chars = set.Chars.OrderBy(ch => ch).Distinct().ToArray();
            if (set.Chars.Count != chars.Length)
            {
                Warning("キャラクタ集合リテラルに同一の文字が複数回あります。",set);
            }
            return new SemanticCharSet(chars);
        }

        public static SemanticCharRange ToSemanticNode(this CharRangeLiteral range)
        {
            if (range.Begin >= range.End)
            {
                Warning("キャラクタ範囲リテラルの開始文字と終了文字の関係が不正です。",range);
            }
            return new SemanticCharRange( range.Begin, range.End ) {Type = "char"};
        }

        private static void Warning(string message, SyntaxNode set)
        {
        }
    }

    public class SemanticNameRef : SemanticNode
    {
        private readonly string _name;

        public SemanticNameRef(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }
    }

    public class SemanticOptional : SemanticNode
    {
        private readonly SemanticNode _child;

        public SemanticOptional(SemanticNode child)
        {
            _child = child;
            Type = string.Format("Option<{0}>", child.Type);
        }

        public SemanticNode Child
        {
            get { return _child; }
        }
    }

    public class SemanticMeny : SemanticNode
    {
        private readonly SemanticNode _child;

        public SemanticMeny(SemanticNode child)
        {
            _child = child;
            Type = string.Format("IList<{0}>", child.Type);
        }

        public SemanticNode Child
        {
            get { return _child; }
        }
    }

    public class SemanticSome : SemanticNode
    {
        private readonly SemanticNode _child;

        public SemanticSome(SemanticNode child)
        {
            _child = child;
            Type = string.Format("IList<{0}>", child.Type);
        }

        public SemanticNode Child
        {
            get { return _child; }
        }
    }

    public class SemanticLambda : SemanticNode
    {
        private readonly string _lambdaText;

        public SemanticLambda(string lambdaText)
        {
            _lambdaText = lambdaText;
        }
    }

    public class SemanticCharRange : SemanticNode
    {
        private readonly char _begin;
        private readonly char _end;

        public SemanticCharRange(char begin, char end)
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
    }

    public class SemanticCharSet : SemanticNode
    {
        private readonly char[] _chars;

        public SemanticCharSet(char[] chars)
        {
            _chars = chars;
        }
    }

    public class SemanticConcat : SemanticNode
    {
        private readonly SemanticNode[] _nodes;

        public SemanticConcat(IEnumerable<SemanticNode> semanticNodes)
        {
            _nodes = semanticNodes.ToArray();
        }

        public SemanticNode[] Nodes
        {
            get { return _nodes; }
        }
    }

    public class SemanticAlternative : SemanticNode
    {
        private readonly SemanticNode[] _semTerms;

        public SemanticAlternative(SemanticNode[] semTerms)
        {
            _semTerms = semTerms;
        }

        public SemanticNode[] Terms
        {
            get { return _semTerms; }
        }
    }

    public class SemanticNode
    {
        public string CodeText { get; set; }

        public string TagName { get; set; }

        public string Type { get; set; }
    }

    public class SemanticDeclare
    {
        private readonly string _name;
        private readonly string _type;
        private readonly SemanticNode _node;

        public SemanticDeclare(string name, string type, SemanticNode node)
        {
            _name = name;
            _type = type;
            _node = node;
        }
    }
}
