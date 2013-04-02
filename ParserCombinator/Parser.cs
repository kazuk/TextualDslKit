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
}
