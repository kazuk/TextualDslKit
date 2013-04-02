using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ParserCombinator
{

	///<summary>
	/// 複数のパーサーから一つの値を選択するパーサーです。
	///</summary>
    public class OrParser<TInputElements,T0,T1,TResult> : Parser<TInputElements,TResult>
    {
        private readonly Parser<TInputElements, T0> _p0;
        private readonly Func<T0, TResult> _apply0;
        private readonly Parser<TInputElements, T1> _p1;
        private readonly Func<T1, TResult> _apply1;

		///<summary>
		/// 選択要素を持つパーサーを構築します。
		///</summary>
        public OrParser( 
			Parser<TInputElements,T0> p0, Func<T0,TResult> apply0 ,
			Parser<TInputElements,T1> p1, Func<T1,TResult> apply1 
		)
        {
			Contract.Requires( p0 !=null );
			Contract.Requires( apply0 !=null );
			Contract.Requires( p1 !=null );
			Contract.Requires( apply1 !=null );

            _p0 = p0;
            _apply0 = apply0;
            _p1 = p1;
            _apply1 = apply1;
        }

		///<summary>
		/// コンストラクタで渡された各パーサーでパースを試行し、最初にパースを成功したパーサーの結果を適用関数を通して返します。
		///</summary>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TResult result)
        {
            T0 v0;
            if (_p0.Parse(input, index, out endInput, out v0))
            {
                result = _apply0(v0);
                return true;
            }
            T1 v1;
            if (_p1.Parse(input, index, out endInput, out v1))
            {
                result = _apply1(v1);
                return true;
            }
            result = default(TResult);
            return false;
        }
    }

	public static partial class Parsers<TInputElements>
		where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
    {
		///<summary>
		/// 各パーサーでパースを試行し、最初にパースに成功したパース結果を apply 操作を通して TOutput に変換します。
		///</summary>
        public static Parser<TInputElements, TOutput> Or<T0,T1, TOutput>(
			Parser<TInputElements, T0> p0, Func<T0, TOutput> apply0 , 
			Parser<TInputElements, T1> p1, Func<T1, TOutput> apply1  
		)
        {
			Contract.Requires( p0!=null );
			Contract.Requires( apply0!=null );
			Contract.Requires( p1!=null );
			Contract.Requires( apply1!=null );

            Parser<TInputElements, TOutput> result = new OrParser<TInputElements, T0,T1, TOutput>(
                p0, apply0 ,
                p1, apply1 
                );
            return EnableTrace ? WrapTracer(result) : result;
        }

	}


	///<summary>
	/// 複数のパーサーから一つの値を選択するパーサーです。
	///</summary>
    public class OrParser<TInputElements,T0,T1,T2,TResult> : Parser<TInputElements,TResult>
    {
        private readonly Parser<TInputElements, T0> _p0;
        private readonly Func<T0, TResult> _apply0;
        private readonly Parser<TInputElements, T1> _p1;
        private readonly Func<T1, TResult> _apply1;
        private readonly Parser<TInputElements, T2> _p2;
        private readonly Func<T2, TResult> _apply2;

		///<summary>
		/// 選択要素を持つパーサーを構築します。
		///</summary>
        public OrParser( 
			Parser<TInputElements,T0> p0, Func<T0,TResult> apply0 ,
			Parser<TInputElements,T1> p1, Func<T1,TResult> apply1 ,
			Parser<TInputElements,T2> p2, Func<T2,TResult> apply2 
		)
        {
			Contract.Requires( p0 !=null );
			Contract.Requires( apply0 !=null );
			Contract.Requires( p1 !=null );
			Contract.Requires( apply1 !=null );
			Contract.Requires( p2 !=null );
			Contract.Requires( apply2 !=null );

            _p0 = p0;
            _apply0 = apply0;
            _p1 = p1;
            _apply1 = apply1;
            _p2 = p2;
            _apply2 = apply2;
        }

		///<summary>
		/// コンストラクタで渡された各パーサーでパースを試行し、最初にパースを成功したパーサーの結果を適用関数を通して返します。
		///</summary>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TResult result)
        {
            T0 v0;
            if (_p0.Parse(input, index, out endInput, out v0))
            {
                result = _apply0(v0);
                return true;
            }
            T1 v1;
            if (_p1.Parse(input, index, out endInput, out v1))
            {
                result = _apply1(v1);
                return true;
            }
            T2 v2;
            if (_p2.Parse(input, index, out endInput, out v2))
            {
                result = _apply2(v2);
                return true;
            }
            result = default(TResult);
            return false;
        }
    }

	public static partial class Parsers<TInputElements>
		where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
    {
		///<summary>
		/// 各パーサーでパースを試行し、最初にパースに成功したパース結果を apply 操作を通して TOutput に変換します。
		///</summary>
        public static Parser<TInputElements, TOutput> Or<T0,T1,T2, TOutput>(
			Parser<TInputElements, T0> p0, Func<T0, TOutput> apply0 , 
			Parser<TInputElements, T1> p1, Func<T1, TOutput> apply1 , 
			Parser<TInputElements, T2> p2, Func<T2, TOutput> apply2  
		)
        {
			Contract.Requires( p0!=null );
			Contract.Requires( apply0!=null );
			Contract.Requires( p1!=null );
			Contract.Requires( apply1!=null );
			Contract.Requires( p2!=null );
			Contract.Requires( apply2!=null );

            Parser<TInputElements, TOutput> result = new OrParser<TInputElements, T0,T1,T2, TOutput>(
                p0, apply0 ,
                p1, apply1 ,
                p2, apply2 
                );
            return EnableTrace ? WrapTracer(result) : result;
        }

	}


	///<summary>
	/// 複数のパーサーから一つの値を選択するパーサーです。
	///</summary>
    public class OrParser<TInputElements,T0,T1,T2,T3,TResult> : Parser<TInputElements,TResult>
    {
        private readonly Parser<TInputElements, T0> _p0;
        private readonly Func<T0, TResult> _apply0;
        private readonly Parser<TInputElements, T1> _p1;
        private readonly Func<T1, TResult> _apply1;
        private readonly Parser<TInputElements, T2> _p2;
        private readonly Func<T2, TResult> _apply2;
        private readonly Parser<TInputElements, T3> _p3;
        private readonly Func<T3, TResult> _apply3;

		///<summary>
		/// 選択要素を持つパーサーを構築します。
		///</summary>
        public OrParser( 
			Parser<TInputElements,T0> p0, Func<T0,TResult> apply0 ,
			Parser<TInputElements,T1> p1, Func<T1,TResult> apply1 ,
			Parser<TInputElements,T2> p2, Func<T2,TResult> apply2 ,
			Parser<TInputElements,T3> p3, Func<T3,TResult> apply3 
		)
        {
			Contract.Requires( p0 !=null );
			Contract.Requires( apply0 !=null );
			Contract.Requires( p1 !=null );
			Contract.Requires( apply1 !=null );
			Contract.Requires( p2 !=null );
			Contract.Requires( apply2 !=null );
			Contract.Requires( p3 !=null );
			Contract.Requires( apply3 !=null );

            _p0 = p0;
            _apply0 = apply0;
            _p1 = p1;
            _apply1 = apply1;
            _p2 = p2;
            _apply2 = apply2;
            _p3 = p3;
            _apply3 = apply3;
        }

		///<summary>
		/// コンストラクタで渡された各パーサーでパースを試行し、最初にパースを成功したパーサーの結果を適用関数を通して返します。
		///</summary>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TResult result)
        {
            T0 v0;
            if (_p0.Parse(input, index, out endInput, out v0))
            {
                result = _apply0(v0);
                return true;
            }
            T1 v1;
            if (_p1.Parse(input, index, out endInput, out v1))
            {
                result = _apply1(v1);
                return true;
            }
            T2 v2;
            if (_p2.Parse(input, index, out endInput, out v2))
            {
                result = _apply2(v2);
                return true;
            }
            T3 v3;
            if (_p3.Parse(input, index, out endInput, out v3))
            {
                result = _apply3(v3);
                return true;
            }
            result = default(TResult);
            return false;
        }
    }

	public static partial class Parsers<TInputElements>
		where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
    {
		///<summary>
		/// 各パーサーでパースを試行し、最初にパースに成功したパース結果を apply 操作を通して TOutput に変換します。
		///</summary>
        public static Parser<TInputElements, TOutput> Or<T0,T1,T2,T3, TOutput>(
			Parser<TInputElements, T0> p0, Func<T0, TOutput> apply0 , 
			Parser<TInputElements, T1> p1, Func<T1, TOutput> apply1 , 
			Parser<TInputElements, T2> p2, Func<T2, TOutput> apply2 , 
			Parser<TInputElements, T3> p3, Func<T3, TOutput> apply3  
		)
        {
			Contract.Requires( p0!=null );
			Contract.Requires( apply0!=null );
			Contract.Requires( p1!=null );
			Contract.Requires( apply1!=null );
			Contract.Requires( p2!=null );
			Contract.Requires( apply2!=null );
			Contract.Requires( p3!=null );
			Contract.Requires( apply3!=null );

            Parser<TInputElements, TOutput> result = new OrParser<TInputElements, T0,T1,T2,T3, TOutput>(
                p0, apply0 ,
                p1, apply1 ,
                p2, apply2 ,
                p3, apply3 
                );
            return EnableTrace ? WrapTracer(result) : result;
        }

	}


	///<summary>
	/// 複数のパーサーから一つの値を選択するパーサーです。
	///</summary>
    public class OrParser<TInputElements,T0,T1,T2,T3,T4,TResult> : Parser<TInputElements,TResult>
    {
        private readonly Parser<TInputElements, T0> _p0;
        private readonly Func<T0, TResult> _apply0;
        private readonly Parser<TInputElements, T1> _p1;
        private readonly Func<T1, TResult> _apply1;
        private readonly Parser<TInputElements, T2> _p2;
        private readonly Func<T2, TResult> _apply2;
        private readonly Parser<TInputElements, T3> _p3;
        private readonly Func<T3, TResult> _apply3;
        private readonly Parser<TInputElements, T4> _p4;
        private readonly Func<T4, TResult> _apply4;

		///<summary>
		/// 選択要素を持つパーサーを構築します。
		///</summary>
        public OrParser( 
			Parser<TInputElements,T0> p0, Func<T0,TResult> apply0 ,
			Parser<TInputElements,T1> p1, Func<T1,TResult> apply1 ,
			Parser<TInputElements,T2> p2, Func<T2,TResult> apply2 ,
			Parser<TInputElements,T3> p3, Func<T3,TResult> apply3 ,
			Parser<TInputElements,T4> p4, Func<T4,TResult> apply4 
		)
        {
			Contract.Requires( p0 !=null );
			Contract.Requires( apply0 !=null );
			Contract.Requires( p1 !=null );
			Contract.Requires( apply1 !=null );
			Contract.Requires( p2 !=null );
			Contract.Requires( apply2 !=null );
			Contract.Requires( p3 !=null );
			Contract.Requires( apply3 !=null );
			Contract.Requires( p4 !=null );
			Contract.Requires( apply4 !=null );

            _p0 = p0;
            _apply0 = apply0;
            _p1 = p1;
            _apply1 = apply1;
            _p2 = p2;
            _apply2 = apply2;
            _p3 = p3;
            _apply3 = apply3;
            _p4 = p4;
            _apply4 = apply4;
        }

		///<summary>
		/// コンストラクタで渡された各パーサーでパースを試行し、最初にパースを成功したパーサーの結果を適用関数を通して返します。
		///</summary>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TResult result)
        {
            T0 v0;
            if (_p0.Parse(input, index, out endInput, out v0))
            {
                result = _apply0(v0);
                return true;
            }
            T1 v1;
            if (_p1.Parse(input, index, out endInput, out v1))
            {
                result = _apply1(v1);
                return true;
            }
            T2 v2;
            if (_p2.Parse(input, index, out endInput, out v2))
            {
                result = _apply2(v2);
                return true;
            }
            T3 v3;
            if (_p3.Parse(input, index, out endInput, out v3))
            {
                result = _apply3(v3);
                return true;
            }
            T4 v4;
            if (_p4.Parse(input, index, out endInput, out v4))
            {
                result = _apply4(v4);
                return true;
            }
            result = default(TResult);
            return false;
        }
    }

	public static partial class Parsers<TInputElements>
		where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
    {
		///<summary>
		/// 各パーサーでパースを試行し、最初にパースに成功したパース結果を apply 操作を通して TOutput に変換します。
		///</summary>
        public static Parser<TInputElements, TOutput> Or<T0,T1,T2,T3,T4, TOutput>(
			Parser<TInputElements, T0> p0, Func<T0, TOutput> apply0 , 
			Parser<TInputElements, T1> p1, Func<T1, TOutput> apply1 , 
			Parser<TInputElements, T2> p2, Func<T2, TOutput> apply2 , 
			Parser<TInputElements, T3> p3, Func<T3, TOutput> apply3 , 
			Parser<TInputElements, T4> p4, Func<T4, TOutput> apply4  
		)
        {
			Contract.Requires( p0!=null );
			Contract.Requires( apply0!=null );
			Contract.Requires( p1!=null );
			Contract.Requires( apply1!=null );
			Contract.Requires( p2!=null );
			Contract.Requires( apply2!=null );
			Contract.Requires( p3!=null );
			Contract.Requires( apply3!=null );
			Contract.Requires( p4!=null );
			Contract.Requires( apply4!=null );

            Parser<TInputElements, TOutput> result = new OrParser<TInputElements, T0,T1,T2,T3,T4, TOutput>(
                p0, apply0 ,
                p1, apply1 ,
                p2, apply2 ,
                p3, apply3 ,
                p4, apply4 
                );
            return EnableTrace ? WrapTracer(result) : result;
        }

	}


	///<summary>
	/// 複数のパーサーから一つの値を選択するパーサーです。
	///</summary>
    public class OrParser<TInputElements,T0,T1,T2,T3,T4,T5,TResult> : Parser<TInputElements,TResult>
    {
        private readonly Parser<TInputElements, T0> _p0;
        private readonly Func<T0, TResult> _apply0;
        private readonly Parser<TInputElements, T1> _p1;
        private readonly Func<T1, TResult> _apply1;
        private readonly Parser<TInputElements, T2> _p2;
        private readonly Func<T2, TResult> _apply2;
        private readonly Parser<TInputElements, T3> _p3;
        private readonly Func<T3, TResult> _apply3;
        private readonly Parser<TInputElements, T4> _p4;
        private readonly Func<T4, TResult> _apply4;
        private readonly Parser<TInputElements, T5> _p5;
        private readonly Func<T5, TResult> _apply5;

		///<summary>
		/// 選択要素を持つパーサーを構築します。
		///</summary>
        public OrParser( 
			Parser<TInputElements,T0> p0, Func<T0,TResult> apply0 ,
			Parser<TInputElements,T1> p1, Func<T1,TResult> apply1 ,
			Parser<TInputElements,T2> p2, Func<T2,TResult> apply2 ,
			Parser<TInputElements,T3> p3, Func<T3,TResult> apply3 ,
			Parser<TInputElements,T4> p4, Func<T4,TResult> apply4 ,
			Parser<TInputElements,T5> p5, Func<T5,TResult> apply5 
		)
        {
			Contract.Requires( p0 !=null );
			Contract.Requires( apply0 !=null );
			Contract.Requires( p1 !=null );
			Contract.Requires( apply1 !=null );
			Contract.Requires( p2 !=null );
			Contract.Requires( apply2 !=null );
			Contract.Requires( p3 !=null );
			Contract.Requires( apply3 !=null );
			Contract.Requires( p4 !=null );
			Contract.Requires( apply4 !=null );
			Contract.Requires( p5 !=null );
			Contract.Requires( apply5 !=null );

            _p0 = p0;
            _apply0 = apply0;
            _p1 = p1;
            _apply1 = apply1;
            _p2 = p2;
            _apply2 = apply2;
            _p3 = p3;
            _apply3 = apply3;
            _p4 = p4;
            _apply4 = apply4;
            _p5 = p5;
            _apply5 = apply5;
        }

		///<summary>
		/// コンストラクタで渡された各パーサーでパースを試行し、最初にパースを成功したパーサーの結果を適用関数を通して返します。
		///</summary>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TResult result)
        {
            T0 v0;
            if (_p0.Parse(input, index, out endInput, out v0))
            {
                result = _apply0(v0);
                return true;
            }
            T1 v1;
            if (_p1.Parse(input, index, out endInput, out v1))
            {
                result = _apply1(v1);
                return true;
            }
            T2 v2;
            if (_p2.Parse(input, index, out endInput, out v2))
            {
                result = _apply2(v2);
                return true;
            }
            T3 v3;
            if (_p3.Parse(input, index, out endInput, out v3))
            {
                result = _apply3(v3);
                return true;
            }
            T4 v4;
            if (_p4.Parse(input, index, out endInput, out v4))
            {
                result = _apply4(v4);
                return true;
            }
            T5 v5;
            if (_p5.Parse(input, index, out endInput, out v5))
            {
                result = _apply5(v5);
                return true;
            }
            result = default(TResult);
            return false;
        }
    }

	public static partial class Parsers<TInputElements>
		where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
    {
		///<summary>
		/// 各パーサーでパースを試行し、最初にパースに成功したパース結果を apply 操作を通して TOutput に変換します。
		///</summary>
        public static Parser<TInputElements, TOutput> Or<T0,T1,T2,T3,T4,T5, TOutput>(
			Parser<TInputElements, T0> p0, Func<T0, TOutput> apply0 , 
			Parser<TInputElements, T1> p1, Func<T1, TOutput> apply1 , 
			Parser<TInputElements, T2> p2, Func<T2, TOutput> apply2 , 
			Parser<TInputElements, T3> p3, Func<T3, TOutput> apply3 , 
			Parser<TInputElements, T4> p4, Func<T4, TOutput> apply4 , 
			Parser<TInputElements, T5> p5, Func<T5, TOutput> apply5  
		)
        {
			Contract.Requires( p0!=null );
			Contract.Requires( apply0!=null );
			Contract.Requires( p1!=null );
			Contract.Requires( apply1!=null );
			Contract.Requires( p2!=null );
			Contract.Requires( apply2!=null );
			Contract.Requires( p3!=null );
			Contract.Requires( apply3!=null );
			Contract.Requires( p4!=null );
			Contract.Requires( apply4!=null );
			Contract.Requires( p5!=null );
			Contract.Requires( apply5!=null );

            Parser<TInputElements, TOutput> result = new OrParser<TInputElements, T0,T1,T2,T3,T4,T5, TOutput>(
                p0, apply0 ,
                p1, apply1 ,
                p2, apply2 ,
                p3, apply3 ,
                p4, apply4 ,
                p5, apply5 
                );
            return EnableTrace ? WrapTracer(result) : result;
        }

	}


	///<summary>
	/// 複数のパーサーから一つの値を選択するパーサーです。
	///</summary>
    public class OrParser<TInputElements,T0,T1,T2,T3,T4,T5,T6,TResult> : Parser<TInputElements,TResult>
    {
        private readonly Parser<TInputElements, T0> _p0;
        private readonly Func<T0, TResult> _apply0;
        private readonly Parser<TInputElements, T1> _p1;
        private readonly Func<T1, TResult> _apply1;
        private readonly Parser<TInputElements, T2> _p2;
        private readonly Func<T2, TResult> _apply2;
        private readonly Parser<TInputElements, T3> _p3;
        private readonly Func<T3, TResult> _apply3;
        private readonly Parser<TInputElements, T4> _p4;
        private readonly Func<T4, TResult> _apply4;
        private readonly Parser<TInputElements, T5> _p5;
        private readonly Func<T5, TResult> _apply5;
        private readonly Parser<TInputElements, T6> _p6;
        private readonly Func<T6, TResult> _apply6;

		///<summary>
		/// 選択要素を持つパーサーを構築します。
		///</summary>
        public OrParser( 
			Parser<TInputElements,T0> p0, Func<T0,TResult> apply0 ,
			Parser<TInputElements,T1> p1, Func<T1,TResult> apply1 ,
			Parser<TInputElements,T2> p2, Func<T2,TResult> apply2 ,
			Parser<TInputElements,T3> p3, Func<T3,TResult> apply3 ,
			Parser<TInputElements,T4> p4, Func<T4,TResult> apply4 ,
			Parser<TInputElements,T5> p5, Func<T5,TResult> apply5 ,
			Parser<TInputElements,T6> p6, Func<T6,TResult> apply6 
		)
        {
			Contract.Requires( p0 !=null );
			Contract.Requires( apply0 !=null );
			Contract.Requires( p1 !=null );
			Contract.Requires( apply1 !=null );
			Contract.Requires( p2 !=null );
			Contract.Requires( apply2 !=null );
			Contract.Requires( p3 !=null );
			Contract.Requires( apply3 !=null );
			Contract.Requires( p4 !=null );
			Contract.Requires( apply4 !=null );
			Contract.Requires( p5 !=null );
			Contract.Requires( apply5 !=null );
			Contract.Requires( p6 !=null );
			Contract.Requires( apply6 !=null );

            _p0 = p0;
            _apply0 = apply0;
            _p1 = p1;
            _apply1 = apply1;
            _p2 = p2;
            _apply2 = apply2;
            _p3 = p3;
            _apply3 = apply3;
            _p4 = p4;
            _apply4 = apply4;
            _p5 = p5;
            _apply5 = apply5;
            _p6 = p6;
            _apply6 = apply6;
        }

		///<summary>
		/// コンストラクタで渡された各パーサーでパースを試行し、最初にパースを成功したパーサーの結果を適用関数を通して返します。
		///</summary>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TResult result)
        {
            T0 v0;
            if (_p0.Parse(input, index, out endInput, out v0))
            {
                result = _apply0(v0);
                return true;
            }
            T1 v1;
            if (_p1.Parse(input, index, out endInput, out v1))
            {
                result = _apply1(v1);
                return true;
            }
            T2 v2;
            if (_p2.Parse(input, index, out endInput, out v2))
            {
                result = _apply2(v2);
                return true;
            }
            T3 v3;
            if (_p3.Parse(input, index, out endInput, out v3))
            {
                result = _apply3(v3);
                return true;
            }
            T4 v4;
            if (_p4.Parse(input, index, out endInput, out v4))
            {
                result = _apply4(v4);
                return true;
            }
            T5 v5;
            if (_p5.Parse(input, index, out endInput, out v5))
            {
                result = _apply5(v5);
                return true;
            }
            T6 v6;
            if (_p6.Parse(input, index, out endInput, out v6))
            {
                result = _apply6(v6);
                return true;
            }
            result = default(TResult);
            return false;
        }
    }

	public static partial class Parsers<TInputElements>
		where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
    {
		///<summary>
		/// 各パーサーでパースを試行し、最初にパースに成功したパース結果を apply 操作を通して TOutput に変換します。
		///</summary>
        public static Parser<TInputElements, TOutput> Or<T0,T1,T2,T3,T4,T5,T6, TOutput>(
			Parser<TInputElements, T0> p0, Func<T0, TOutput> apply0 , 
			Parser<TInputElements, T1> p1, Func<T1, TOutput> apply1 , 
			Parser<TInputElements, T2> p2, Func<T2, TOutput> apply2 , 
			Parser<TInputElements, T3> p3, Func<T3, TOutput> apply3 , 
			Parser<TInputElements, T4> p4, Func<T4, TOutput> apply4 , 
			Parser<TInputElements, T5> p5, Func<T5, TOutput> apply5 , 
			Parser<TInputElements, T6> p6, Func<T6, TOutput> apply6  
		)
        {
			Contract.Requires( p0!=null );
			Contract.Requires( apply0!=null );
			Contract.Requires( p1!=null );
			Contract.Requires( apply1!=null );
			Contract.Requires( p2!=null );
			Contract.Requires( apply2!=null );
			Contract.Requires( p3!=null );
			Contract.Requires( apply3!=null );
			Contract.Requires( p4!=null );
			Contract.Requires( apply4!=null );
			Contract.Requires( p5!=null );
			Contract.Requires( apply5!=null );
			Contract.Requires( p6!=null );
			Contract.Requires( apply6!=null );

            Parser<TInputElements, TOutput> result = new OrParser<TInputElements, T0,T1,T2,T3,T4,T5,T6, TOutput>(
                p0, apply0 ,
                p1, apply1 ,
                p2, apply2 ,
                p3, apply3 ,
                p4, apply4 ,
                p5, apply5 ,
                p6, apply6 
                );
            return EnableTrace ? WrapTracer(result) : result;
        }

	}


	///<summary>
	/// 複数のパーサーから一つの値を選択するパーサーです。
	///</summary>
    public class OrParser<TInputElements,T0,T1,T2,T3,T4,T5,T6,T7,TResult> : Parser<TInputElements,TResult>
    {
        private readonly Parser<TInputElements, T0> _p0;
        private readonly Func<T0, TResult> _apply0;
        private readonly Parser<TInputElements, T1> _p1;
        private readonly Func<T1, TResult> _apply1;
        private readonly Parser<TInputElements, T2> _p2;
        private readonly Func<T2, TResult> _apply2;
        private readonly Parser<TInputElements, T3> _p3;
        private readonly Func<T3, TResult> _apply3;
        private readonly Parser<TInputElements, T4> _p4;
        private readonly Func<T4, TResult> _apply4;
        private readonly Parser<TInputElements, T5> _p5;
        private readonly Func<T5, TResult> _apply5;
        private readonly Parser<TInputElements, T6> _p6;
        private readonly Func<T6, TResult> _apply6;
        private readonly Parser<TInputElements, T7> _p7;
        private readonly Func<T7, TResult> _apply7;

		///<summary>
		/// 選択要素を持つパーサーを構築します。
		///</summary>
        public OrParser( 
			Parser<TInputElements,T0> p0, Func<T0,TResult> apply0 ,
			Parser<TInputElements,T1> p1, Func<T1,TResult> apply1 ,
			Parser<TInputElements,T2> p2, Func<T2,TResult> apply2 ,
			Parser<TInputElements,T3> p3, Func<T3,TResult> apply3 ,
			Parser<TInputElements,T4> p4, Func<T4,TResult> apply4 ,
			Parser<TInputElements,T5> p5, Func<T5,TResult> apply5 ,
			Parser<TInputElements,T6> p6, Func<T6,TResult> apply6 ,
			Parser<TInputElements,T7> p7, Func<T7,TResult> apply7 
		)
        {
			Contract.Requires( p0 !=null );
			Contract.Requires( apply0 !=null );
			Contract.Requires( p1 !=null );
			Contract.Requires( apply1 !=null );
			Contract.Requires( p2 !=null );
			Contract.Requires( apply2 !=null );
			Contract.Requires( p3 !=null );
			Contract.Requires( apply3 !=null );
			Contract.Requires( p4 !=null );
			Contract.Requires( apply4 !=null );
			Contract.Requires( p5 !=null );
			Contract.Requires( apply5 !=null );
			Contract.Requires( p6 !=null );
			Contract.Requires( apply6 !=null );
			Contract.Requires( p7 !=null );
			Contract.Requires( apply7 !=null );

            _p0 = p0;
            _apply0 = apply0;
            _p1 = p1;
            _apply1 = apply1;
            _p2 = p2;
            _apply2 = apply2;
            _p3 = p3;
            _apply3 = apply3;
            _p4 = p4;
            _apply4 = apply4;
            _p5 = p5;
            _apply5 = apply5;
            _p6 = p6;
            _apply6 = apply6;
            _p7 = p7;
            _apply7 = apply7;
        }

		///<summary>
		/// コンストラクタで渡された各パーサーでパースを試行し、最初にパースを成功したパーサーの結果を適用関数を通して返します。
		///</summary>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TResult result)
        {
            T0 v0;
            if (_p0.Parse(input, index, out endInput, out v0))
            {
                result = _apply0(v0);
                return true;
            }
            T1 v1;
            if (_p1.Parse(input, index, out endInput, out v1))
            {
                result = _apply1(v1);
                return true;
            }
            T2 v2;
            if (_p2.Parse(input, index, out endInput, out v2))
            {
                result = _apply2(v2);
                return true;
            }
            T3 v3;
            if (_p3.Parse(input, index, out endInput, out v3))
            {
                result = _apply3(v3);
                return true;
            }
            T4 v4;
            if (_p4.Parse(input, index, out endInput, out v4))
            {
                result = _apply4(v4);
                return true;
            }
            T5 v5;
            if (_p5.Parse(input, index, out endInput, out v5))
            {
                result = _apply5(v5);
                return true;
            }
            T6 v6;
            if (_p6.Parse(input, index, out endInput, out v6))
            {
                result = _apply6(v6);
                return true;
            }
            T7 v7;
            if (_p7.Parse(input, index, out endInput, out v7))
            {
                result = _apply7(v7);
                return true;
            }
            result = default(TResult);
            return false;
        }
    }

	public static partial class Parsers<TInputElements>
		where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
    {
		///<summary>
		/// 各パーサーでパースを試行し、最初にパースに成功したパース結果を apply 操作を通して TOutput に変換します。
		///</summary>
        public static Parser<TInputElements, TOutput> Or<T0,T1,T2,T3,T4,T5,T6,T7, TOutput>(
			Parser<TInputElements, T0> p0, Func<T0, TOutput> apply0 , 
			Parser<TInputElements, T1> p1, Func<T1, TOutput> apply1 , 
			Parser<TInputElements, T2> p2, Func<T2, TOutput> apply2 , 
			Parser<TInputElements, T3> p3, Func<T3, TOutput> apply3 , 
			Parser<TInputElements, T4> p4, Func<T4, TOutput> apply4 , 
			Parser<TInputElements, T5> p5, Func<T5, TOutput> apply5 , 
			Parser<TInputElements, T6> p6, Func<T6, TOutput> apply6 , 
			Parser<TInputElements, T7> p7, Func<T7, TOutput> apply7  
		)
        {
			Contract.Requires( p0!=null );
			Contract.Requires( apply0!=null );
			Contract.Requires( p1!=null );
			Contract.Requires( apply1!=null );
			Contract.Requires( p2!=null );
			Contract.Requires( apply2!=null );
			Contract.Requires( p3!=null );
			Contract.Requires( apply3!=null );
			Contract.Requires( p4!=null );
			Contract.Requires( apply4!=null );
			Contract.Requires( p5!=null );
			Contract.Requires( apply5!=null );
			Contract.Requires( p6!=null );
			Contract.Requires( apply6!=null );
			Contract.Requires( p7!=null );
			Contract.Requires( apply7!=null );

            Parser<TInputElements, TOutput> result = new OrParser<TInputElements, T0,T1,T2,T3,T4,T5,T6,T7, TOutput>(
                p0, apply0 ,
                p1, apply1 ,
                p2, apply2 ,
                p3, apply3 ,
                p4, apply4 ,
                p5, apply5 ,
                p6, apply6 ,
                p7, apply7 
                );
            return EnableTrace ? WrapTracer(result) : result;
        }

	}


	///<summary>
	/// 複数のパーサーから一つの値を選択するパーサーです。
	///</summary>
    public class OrParser<TInputElements,T0,T1,T2,T3,T4,T5,T6,T7,T8,TResult> : Parser<TInputElements,TResult>
    {
        private readonly Parser<TInputElements, T0> _p0;
        private readonly Func<T0, TResult> _apply0;
        private readonly Parser<TInputElements, T1> _p1;
        private readonly Func<T1, TResult> _apply1;
        private readonly Parser<TInputElements, T2> _p2;
        private readonly Func<T2, TResult> _apply2;
        private readonly Parser<TInputElements, T3> _p3;
        private readonly Func<T3, TResult> _apply3;
        private readonly Parser<TInputElements, T4> _p4;
        private readonly Func<T4, TResult> _apply4;
        private readonly Parser<TInputElements, T5> _p5;
        private readonly Func<T5, TResult> _apply5;
        private readonly Parser<TInputElements, T6> _p6;
        private readonly Func<T6, TResult> _apply6;
        private readonly Parser<TInputElements, T7> _p7;
        private readonly Func<T7, TResult> _apply7;
        private readonly Parser<TInputElements, T8> _p8;
        private readonly Func<T8, TResult> _apply8;

		///<summary>
		/// 選択要素を持つパーサーを構築します。
		///</summary>
        public OrParser( 
			Parser<TInputElements,T0> p0, Func<T0,TResult> apply0 ,
			Parser<TInputElements,T1> p1, Func<T1,TResult> apply1 ,
			Parser<TInputElements,T2> p2, Func<T2,TResult> apply2 ,
			Parser<TInputElements,T3> p3, Func<T3,TResult> apply3 ,
			Parser<TInputElements,T4> p4, Func<T4,TResult> apply4 ,
			Parser<TInputElements,T5> p5, Func<T5,TResult> apply5 ,
			Parser<TInputElements,T6> p6, Func<T6,TResult> apply6 ,
			Parser<TInputElements,T7> p7, Func<T7,TResult> apply7 ,
			Parser<TInputElements,T8> p8, Func<T8,TResult> apply8 
		)
        {
			Contract.Requires( p0 !=null );
			Contract.Requires( apply0 !=null );
			Contract.Requires( p1 !=null );
			Contract.Requires( apply1 !=null );
			Contract.Requires( p2 !=null );
			Contract.Requires( apply2 !=null );
			Contract.Requires( p3 !=null );
			Contract.Requires( apply3 !=null );
			Contract.Requires( p4 !=null );
			Contract.Requires( apply4 !=null );
			Contract.Requires( p5 !=null );
			Contract.Requires( apply5 !=null );
			Contract.Requires( p6 !=null );
			Contract.Requires( apply6 !=null );
			Contract.Requires( p7 !=null );
			Contract.Requires( apply7 !=null );
			Contract.Requires( p8 !=null );
			Contract.Requires( apply8 !=null );

            _p0 = p0;
            _apply0 = apply0;
            _p1 = p1;
            _apply1 = apply1;
            _p2 = p2;
            _apply2 = apply2;
            _p3 = p3;
            _apply3 = apply3;
            _p4 = p4;
            _apply4 = apply4;
            _p5 = p5;
            _apply5 = apply5;
            _p6 = p6;
            _apply6 = apply6;
            _p7 = p7;
            _apply7 = apply7;
            _p8 = p8;
            _apply8 = apply8;
        }

		///<summary>
		/// コンストラクタで渡された各パーサーでパースを試行し、最初にパースを成功したパーサーの結果を適用関数を通して返します。
		///</summary>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TResult result)
        {
            T0 v0;
            if (_p0.Parse(input, index, out endInput, out v0))
            {
                result = _apply0(v0);
                return true;
            }
            T1 v1;
            if (_p1.Parse(input, index, out endInput, out v1))
            {
                result = _apply1(v1);
                return true;
            }
            T2 v2;
            if (_p2.Parse(input, index, out endInput, out v2))
            {
                result = _apply2(v2);
                return true;
            }
            T3 v3;
            if (_p3.Parse(input, index, out endInput, out v3))
            {
                result = _apply3(v3);
                return true;
            }
            T4 v4;
            if (_p4.Parse(input, index, out endInput, out v4))
            {
                result = _apply4(v4);
                return true;
            }
            T5 v5;
            if (_p5.Parse(input, index, out endInput, out v5))
            {
                result = _apply5(v5);
                return true;
            }
            T6 v6;
            if (_p6.Parse(input, index, out endInput, out v6))
            {
                result = _apply6(v6);
                return true;
            }
            T7 v7;
            if (_p7.Parse(input, index, out endInput, out v7))
            {
                result = _apply7(v7);
                return true;
            }
            T8 v8;
            if (_p8.Parse(input, index, out endInput, out v8))
            {
                result = _apply8(v8);
                return true;
            }
            result = default(TResult);
            return false;
        }
    }

	public static partial class Parsers<TInputElements>
		where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
    {
		///<summary>
		/// 各パーサーでパースを試行し、最初にパースに成功したパース結果を apply 操作を通して TOutput に変換します。
		///</summary>
        public static Parser<TInputElements, TOutput> Or<T0,T1,T2,T3,T4,T5,T6,T7,T8, TOutput>(
			Parser<TInputElements, T0> p0, Func<T0, TOutput> apply0 , 
			Parser<TInputElements, T1> p1, Func<T1, TOutput> apply1 , 
			Parser<TInputElements, T2> p2, Func<T2, TOutput> apply2 , 
			Parser<TInputElements, T3> p3, Func<T3, TOutput> apply3 , 
			Parser<TInputElements, T4> p4, Func<T4, TOutput> apply4 , 
			Parser<TInputElements, T5> p5, Func<T5, TOutput> apply5 , 
			Parser<TInputElements, T6> p6, Func<T6, TOutput> apply6 , 
			Parser<TInputElements, T7> p7, Func<T7, TOutput> apply7 , 
			Parser<TInputElements, T8> p8, Func<T8, TOutput> apply8  
		)
        {
			Contract.Requires( p0!=null );
			Contract.Requires( apply0!=null );
			Contract.Requires( p1!=null );
			Contract.Requires( apply1!=null );
			Contract.Requires( p2!=null );
			Contract.Requires( apply2!=null );
			Contract.Requires( p3!=null );
			Contract.Requires( apply3!=null );
			Contract.Requires( p4!=null );
			Contract.Requires( apply4!=null );
			Contract.Requires( p5!=null );
			Contract.Requires( apply5!=null );
			Contract.Requires( p6!=null );
			Contract.Requires( apply6!=null );
			Contract.Requires( p7!=null );
			Contract.Requires( apply7!=null );
			Contract.Requires( p8!=null );
			Contract.Requires( apply8!=null );

            Parser<TInputElements, TOutput> result = new OrParser<TInputElements, T0,T1,T2,T3,T4,T5,T6,T7,T8, TOutput>(
                p0, apply0 ,
                p1, apply1 ,
                p2, apply2 ,
                p3, apply3 ,
                p4, apply4 ,
                p5, apply5 ,
                p6, apply6 ,
                p7, apply7 ,
                p8, apply8 
                );
            return EnableTrace ? WrapTracer(result) : result;
        }

	}


	///<summary>
	/// 複数のパーサーから一つの値を選択するパーサーです。
	///</summary>
    public class OrParser<TInputElements,T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,TResult> : Parser<TInputElements,TResult>
    {
        private readonly Parser<TInputElements, T0> _p0;
        private readonly Func<T0, TResult> _apply0;
        private readonly Parser<TInputElements, T1> _p1;
        private readonly Func<T1, TResult> _apply1;
        private readonly Parser<TInputElements, T2> _p2;
        private readonly Func<T2, TResult> _apply2;
        private readonly Parser<TInputElements, T3> _p3;
        private readonly Func<T3, TResult> _apply3;
        private readonly Parser<TInputElements, T4> _p4;
        private readonly Func<T4, TResult> _apply4;
        private readonly Parser<TInputElements, T5> _p5;
        private readonly Func<T5, TResult> _apply5;
        private readonly Parser<TInputElements, T6> _p6;
        private readonly Func<T6, TResult> _apply6;
        private readonly Parser<TInputElements, T7> _p7;
        private readonly Func<T7, TResult> _apply7;
        private readonly Parser<TInputElements, T8> _p8;
        private readonly Func<T8, TResult> _apply8;
        private readonly Parser<TInputElements, T9> _p9;
        private readonly Func<T9, TResult> _apply9;

		///<summary>
		/// 選択要素を持つパーサーを構築します。
		///</summary>
        public OrParser( 
			Parser<TInputElements,T0> p0, Func<T0,TResult> apply0 ,
			Parser<TInputElements,T1> p1, Func<T1,TResult> apply1 ,
			Parser<TInputElements,T2> p2, Func<T2,TResult> apply2 ,
			Parser<TInputElements,T3> p3, Func<T3,TResult> apply3 ,
			Parser<TInputElements,T4> p4, Func<T4,TResult> apply4 ,
			Parser<TInputElements,T5> p5, Func<T5,TResult> apply5 ,
			Parser<TInputElements,T6> p6, Func<T6,TResult> apply6 ,
			Parser<TInputElements,T7> p7, Func<T7,TResult> apply7 ,
			Parser<TInputElements,T8> p8, Func<T8,TResult> apply8 ,
			Parser<TInputElements,T9> p9, Func<T9,TResult> apply9 
		)
        {
			Contract.Requires( p0 !=null );
			Contract.Requires( apply0 !=null );
			Contract.Requires( p1 !=null );
			Contract.Requires( apply1 !=null );
			Contract.Requires( p2 !=null );
			Contract.Requires( apply2 !=null );
			Contract.Requires( p3 !=null );
			Contract.Requires( apply3 !=null );
			Contract.Requires( p4 !=null );
			Contract.Requires( apply4 !=null );
			Contract.Requires( p5 !=null );
			Contract.Requires( apply5 !=null );
			Contract.Requires( p6 !=null );
			Contract.Requires( apply6 !=null );
			Contract.Requires( p7 !=null );
			Contract.Requires( apply7 !=null );
			Contract.Requires( p8 !=null );
			Contract.Requires( apply8 !=null );
			Contract.Requires( p9 !=null );
			Contract.Requires( apply9 !=null );

            _p0 = p0;
            _apply0 = apply0;
            _p1 = p1;
            _apply1 = apply1;
            _p2 = p2;
            _apply2 = apply2;
            _p3 = p3;
            _apply3 = apply3;
            _p4 = p4;
            _apply4 = apply4;
            _p5 = p5;
            _apply5 = apply5;
            _p6 = p6;
            _apply6 = apply6;
            _p7 = p7;
            _apply7 = apply7;
            _p8 = p8;
            _apply8 = apply8;
            _p9 = p9;
            _apply9 = apply9;
        }

		///<summary>
		/// コンストラクタで渡された各パーサーでパースを試行し、最初にパースを成功したパーサーの結果を適用関数を通して返します。
		///</summary>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TResult result)
        {
            T0 v0;
            if (_p0.Parse(input, index, out endInput, out v0))
            {
                result = _apply0(v0);
                return true;
            }
            T1 v1;
            if (_p1.Parse(input, index, out endInput, out v1))
            {
                result = _apply1(v1);
                return true;
            }
            T2 v2;
            if (_p2.Parse(input, index, out endInput, out v2))
            {
                result = _apply2(v2);
                return true;
            }
            T3 v3;
            if (_p3.Parse(input, index, out endInput, out v3))
            {
                result = _apply3(v3);
                return true;
            }
            T4 v4;
            if (_p4.Parse(input, index, out endInput, out v4))
            {
                result = _apply4(v4);
                return true;
            }
            T5 v5;
            if (_p5.Parse(input, index, out endInput, out v5))
            {
                result = _apply5(v5);
                return true;
            }
            T6 v6;
            if (_p6.Parse(input, index, out endInput, out v6))
            {
                result = _apply6(v6);
                return true;
            }
            T7 v7;
            if (_p7.Parse(input, index, out endInput, out v7))
            {
                result = _apply7(v7);
                return true;
            }
            T8 v8;
            if (_p8.Parse(input, index, out endInput, out v8))
            {
                result = _apply8(v8);
                return true;
            }
            T9 v9;
            if (_p9.Parse(input, index, out endInput, out v9))
            {
                result = _apply9(v9);
                return true;
            }
            result = default(TResult);
            return false;
        }
    }

	public static partial class Parsers<TInputElements>
		where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
    {
		///<summary>
		/// 各パーサーでパースを試行し、最初にパースに成功したパース結果を apply 操作を通して TOutput に変換します。
		///</summary>
        public static Parser<TInputElements, TOutput> Or<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9, TOutput>(
			Parser<TInputElements, T0> p0, Func<T0, TOutput> apply0 , 
			Parser<TInputElements, T1> p1, Func<T1, TOutput> apply1 , 
			Parser<TInputElements, T2> p2, Func<T2, TOutput> apply2 , 
			Parser<TInputElements, T3> p3, Func<T3, TOutput> apply3 , 
			Parser<TInputElements, T4> p4, Func<T4, TOutput> apply4 , 
			Parser<TInputElements, T5> p5, Func<T5, TOutput> apply5 , 
			Parser<TInputElements, T6> p6, Func<T6, TOutput> apply6 , 
			Parser<TInputElements, T7> p7, Func<T7, TOutput> apply7 , 
			Parser<TInputElements, T8> p8, Func<T8, TOutput> apply8 , 
			Parser<TInputElements, T9> p9, Func<T9, TOutput> apply9  
		)
        {
			Contract.Requires( p0!=null );
			Contract.Requires( apply0!=null );
			Contract.Requires( p1!=null );
			Contract.Requires( apply1!=null );
			Contract.Requires( p2!=null );
			Contract.Requires( apply2!=null );
			Contract.Requires( p3!=null );
			Contract.Requires( apply3!=null );
			Contract.Requires( p4!=null );
			Contract.Requires( apply4!=null );
			Contract.Requires( p5!=null );
			Contract.Requires( apply5!=null );
			Contract.Requires( p6!=null );
			Contract.Requires( apply6!=null );
			Contract.Requires( p7!=null );
			Contract.Requires( apply7!=null );
			Contract.Requires( p8!=null );
			Contract.Requires( apply8!=null );
			Contract.Requires( p9!=null );
			Contract.Requires( apply9!=null );

            Parser<TInputElements, TOutput> result = new OrParser<TInputElements, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9, TOutput>(
                p0, apply0 ,
                p1, apply1 ,
                p2, apply2 ,
                p3, apply3 ,
                p4, apply4 ,
                p5, apply5 ,
                p6, apply6 ,
                p7, apply7 ,
                p8, apply8 ,
                p9, apply9 
                );
            return EnableTrace ? WrapTracer(result) : result;
        }

	}


}