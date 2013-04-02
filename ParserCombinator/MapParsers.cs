
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ParserCombinator
{

	///<summary>
	/// パーサーによって取得された値の変換を行うパーサーです
	///</summary>
    public class MapParser<TInputElements, T0, TResult > : Parser<TInputElements, TResult>
    {
        private readonly Parser<TInputElements, T0> _p0;
        private readonly Func<T0, TResult> _mapper;

		///<summary>
		/// 変換操作を行うパーサーを構築します。
		///</summary>
        public MapParser(
			Parser<TInputElements, T0> p0, 
			Func<T0, TResult> mapper )
        {
            Contract.Requires(p0!=null);
            Contract.Requires(mapper!=null);
            _p0 = p0;
            _mapper = mapper;
        }

		///<summary>
		/// 入力をパースします。
		///</summary>
		///<remarks>
		/// コンストラクタで指定されたパーサーすべてが true を返すと変換操作が行われます。
		/// 配下のパーサーいずれかが false を返し失敗した場合、endInput は入力された index を返します。
		///</remarks>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TResult result)
        {
            result = default(TResult);
			endInput = index;
			int currentIndex = index;
            T0 v0;
            if(! _p0.Parse(input, currentIndex, out currentIndex, out v0) )
			{
				return false;
			}
			endInput = currentIndex;
			result = _mapper(v0 );
            return true;
        }
    }

	public static partial class Parsers<TInputElements>
		where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
	{
		///<summary>
		/// 指定された各パーサーの実行結果を mapper で変換するパーサーを取得します。
		///</summary>
        public static Parser<TInputElements, TResult> Map<T0, TResult>(
			Parser<TInputElements, T0> p0, 
			Func<T0, TResult> mapper)
        {
			Contract.Requires( p0!=null );
			Contract.Requires( mapper!=null );
			Parser<TInputElements, TResult> parser =
				new MapParser<TInputElements, T0, TResult>(
					p0, mapper);
			return EnableTrace 
				? WrapTracer(parser)
				: parser;
        }
	}


	///<summary>
	/// パーサーによって取得された値の変換を行うパーサーです
	///</summary>
    public class MapParser<TInputElements, T0,T1, TResult > : Parser<TInputElements, TResult>
    {
        private readonly Parser<TInputElements, T0> _p0;
        private readonly Parser<TInputElements, T1> _p1;
        private readonly Func<T0,T1, TResult> _mapper;

		///<summary>
		/// 変換操作を行うパーサーを構築します。
		///</summary>
        public MapParser(
			Parser<TInputElements, T0> p0, 
			Parser<TInputElements, T1> p1, 
			Func<T0,T1, TResult> mapper )
        {
            Contract.Requires(p0!=null);
            Contract.Requires(p1!=null);
            Contract.Requires(mapper!=null);
            _p0 = p0;
            _p1 = p1;
            _mapper = mapper;
        }

		///<summary>
		/// 入力をパースします。
		///</summary>
		///<remarks>
		/// コンストラクタで指定されたパーサーすべてが true を返すと変換操作が行われます。
		/// 配下のパーサーいずれかが false を返し失敗した場合、endInput は入力された index を返します。
		///</remarks>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TResult result)
        {
            result = default(TResult);
			endInput = index;
			int currentIndex = index;
            T0 v0;
            if(! _p0.Parse(input, currentIndex, out currentIndex, out v0) )
			{
				return false;
			}
            T1 v1;
            if(! _p1.Parse(input, currentIndex, out currentIndex, out v1) )
			{
				return false;
			}
			endInput = currentIndex;
			result = _mapper(v0,v1 );
            return true;
        }
    }

	public static partial class Parsers<TInputElements>
		where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
	{
		///<summary>
		/// 指定された各パーサーの実行結果を mapper で変換するパーサーを取得します。
		///</summary>
        public static Parser<TInputElements, TResult> Map<T0,T1, TResult>(
			Parser<TInputElements, T0> p0, 
			Parser<TInputElements, T1> p1, 
			Func<T0,T1, TResult> mapper)
        {
			Contract.Requires( p0!=null );
			Contract.Requires( p1!=null );
			Contract.Requires( mapper!=null );
			Parser<TInputElements, TResult> parser =
				new MapParser<TInputElements, T0,T1, TResult>(
					p0,p1, mapper);
			return EnableTrace 
				? WrapTracer(parser)
				: parser;
        }
	}


	///<summary>
	/// パーサーによって取得された値の変換を行うパーサーです
	///</summary>
    public class MapParser<TInputElements, T0,T1,T2, TResult > : Parser<TInputElements, TResult>
    {
        private readonly Parser<TInputElements, T0> _p0;
        private readonly Parser<TInputElements, T1> _p1;
        private readonly Parser<TInputElements, T2> _p2;
        private readonly Func<T0,T1,T2, TResult> _mapper;

		///<summary>
		/// 変換操作を行うパーサーを構築します。
		///</summary>
        public MapParser(
			Parser<TInputElements, T0> p0, 
			Parser<TInputElements, T1> p1, 
			Parser<TInputElements, T2> p2, 
			Func<T0,T1,T2, TResult> mapper )
        {
            Contract.Requires(p0!=null);
            Contract.Requires(p1!=null);
            Contract.Requires(p2!=null);
            Contract.Requires(mapper!=null);
            _p0 = p0;
            _p1 = p1;
            _p2 = p2;
            _mapper = mapper;
        }

		///<summary>
		/// 入力をパースします。
		///</summary>
		///<remarks>
		/// コンストラクタで指定されたパーサーすべてが true を返すと変換操作が行われます。
		/// 配下のパーサーいずれかが false を返し失敗した場合、endInput は入力された index を返します。
		///</remarks>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TResult result)
        {
            result = default(TResult);
			endInput = index;
			int currentIndex = index;
            T0 v0;
            if(! _p0.Parse(input, currentIndex, out currentIndex, out v0) )
			{
				return false;
			}
            T1 v1;
            if(! _p1.Parse(input, currentIndex, out currentIndex, out v1) )
			{
				return false;
			}
            T2 v2;
            if(! _p2.Parse(input, currentIndex, out currentIndex, out v2) )
			{
				return false;
			}
			endInput = currentIndex;
			result = _mapper(v0,v1,v2 );
            return true;
        }
    }

	public static partial class Parsers<TInputElements>
		where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
	{
		///<summary>
		/// 指定された各パーサーの実行結果を mapper で変換するパーサーを取得します。
		///</summary>
        public static Parser<TInputElements, TResult> Map<T0,T1,T2, TResult>(
			Parser<TInputElements, T0> p0, 
			Parser<TInputElements, T1> p1, 
			Parser<TInputElements, T2> p2, 
			Func<T0,T1,T2, TResult> mapper)
        {
			Contract.Requires( p0!=null );
			Contract.Requires( p1!=null );
			Contract.Requires( p2!=null );
			Contract.Requires( mapper!=null );
			Parser<TInputElements, TResult> parser =
				new MapParser<TInputElements, T0,T1,T2, TResult>(
					p0,p1,p2, mapper);
			return EnableTrace 
				? WrapTracer(parser)
				: parser;
        }
	}


	///<summary>
	/// パーサーによって取得された値の変換を行うパーサーです
	///</summary>
    public class MapParser<TInputElements, T0,T1,T2,T3, TResult > : Parser<TInputElements, TResult>
    {
        private readonly Parser<TInputElements, T0> _p0;
        private readonly Parser<TInputElements, T1> _p1;
        private readonly Parser<TInputElements, T2> _p2;
        private readonly Parser<TInputElements, T3> _p3;
        private readonly Func<T0,T1,T2,T3, TResult> _mapper;

		///<summary>
		/// 変換操作を行うパーサーを構築します。
		///</summary>
        public MapParser(
			Parser<TInputElements, T0> p0, 
			Parser<TInputElements, T1> p1, 
			Parser<TInputElements, T2> p2, 
			Parser<TInputElements, T3> p3, 
			Func<T0,T1,T2,T3, TResult> mapper )
        {
            Contract.Requires(p0!=null);
            Contract.Requires(p1!=null);
            Contract.Requires(p2!=null);
            Contract.Requires(p3!=null);
            Contract.Requires(mapper!=null);
            _p0 = p0;
            _p1 = p1;
            _p2 = p2;
            _p3 = p3;
            _mapper = mapper;
        }

		///<summary>
		/// 入力をパースします。
		///</summary>
		///<remarks>
		/// コンストラクタで指定されたパーサーすべてが true を返すと変換操作が行われます。
		/// 配下のパーサーいずれかが false を返し失敗した場合、endInput は入力された index を返します。
		///</remarks>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TResult result)
        {
            result = default(TResult);
			endInput = index;
			int currentIndex = index;
            T0 v0;
            if(! _p0.Parse(input, currentIndex, out currentIndex, out v0) )
			{
				return false;
			}
            T1 v1;
            if(! _p1.Parse(input, currentIndex, out currentIndex, out v1) )
			{
				return false;
			}
            T2 v2;
            if(! _p2.Parse(input, currentIndex, out currentIndex, out v2) )
			{
				return false;
			}
            T3 v3;
            if(! _p3.Parse(input, currentIndex, out currentIndex, out v3) )
			{
				return false;
			}
			endInput = currentIndex;
			result = _mapper(v0,v1,v2,v3 );
            return true;
        }
    }

	public static partial class Parsers<TInputElements>
		where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
	{
		///<summary>
		/// 指定された各パーサーの実行結果を mapper で変換するパーサーを取得します。
		///</summary>
        public static Parser<TInputElements, TResult> Map<T0,T1,T2,T3, TResult>(
			Parser<TInputElements, T0> p0, 
			Parser<TInputElements, T1> p1, 
			Parser<TInputElements, T2> p2, 
			Parser<TInputElements, T3> p3, 
			Func<T0,T1,T2,T3, TResult> mapper)
        {
			Contract.Requires( p0!=null );
			Contract.Requires( p1!=null );
			Contract.Requires( p2!=null );
			Contract.Requires( p3!=null );
			Contract.Requires( mapper!=null );
			Parser<TInputElements, TResult> parser =
				new MapParser<TInputElements, T0,T1,T2,T3, TResult>(
					p0,p1,p2,p3, mapper);
			return EnableTrace 
				? WrapTracer(parser)
				: parser;
        }
	}


	///<summary>
	/// パーサーによって取得された値の変換を行うパーサーです
	///</summary>
    public class MapParser<TInputElements, T0,T1,T2,T3,T4, TResult > : Parser<TInputElements, TResult>
    {
        private readonly Parser<TInputElements, T0> _p0;
        private readonly Parser<TInputElements, T1> _p1;
        private readonly Parser<TInputElements, T2> _p2;
        private readonly Parser<TInputElements, T3> _p3;
        private readonly Parser<TInputElements, T4> _p4;
        private readonly Func<T0,T1,T2,T3,T4, TResult> _mapper;

		///<summary>
		/// 変換操作を行うパーサーを構築します。
		///</summary>
        public MapParser(
			Parser<TInputElements, T0> p0, 
			Parser<TInputElements, T1> p1, 
			Parser<TInputElements, T2> p2, 
			Parser<TInputElements, T3> p3, 
			Parser<TInputElements, T4> p4, 
			Func<T0,T1,T2,T3,T4, TResult> mapper )
        {
            Contract.Requires(p0!=null);
            Contract.Requires(p1!=null);
            Contract.Requires(p2!=null);
            Contract.Requires(p3!=null);
            Contract.Requires(p4!=null);
            Contract.Requires(mapper!=null);
            _p0 = p0;
            _p1 = p1;
            _p2 = p2;
            _p3 = p3;
            _p4 = p4;
            _mapper = mapper;
        }

		///<summary>
		/// 入力をパースします。
		///</summary>
		///<remarks>
		/// コンストラクタで指定されたパーサーすべてが true を返すと変換操作が行われます。
		/// 配下のパーサーいずれかが false を返し失敗した場合、endInput は入力された index を返します。
		///</remarks>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TResult result)
        {
            result = default(TResult);
			endInput = index;
			int currentIndex = index;
            T0 v0;
            if(! _p0.Parse(input, currentIndex, out currentIndex, out v0) )
			{
				return false;
			}
            T1 v1;
            if(! _p1.Parse(input, currentIndex, out currentIndex, out v1) )
			{
				return false;
			}
            T2 v2;
            if(! _p2.Parse(input, currentIndex, out currentIndex, out v2) )
			{
				return false;
			}
            T3 v3;
            if(! _p3.Parse(input, currentIndex, out currentIndex, out v3) )
			{
				return false;
			}
            T4 v4;
            if(! _p4.Parse(input, currentIndex, out currentIndex, out v4) )
			{
				return false;
			}
			endInput = currentIndex;
			result = _mapper(v0,v1,v2,v3,v4 );
            return true;
        }
    }

	public static partial class Parsers<TInputElements>
		where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
	{
		///<summary>
		/// 指定された各パーサーの実行結果を mapper で変換するパーサーを取得します。
		///</summary>
        public static Parser<TInputElements, TResult> Map<T0,T1,T2,T3,T4, TResult>(
			Parser<TInputElements, T0> p0, 
			Parser<TInputElements, T1> p1, 
			Parser<TInputElements, T2> p2, 
			Parser<TInputElements, T3> p3, 
			Parser<TInputElements, T4> p4, 
			Func<T0,T1,T2,T3,T4, TResult> mapper)
        {
			Contract.Requires( p0!=null );
			Contract.Requires( p1!=null );
			Contract.Requires( p2!=null );
			Contract.Requires( p3!=null );
			Contract.Requires( p4!=null );
			Contract.Requires( mapper!=null );
			Parser<TInputElements, TResult> parser =
				new MapParser<TInputElements, T0,T1,T2,T3,T4, TResult>(
					p0,p1,p2,p3,p4, mapper);
			return EnableTrace 
				? WrapTracer(parser)
				: parser;
        }
	}


	///<summary>
	/// パーサーによって取得された値の変換を行うパーサーです
	///</summary>
    public class MapParser<TInputElements, T0,T1,T2,T3,T4,T5, TResult > : Parser<TInputElements, TResult>
    {
        private readonly Parser<TInputElements, T0> _p0;
        private readonly Parser<TInputElements, T1> _p1;
        private readonly Parser<TInputElements, T2> _p2;
        private readonly Parser<TInputElements, T3> _p3;
        private readonly Parser<TInputElements, T4> _p4;
        private readonly Parser<TInputElements, T5> _p5;
        private readonly Func<T0,T1,T2,T3,T4,T5, TResult> _mapper;

		///<summary>
		/// 変換操作を行うパーサーを構築します。
		///</summary>
        public MapParser(
			Parser<TInputElements, T0> p0, 
			Parser<TInputElements, T1> p1, 
			Parser<TInputElements, T2> p2, 
			Parser<TInputElements, T3> p3, 
			Parser<TInputElements, T4> p4, 
			Parser<TInputElements, T5> p5, 
			Func<T0,T1,T2,T3,T4,T5, TResult> mapper )
        {
            Contract.Requires(p0!=null);
            Contract.Requires(p1!=null);
            Contract.Requires(p2!=null);
            Contract.Requires(p3!=null);
            Contract.Requires(p4!=null);
            Contract.Requires(p5!=null);
            Contract.Requires(mapper!=null);
            _p0 = p0;
            _p1 = p1;
            _p2 = p2;
            _p3 = p3;
            _p4 = p4;
            _p5 = p5;
            _mapper = mapper;
        }

		///<summary>
		/// 入力をパースします。
		///</summary>
		///<remarks>
		/// コンストラクタで指定されたパーサーすべてが true を返すと変換操作が行われます。
		/// 配下のパーサーいずれかが false を返し失敗した場合、endInput は入力された index を返します。
		///</remarks>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TResult result)
        {
            result = default(TResult);
			endInput = index;
			int currentIndex = index;
            T0 v0;
            if(! _p0.Parse(input, currentIndex, out currentIndex, out v0) )
			{
				return false;
			}
            T1 v1;
            if(! _p1.Parse(input, currentIndex, out currentIndex, out v1) )
			{
				return false;
			}
            T2 v2;
            if(! _p2.Parse(input, currentIndex, out currentIndex, out v2) )
			{
				return false;
			}
            T3 v3;
            if(! _p3.Parse(input, currentIndex, out currentIndex, out v3) )
			{
				return false;
			}
            T4 v4;
            if(! _p4.Parse(input, currentIndex, out currentIndex, out v4) )
			{
				return false;
			}
            T5 v5;
            if(! _p5.Parse(input, currentIndex, out currentIndex, out v5) )
			{
				return false;
			}
			endInput = currentIndex;
			result = _mapper(v0,v1,v2,v3,v4,v5 );
            return true;
        }
    }

	public static partial class Parsers<TInputElements>
		where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
	{
		///<summary>
		/// 指定された各パーサーの実行結果を mapper で変換するパーサーを取得します。
		///</summary>
        public static Parser<TInputElements, TResult> Map<T0,T1,T2,T3,T4,T5, TResult>(
			Parser<TInputElements, T0> p0, 
			Parser<TInputElements, T1> p1, 
			Parser<TInputElements, T2> p2, 
			Parser<TInputElements, T3> p3, 
			Parser<TInputElements, T4> p4, 
			Parser<TInputElements, T5> p5, 
			Func<T0,T1,T2,T3,T4,T5, TResult> mapper)
        {
			Contract.Requires( p0!=null );
			Contract.Requires( p1!=null );
			Contract.Requires( p2!=null );
			Contract.Requires( p3!=null );
			Contract.Requires( p4!=null );
			Contract.Requires( p5!=null );
			Contract.Requires( mapper!=null );
			Parser<TInputElements, TResult> parser =
				new MapParser<TInputElements, T0,T1,T2,T3,T4,T5, TResult>(
					p0,p1,p2,p3,p4,p5, mapper);
			return EnableTrace 
				? WrapTracer(parser)
				: parser;
        }
	}


	///<summary>
	/// パーサーによって取得された値の変換を行うパーサーです
	///</summary>
    public class MapParser<TInputElements, T0,T1,T2,T3,T4,T5,T6, TResult > : Parser<TInputElements, TResult>
    {
        private readonly Parser<TInputElements, T0> _p0;
        private readonly Parser<TInputElements, T1> _p1;
        private readonly Parser<TInputElements, T2> _p2;
        private readonly Parser<TInputElements, T3> _p3;
        private readonly Parser<TInputElements, T4> _p4;
        private readonly Parser<TInputElements, T5> _p5;
        private readonly Parser<TInputElements, T6> _p6;
        private readonly Func<T0,T1,T2,T3,T4,T5,T6, TResult> _mapper;

		///<summary>
		/// 変換操作を行うパーサーを構築します。
		///</summary>
        public MapParser(
			Parser<TInputElements, T0> p0, 
			Parser<TInputElements, T1> p1, 
			Parser<TInputElements, T2> p2, 
			Parser<TInputElements, T3> p3, 
			Parser<TInputElements, T4> p4, 
			Parser<TInputElements, T5> p5, 
			Parser<TInputElements, T6> p6, 
			Func<T0,T1,T2,T3,T4,T5,T6, TResult> mapper )
        {
            Contract.Requires(p0!=null);
            Contract.Requires(p1!=null);
            Contract.Requires(p2!=null);
            Contract.Requires(p3!=null);
            Contract.Requires(p4!=null);
            Contract.Requires(p5!=null);
            Contract.Requires(p6!=null);
            Contract.Requires(mapper!=null);
            _p0 = p0;
            _p1 = p1;
            _p2 = p2;
            _p3 = p3;
            _p4 = p4;
            _p5 = p5;
            _p6 = p6;
            _mapper = mapper;
        }

		///<summary>
		/// 入力をパースします。
		///</summary>
		///<remarks>
		/// コンストラクタで指定されたパーサーすべてが true を返すと変換操作が行われます。
		/// 配下のパーサーいずれかが false を返し失敗した場合、endInput は入力された index を返します。
		///</remarks>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TResult result)
        {
            result = default(TResult);
			endInput = index;
			int currentIndex = index;
            T0 v0;
            if(! _p0.Parse(input, currentIndex, out currentIndex, out v0) )
			{
				return false;
			}
            T1 v1;
            if(! _p1.Parse(input, currentIndex, out currentIndex, out v1) )
			{
				return false;
			}
            T2 v2;
            if(! _p2.Parse(input, currentIndex, out currentIndex, out v2) )
			{
				return false;
			}
            T3 v3;
            if(! _p3.Parse(input, currentIndex, out currentIndex, out v3) )
			{
				return false;
			}
            T4 v4;
            if(! _p4.Parse(input, currentIndex, out currentIndex, out v4) )
			{
				return false;
			}
            T5 v5;
            if(! _p5.Parse(input, currentIndex, out currentIndex, out v5) )
			{
				return false;
			}
            T6 v6;
            if(! _p6.Parse(input, currentIndex, out currentIndex, out v6) )
			{
				return false;
			}
			endInput = currentIndex;
			result = _mapper(v0,v1,v2,v3,v4,v5,v6 );
            return true;
        }
    }

	public static partial class Parsers<TInputElements>
		where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
	{
		///<summary>
		/// 指定された各パーサーの実行結果を mapper で変換するパーサーを取得します。
		///</summary>
        public static Parser<TInputElements, TResult> Map<T0,T1,T2,T3,T4,T5,T6, TResult>(
			Parser<TInputElements, T0> p0, 
			Parser<TInputElements, T1> p1, 
			Parser<TInputElements, T2> p2, 
			Parser<TInputElements, T3> p3, 
			Parser<TInputElements, T4> p4, 
			Parser<TInputElements, T5> p5, 
			Parser<TInputElements, T6> p6, 
			Func<T0,T1,T2,T3,T4,T5,T6, TResult> mapper)
        {
			Contract.Requires( p0!=null );
			Contract.Requires( p1!=null );
			Contract.Requires( p2!=null );
			Contract.Requires( p3!=null );
			Contract.Requires( p4!=null );
			Contract.Requires( p5!=null );
			Contract.Requires( p6!=null );
			Contract.Requires( mapper!=null );
			Parser<TInputElements, TResult> parser =
				new MapParser<TInputElements, T0,T1,T2,T3,T4,T5,T6, TResult>(
					p0,p1,p2,p3,p4,p5,p6, mapper);
			return EnableTrace 
				? WrapTracer(parser)
				: parser;
        }
	}


	///<summary>
	/// パーサーによって取得された値の変換を行うパーサーです
	///</summary>
    public class MapParser<TInputElements, T0,T1,T2,T3,T4,T5,T6,T7, TResult > : Parser<TInputElements, TResult>
    {
        private readonly Parser<TInputElements, T0> _p0;
        private readonly Parser<TInputElements, T1> _p1;
        private readonly Parser<TInputElements, T2> _p2;
        private readonly Parser<TInputElements, T3> _p3;
        private readonly Parser<TInputElements, T4> _p4;
        private readonly Parser<TInputElements, T5> _p5;
        private readonly Parser<TInputElements, T6> _p6;
        private readonly Parser<TInputElements, T7> _p7;
        private readonly Func<T0,T1,T2,T3,T4,T5,T6,T7, TResult> _mapper;

		///<summary>
		/// 変換操作を行うパーサーを構築します。
		///</summary>
        public MapParser(
			Parser<TInputElements, T0> p0, 
			Parser<TInputElements, T1> p1, 
			Parser<TInputElements, T2> p2, 
			Parser<TInputElements, T3> p3, 
			Parser<TInputElements, T4> p4, 
			Parser<TInputElements, T5> p5, 
			Parser<TInputElements, T6> p6, 
			Parser<TInputElements, T7> p7, 
			Func<T0,T1,T2,T3,T4,T5,T6,T7, TResult> mapper )
        {
            Contract.Requires(p0!=null);
            Contract.Requires(p1!=null);
            Contract.Requires(p2!=null);
            Contract.Requires(p3!=null);
            Contract.Requires(p4!=null);
            Contract.Requires(p5!=null);
            Contract.Requires(p6!=null);
            Contract.Requires(p7!=null);
            Contract.Requires(mapper!=null);
            _p0 = p0;
            _p1 = p1;
            _p2 = p2;
            _p3 = p3;
            _p4 = p4;
            _p5 = p5;
            _p6 = p6;
            _p7 = p7;
            _mapper = mapper;
        }

		///<summary>
		/// 入力をパースします。
		///</summary>
		///<remarks>
		/// コンストラクタで指定されたパーサーすべてが true を返すと変換操作が行われます。
		/// 配下のパーサーいずれかが false を返し失敗した場合、endInput は入力された index を返します。
		///</remarks>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TResult result)
        {
            result = default(TResult);
			endInput = index;
			int currentIndex = index;
            T0 v0;
            if(! _p0.Parse(input, currentIndex, out currentIndex, out v0) )
			{
				return false;
			}
            T1 v1;
            if(! _p1.Parse(input, currentIndex, out currentIndex, out v1) )
			{
				return false;
			}
            T2 v2;
            if(! _p2.Parse(input, currentIndex, out currentIndex, out v2) )
			{
				return false;
			}
            T3 v3;
            if(! _p3.Parse(input, currentIndex, out currentIndex, out v3) )
			{
				return false;
			}
            T4 v4;
            if(! _p4.Parse(input, currentIndex, out currentIndex, out v4) )
			{
				return false;
			}
            T5 v5;
            if(! _p5.Parse(input, currentIndex, out currentIndex, out v5) )
			{
				return false;
			}
            T6 v6;
            if(! _p6.Parse(input, currentIndex, out currentIndex, out v6) )
			{
				return false;
			}
            T7 v7;
            if(! _p7.Parse(input, currentIndex, out currentIndex, out v7) )
			{
				return false;
			}
			endInput = currentIndex;
			result = _mapper(v0,v1,v2,v3,v4,v5,v6,v7 );
            return true;
        }
    }

	public static partial class Parsers<TInputElements>
		where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
	{
		///<summary>
		/// 指定された各パーサーの実行結果を mapper で変換するパーサーを取得します。
		///</summary>
        public static Parser<TInputElements, TResult> Map<T0,T1,T2,T3,T4,T5,T6,T7, TResult>(
			Parser<TInputElements, T0> p0, 
			Parser<TInputElements, T1> p1, 
			Parser<TInputElements, T2> p2, 
			Parser<TInputElements, T3> p3, 
			Parser<TInputElements, T4> p4, 
			Parser<TInputElements, T5> p5, 
			Parser<TInputElements, T6> p6, 
			Parser<TInputElements, T7> p7, 
			Func<T0,T1,T2,T3,T4,T5,T6,T7, TResult> mapper)
        {
			Contract.Requires( p0!=null );
			Contract.Requires( p1!=null );
			Contract.Requires( p2!=null );
			Contract.Requires( p3!=null );
			Contract.Requires( p4!=null );
			Contract.Requires( p5!=null );
			Contract.Requires( p6!=null );
			Contract.Requires( p7!=null );
			Contract.Requires( mapper!=null );
			Parser<TInputElements, TResult> parser =
				new MapParser<TInputElements, T0,T1,T2,T3,T4,T5,T6,T7, TResult>(
					p0,p1,p2,p3,p4,p5,p6,p7, mapper);
			return EnableTrace 
				? WrapTracer(parser)
				: parser;
        }
	}


	///<summary>
	/// パーサーによって取得された値の変換を行うパーサーです
	///</summary>
    public class MapParser<TInputElements, T0,T1,T2,T3,T4,T5,T6,T7,T8, TResult > : Parser<TInputElements, TResult>
    {
        private readonly Parser<TInputElements, T0> _p0;
        private readonly Parser<TInputElements, T1> _p1;
        private readonly Parser<TInputElements, T2> _p2;
        private readonly Parser<TInputElements, T3> _p3;
        private readonly Parser<TInputElements, T4> _p4;
        private readonly Parser<TInputElements, T5> _p5;
        private readonly Parser<TInputElements, T6> _p6;
        private readonly Parser<TInputElements, T7> _p7;
        private readonly Parser<TInputElements, T8> _p8;
        private readonly Func<T0,T1,T2,T3,T4,T5,T6,T7,T8, TResult> _mapper;

		///<summary>
		/// 変換操作を行うパーサーを構築します。
		///</summary>
        public MapParser(
			Parser<TInputElements, T0> p0, 
			Parser<TInputElements, T1> p1, 
			Parser<TInputElements, T2> p2, 
			Parser<TInputElements, T3> p3, 
			Parser<TInputElements, T4> p4, 
			Parser<TInputElements, T5> p5, 
			Parser<TInputElements, T6> p6, 
			Parser<TInputElements, T7> p7, 
			Parser<TInputElements, T8> p8, 
			Func<T0,T1,T2,T3,T4,T5,T6,T7,T8, TResult> mapper )
        {
            Contract.Requires(p0!=null);
            Contract.Requires(p1!=null);
            Contract.Requires(p2!=null);
            Contract.Requires(p3!=null);
            Contract.Requires(p4!=null);
            Contract.Requires(p5!=null);
            Contract.Requires(p6!=null);
            Contract.Requires(p7!=null);
            Contract.Requires(p8!=null);
            Contract.Requires(mapper!=null);
            _p0 = p0;
            _p1 = p1;
            _p2 = p2;
            _p3 = p3;
            _p4 = p4;
            _p5 = p5;
            _p6 = p6;
            _p7 = p7;
            _p8 = p8;
            _mapper = mapper;
        }

		///<summary>
		/// 入力をパースします。
		///</summary>
		///<remarks>
		/// コンストラクタで指定されたパーサーすべてが true を返すと変換操作が行われます。
		/// 配下のパーサーいずれかが false を返し失敗した場合、endInput は入力された index を返します。
		///</remarks>
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TResult result)
        {
            result = default(TResult);
			endInput = index;
			int currentIndex = index;
            T0 v0;
            if(! _p0.Parse(input, currentIndex, out currentIndex, out v0) )
			{
				return false;
			}
            T1 v1;
            if(! _p1.Parse(input, currentIndex, out currentIndex, out v1) )
			{
				return false;
			}
            T2 v2;
            if(! _p2.Parse(input, currentIndex, out currentIndex, out v2) )
			{
				return false;
			}
            T3 v3;
            if(! _p3.Parse(input, currentIndex, out currentIndex, out v3) )
			{
				return false;
			}
            T4 v4;
            if(! _p4.Parse(input, currentIndex, out currentIndex, out v4) )
			{
				return false;
			}
            T5 v5;
            if(! _p5.Parse(input, currentIndex, out currentIndex, out v5) )
			{
				return false;
			}
            T6 v6;
            if(! _p6.Parse(input, currentIndex, out currentIndex, out v6) )
			{
				return false;
			}
            T7 v7;
            if(! _p7.Parse(input, currentIndex, out currentIndex, out v7) )
			{
				return false;
			}
            T8 v8;
            if(! _p8.Parse(input, currentIndex, out currentIndex, out v8) )
			{
				return false;
			}
			endInput = currentIndex;
			result = _mapper(v0,v1,v2,v3,v4,v5,v6,v7,v8 );
            return true;
        }
    }

	public static partial class Parsers<TInputElements>
		where TInputElements : IEquatable<TInputElements>, IComparable<TInputElements>
	{
		///<summary>
		/// 指定された各パーサーの実行結果を mapper で変換するパーサーを取得します。
		///</summary>
        public static Parser<TInputElements, TResult> Map<T0,T1,T2,T3,T4,T5,T6,T7,T8, TResult>(
			Parser<TInputElements, T0> p0, 
			Parser<TInputElements, T1> p1, 
			Parser<TInputElements, T2> p2, 
			Parser<TInputElements, T3> p3, 
			Parser<TInputElements, T4> p4, 
			Parser<TInputElements, T5> p5, 
			Parser<TInputElements, T6> p6, 
			Parser<TInputElements, T7> p7, 
			Parser<TInputElements, T8> p8, 
			Func<T0,T1,T2,T3,T4,T5,T6,T7,T8, TResult> mapper)
        {
			Contract.Requires( p0!=null );
			Contract.Requires( p1!=null );
			Contract.Requires( p2!=null );
			Contract.Requires( p3!=null );
			Contract.Requires( p4!=null );
			Contract.Requires( p5!=null );
			Contract.Requires( p6!=null );
			Contract.Requires( p7!=null );
			Contract.Requires( p8!=null );
			Contract.Requires( mapper!=null );
			Parser<TInputElements, TResult> parser =
				new MapParser<TInputElements, T0,T1,T2,T3,T4,T5,T6,T7,T8, TResult>(
					p0,p1,p2,p3,p4,p5,p6,p7,p8, mapper);
			return EnableTrace 
				? WrapTracer(parser)
				: parser;
        }
	}

}
