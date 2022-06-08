using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rn.TvDb.Data
{
    public class MySqlAdapter : DatabaseBase
    {


        public override void SaveSearchResult(TvdbSearchResult result)
        {
            /*
            INSERT INTO tb_series
             (seriesId, id, language, seriesName, imdbId, zap2itId, banner, overview)
            VALUES
             (seriesId, id, language, seriesName, imdbId, zap2itId, banner, overview)
            */
        }

    }
}
