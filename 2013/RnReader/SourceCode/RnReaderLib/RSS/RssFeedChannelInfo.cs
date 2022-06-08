using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RnReaderLib.RSS
{
    public class RssFeedChannelInfo
    {
        public Version RssVersion { get; private set; }
        public string Title { get; private set; }
        public string Link { get; private set; }
        public string Description { get; private set; }
        public string Language { get; private set; }
        public string Copyright { get; private set; }
        public string ManagingEditor { get; private set; }
        public string WebMaster { get; private set; }

        /*
		                <image>
			                <title>WriteTheWeb</title> 
			                <url>http://writetheweb.com/images/mynetscape88.gif</url> 
			                <link>http://writetheweb.com</link> 
			                <width>88</width> 
			                <height>31</height> 
			                <description>News for web users that write back</description> 
			            </image>
                 */


        public RssFeedChannelInfo(XmlNode n, Version feedVersion)
        {
            try
            {
                RssVersion = feedVersion;
                Title = n.GetInnerText("title");
                Link = n.GetInnerText("link");
                Description = n.GetInnerText("description");
                Language = n.GetInnerText("language");
                Copyright = n.GetInnerText("copyright");
                ManagingEditor = n.GetInnerText("managingEditor");
                WebMaster = n.GetInnerText("webMaster");

                /*
		                <image>
			                <title>WriteTheWeb</title> 
			                <url>http://writetheweb.com/images/mynetscape88.gif</url> 
			                <link>http://writetheweb.com</link> 
			                <width>88</width> 
			                <height>31</height> 
			                <description>News for web users that write back</description> 
			            </image>
                 */
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
    }
}
