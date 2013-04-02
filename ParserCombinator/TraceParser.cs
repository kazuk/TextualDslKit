using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ParserCombinator
{
    /// <summary>
    /// パースの進行状況をトレースするパーサーに対するアダプターです。
    /// </summary>
    /// <typeparam name="TInputElements"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class TraceParser<TInputElements, TOutput> : Parser<TInputElements, TOutput>
    {
        private readonly Parser<TInputElements, TOutput> _baseParser;
        private readonly Action<Parser<TInputElements, TOutput>,IList<TInputElements>, int> _onParse;
        private readonly Action<Parser<TInputElements, TOutput>,bool, IList<TInputElements>, int, int, TOutput> _onComplete;

        /// <summary>
        /// 既存のパーサーをラップし、パース前、パース後にそれぞれ Action を実行するパーサーを構築します。
        /// </summary>
        /// <param name="baseParser"></param>
        /// <param name="onParse"></param>
        /// <param name="onComplete"></param>
        public TraceParser( 
            Parser<TInputElements, TOutput> baseParser,
            Action<Parser<TInputElements, TOutput>,IList<TInputElements>,int> onParse,
            Action<Parser<TInputElements, TOutput>,bool,IList<TInputElements>,int,int,TOutput> onComplete )
        {
            Contract.Requires(baseParser!=null);
            Contract.Requires(onParse!=null);
            Contract.Requires(onComplete!=null);

            _baseParser = baseParser;
            _onParse = onParse;
            _onComplete = onComplete;
        }

        /// <summary>
        /// コンストラクタで指定されたパーサーのパース動作の前後に、コンストラクタで指定された Action を実行します。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="endInput"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TOutput result)
        {
            _onParse(_baseParser, input, index);
            bool b = _baseParser.Parse(input, index, out endInput, out result);
            _onComplete(_baseParser, b, input, index, endInput, result);
            return b;
        }
    }
}