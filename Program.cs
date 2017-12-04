using System;
using System.Linq;
using System.Text;
using JXW.Crawl.Crawls;
using JXW.Crawl.DBModel;

namespace JXW.Crawl
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("开始导入");
            CtiscCrawl cc = new CtiscCrawl();
            cc.Crawl();
        }
    }
}
