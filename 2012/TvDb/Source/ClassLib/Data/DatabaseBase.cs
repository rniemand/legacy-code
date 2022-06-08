using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rn.TvDb.Data
{
    public abstract class DatabaseBase
    {

        public virtual void SaveSearchResult(TvdbSearchResult result)
        {
            throw new NotImplementedException("The method SaveSearchResult() has not been implemented!");
        }

    }
}
