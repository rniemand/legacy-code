using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Massive;

namespace SnippyWeb.Snippy
{
    public static class SnippyDbHelper
    {

        public static dynamic UsersTable()
        {
            return new DynamicModel("SnippyDB", "tb_snippy_users", "userId");
        }

    }
}