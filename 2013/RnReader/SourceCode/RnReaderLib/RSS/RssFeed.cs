using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RnReaderLib.RSS
{
    public class RssFeed
    {
        public Version RssVersion { get; private set; }
        public RssFeedChannelInfo Channel { get; private set; }

        private XmlDocument _feedXml;

        

        #region [Constructors] RssFeed Constructor Methods / Overloads
        public RssFeed(Uri feedUrl)
        {
            // holder
        }

        public RssFeed(string dataOrUrl, bool isRawXml = false)
        {
            try
            {
                _feedXml = new XmlDocument();

                if (isRawXml)
                    _feedXml.LoadXml(dataOrUrl);

                ProcessRssFeed();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public RssFeed(XmlDocument data)
        {
            // holder
        } 
        #endregion

        // Startup
        private void ProcessRssFeed()
        {
            try
            {
                if (_feedXml == null)
                {
                    RnLogger.LogError("The XML Document is empty");
                    return;
                }

                SetRssVersion();
                SetChannelInfo();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void SetRssVersion()
        {
            try
            {
                var n = _feedXml.SelectSingleNode("/rss/@version");
                if (n != null)
                    RssVersion = new Version(n.Value);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void SetChannelInfo()
        {
            try
            {
                var channelNode = _feedXml.SelectSingleNode("/rss/channel");
                if (channelNode == null)
                {
                    RnLogger.LogError("Could not find the /rss/channel node!");
                    return;
                }

                Channel = new RssFeedChannelInfo(channelNode, RssVersion);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }


        /*
         <rss version="0.91">
	        <channel>
		        <title>WriteTheWeb</title> 
		        <link>http://writetheweb.com</link> 
		        <description>News for web users that write back</description> 
		        <language>en-us</language> 
		        <copyright>Copyright 2000, WriteTheWeb team.</copyright> 
		        <managingEditor>editor@writetheweb.com</managingEditor> 
		        <webMaster>webmaster@writetheweb.com</webMaster> 
		        <image>
			        <title>WriteTheWeb</title> 
			        <url>http://writetheweb.com/images/mynetscape88.gif</url> 
			        <link>http://writetheweb.com</link> 
			        <width>88</width> 
			        <height>31</height> 
			        <description>News for web users that write back</description> 
			    </image>
		        <item>
			        <title>Giving the world a pluggable Gnutella</title> 
			        <link>http://writetheweb.com/read.php?item=24</link> 
			        <description>WorldOS is a framework on which to build programs that work like Freenet or Gnutella -allowing distributed applications using peer-to-peer routing.</description> 
			        </item>
		        <item>
			        <title>Syndication discussions hot up</title> 
			        <link>http://writetheweb.com/read.php?item=23</link> 
			        <description>After a period of dormancy, the Syndication mailing list has become active again, with contributions from leaders in traditional media and Web syndication.</description> 
			        </item>
		        <item>
			        <title>Personal web server integrates file sharing and messaging</title> 
			        <link>http://writetheweb.com/read.php?item=22</link> 
			        <description>The Magi Project is an innovative project to create a combined personal web server and messaging system that enables the sharing and synchronization of information across desktop, laptop and palmtop devices.</description> 
			        </item>
		        <item>
			        <title>Syndication and Metadata</title> 
			        <link>http://writetheweb.com/read.php?item=21</link> 
			        <description>RSS is probably the best known metadata format around. RDF is probably one of the least understood. In this essay, published on my O'Reilly Network weblog, I argue that the next generation of RSS should be based on RDF.</description> 
			        </item>
		        <item>
			        <title>UK bloggers get organised</title> 
			        <link>http://writetheweb.com/read.php?item=20</link> 
			        <description>Looks like the weblogs scene is gathering pace beyond the shores of the US. There's now a UK-specific page on weblogs.com, and a mailing list at egroups.</description> 
			        </item>
		        <item>
			        <title>Yournamehere.com more important than anything</title> 
			        <link>http://writetheweb.com/read.php?item=19</link> 
			        <description>Whatever you're publishing on the web, your site name is the most valuable asset you have, according to Carl Steadman.</description> 
			        </item>
		        </channel>
	        </rss>
         */

    }
}
