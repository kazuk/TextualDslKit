TextualDslKit readme
=================================================================

TextualDslKit はテキストベースのDSLで必要となる構文解析機能その他を
まとめたプロジェクトになる予定の物です。

現状
-----

現状は ParserCombinator モジュールを書いており、基本的な構文解析は
パーサーコンビネータによりLL(n) の文法を受理可能です。

次の段階
-----

EBNF構文木を受理し、パーサーコンビネータを組立する処理系を記述する予定
です。

これにより、EBNFでの文法記述を元に任意のパーサーを組み立てる事が可能に
なります。

ParserCombinator
-----

Parser の抽象クラスは ParserCombinator.Parser として２引数のジェネリック
クラスになっています。

    public abstract class Parser<TInputElements, TOutput>

- TInputElements は入力要素の型です。通常の文字列をパースする場合にはcharを指定する事になるでしょう。
- TOutput はそのパーサーが出力する物です。これは構文上から拾い上げられた情報を保持するプリミティブ型、ないしは複合型になるでしょう。

Parserは以下の抽象メソッドを持ちます。

    public abstract bool Parse(
        IList<TInputElements> input, 
        int index, 
        out int endInput, 
        out TOutput result);

- input 引数は入力列を示す IList です。パーサーが受理するべきデータは index引数で示される位置から始まります。
- index 引数は入力列上で解釈すべき要素のインデックスです。index は input の終端以降を指し示す事が許されます。
- out endInput 引数はパーサーが解釈を行った後、出力を受理していない（別のパーサーで解釈が必要な）要素の位置を示します。
- out result 引数にはパーサーが解釈を行った結果としてパーサーの出力が設定されます。

Parseメソッドは戻り値として bool を返し、true の場合には result に有効な出力が設定されます。 false の場合には result は無意味です。
Parseメソッドが true / false いずれを返す場合にも endInput は index 以上を指し示さなければなりません。

### Parsers `<TInputElements>`

各種パーサーの実装は `Parsers<TInputElements>` のスタティックメソッドから取得できます。

#### Generic メソッド

predicate によって入力を受理するか判定し、受理すれば入力を返すパーサーを返します。

    public static Parser<TInputElements,TInputElements> 
        Generic(Func<TInputElements, bool> predicate)

example.

    using Lex = Parsers<char>;
    // 数字を受け入れるパーサーが構築されます
    var digitParser =
        Lex.Generic( (ch)=> char.IsDigit(ch) ); 

#### Some メソッド

elementParser が受理する入力の一回以上の繰り返しを受理するパーサーを返します。

    public static Parser<TInputElements, IList<TOutput>> 
        Some<TOutput>(
            Parser<TInputElements, TOutput> elementParser)

example.

    using Lex = Parsers<char>;
    // 空白文字の列を受理するパーサーが構築されます。
    var whiteSpecesParser =
       Lex.Some( Lex.Generic( (ch)=>char.IsWhiteSpeces(ch) ) );

#### IsIn メソッド

値の一覧のいずれかに属する入力を受け入れるパーサーを返します。

    public static Parser<TInputElements, TInputElements>
         IsIn(params TInputElements[] values)

example.

    using Lex = Parsers<char>;
    // + 又は - の文字を受け入れるパーサーが構築されます。
    var sign =
        Lex.IsIn( '+', '-' );

#### Range メソッド

begin , end の範囲内に存在する入力を受け入れるパーサーを返します。
begin または end に一致する入力は範囲内と判定されます。

    public static Parser<TInputElements, TInputElements> 
         Range(TInputElements begin, TInputElements end)

example.

    using Lex = Parsers<char>;
    // [A..Z] の文字を受け入れるパーサーが構築されます。
    var largeAlpha =
         Lex.Range('A', 'Z');

#### Map メソッド

一つ以上のパーサー出力をマップするパーサーが構築されます。

    public static Parser<TInputElements, TResult> Map<T0, TResult>(
        Parser<TInputElements, T0> p0, 
        Func<T0, TResult> mapper)

    public static Parser<TInputElements, TResult> Map<T0,T1, TResult>(
        Parser<TInputElements, T0> p0, 
        Parser<TInputElements, T1> p1, 
        Func<T0,T1, TResult> mapper)

 ...

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

p0 から pN までのすべてパーサーが入力を受理すると、mapper が呼び出され、全体として受理されます。

example.

	using Lex = Parsers<char>;
    // 数字を受け入れるパーサーが構築されます
    var digitParser =
        Lex.Generic( (ch)=> char.IsDigit(ch) ); 
    // 一つ以上の数字を受理し int 値として返すパーサーが構築されます。
    var intParser = Lex.Map( Lex.Some( digitParser ),
         (digits) => { 
              var s = new string( digits.ToArray() );
              return int.Parse(s);
         } );

#### Or メソッド

複数のパーサーで解釈を試行し、受理できたパーサーの結果を出力とするパーサーを返します。

    public static Parser<TInputElements, TOutput> Or<T0,T1, TOutput>(
        Parser<TInputElements, T0> p0, Func<T0, TOutput> apply0 , 
        Parser<TInputElements, T1> p1, Func<T1, TOutput> apply1  
	)

 ...

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

p0 から pN までのパーサーで順次入力を評価し、最初に受理したパーサーの出力を applyX で出力にマップします。
apply 関数を通す事により、各パーサーの出力型は異なっていても構いません。

example.

    // 二項の加算または減算を受理します
    var termParser = Lex.Or(
        Lex.Map( intParser, Lex.Is( '+' ), intParser ,
             (left,_,right) => Expression.Add( left, right ) ), _=>_ ,
        Lex.Map( intParser, Lex.Is( '-' ), intParser ,
             (left,_,_right) => Expression.Sub( left, right ) ), _=>_ );

