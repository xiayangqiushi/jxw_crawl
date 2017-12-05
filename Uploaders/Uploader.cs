using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using JXW.Crawl.DBModel;
using JXW.Crawl.Utils;
using Newtonsoft.Json;

namespace JXW.Crawl.Uploaders
{
    public abstract class Uploader
    {
        private const string CreateManuscriptIdUrl = @"http://202.61.89.178:8888/website-webapp/wcm/addManuscriptForTitle_manuscript.action";
        private const string CreateTopicUrl = @"http://202.61.89.178:8888/website-webapp/wcm/addManuscriptForTitle_manuscript.action";
        private const string CookieDomain = "202.61.89.178";

        private string GetQueryListUrl(string channelId)
        {
            return string.Format(@"http://202.61.89.178:8888/website-webapp/ajax_wcm/ajaxlist_editManuscript.action?channelFrame=true&channelId={0}&websiteId={1}&isLinkChannel=false",channelId, websiteId);
        }

        private const string SubmitUrl = @"http://202.61.89.178:8888/website-webapp/ajax_wcm/submitAjax_manuscript.action";

        //所有站点共有的Cookie变量
        private const string ucap_cate_nodepath = "12346789%2C2b3c597487e44047933ee6617b00e017";
        private const string JSESSIONID = "88E9B28250B7C0327AC72C17CC959AAE";
        private const string DWRSESSIONID = "rYlKElhN$sMSLfu$aV9PNM9qY$l";

        private const string userId = "ac2e2da4a8754dfb92012ab007ccba15";
        private const string UM_distinctid = "15fb4ba96679a9-00ee45c4641ae6-3e63430c-1fa400-15fb4ba9668702";
        private const string CNZZDATA1262703497 = "1039796044-1510565173-%7C1510565173";
        private const string rp = "50";
        private const string page = "1";
        private const string qtype = "title";
        private const string timeZoneId = "GMT%2B8%3A00";
        
        /*不同站点需要传入的Cookie变量*/
        private string websiteId;

        public abstract void Upload();
        
        protected Uploader(string _websiteId)
        {
            if (string.IsNullOrWhiteSpace(_websiteId)) throw new ArgumentNullException("websiteId need");
            this.websiteId = _websiteId;
        }

        private string ConsoleWriteLocal = "UploaderConsoleWriteLocal";
        protected void ThreadConsoleWrite(string output)
        {
            lock (ConsoleWriteLocal)
            {
                Console.Write(output);
            }
        }

        private CookieContainer GetCookie(string channelId)
        {
            CookieContainer result = new CookieContainer();
            result.Add(new Cookie("JSESSIONID", JSESSIONID, "/", CookieDomain));
            result.Add(new Cookie("UM_distinctid", UM_distinctid, "/", CookieDomain));
            result.Add(new Cookie("CNZZDATA1262703497", CNZZDATA1262703497, "/", CookieDomain));
            result.Add(new Cookie("rp", rp, "/", CookieDomain));
            result.Add(new Cookie("websiteId", websiteId, "/", CookieDomain));
            result.Add(new Cookie("page", page, "/", CookieDomain));
            result.Add(new Cookie("channelId", channelId, "/", CookieDomain));
            result.Add(new Cookie("userId", userId, "/", CookieDomain));
            result.Add(new Cookie("qtype", qtype, "/", CookieDomain));
            result.Add(new Cookie("timeZoneId", timeZoneId, "/", CookieDomain));
            result.Add(new Cookie("DWRSESSIONID", DWRSESSIONID, "/", CookieDomain));
            result.Add(new Cookie("ucap_cate_nodepath", ucap_cate_nodepath, "/", CookieDomain));
            return result;
        }

        private long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;
            return t;
        }

        protected void CreateTopic(string channelId, string title, string subtitle, string content,DateTime? publishDate, Dictionary<string, string> specifyParameters=null) 
        {
            if (string.IsNullOrWhiteSpace(channelId)) throw new ArgumentNullException("channelId need");

            if (!publishDate.HasValue) publishDate = DateTime.Now;

            #region Create Manuscript Id
            Dictionary<string, string> ManuParameters = new Dictionary<string, string>();
            ManuParameters.Add("manuscript.channelId", channelId);
            ManuParameters.Add("viewStatus", "3");
            ManuParameters.Add("isFromAddTitle", "true");
            string result_json = HttpUtil.HttpPost(CreateManuscriptIdUrl, ManuParameters, GetCookie(channelId));

            var jsonEntity = new { manuscriptId = string.Empty, url = string.Empty };
            var o2 = JsonConvert.DeserializeAnonymousType(result_json, jsonEntity);
            var manuscriptId = o2.manuscriptId;
            #endregion

            Dictionary<string, string> parameters = new Dictionary<string, string>();
           
            parameters.Add("isSignatureAuditSubmit", "0");
            parameters.Add("doPreviewValue", "0");
            parameters.Add("isClickRelatedRes", "0");
            parameters.Add("isTempSave", "false");
            parameters.Add("manuscript.manuscriptId", manuscriptId);
            parameters.Add("channel.manuscriptTypeId", "0");
            parameters.Add("manuscript.createdTime", ConvertDateTimeToInt(publishDate.Value).ToString());
            parameters.Add("manuscript.jcrResId", "067fb997e973407db97c9e577b49109c");
            parameters.Add("manuscript.jcrVer", "1.0");
            parameters.Add("manuscript.displayVer", "1.0");
            parameters.Add("hasInfoResEditPriv", "true");
            parameters.Add("channelId", channelId);
            parameters.Add("manuscript.seqNum", ConvertDateTimeToInt(publishDate.Value).ToString());
            parameters.Add("manuscript.isHasAuditComments", "0");
            parameters.Add("isCheckIn", "0");
            parameters.Add("maxFileSize_YUI", "524288000");
            parameters.Add("cstatus", "1");
            parameters.Add("editorType", "0");
            parameters.Add("manuscriptTypeId", "0");
            parameters.Add("isEdit", "true");
            parameters.Add("isCloseWindow", "false");
            parameters.Add("manuscript.title", title);
            parameters.Add("manuscript.subTitle", subtitle);
            parameters.Add("content", content);
            parameters.Add("totalNumber", "0");
            parameters.Add("manuscript.channelId", channelId);
            parameters.Add("publishedTime", publishDate.Value.ToString("yyyy-MM-dd 08:00:00"));
            parameters.Add("selectManuscripType", "0");
            parameters.Add("manuscript.urlRule", "0");
            parameters.Add("manuscript.isAllowComments", "0");

            if (specifyParameters != null)
            {
                foreach (string key in specifyParameters.Keys)
                {
                    if (parameters.ContainsKey(key))
                    {
                        parameters[key] = specifyParameters[key];
                    }
                    else
                    {
                        parameters.Add(key, specifyParameters[key]);
                    }
                }
            }

            var createResult = HttpUtil.MultipartHttpPost(CreateTopicUrl, parameters, GetCookie(channelId));
        }

        protected void ParallelCreateTopics(string channelId,IEnumerable<Topic> topics, Dictionary<string, string> specifyParameters = null)
        {
            Parallel.ForEach(topics, topic =>
            {
                CreateTopic(channelId, topic.title, string.Empty, topic.content, topic.publicDate, specifyParameters);
                ThreadConsoleWrite("*");
            });
            ThreadConsoleWrite(Environment.NewLine);
        }

    }
}