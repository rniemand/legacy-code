using System;
using System.Globalization;
using System.IO;
using RnCore.Config;
using RnCore.Helpers;
using RnCore.Logging;
using RnCore.Web.Rss;

namespace RnRssWebappDev
{
    class Program
    {
        static void Main(string[] args)
        {

            //RunFeedTest("http://rniemand.com/site/feed/");
            //RunFeedTest("http://androidandme.com/feed/");
            //RunFeedTest("http://notalwaysright.com/feed");

            RnConfig.Instance.RegisterLoggers();
            RnLogger.LogInfo("Hello world");


            Console.WriteLine();
            Console.WriteLine("=================================");
            Console.WriteLine("... All done ...");
            Console.WriteLine("=================================");
            Console.ReadLine();
        }

        private static void RunFeedTest(string url)
        {
            // http://www.c-sharpcorner.com/rss/latestarticles.aspx?SubSectionId=149&Type=articles

            /*
             * http://feed2.w3.org/docs/rss2.html#sampleFiles
             * (0.91) http://static.userland.com/gems/backend/sampleRss.xml
             * (0.92) http://static.userland.com/gems/backend/gratefulDead.xml
             * (2.00) http://static.userland.com/gems/backend/rssTwoExample2.xml
             * (2.00) http://rniemand.com/site/feed/
             */

            //var feed = new RssFeed(GetFeedXml("http://rniemand.com/site/feed/"));
            var feed = new RssFeed(url);
            DumpFeedInfo(feed);
        }

        private static string GetFeedXml(string url)
        {
            var rawXml = "";
            var uri = new Uri(url);
            var fName = String.Format("{0}{1}.{2}.xml", @"c:\Rn-Playing\RnReader\", uri.Host, uri.Port);
            
            try
            {
                if (!File.Exists(fName))
                {
                    rawXml = RnWeb.GetUrlAsString(uri.ToString());
                    RnIO.WriteFile(fName, rawXml, true, true);
                }
                else
                {
                    rawXml = RnIO.ReadFileAsString(fName);
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return rawXml;
        }

        private static void DumpFeedInfo(RssFeed feed)
        {
            Console.WriteLine("RSS Version: {0}", feed.RssVersion);
            Console.WriteLine("");
            Console.WriteLine("=======================================================");
            Console.WriteLine("             [[[[[ CHANNEL INFO ]]]]]");
            Console.WriteLine("=======================================================");
            Console.WriteLine("Title          : {0}", feed.Channel.Title);
            Console.WriteLine("URL            : {0}", feed.Channel.Link);
            Console.WriteLine("Desc           : {0}", feed.Channel.Description);
            Console.WriteLine("Lang           : {0}", feed.Channel.Language);
            Console.WriteLine("Copy           : {0}", feed.Channel.Copyright);
            Console.WriteLine("ManagingEditor : {0}", feed.Channel.ManagingEditor);
            Console.WriteLine("WebMaster      : {0}", feed.Channel.WebMaster);

            if (feed.Channel.Image.ImageSet)
            {
                Console.WriteLine("");
                Console.WriteLine("---------------------------------");
                Console.WriteLine("         [[[[ IMAGE ]]]] ");
                Console.WriteLine("---------------------------------");
                Console.WriteLine(" Title      : {0}", feed.Channel.Image.Title);
                Console.WriteLine(" Url        : {0}", feed.Channel.Image.Url);
                Console.WriteLine(" Width      : {0}", feed.Channel.Image.Width);
                Console.WriteLine(" Height     : {0}", feed.Channel.Image.Height);
                Console.WriteLine(" Link       : {0}", feed.Channel.Image.Link);
                Console.WriteLine(" Description: {0}", feed.Channel.Image.Description);
            }

            Console.WriteLine();
            Console.WriteLine("=======================================================");
            Console.WriteLine("            [[[[[ CHANNEL ITEMS ]]]]]");
            Console.WriteLine("=======================================================");
            var itemCount = 1;

            foreach (RssFeedItem i in feed.Items)
            {
                Console.WriteLine(" {0}: {1}", itemCount.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'), i.Title);
                itemCount++;
            }
        }

    }

}



/*
<?xml version="1.0" encoding="UTF-8"?>
<rss version="2.0"
	xmlns:content="http://purl.org/rss/1.0/modules/content/"
	xmlns:wfw="http://wellformedweb.org/CommentAPI/"
	xmlns:dc="http://purl.org/dc/elements/1.1/"
	xmlns:atom="http://www.w3.org/2005/Atom"
	xmlns:sy="http://purl.org/rss/1.0/modules/syndication/"
	xmlns:slash="http://purl.org/rss/1.0/modules/slash/"
	>

	<channel>
		<item>
			<title>IIS bad module &#8220;ManagedPipelineHandler&#8221;</title>
			<link>http://rniemand.com/site/2013/05/556/iis-bad-module-managedpipelinehandler/?utm_source=rss&#038;utm_medium=rss&#038;utm_campaign=iis-bad-module-managedpipelinehandler</link>
			<comments>http://rniemand.com/site/2013/05/556/iis-bad-module-managedpipelinehandler/#comments</comments>
			<pubDate>Fri, 31 May 2013 18:47:20 +0000</pubDate>
			<dc:creator>richardn</dc:creator>
			<category><![CDATA[Uncategorized]]></category>
			<guid isPermaLink="false">http://rniemand.com/site/?p=556</guid>
			<description></description>
			<wfw:commentRss>http://rniemand.com/site/2013/05/556/iis-bad-module-managedpipelinehandler/feed/</wfw:commentRss>
			<slash:comments>0</slash:comments>
		</item>
		<item>
			<title>SVN + C# = Automated Backups! (Part 2)</title>
			<link>http://rniemand.com/site/2013/05/426/svn-c-automated-backups-part-2/?utm_source=rss&#038;utm_medium=rss&#038;utm_campaign=svn-c-automated-backups-part-2</link>
			<comments>http://rniemand.com/site/2013/05/426/svn-c-automated-backups-part-2/#comments</comments>
			<pubDate>Thu, 23 May 2013 19:28:40 +0000</pubDate>
			<dc:creator>richardn</dc:creator>
			<category><![CDATA[C#]]></category>
			<guid isPermaLink="false">http://rniemand.com/site/?p=426</guid>
			<description></description>
			<wfw:commentRss>http://rniemand.com/site/2013/05/426/svn-c-automated-backups-part-2/feed/</wfw:commentRss>
			<slash:comments>0</slash:comments>
		</item>
		<item>
			<title>SVN + C# = Automated Backups!</title>
			<link>http://rniemand.com/site/2013/05/404/svn-c-automated-backups/?utm_source=rss&#038;utm_medium=rss&#038;utm_campaign=svn-c-automated-backups</link>
			<comments>http://rniemand.com/site/2013/05/404/svn-c-automated-backups/#comments</comments>
			<pubDate>Tue, 21 May 2013 19:48:04 +0000</pubDate>
			<dc:creator>richardn</dc:creator>
			<category><![CDATA[C#]]></category>
			<guid isPermaLink="false">http://rniemand.com/site/?p=404</guid>
			<description></description>
			<wfw:commentRss>http://rniemand.com/site/2013/05/404/svn-c-automated-backups/feed/</wfw:commentRss>
			<slash:comments>0</slash:comments>
		</item>
	</channel>
</rss>
*/