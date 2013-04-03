using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ParserCombinator
{
    /// <summary>
    /// 既存パーサーに除外パターンを付与したパーサーです
    /// </summary>
    /// <typeparam name="TInputElements"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <typeparam name="TExclude"></typeparam>
    public class ExcludeParser<TInputElements, TOutput, TExclude> : Parser<TInputElements, TOutput>
        where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
    {
        private readonly Parser<TInputElements, TOutput> _parser;
        private readonly Parser<TInputElements, TExclude> _excludePattern;

        /// <summary>
        /// 除外パターンを持つパーサーを構築します
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="excludePattern"></param>
        public ExcludeParser(
            Parser<TInputElements, TOutput> parser, 
            Parser<TInputElements, TExclude> excludePattern)
        {
            Contract.Requires(parser!=null);
            Contract.Requires(excludePattern!=null);

            _parser = parser;
            _excludePattern = excludePattern;
        }

        /// <summary>
        /// 除外パターンが受理された場合、失敗を返します。受理されなかった場合には元になったパーサーでパースします。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="endInput"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TOutput result)
        {
            int temp;
            TExclude exclude;
            if (_excludePattern.Parse(input, index, out temp, out exclude))
            {
                endInput = index;
                result = default(TOutput);
                return false;
            }
            return _parser.Parse(input, index, out endInput, out result);
        }
    }
}