using System.IO;
using Microsoft.Extensions.Configuration;

namespace JXW.Crawl.Utils
{
    public static class ConfigUtil
    {
        private static IConfigurationRoot _ConfigRoot;
        public static IConfigurationRoot ConfigRoot
        {
            get
            {
                if(_ConfigRoot==null)
                {
                    _ConfigRoot= new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json",true,true).Build();
                }
                return _ConfigRoot;
            }
        }

        public static string CtiscConnection
        {
            get
            {
                return ConfigRoot["ConnectionStrings:CtiscConnectionString"];
            }
        }

        public static string ScdqbConnection
        {
            get
            {
                return ConfigRoot["ConnectionStrings:ScdqbConnectionString"];
            }
        }

        public static string SczzwConnection
        {
            get
            {
                return ConfigRoot["ConnectionStrings:SczzwConnectionString"];
            }
        }

    }
}