using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ParserCombinator
{
    /// <summary>
    /// 各種パーサーを構築します。
    /// </summary>
    /// <typeparam name="TInputElements"></typeparam>
    public static partial class Parsers<TInputElements>
        where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
    {
        /// <summary>
        /// トレースの有効、無効を取得または設定します。
        /// </summary>
        public static bool EnableTrace { get; set; }

        private static Parser<TInputElements, TOutput> WrapTracer<TOutput>(Parser<TInputElements, TOutput> parser)
        {
            Contract.Requires(parser!=null);

            return new TraceParser<TInputElements, TOutput>(parser, OnParse, OnComplete);
        }

        /// <summary>
        /// predicate によってパースの可否を判定する汎用パーサーを構築します。
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static Parser<TInputElements,TInputElements> Generic(Func<TInputElements, bool> predicate)
        {
            Contract.Requires(predicate!=null);

            Parser<TInputElements, TInputElements> result 
                = new GenericParser<TInputElements>(predicate);
            return EnableTrace
                       ? WrapTracer(result)
                       : result;
        }

        /// <summary>
        /// パース操作の一回以上の繰り返しを行うパーサーを構築します。
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="elementParser"></param>
        /// <returns></returns>
        public static Parser<TInputElements, IList<TOutput>> Some<TOutput>(Parser<TInputElements, TOutput> elementParser)
        {
            Contract.Requires(elementParser!=null);

            Parser<TInputElements, IList<TOutput>> someParser 
                = new SomeParser<TInputElements, TOutput>(elementParser);
            return EnableTrace
                       ? WrapTracer(someParser)
                       : someParser; 
        }

        private static void OnParse<TResult>(Parser<TInputElements, TResult> parser, IList<TInputElements> input, int index)
        {
        }

        private static void OnComplete<TResult>(Parser<TInputElements, TResult> parser, bool result, IList<TInputElements> arg3, int arg4, int arg5, TResult arg6)
        {

        }

        /// <summary>
        /// 指定要素を受け取るパーサーを構築して返します
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Parser<TInputElements, Unit> Is(TInputElements value)
        {
            Contract.Requires(value!=null);

            var isParser = new IsParser<TInputElements>(value);
            return EnableTrace ? WrapTracer(isParser) : isParser;
        }

        /// <summary>
        /// 指定要素の一覧に存在すれば受け取るパーサーを構築して返します。
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Parser<TInputElements, TInputElements> IsIn(params TInputElements[] values)
        {
            Contract.Requires(values!=null);

            var isinParser = new IsInParser<TInputElements>(values);
            return EnableTrace ? WrapTracer(isinParser) : isinParser;
        }

        /// <summary>
        /// 開始から終了までの範囲を受け取るパーサーを構築して返します。
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Parser<TInputElements, TInputElements> Range(TInputElements begin, TInputElements end)
        {
            Contract.Requires(begin!=null);
            Contract.Requires(end!=null);
            Contract.Requires(begin.CompareTo(end)<=0);

            var rangeParser = new RangeParser<TInputElements>(begin, end);
            return EnableTrace ? WrapTracer(rangeParser) : rangeParser;
        }
    }
}