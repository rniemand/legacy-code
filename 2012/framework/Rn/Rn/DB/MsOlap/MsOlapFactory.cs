using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rn.Configuration;

namespace Rn.DB.MsOlap
{
    public static class MsOlapFactory
    {
        public static string ConfigObjName { get; private set; }

        static MsOlapFactory()
        {
            if (String.IsNullOrEmpty(ConfigObjName)) SetConfigObjName();
        }


        public static void SetConfigObjName(string name = "default")
        {
            ConfigObjName = name;
        }

        public static void GetConnection(string name = "default")
        {
            var rNode = Configuration.ConfigObj.GetConfig(ConfigObjName).GetRootNode("DBConnections");
            const string xpath = "Connection[@Type='MSSQL' and @Enabled='true']";
            var nodes = rNode.SelectNodes(xpath);


            Console.WriteLine(nodes.Count);
        }
    }
}
