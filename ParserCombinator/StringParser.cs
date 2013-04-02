using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ParserCombinator
{
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
}