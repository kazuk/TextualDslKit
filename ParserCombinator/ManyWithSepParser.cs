using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ParserCombinator
{
    /// <summary>
    /// 区切りによって分割される要素列をパースします
    /// </summary>
    /// <typeparam name="TInputElements"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class ManyWithSepParser<TInputElements, TOutput> : Parser<TInputElements,IList<TOutput>>
    {
        private readonly Parser<TInputElements, TOutput> _elementParser;
        private readonly Parser<TInputElements, Unit> _sepParser;

        /// <summary>
        /// 要素のパーサーと、区切りのパーサーから区切り記号によって分割される要素列のパーサーを構築します
        /// </summary>
        /// <param name="elementParser"></param>
        /// <param name="sepParser"></param>
        /// <exception cref="NotImplementedException"></exception>
        public ManyWithSepParser(Parser<TInputElements, TOutput> elementParser, Parser<TInputElements,Unit> sepParser )
        {
            Contract.Requires(elementParser != null);
            Contract.Requires(sepParser != null);

            _elementParser = elementParser;
            _sepParser = sepParser;
        }

        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out IList<TOutput> result)
        {
            var results = new List<TOutput>();
            var currentIndex = index;
            TOutput elm;
            while ( _elementParser.Parse(input, currentIndex, out currentIndex,out elm))
            {
                results.Add(elm);
                Unit sep;
                if( !_sepParser.Parse(input,currentIndex,out currentIndex,out sep)) break;
            }
            endInput = currentIndex;
            result = results;
            return true;
        }
    }
}