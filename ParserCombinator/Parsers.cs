﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private static void OnParse<TResult>(Parser<TInputElements, TResult> parser, IList<TInputElements> input, int index)
        {
        }

        private static void OnComplete<TResult>(Parser<TInputElements, TResult> parser, bool result, IList<TInputElements> arg3, int index, int arg5, TResult arg6)
        {
        }


        /// <summary>
        /// 指定したパーサーの結果をメモ化します
        /// </summary>
        /// <param name="memoizedParser"></param>
        /// <typeparam name="TOutput"></typeparam>
        /// <returns></returns>
        public static Parser<TInputElements, TOutput> Memoize<TOutput>(Parser<TInputElements, TOutput> memoizedParser)
        {
            Contract.Requires(memoizedParser!=null);

            Parser<TInputElements, TOutput> parser 
                = new MemoizeParser<TInputElements,TOutput>(memoizedParser);
            return EnableTrace ? WrapTracer(parser) : parser;
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
        /// 要素と区切りによる１回以上の繰り返しを受理するパーサーを構築します
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="elementParser"></param>
        /// <param name="separatorParser"></param>
        /// <returns></returns>
        public static Parser<TInputElements, IList<TOutput>> Some<TOutput>(Parser<TInputElements, TOutput> elementParser, Parser<TInputElements, Unit> separatorParser)
        {
            Contract.Requires(elementParser != null);
            Contract.Requires(separatorParser!=null);

            var separatorAndElement = Map(separatorParser, elementParser, (unit, output) => output);
            return Map(elementParser, Many(separatorAndElement), 
                (first,last) =>
                {
                    var result = new List<TOutput> {first};
                    result.AddRange(last);
                    return result as IList<TOutput>;
                });
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
        /// 
        /// </summary>
        /// <param name="elementParser"></param>
        /// <param name="separatorParser"></param>
        /// <typeparam name="TOutput"></typeparam>
        /// <returns></returns>
        public static Parser<TInputElements, IList<TOutput>> Many<TOutput>(
            Parser<TInputElements, TOutput> elementParser, Parser<TInputElements, Unit> separatorParser)
        {
            Contract.Requires(elementParser!=null);
            Contract.Requires(separatorParser!=null);

            Parser<TInputElements, IList<TOutput>> manyParser
                = new ManyWithSepParser<TInputElements, TOutput>(elementParser,separatorParser);
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

        /// <summary>
        /// 省略可能要素のパーサーを構築します
        /// </summary>
        /// <param name="optionParser"></param>
        /// <typeparam name="TOutput"></typeparam>
        /// <returns></returns>
        public static Parser<TInputElements, Option<TOutput>> Optional<TOutput>(Parser<TInputElements, TOutput> optionParser)
        {
            var optional = new OptionParser<TInputElements, TOutput>(optionParser);
            return EnableTrace ? WrapTracer(optional) : optional;
        }

        /// <summary>
        /// 文字列を受理するパーサーを作成します
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Parser<char, string> String(string text)
        {
            return new StringParser(text);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInputElements"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class ManyWithSepParser<TInputElements, TOutput> : Parser<TInputElements,IList<TOutput>>
    {
        private readonly Parser<TInputElements, TOutput> _elementParser;
        private readonly Parser<TInputElements, Unit> _sepParser;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementParser"></param>
        /// <param name="sepParser"></param>
        /// <exception cref="NotImplementedException"></exception>
        public ManyWithSepParser(Parser<TInputElements, TOutput> elementParser, Parser<TInputElements,Unit> sepParser )
        {
            Contract.Requires(elementParser != null);
            Contract.Requires(sepParser != null);

            _elementParser = elementParser;
            _sepParser = sepParser;
        }

        /// <summary>
        /// 入力を <paramref name="input"/> の <paramref name="index"/> 要素から読み取り、結果を <paramref name="result">に返します。</paramref>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="endInput"></param>
        /// <param name="result"></param>
        /// <returns>読み込みに成功した場合にはtrue、読み込みに失敗した場合にはfalse。</returns>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out IList<TOutput> result)
        {
            var results = new List<TOutput>();
            var currentIndex = index;
            TOutput elm;
            while ( _elementParser.Parse(input, currentIndex, out currentIndex,out elm))
            {
                results.Add(elm);
                Unit sep;
                if( !_sepParser.Parse(input,currentIndex,out currentIndex,out sep)) break;
            }
            endInput = currentIndex;
            result = results;
            return true;
        }
    }
}