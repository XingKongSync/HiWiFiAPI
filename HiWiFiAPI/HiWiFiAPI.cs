using HiWiFiAPI.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XingKongUtils;

namespace HiWiFiAPI
{
    public class HiWiFiAPI
    {
        public static readonly string LOGIN_API_URL_FORMAT = "http://{0}/cgi-bin/turbo/api/login/login_admin?username=admin&password={1}";

        public static LoginInfo Login(string host, string password)
        {
            string url = string.Format(LOGIN_API_URL_FORMAT, host, password);
            string responseStr = HttpUtils.Get(url);
            LoginInfo loginInfo = JsonConvert.DeserializeObject<LoginInfo>(responseStr);
            return loginInfo;
        }
    }
}
