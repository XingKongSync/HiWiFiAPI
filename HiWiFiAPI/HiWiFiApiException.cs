using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWiFiAPI
{
    public class HiWiFiApiException : Exception
    {
        public HiWiFiApiException(string msg) : base(msg) { }
    }

    /// <summary>
    /// Token失效禁止访问
    /// </summary>
    public class HiWiFiApiAccessDeniedException : HiWiFiApiException
    {
        public HiWiFiApiAccessDeniedException(string msg) : base(msg) { }
    }
}
