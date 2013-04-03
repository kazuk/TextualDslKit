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
        /// <summary>
        /// パース要求の転送先となるパーサーを取得または設定します。
        /// </summary>
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

        /// <summary>
        /// リダイレクト先パーサーを呼び出し結果を返します
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="endInput"></param>
        /// <param name="result"></param>
        /// <returns>読み込みに成功した場合にはtrue、読み込みに失敗した場合にはfalse。</returns>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TOutput result)
        {
            if (EnableTrace && OnParse != null) OnParse(this, input, index);
            var b = FowardTo.Parse(input, index, out endInput, out result);
            if (EnableTrace && OnComplete != null) OnComplete(this, b, input, index, endInput, result);
            return b;
        }
    }
}