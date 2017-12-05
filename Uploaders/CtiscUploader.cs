using System;
using System.IO;
using System.Linq;
using System.Text;
using JXW.Crawl.DBModel;

namespace JXW.Crawl.Uploaders
{
    public class CtiscUploader: Uploader
    {
        /// <summary>
        /// 技术创新在CMS的站点标识cookie
        /// </summary>
        private const string websiteId = "1723c8c5a281423487c85898bc6db29d";
        /// <summary>
        /// 重要通知
        /// </summary>
        private const string ChannelId_zytz = "734b9bcb74044f85855c52a39beda400";
        /// <summary>
        /// 新闻快讯
        /// </summary>
        private const string ChannelId_xwkx = "c2a02f78e6f24e73853db23f77a5ca83";
        /// <summary>
        /// 技术中心
        /// </summary>
        private const string ChannelId_jszx = "f3b05db7443047e58a99706f9e7bc4b6";
        /// <summary>
        /// 科技成果
        /// </summary>
        private const string ChannelId_kjcg = "02fb394a5929489b8cecfd2e5b66502e";
        /// <summary>
        /// 政策法规
        /// </summary>
        private const string ChannelId_zcfg = "b625c56181fc4c3bb9783e73686839aa";
        /// <summary>
        /// 新产品新技术新工艺
        /// </summary>
        private const string ChannelId_xcpxjsxgy = "32950b2d2c454783b0f2ca2bb6f3623c";
        /// <summary>
        /// 首页
        /// </summary>
        private const string ChannelId_sy = "55d816c7011343f0a5b876f88289105c";

        public CtiscUploader() : base(websiteId){ }

        override
        public void Upload()
        {
            using (CtiscContext db = new CtiscContext())
            {
                Console.WriteLine("开始上传重要通知");
                var topics_zytz = (from t in db.Topics where t.classify == "重要通知" select t).ToList();
                ParallelCreateTopics(ChannelId_zytz, topics_zytz);
                
                Console.WriteLine("开始上传新闻快讯");
                var topics_xwkx = (from t in db.Topics where t.classify == "新闻快讯" select t).ToList();
                ParallelCreateTopics(ChannelId_xwkx, topics_xwkx);
                
                Console.WriteLine("开始上传技术中心");
                var topics_jszx = (from t in db.Topics where t.classify == "技术中心" select t).ToList();
                ParallelCreateTopics(ChannelId_jszx, topics_jszx);

                Console.WriteLine("开始上传科技成果");
                var topics_kjcg = (from t in db.Topics where t.classify == "科技成果" select t).ToList();
                ParallelCreateTopics(ChannelId_kjcg, topics_kjcg);
                
                Console.WriteLine("开始上传政策法规");
                var topics_zcfg = (from t in db.Topics where t.classify == "政策法规" select t).ToList();
                ParallelCreateTopics(ChannelId_zcfg, topics_zcfg);
                
                Console.WriteLine("开始上传新产品新技术新工艺");
                var topics_xcpxjsxgy = (from t in db.Topics where t.classify == "新产品新技术新工艺" select t).ToList();
                ParallelCreateTopics(ChannelId_xcpxjsxgy, topics_xcpxjsxgy);
            }
        }
    }
}