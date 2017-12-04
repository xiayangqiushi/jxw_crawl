using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using AngleSharp.Parser.Html;
using JXW.Crawl.DBModel;
using JXW.Crawl.Utils;

namespace JXW.Crawl.Crawls
{
    /// <summary>
    /// 技术创新爬虫
    /// </summary>
    public class CtiscCrawl
    {
        /// <summary>
        /// 技术创新站点url
        /// </summary>
        private const string CTISC_URL = @"http://www.ctisc.com.cn";

        /// <summary>
        /// 重要通知本地目录
        /// </summary>
        private const string PATH_ZYTZ = @"C:\DataScraperWorks\技术创新_重要通知";

        /// <summary>
        /// 新闻快讯本地目录
        /// </summary>
        private const string PATH_XWKX = @"C:\DataScraperWorks\技术创新_新闻快讯";

        /// <summary>
        /// 技术中心本地目录
        /// </summary>
        private const string PATH_JSZX = @"C:\DataScraperWorks\技术创新_技术中心";

        /// <summary>
        /// 科技成果本地目录
        /// </summary>
        private const string PATH_KJCG = @"C:\DataScraperWorks\技术创新_科技成果";

        /// <summary>
        /// 政策法规本地目录
        /// </summary>
        private const string PATH_ZCFG = @"C:\DataScraperWorks\技术创新_政策法规";

        /// <summary>
        /// 新产品新技术新工艺本地目录
        /// </summary>
        private const string PATH_XCPXJSXGY = @"C:\DataScraperWorks\技术创新_新产品新技术新工艺";

        private string GetClassifyByPATH(string xmlPath)
        {
            switch (xmlPath)
            {
                case PATH_ZYTZ:
                    return "重要通知";
                case PATH_XWKX:
                    return "新闻快讯";
                case PATH_JSZX:
                    return "技术中心";
                case PATH_KJCG:
                    return "科技成果";
                case PATH_ZCFG:
                    return "政策法规";
                case PATH_XCPXJSXGY:
                    return "新产品新技术新工艺";
                default:
                    return string.Empty;
            }
        }

        private void Crawl(string xmlPath)
        {
            List<XmlTopic> list = XmlUtils.LoadTopicListFromDirectory(xmlPath);

            Parallel.ForEach(list, topicXml =>
            {
                topicXml.Classify = GetClassifyByPATH(xmlPath);
                var title = topicXml.Title.Trim('\n').Trim();
                CrawlPageFormXmlTopic(topicXml);
                ThreadConsoleWrite("*");
            });
        }

        private string ConsoleWriteLocal = "ConsoleWriteLocal";
        protected void ThreadConsoleWrite(string output)
        {
            lock (ConsoleWriteLocal)
            {
                Console.Write(output);
            }
        }

        public void Crawl()
        {
            Console.WriteLine("开始爬取 重要通知");
            Crawl(PATH_ZYTZ);
            Console.WriteLine();

            Console.WriteLine("开始爬取 新闻快讯");
            Crawl(PATH_XWKX);
            Console.WriteLine();

            Console.WriteLine("开始爬取 技术中心");
            Crawl(PATH_JSZX);
            Console.WriteLine();

            Console.WriteLine("开始爬取 科技成果");
            Crawl(PATH_KJCG);
            Console.WriteLine();

            Console.WriteLine("开始爬取 政策法规");
            Crawl(PATH_ZCFG);
            Console.WriteLine();

            Console.WriteLine("开始爬取 新产品新技术新工艺");
            Crawl(PATH_XCPXJSXGY);
            Console.WriteLine();
        }

        private void CrawlPageFormXmlTopic(XmlTopic xmltopic)
        {
            if (string.IsNullOrWhiteSpace(xmltopic.Classify))
            {
                throw new ArgumentNullException("Classify is Requid");
            }
            var targetUrl = xmltopic.Link.Substring(0, 1).Equals("/") ? CTISC_URL + xmltopic.Link : CTISC_URL + "/" + xmltopic.Link;
            var pageHtmlStr = HttpUtil.HttpGet(targetUrl, Encoding.GetEncoding("GB2312"));
            
            var document = new HtmlParser().Parse(pageHtmlStr);
            
            Topic t = new Topic();
            t.classify = xmltopic.Classify;
            t.title = xmltopic.Title;
            DateTime publicDate;
            if(DateTime.TryParse(xmltopic.PublishDate,out publicDate))
            {
                t.publicDate  = publicDate;
            }

            var contentDom = document.QuerySelector(".mT10.mB10 .f14-h");

            /*获取文章内容html*/
            t.content = contentDom.InnerHtml;

            
        }

    }
}