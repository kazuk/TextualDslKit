using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ParserCombinator
{
    /// <summary>
    /// 値の一覧に現在要素が含まれていれば受け入れるパーサーです。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IsInParser<T> : Parser<T,T>
        where T : IEquatable<T>
    {
        private readonly T[] _values;

        /// <summary>
        /// パラメータで指定された値に要素が含まれていれば受け入れるパーサーを構築します。
        /// </summary>
        /// <param name="values"></param>
        public IsInParser(params T[] values)
        {
            Contract.Requires(values!=null);
            _values = values;
        }

        [ContractInvariantMethod]
        private void ObjectInvaliant()
        {
            Contract.Invariant(_values!=null);
        }

        /// <summary>
        /// 現在位置の要素がコンストラクタパラメータで指定された値配列と一致する場合に受け入れます。
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
            if (!_values.Contains(value)) return false;
            result = value;
            endInput = index + 1;
            return true;
        }
    }
}