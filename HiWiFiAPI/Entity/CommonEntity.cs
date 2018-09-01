using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWiFiAPI.Entity
{
    public class ProxyError
    {
        public string proxy_msg;

        public int proxy_code = 0;
    }

    public class CommonResponse<T>
    {
        public int code;

        public string msg;

        public T data;

        public int req_id;
    }

    public class MuticallResultCollection<T>
    {
        public List<MuticallResult<T>> results;
    }

    public class MuticallResult<T>
    {
        public string method;

        public CommonResult<T> result;
    }

    public class CommonResult<T>
    {
        public T data;

        public int seqnum;

        public int code;
    }

}
