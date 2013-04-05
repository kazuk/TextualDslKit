using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParserBuilder;

namespace ParserCombinator.Tests
{
    [TestClass]
    public class SemTest
    {
        [TestMethod]
        public void CharRangeLiteralToSem()
        {
            var lowerAlpha = new CharRangeLiteral('a','z');
            var sem = lowerAlpha.ToSemanticNode();
            sem.Type.Is("char");
            sem.Begin.Is('a');
            sem.End.Is('z');
        }

        [TestMethod]
        public void AlternativeToSem()
        {
            var alt = new Alternative(new List<SyntaxNode>
                {
                    new CharRangeLiteral('a', 'z'),
                    new CharRangeLiteral('A', 'Z')
                } );
            var sem = alt.ToSemanticNode();
            sem.Type.Is("char");
        }

        [TestMethod]
        public void ConcatSingleMeny()
        {
            var lowerAlpha = new CharRangeLiteral('a', 'z');
            var next = new Many(
                new Alternative(new List<SyntaxNode>
                    {
                        new CharRangeLiteral('a', 'z'),
                        new CharRangeLiteral('A', 'Z')
                    } ));
            var concat = new Concat(new List<SyntaxNode> {lowerAlpha, next});
            var sem = concat.ToSemanticNode();
            sem.IsNotNull();
            sem.Nodes[0].Type.Is("char");
            sem.Nodes[1].Type.Is("IList<char>");
            sem.Type.IsNull();
        }

        [TestMethod]
        public void ConcatSingleMenyWithTag()
        {
            var lowerAlpha = new TaggedSyntaxNode( new CharRangeLiteral('a', 'z'), "first" );
            var next = new TaggedSyntaxNode( new Many(
                new Alternative(new List<SyntaxNode>
                    {
                        new CharRangeLiteral('a', 'z'),
                        new CharRangeLiteral('A', 'Z')
                    })), "next");
            var concat = new Concat(new List<SyntaxNode> { lowerAlpha, next });
            var sem = concat.ToSemanticNode();
            sem.IsNotNull();
            sem.Nodes[0].Type.Is("char");
            sem.Nodes[0].TagName.Is("first");
            sem.Nodes[1].Type.Is("IList<char>");
            sem.Nodes[1].TagName.Is("next");
            sem.Type.IsNull();
        }

        [TestMethod]
        public void NameRefTypePropergation()
        {
            var hogeDecl = new Declare("THoge", "hoge", null);
            var hageDecl = new Declare("THage", "hage", null);
            var decls = new[] {hogeDecl, hageDecl};

            SemanticProcessor.Initialize( decls.ToDictionary(decl=>decl.Name ) );
            var concat = new Concat(new List<SyntaxNode>
                {
                    new NameReference("hoge"),
                    new NameReference("hage")
                });
            var sem = concat.ToSemanticNode();
            sem.IsNotNull();
            sem.Nodes[0].Type.Is( "THoge" );
            sem.Nodes[1].Type.Is( "THage" );
        }

        [TestMethod]
        public void NameRefTypePropergationWithNotFound()
        {
            var hogeDecl = new Declare("THoge", "hoge", null);
            var hageDecl = new Declare("THage", "hage", null);
            var decls = new[] { hogeDecl, hageDecl };

            SemanticProcessor.Initialize(decls.ToDictionary(decl => decl.Name));
            var concat = new Concat(new List<SyntaxNode>
                {
                    new NameReference("moge"),
                    new NameReference("page")
                });
            var sem = concat.ToSemanticNode();
            sem.IsNotNull();
            sem.Nodes[0].Type.IsNull();
            sem.Nodes[1].Type.IsNull();
        }
    }
}
