using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ParserCombinator
{
    [ContractClassFor(typeof(Parser<,>))]
    abstract class ParserContract<TInputElements, TOutput> : Parser<TInputElements,TOutput>
    {
        public override bool Parse(IList<TInputElements> input, int index, out int endInput, out TOutput result)
        {
            // input must be not null
            Contract.Requires(input!=null);
            // index must be plus or 0
            Contract.Requires(index>=0);
            // result endInput must be grater then index
            Contract.Ensures( Contract.ValueAtReturn(out endInput)>=index );
            throw new NotImplementedException();
        }
    }
}