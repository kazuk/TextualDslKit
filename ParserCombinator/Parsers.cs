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
            return new ExcludeParser<TInputElements, TOutput, TExclude>(parser, excludePattern);
        }
    }

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

        private static void OnParse<TResult>(Parser<TInputElements, TResult> parser, IList<TInputElements> input, int index)
        {
        }

        private static void OnComplete<TResult>(Parser<TInputElements, TResult> parser, bool result, IList<TInputElements> arg3, int arg4, int arg5, TResult arg6)
        {

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

        /// <summary>
        /// パース操作の０回以上の繰り返しを行うパーサーを構築します。
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="elementParser"></param>
        /// <returns></returns>
        public static Parser<TInputElements, IList<TOutput>> Many<TOutput>(Parser<TInputElements, TOutput> elementParser)
        {
            Contract.Requires(elementParser!=null);
            Parser<TInputElements,IList<TOutput>> manyParser
                = new ManyParser<TInputElements, TOutput>(elementParser);
            return EnableTrace
                       ? WrapTracer(manyParser)
                       : manyParser;
        }


        /// <summary>
        /// 指定要素を受け取るパーサーを構築して返します
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Parser<TInputElements, Unit> Is(TInputElements value)
        {
// ReSharper disable CompareNonConstrainedGenericWithNull
            Contract.Requires(value!=null);
// ReSharper restore CompareNonConstrainedGenericWithNull

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
// ReSharper disable CompareNonConstrainedGenericWithNull
            Contract.Requires(begin!=null);
            Contract.Requires(end!=null);
// ReSharper restore CompareNonConstrainedGenericWithNull
            Contract.Requires(begin.CompareTo(end) <= 0);

            var rangeParser = new RangeParser<TInputElements>(begin, end);
            return EnableTrace ? WrapTracer(rangeParser) : rangeParser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <returns></returns>
        public static FowardParser<TInputElements, TOutput> Foward<TOutput>()
        {
            return new FowardParser<TInputElements, TOutput>
                {
                    EnableTrace = EnableTrace, 
                    OnParse = OnParse,
                    OnComplete = OnComplete
                };
        }
    }
}