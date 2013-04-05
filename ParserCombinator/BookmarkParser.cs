using System.Collections.Generic;

namespace ParserCombinator
{
    /// <summary>
    /// 現在のパース位置をパース結果として返すパーサーを構築します。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BookmarkParser<T> : Parser<T,int>
    {
        public override bool Parse(IList<T> input, int index, out int endInput, out int result)
        {
            result = index;
            endInput = index;
            return true;
        }
    }
}