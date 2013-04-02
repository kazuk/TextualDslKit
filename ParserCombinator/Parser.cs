using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using System.Threading.Tasks;

namespace ParserCombinator
{
    /// <summary>
    /// すべてのパーサーの基本機能を宣言します
    /// </summary>
    /// <typeparam name="TInputElements">入力ソースの文字型</typeparam>
    /// <typeparam name="TOutput">パーサーが出力するデータの型</typeparam>
    [ContractClass(typeof(ParserContract<,>))]
    public abstract class Parser<TInputElements, TOutput>
    {
        /// <summary>
        /// 入力を <paramref name="input"/> の <paramref name="index"/> 要素から読み取り、結果を <paramref name="result">に返します。</paramref>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="endInput"></param>
        /// <param name="result"></param>
        /// <returns>読み込みに成功した場合にはtrue、読み込みに失敗した場合にはfalse。</returns>
        public abstract bool Parse(IList<TInputElements> input, int index, out int endInput, out TOutput result);

        /// <summary>
        /// パーサーの名前を取得または設定します
        /// </summary>
        /// <value>
        /// パーサーの名前が設定されていない場合このプロパティは null になります。
        /// </value>
        public string Name { get; set; }
    }


    /// <summary>
    /// 現在要素が指定の範囲内であれば受け入れるパーサーです
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RangeParser<T> : Parser<T,T>
        where T : IComparable<T>
    {
        private readonly T _begin;
        private readonly T _end;

        /// <summary>
        /// 範囲を指定して範囲内であれば受け入れるパーサーを構築します。
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        public RangeParser(T begin, T end )
        {
            Contract.Requires(begin!=null);
            Contract.Requires(end!=null);
            Contract.Requires( begin.CompareTo(end)<=0 );
            _begin = begin;
            _end = end;
        }

        [ContractInvariantMethod]
        private void ObjectInvaliant()
        {
            Contract.Invariant(_begin!=null) ;
            Contract.Invariant(_end!=null);
        }

        /// <summary>
        /// 現在要素がコンストラクタパラメータで指定された範囲内であれば受け入れます。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="endInput"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool Parse(IList<T> input, int index, out int endInput, out T result)
        {
            endInput = index;
            result = default(T);
            if (index >= input.Count) return false;
            var value = input[index];
            if (value == null) return false;
            if (!value.IsInRange(_begin, _end)) return false;
            result = value;
            endInput = index + 1;
            return true;
        }
    }

    /// <summary>
    /// 指定文字列を受け取るパーサーです
    /// </summary>
    public class StringParser : Parser<char,string>
    {
        private readonly string _expected;

        /// <summary>
        /// 受け取る文字列を指定して指定文字列を受け取るパーサーを構築します
        /// </summary>
        /// <param name="expected"></param>
        public StringParser(string expected)
        {
            Contract.Requires(expected!=null);

            _expected = expected;
        }

        [ContractInvariantMethod]
        private void ObjectInvaliant()
        {
            Contract.Invariant(_expected!=null);
        }

        /// <summary>
        /// 現在位置から指定の文字列が存在すれば受け取ります
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="endInput"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool Parse(IList<char> input, int index, out int endInput, out string result)
        {
            int length = _expected.Length;
            int inputCount = input.Count;
            for (int i = 0; i < length; i++)
            {
                if (index + i >= inputCount || input[index + i] != _expected[i])
                {
                    endInput = index;
                    result = null;
                    return false;
                }
            }
            result = _expected;
            endInput = index + _expected.Length;
            return true;
        }
    }

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

    /// <summary>
    /// 省略可能要素を受け取るパーサーです
    /// </summary>
    /// <typeparam name="TInputElements"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class OptionParser<TInputElements, TOutput> : Parser<TInputElements, Option<TOutput>>
    {
        private readonly Parser<TInputElements, TOutput> _baseParser;

        /// <summary>
        /// 省略可能要素を受け取るパーサーを構築します
        /// </summary>
        /// <param name="baseParser"></param>
        public OptionParser(Parser<TInputElements, TOutput> baseParser)
        {
            Contract.Requires(baseParser!=null);

            _baseParser = baseParser;
        }

        /// <summary>
        /// 省略可能要素を受け取ります。省略可能要素がパースできなかった場合には Noneの Option値として入力を受け取ります
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="endInput"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out Option<TOutput> result)
        {
            TOutput valueResult;
            int resultInput;
            if (_baseParser.Parse(input, index, out resultInput, out valueResult))
            {
                if (valueResult != null)
                {
                    result = new Option<TOutput>(valueResult);
                }
                else
                {
                    result = Option<TOutput>.None();
                }
                endInput = resultInput;
                return true;
            }
            result = Option<TOutput>.None();
            endInput = index;
            return true;
        }
    }

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

    /// <summary>
    /// １回以上の繰り返しを受理するパーサーです
    /// </summary>
    /// <typeparam name="TInputElements"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class SomeParser<TInputElements,TOutput> : Parser<TInputElements, IList<TOutput>>
    {
        private readonly Parser<TInputElements, TOutput> _elementParser;

        /// <summary>
        /// １回以上の繰り返し要素を受理するパーサーを構築します
        /// </summary>
        /// <param name="elementParser"></param>
        public SomeParser( Parser<TInputElements, TOutput> elementParser )
        {
            Contract.Requires(elementParser!=null);

            _elementParser = elementParser;
        }

        /// <summary>
        /// 指定要素の一回以上の繰り返しがあれば受理します
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="endInput"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out IList<TOutput> result)
        {
            var results = new List<TOutput>();
            TOutput element;

            int currentIndex = index;
            int resultIndex;

            Action<TOutput,int> append = (elm,rIndex) =>
                {
                    results.Add(elm);
                    currentIndex = rIndex;
                };

            if (!_elementParser.Parse(input, currentIndex, out resultIndex, out element))
            {
                endInput = index;
                result = default(IList<TOutput>);
                return false;
            }

            append(element, resultIndex);
            while (_elementParser.Parse(input, currentIndex, out resultIndex, out element))
            {
                append(element, resultIndex);
            }
            endInput = currentIndex;
            result = results;
            return true; 
        }
    }

}
