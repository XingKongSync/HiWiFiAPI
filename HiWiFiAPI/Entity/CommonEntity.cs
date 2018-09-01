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

    public class DeviceMethodResultData
    {
        public List<DeviceInfo> list;
    }

    public class DeviceInfo
    {
        /// <summary>
        /// 物理地址
        /// </summary>
        public string mac;

        public string conn_aptype;

        public string type;

        /// <summary>
        /// 当日发送数据总字节数
        /// </summary>
        public long txbytes;

        /// <summary>
        /// wire, 2.4G, 5G
        /// </summary>
        public string phy;

        /// <summary>
        /// 当日接收数据总字节数
        /// </summary>
        public long rxbytes;

        public string ip;

        public long last_offline;

        /// <summary>
        /// 1：在线，0：不在线
        /// </summary>
        public int online;

        public string name;

        public long last_online;

        public int master;

        /// <summary>
        /// 无线信号强度
        /// </summary>
        public int rssi;

        public string conn_ap;

        public long first_online;
    }
}
