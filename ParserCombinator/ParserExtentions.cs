using System;
using System.Diagnostics.Contracts;

namespace ParserCombinator
{
    /// <summary>
    /// パーサーの追加構築をサポートします
    /// </summary>
    public static partial class ParserExtentions
    {
        /// <summary>
        /// 存在するパーサーを元に除外パターンを設定したパーサーを作成します。
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="excludePattern"></param>
        /// <typeparam name="TInputElements"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <typeparam name="TExclude"></typeparam>
        /// <returns></returns>
        public static Parser<TInputElements, TOutput> 
            Exclude< TInputElements,TOutput,TExclude>(
            this Parser<TInputElements, TOutput> parser,
            Parser<TInputElements, TExclude> excludePattern)
            where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
        {
            Contract.Requires(parser!=null);
            Contract.Requires(excludePattern!=null);
            return new ExcludeParser<TInputElements, TOutput, TExclude>(parser, excludePattern);
        }
    }
}