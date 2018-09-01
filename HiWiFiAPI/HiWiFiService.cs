using HiWiFiAPI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWiFiAPI
{
    public class HiWiFiService
    {
        private string _ip;
        private string _password;
        private LoginInfoEx _loginInfo;

        public HiWiFiService(string ip, string password)
        {
            _ip = ip;
            _password = password;

            Login();
        }

        private void Login()
        {
            if (string.IsNullOrWhiteSpace(_ip) || string.IsNullOrEmpty(_password))
            {
                throw new HiWiFiApiException("[HiWiFiService]Invalid IP or Password");
            }

            _loginInfo = HiWiFiApi.Login(_ip, _password);
        }
    }
}
