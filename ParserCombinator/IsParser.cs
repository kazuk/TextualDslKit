using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ParserCombinator
{
    /// <summary>
    /// 現在要素が指定の値であれば受け入れるパーサーを返します。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IsParser<T> : Parser<T, Unit>
        where T : IEquatable<T>
    {
        private readonly T _value;

        /// <summary>
        /// 受け入れる値を指定し、パーサーを構築します。
        /// </summary>
        /// <param name="value"></param>
        public IsParser(T value)
        {
            Contract.Requires(value!=null);

            _value = value;
        }

        /// <summary>
        /// 現在要素がコンストラクタで指定された値であれば受け入れます。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="endInput"></param>
        /// <param name="result">要素の値は既知のため Unit です。</param>
        /// <returns></returns>
        public override bool Parse(IList<T> input, int index, out int endInput, out Unit result)
        {
            endInput = index;
            result = Unit.Default();
            var ret = index < input.Count && input[index].Equals(_value);
            if (ret) endInput = index + 1;
            return ret;
        }
    }
}