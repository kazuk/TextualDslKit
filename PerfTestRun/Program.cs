using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParserBuilder;

namespace PerfTestRun
{
    class Program
    {
        static void Main(string[] args)
        {
            EbnfParser parser = new EbnfParser();
            for (int i = 0; i < 100000; i++)
            {
                Node result;
                parser.TryParseExpression("a:first a:next a:last { this is code }", out result);
            }
        }
    }
}
