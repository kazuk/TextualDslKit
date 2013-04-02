using System;
using System.Collections.Generic;

namespace ParserCombinator
{
    /// <summary>
    /// 実行時に別のパーサへ処理を委譲するパーサーです
    /// </summary>
    /// <typeparam name="TInputElements"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class FowardParser<TInputElements, TOutput> : 
        Parser<TInputElements,TOutput>
    {
        public Parser<TInputElements, TOutput> FowardTo { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public bool EnableTrace { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public Action<Parser<TInputElements,TOutput>,IList<TInputElements>,int> 
            OnParse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Action<Parser<TInputElements, TOutput>,bool, IList<TInputElements>, int, int, TOutput>
            OnComplete { get; set; }

        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TOutput result)
        {
            if (EnableTrace && OnParse != null) OnParse(this, input, index);
            var b = FowardTo.Parse(input, index, out endInput, out result);
            if (EnableTrace && OnComplete != null) OnComplete(this, b, input, index, endInput, result);
            return b;
        }
    }
}