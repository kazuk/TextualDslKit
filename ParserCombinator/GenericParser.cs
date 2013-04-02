using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ParserCombinator
{
    /// <summary>
    /// predicate による判定を行う汎用パーサーを構築します。
    /// </summary>
    /// <typeparam name="TInputElements"></typeparam>
    public class GenericParser<TInputElements> : Parser<TInputElements,TInputElements>
    {
        private readonly Func<TInputElements, bool> _predicate;

        /// <summary>
        /// 指定要素を受け入れるか判定する predicate を指定して汎用パーサーを構築します。
        /// </summary>
        /// <param name="predicate"></param>
        public GenericParser(Func<TInputElements, bool> predicate)
        {
            Contract.Requires(predicate!=null);

            _predicate = predicate;
        }

        /// <summary>
        /// 現在要素を predicate により受け入れ判定を行い、現在要素をパース結果として出力します。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="endInput"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TInputElements result)
        {
            endInput = index;
            result = default(TInputElements);
            var ret = index < input.Count && _predicate(input[index]);
            if (ret)
            {
                endInput = index + 1;
                result = input[index];
            }
            return ret;
        }
    }
}