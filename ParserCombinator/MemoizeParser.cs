using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ParserCombinator
{
    /// <summary>
    /// 指定されたパーサーのパース結果をメモ化します
    /// </summary>
    /// <typeparam name="TInputElements"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class MemoizeParser<TInputElements, TOutput> : Parser<TInputElements,TOutput>
    {

        private readonly Parser<TInputElements, TOutput> _memoizedParser;
        private readonly IDictionary<MemoKey,MemoData> _memoizeBuffer = 
            new Dictionary<MemoKey, MemoData>(); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoizedParser"></param>
        public MemoizeParser(Parser<TInputElements, TOutput> memoizedParser)
        {
            Contract.Requires(memoizedParser!=null);

            _memoizedParser = memoizedParser;
        }

        /// <summary>
        /// メモ化済みのパース結果があればそれを返します。　存在しなければパーサーを呼び出しメモ化してパース結果を返します。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="endInput"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TOutput result)
        {
            MemoData memo;
            var memoKey = new MemoKey(input, index);
            if (_memoizeBuffer.TryGetValue(memoKey, out memo))
            {
                endInput = memo.EndInput;
                result = memo.Result;
                return memo.Success;
            }
            var success = _memoizedParser.Parse(input, index,out endInput,out result);
            _memoizeBuffer.Add(memoKey, new MemoData(endInput,result,success));
            Contract.Assert(endInput>=index);
            return success;
        }

        struct MemoKey : IEquatable<MemoKey>
        {
            public override int GetHashCode()
            {
                unchecked
                {
                    return ((_input != null ? _input.GetHashCode() : 0) * 397) ^ _index;
                }
            }

            private readonly IList<TInputElements> _input;
            private readonly int _index;

            public MemoKey(IList<TInputElements> input, int index)
            {
                _input = input;
                _index = index;
            }

            public bool Equals(MemoKey other)
            {
                return Equals(_input, other._input) && _index == other._index;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is MemoKey && Equals((MemoKey)obj);
            }
        }

        struct MemoData
        {
            private readonly int _endInput;
            private readonly TOutput _result;
            private readonly bool _success;

            public MemoData(int endInput, TOutput result, bool success)
            {
                _endInput = endInput;
                _result = result;
                _success = success;
            }

            public int EndInput
            {
                get { return _endInput; }
            }

            public TOutput Result
            {
                get { return _result; }
            }

            public bool Success
            {
                get { return _success; }
            }
        }
    }
}