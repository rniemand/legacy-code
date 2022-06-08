using System.IO;
using System.Xml;

namespace NzbScraper.Services
{
    public abstract class RssFileProcessorBase
    {
        public string FilePath { get; private set; }
        private readonly XmlDocument _xmlDoc;

        protected RssFileProcessorBase(string file)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException("The given RSS file could not be found", file);
            }

            FilePath = file;
            _xmlDoc = new XmlDocument();
            _xmlDoc.Load(FilePath);
        }

        public XmlNodeList GetItemNodes()
        {
            return _xmlDoc.SelectNodes("/rss/channel/item");
        }
    }
}
