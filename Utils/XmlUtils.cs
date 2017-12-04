using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace JXW.Crawl.Utils
{
    public static class XmlUtils
    {
        public static List<XmlTopic> LoadTopicListFromDirectory(string directoryPath)
        {
            List<XmlTopic> result = new List<XmlTopic>();
            var files = Directory.GetFiles(directoryPath, "*.xml");

            if (files == null || files.Count() == 0)
            {
                return result;
            }

            foreach (var pathXml in files)
            {
                string xmlContent = File.ReadAllText(pathXml);

                using (TextReader sr = (TextReader)new StringReader(xmlContent))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(XmlExtraction));
                    var tmp = serializer.Deserialize(sr) as XmlExtraction;
                    result.AddRange(tmp.TopicList);
                }
            }

            return result;
        }
    }

    [XmlRoot("extraction")]
    public class XmlExtraction
    {
        [XmlArrayItem("item")]
        public List<XmlTopic> TopicList { get; set; }
        
    }

    public class XmlTopic
    {
        [XmlElement("Title")]
        public string Title { get; set; }

        [XmlElement("Link")]
        public string Link { get; set; }

        [XmlElement("PublishDate")]
        public string PublishDate { get; set; }

        public string Classify { get; set; }
    }

}