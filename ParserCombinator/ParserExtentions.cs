using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 存在するパーサーを元にパース失敗時のレポートを行うパーサーを作成します。
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="message"></param>
        /// <param name="failerAction"></param>
        /// <typeparam name="TInputElemens"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <returns></returns>
        public static Parser<TInputElemens, TOutput>
            ReportFail<TInputElemens, TOutput>(this Parser<TInputElemens, TOutput> parser, string message,
                                               Action<int, string> failerAction)
        {
            Contract.Requires(parser!=null);
            Contract.Requires(message!=null);
            Contract.Requires(failerAction!=null);
            return new ReportFailParser<TInputElemens, TOutput>(parser, message, failerAction);
        }
    }

    /// <summary>
    /// パース失敗時のレポートを行います。
    /// </summary>
    /// <typeparam name="TInputElemens"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class ReportFailParser<TInputElemens, TOutput> : Parser<TInputElemens, TOutput>
    {
        private readonly Parser<TInputElemens, TOutput> _parser;
        private readonly string _message;
        private readonly Action<int, string> _failerAction;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="message"></param>
        /// <param name="failerAction"></param>
        public ReportFailParser(Parser<TInputElemens, TOutput> parser, string message, Action<int, string> failerAction)
        {
            _parser = parser;
            _message = message;
            _failerAction = failerAction;
        }

        /// <summary>
        /// 入力を <paramref name="input"/> の <paramref name="index"/> 要素から読み取り、結果を <paramref name="result">に返します。</paramref>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="endInput"></param>
        /// <param name="result"></param>
        /// <returns>読み込みに成功した場合にはtrue、読み込みに失敗した場合にはfalse。</returns>
        public override bool Parse(IList<TInputElemens> input, int index, out int endInput, out TOutput result)
        {
            if (!_parser.Parse(input, index, out endInput, out result))
            {
                _failerAction(index, _message);
                return false;
            }
            return true;
        }
    }
}