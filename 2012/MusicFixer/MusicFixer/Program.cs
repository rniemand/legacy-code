using System;
using HundredMilesSoftware.UltraID3Lib;
using MfLib.Database;
using MfLib.Providers.MusicBrainz;
using RnCore.Configuration;

namespace MusicFixer
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigFactory.RegisterConfig("MusicFixerConfig", "MusicFixer");
            ConfigFactory.CreateLoggers();
            MusicDb.RegisterDb(@"c:\bob.db");


            var ui3 = new UltraID3();
            const string file = @"C:\music\1.mp3";
            ui3.Read(file);


            Console.WriteLine(ui3.Artist);
            Console.WriteLine(ui3.Album);
            Console.WriteLine(ui3.Title);
            
            




            Console.WriteLine("All done");
            Console.ReadLine();
        }


        private static void PlayingWithNamedLookup()
        {
            var artist = new MbArtist("Breaking Benjamin");
            artist.Save();

            var albums = artist.GetReleases();

            foreach (MbRelease a in albums)
            {
                a.Save();
                Console.WriteLine("Looking up tracks for: {0}", a.Title);
                var tracks = a.GetRecordings();
                Console.WriteLine("    Found {0} tracks", tracks.Count);
                Console.WriteLine();
            }
        }

        private static void PlayingLookupInfoFromFileName()
        {
            var ui3 = new UltraID3();
            const string file = @"C:\music\1.mp3";
            ui3.Read(file);


            // (1) Get Artist Information
            var artist = new MbArtist(ui3.Artist);
            if (String.IsNullOrEmpty(artist.Id))
            {
                Console.WriteLine("Could not find the artist!");
                return;
            }
            artist.Save();

            // (2) Get release
            var release = artist.GetRelease(ui3.Album);
            if (String.IsNullOrEmpty(release.Id))
            {
                Console.WriteLine("Could not find the release");
                return;
            }
            release.Save();

            // (3) Get the Recordings
            var recordings = release.GetRecordings();
            if (recordings.Count > 0)
                foreach (var rec in recordings)
                    rec.Save();
        }


    }
}
