using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ParserCombinator
{
    /// <summary>
    /// ０個以上の要素の繰り返しを受理するパーサーです
    /// </summary>
    /// <typeparam name="TInputElements"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class ManyParser<TInputElements, TOutput> : Parser<TInputElements, IList<TOutput>>
    {
        private readonly Parser<TInputElements, TOutput> _elementParser;

        /// <summary>
        /// 要素のパーサーを元に繰り返し要素のパーサーを構築します
        /// </summary>
        /// <param name="elementParser"></param>
        public ManyParser(Parser<TInputElements, TOutput> elementParser)
        {
            Contract.Requires(elementParser!=null);

            _elementParser = elementParser;
        }
        /// <summary>
        /// 繰り返し要素を受け取ります。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="endInput"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <remarks>繰り返し要素が存在しなかった場合には０回として受理されます。</remarks>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out IList<TOutput> result)
        {
            var results = new List<TOutput>();
            TOutput element;

            int currentIndex = index;
            int resultIndex;
            while (_elementParser.Parse(input, currentIndex, out resultIndex, out element))
            {
                results.Add(element);
                currentIndex = resultIndex;
            }
            endInput = currentIndex;
            result = results;
            return true;
        }
    }
}