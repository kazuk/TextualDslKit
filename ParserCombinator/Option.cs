using System.Diagnostics.Contracts;

namespace ParserCombinator
{
    /// <summary>
    /// 値が存在するか、しないかの二通りの状態を示します
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public struct Option<TValue>
    {
        private readonly TValue _value;
        private readonly bool _hasValue;

        /// <summary>
        /// 値が存在する状態の Option 値を構築します
        /// </summary>
        /// <param name="value"></param>
        public Option(TValue value) : this( value, true )
        {
            Contract.Requires(value!=null);
        }

        private Option(TValue value,bool hasValue)
        {
            _value = value;
            _hasValue = hasValue;
        }

        /// <summary>
        /// 値が存在するかを返します
        /// </summary>
        public bool HasValue
        {
            get
            {
                return _hasValue;
            }
        }

        /// <summary>
        /// 値を返します
        /// </summary>
        public TValue Value
        {
            get
            {
                Contract.Requires(HasValue);
                return _value;
            }
        }

        /// <summary>
        /// 値が存在しない状態の Option 値を返します。
        /// </summary>
        /// <returns></returns>
        public static Option<TValue> None()
        {
            return new Option<TValue>(default(TValue), false);
        }
    }
}