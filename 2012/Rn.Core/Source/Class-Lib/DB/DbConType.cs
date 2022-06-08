using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rn.Core.DB
{
    public enum DbConType
    {
        MySql
    }

    public static class DbConTypeHelper
    {
        public static string AsString(this DbConType t)
        {
            switch (t)
            {
                case DbConType.MySql:
                    return "MySql";

                default:
                    return "Unknown";
            }
        }
    }
}
