using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RnCore.Logging;

namespace RnCore.Utils
{
    public static class RnRandomString
    {
        private static readonly Random Rand;
        private static readonly List<char> Seed;

        static RnRandomString()
        {
            try
            {
                Rand = new Random();
                Seed = new List<char>();
                SetDefaultSeed();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }


        private static void SetDefaultSeed()
        {
            try
            {
                // Lower alpha numeric
                const string seed = "abcdefghijklmnopqrstuvwxyz";
                PushSeed(seed);
                PushSeed(seed.ToUpper());
                PushSeed("0123456789");
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private static void PushSeed(string seed)
        {
            try
            {
                foreach (var c in seed.ToCharArray().Where(c => !Seed.Contains(c)))
                    Seed.Add(c);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }



        public static void AddSeed(string seed)
        {
            try
            {
                PushSeed(seed);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public static string GenerateString(int length = 25)
        {
            var randomString = new StringBuilder();

            try
            {
                var seedLength = Seed.Count - 1;
                for (var i = 0; i < length; i++)
                    randomString.Append(Seed[Rand.Next(0, seedLength)]);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return randomString.ToString();
        }
    }
}
