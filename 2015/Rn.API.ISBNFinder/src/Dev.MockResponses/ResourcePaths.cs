using System;
using Rn.API.ISBNFinder.Enums;

namespace RnDev.ISBNFinder.MockResponses
{
    public class ResourcePaths
    {
        public static string BasePath { get; set; }

        static ResourcePaths()
        {
            BasePath = "./API-Responses/";
        }

        public static void SetBasePath(string path)
        {
            BasePath = path;
        }

        public static string GetCachePath(LookupApi api)
        {
            var name = Enum.GetName(typeof (LookupApi), api);

            if (string.IsNullOrWhiteSpace(name))
            {
                // todo: get a better story here
                throw new Exception("Poo");
            }

            return string.Format("{0}{1}/", BasePath, name.ToLower());
        }
    }
}
