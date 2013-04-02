using System.Collections.Generic;

namespace ParserCombinator
{
    /// <summary>
    /// 入力の終端を受理するパーサーです
    /// </summary>
    /// <typeparam name="TInputElements"></typeparam>
    public class EndParser<TInputElements> : Parser<TInputElements,Unit>
    {
        /// <summary>
        /// 入力に対してindexが終端に達している場合にtrueを返します
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="endInput"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out Unit result)
        {
            endInput = index;
            result = Unit.Default();
            return input.Count <= index;
        }
    }
}