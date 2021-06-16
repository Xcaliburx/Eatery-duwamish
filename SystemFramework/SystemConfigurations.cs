using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemFramework
{
    public class SystemConfigurations
    {
        public static string EateryConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString; }
        }
        public static string EncryptionKey
        {
            get { return ConfigurationManager.AppSettings["EncryptionKey"]; }
        }
    }
}
