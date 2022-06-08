using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RnReaderLib;
using RnReaderLib.RSS;

namespace LibTesterConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            // http://www.c-sharpcorner.com/rss/latestarticles.aspx?SubSectionId=149&Type=articles

            /*
             <?xml version="1.0" encoding="UTF-8" ?>
                <rss version="2.0">
                <channel>
                 <title>RSS Title</title>
                 <description>This is an example of an RSS feed</description>
                 <link>http://www.someexamplerssdomain.com/main.html</link>
                 <lastBuildDate>Mon, 06 Sep 2010 00:01:00 +0000 </lastBuildDate>
                 <pubDate>Mon, 06 Sep 2009 16:45:00 +0000 </pubDate>
                 <ttl>1800</ttl>
 
                 <item>
                  <title>Example entry</title>
                  <description>Here is some text containing an interesting description.</description>
                  <link>http://www.wikipedia.org/</link>
                  <guid>unique string per item</guid>
                  <pubDate>Mon, 06 Sep 2009 16:45:00 +0000 </pubDate>
                 </item>
 
                </channel>
                </rss>
             * 
             * http://feed2.w3.org/docs/rss2.html#sampleFiles
             * (0.91) http://static.userland.com/gems/backend/sampleRss.xml
             * (0.92) http://static.userland.com/gems/backend/gratefulDead.xml
             * (2.00) http://static.userland.com/gems/backend/rssTwoExample2.xml
             * 
             */

            const string xmlFilePath = @"c:\Rn-Playing\RnReader\xml-0-91.xml";
            //var rss091Contents = RnWeb.GetUrlAsString("http://static.userland.com/gems/backend/sampleRss.xml");
            //RnIO.WriteFile(xmlFilePath, rss091Contents, true, true);
            var rss091Contents = RnIO.ReadFileAsString(xmlFilePath);

            var feed = new RssFeed(rss091Contents, true);


            Console.WriteLine("RSS Version: {0}", feed.RssVersion);

            Console.WriteLine("");
            Console.WriteLine("=======================================================");
            Console.WriteLine("             [[[[[ CHANNEL INFO ]]]]]");
            Console.WriteLine("=======================================================");
            Console.WriteLine("\tTitle          : {0}", feed.Channel.Title);
            Console.WriteLine("\tURL            : {0}", feed.Channel.Link);
            Console.WriteLine("\tDesc           : {0}", feed.Channel.Description);
            Console.WriteLine("\tLang           : {0}", feed.Channel.Language);
            Console.WriteLine("\tCopy           : {0}", feed.Channel.Copyright);
            Console.WriteLine("\tManagingEditor : {0}", feed.Channel.ManagingEditor);
            Console.WriteLine("\tWebMaster      : {0}", feed.Channel.WebMaster);



            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("=======================================");
            Console.WriteLine("= All Done");
            Console.WriteLine("=======================================");
            Console.ReadLine();
        }
    }
}
