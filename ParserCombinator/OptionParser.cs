using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ParserCombinator
{
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
}