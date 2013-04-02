namespace ParserCombinator
{
    /// <summary>
    /// 値が存在しない事を示します
    /// </summary>
    public class Unit
    {
        private Unit()
        {
        }

        /// <summary>
        /// 規定値を返します
        /// </summary>
        /// <returns></returns>
        public static Unit Default()
        {
            return default(Unit);
        }
    }
}