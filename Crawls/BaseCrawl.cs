using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AngleSharp.Dom;
using JXW.Crawl.DBModel;
using JXW.Crawl.Utils;

namespace JXW.Crawl.Crawls
{
    public abstract class BaseCrawl
    {
        protected readonly string WebBaseUrl;
        protected readonly string AttachmentSaveDic;

        public BaseCrawl(string _WebBaseUrl,string _AttachmentSaveDic)
        {
            WebBaseUrl = _WebBaseUrl;
            AttachmentSaveDic = _AttachmentSaveDic;
        }

        protected void GetAttachment(IElement contentDom,Topic t)
        {
            List<Attachment> atts = new List<Attachment>();
            var aTags = contentDom.QuerySelectorAll("a").ToList();
            var imgTags = contentDom.QuerySelectorAll("img").ToList();

            //创建附件本地存放目录
            var fileDirectory = AttachmentSaveDic + t.classify;
            
            /*解析a标签,替换文件相对路径为绝对路径*/
            foreach (var a in aTags)
            {
                Attachment att = new Attachment();
                var href = a.GetAttribute("href");
                if (HttpUtil.UrlShouldDownload(href) == false)
                {
                    continue;
                }

                var fileExtension = href.Substring(href.LastIndexOf('.'));
                att.filePath = Path.Combine(fileDirectory, Guid.NewGuid().ToString("N") + fileExtension);
                att.fileName = a.TextContent.Trim();
                if (href.StartsWith("http"))
                {
                    att.sourceUrl = href;
                }
                else
                {
                    att.sourceUrl = href.Substring(0, 1).Equals("/") ? WebBaseUrl + href : WebBaseUrl + "/" + href;
                    t.content = t.content.Replace("href=\"" + href, "href=\"" + att.sourceUrl);
                }
                atts.Add(att);
            }

            /*解析img标签,替换文件相对路径为绝对路径*/
            foreach (var img in imgTags)
            {
                Attachment att = new Attachment();
                var src = img.GetAttribute("src");
                if (HttpUtil.UrlShouldDownload(src) == false)
                {
                    continue;
                }

                var fileExtension = src.Substring(src.LastIndexOf('.'));
                att.filePath = Path.Combine(fileDirectory, Guid.NewGuid().ToString("N") + fileExtension);
                att.fileName = "img file";
                if (src.StartsWith("http"))
                {
                    att.sourceUrl = src;
                }
                else
                {
                    att.sourceUrl = src.Substring(0, 1).Equals("/") ? WebBaseUrl + src : WebBaseUrl + "/" + src;
                    t.content = t.content.Replace("src=\"" + src, "src=\"" + att.sourceUrl);
                }
                atts.Add(att);
            }

            /*下载附件到本地*/
            foreach (var att in atts)
            {
                try
                {
                    HttpUtil.HttpDownloadFile(att.sourceUrl, att.filePath);
                    att.downLoadFaild = 0;
                }
                catch
                {
                    //标识下载失败
                    att.downLoadFaild = 1;
                }

                t.attachments.Add(att);
            }
        }
    }
}