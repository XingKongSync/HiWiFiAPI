using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HiWiFiAPI.Entity
{
    public class LoginInfo
    {
        public int remaining_lock_time { get; set; }

        public string stok { get; set; }

        public string first_install_complete { get; set; }

        public int remaining_num { get; set; }
    }

    public class LoginInfoEx
    {
        public LoginInfo Info;
        public CookieCollection Cookies;
    }
}
