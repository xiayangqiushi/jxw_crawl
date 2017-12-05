using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace JXW.Crawl.Utils
{

    public static class HttpUtil
    {

        /// <summary>
        /// 过滤要下载请求
        /// </summary>
        /// <returns>返回true则表示需要下载</returns>
        public static bool UrlShouldDownload(string url)
        {
            if(string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            var lastDot_index = url.LastIndexOf('.');
            if (lastDot_index < 0)
            {
                return false;
            }

            var fileExtension = url.Substring(lastDot_index);
            if (fileExtension.ToLower().StartsWith(".html")
                    || fileExtension.ToLower().StartsWith(".htm")
                    || fileExtension.ToLower().StartsWith(".cn")
                    || fileExtension.ToLower().StartsWith(".com")
                    || fileExtension.ToLower().StartsWith(".asp")
                    || fileExtension.ToLower().StartsWith(".jsp")
                    || fileExtension.ToLower().StartsWith(".php")
                    || fileExtension.ToLower().StartsWith(".shtml"))
            {
                return false;
            }
            return true;
        }

        public static string HttpDownloadFile(string downloadUrl, string savePath)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(downloadUrl) as HttpWebRequest;
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            //创建本地文件写入流
            Stream stream = new FileStream(savePath, FileMode.Create);
            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
            }
            stream.Close();
            responseStream.Close();
            return savePath;
        }

        public static string HttpGet(string Url, Encoding encoding, IDictionary<string, string> parameters = null)
        {
            if (parameters != null && parameters.Count > 0)
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        i++;
                    }
                }

                if (Url.Contains("?"))
                {
                    Url += "&" + buffer.ToString();
                }
                else
                {
                    Url += "?" + buffer.ToString();
                }
            }


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            //request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, encoding);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

       public static  string HttpPost(string Url, IDictionary<string, string> parameters = null, CookieContainer cookie = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (cookie != null)
            {
                request.CookieContainer = cookie;
            }
            string postDataStr = string.Empty;
            //发送POST数据  
            if (parameters != null && parameters.Count > 0)
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        i++;
                    }
                }
                postDataStr = buffer.ToString();
            }
            request.ContentLength = Encoding.Default.GetByteCount(postDataStr);
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.Default);
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (cookie != null)
            {
                response.Cookies = cookie.GetCookies(response.ResponseUri);
            }

            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.Default);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

    }

}