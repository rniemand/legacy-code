using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rn.TvDb
{
    public static class TvDbHelper
    {
        public static string PadNumber(this int n, int padBy = 2, char padWith = '0')
        {
            return n.ToString().PadLeft(padBy, padWith);
        }
    }
}
