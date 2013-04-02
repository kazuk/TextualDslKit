using System;
using System.Diagnostics.Contracts;

namespace ParserCombinator
{
    /// <summary>
    /// ICompareable の操作を提供します
    /// </summary>
    internal static class CompareableExtention
    {
        /// <summary>
        /// <see cref="System.IComparable{T}"/> によって <paramref name="value"/> を <paramref name="begin"/>,<paramref name="end"/> と比較し範囲内であるかを返します。
        /// </summary>
        /// <typeparam name="T"><paramref name="value"/>,<paramref name="begin"/>,<paramref name="end"/> の型</typeparam>
        /// <param name="value">begin end 間であることを評価する値</param>
        /// <param name="begin">範囲の開始値</param>
        /// <param name="end">範囲の終了値</param>
        /// <returns><paramref name="value"/>がbeginからendの範囲内の場合には ture、範囲外の場合はfalse。範囲の開始値および終了値に一致する値は範囲内される。</returns>
        /// <remarks>
        /// </remarks>
        /// <example>
        /// </example>
        public static bool IsInRange<T>(this T value, T begin, T end) 
            where T : IComparable<T>
        {
            Contract.Requires( value!=null );
            Contract.Requires( begin!=null );
            Contract.Requires( end!=null );
            return begin.CompareTo(value) <= 0 && end.CompareTo(value) >= 0;
        }

        /// <summary>
        /// <see cref="IsInRange{T}"/> の開始値および終了値を範囲外とするバージョンです。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static bool IsInRangeNotEqual<T>(this T value, T begin, T end) where T : IComparable<T>
        {
            return begin.CompareTo(value) < 0 && end.CompareTo(value) > 0;
        }
    }
}