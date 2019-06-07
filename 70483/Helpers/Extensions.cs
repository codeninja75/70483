using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace DotNet.E70483.Helpers
{
    public static class Extention
    {
        public static string PrettyPrint(this IEnumerable<int> x)
        {
            return string.Join(",", x.ToArray());
        }
    }
}
