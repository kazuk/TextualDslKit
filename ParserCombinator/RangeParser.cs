using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ParserCombinator
{
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
}