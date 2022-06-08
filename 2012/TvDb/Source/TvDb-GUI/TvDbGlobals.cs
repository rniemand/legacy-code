using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rn.TvDb;
using Rn.TvDb.Data;

namespace TvDvGui
{
    public static class TvDbGlobals
    {
        private static DatabaseBase _adapter;
        public static TvDbConfig TvDbCfg { get; set; }
        public static TVDB TVDB { get; set; }

        public static DatabaseBase GetDbAdapter()
        {
            if (_adapter != null) return _adapter;
            
            _adapter = new MySqlAdapter();
            return _adapter;
        }
    }
}
