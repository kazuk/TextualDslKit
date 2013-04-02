using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ParserCombinator
{
    /// <summary>
    /// １回以上の繰り返しを受理するパーサーです
    /// </summary>
    /// <typeparam name="TInputElements"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class SomeParser<TInputElements,TOutput> : Parser<TInputElements, IList<TOutput>>
    {
        private readonly Parser<TInputElements, TOutput> _elementParser;

        /// <summary>
        /// １回以上の繰り返し要素を受理するパーサーを構築します
        /// </summary>
        /// <param name="elementParser"></param>
        public SomeParser( Parser<TInputElements, TOutput> elementParser )
        {
            Contract.Requires(elementParser!=null);

            _elementParser = elementParser;
        }

        /// <summary>
        /// 指定要素の一回以上の繰り返しがあれば受理します
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="endInput"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out IList<TOutput> result)
        {
            var results = new List<TOutput>();
            TOutput element;

            int currentIndex = index;
            int resultIndex;

            Action<TOutput,int> append = (elm,rIndex) =>
                {
                    results.Add(elm);
                    currentIndex = rIndex;
                };

            if (!_elementParser.Parse(input, currentIndex, out resultIndex, out element))
            {
                endInput = index;
                result = default(IList<TOutput>);
                return false;
            }

            append(element, resultIndex);
            while (_elementParser.Parse(input, currentIndex, out resultIndex, out element))
            {
                append(element, resultIndex);
            }
            endInput = currentIndex;
            result = results;
            return true; 
        }
    }
}