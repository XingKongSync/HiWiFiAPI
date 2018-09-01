using HiWiFiAPI.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XingKongUtils;

namespace HiWiFiAPI
{
    public class HiWiFiApi
    {
        public static readonly string LOGIN_API_URL_FORMAT = "http://{0}/cgi-bin/turbo/api/login/login_admin?username=admin&password={1}";

        public static readonly string DEVICES_STATUS_URL_FORMAT = "http://{0}/cgi-bin/turbo{1}/proxy/call?_roundRobinUpdateData";

        private static readonly string DEVICE_STATUS_REQUEST_PAYLOAD_FORMAT = "{{\"muticall\":\"1\",\"mutiargs\":[{{\"method\":\"device.sta.list\",\"data\":{{\"date\":\"{0}\"}}}}],\"lang\":\"zh-CN\",\"version\":\"v1\"}}";

        private static readonly string DEVICE_NETSPEED_URL_FORMAT = "http://{0}/cgi-bin/turbo{1}/proxy/call?_network.ipstat.get_realspeed";

        private static readonly string DEVICE_NETSPEED_REQUEST_PAYLOAD_FORMAT = "{\"method\":\"network.ipstat.get_realspeed\",\"data\":{},\"lang\":\"zh-CN\",\"version\":\"v1\"}";

        public static LoginInfoEx Login(string host, string password)
        {
            string url = string.Format(LOGIN_API_URL_FORMAT, host, password);
            string responseStr = HttpUtils.Get(url, out CookieCollection cookies);
            LoginInfo loginInfo = JsonConvert.DeserializeObject<LoginInfo>(responseStr);

            if (loginInfo == null || string.IsNullOrEmpty(loginInfo.stok))
            {
                throw new HiWiFiApiAccessDeniedException("[HiWiFiAPI]Login Failed.");
            }

            return new LoginInfoEx()
            {
                Info = loginInfo,
                Cookies = cookies
            };
        }

        /// <summary>
        /// 检查服务器是否返回了错误
        /// </summary>
        /// <param name="responseStr"></param>
        private static void CheckResponse(string responseStr)
        {
            ProxyError proxyError = JsonConvert.DeserializeObject<ProxyError>(responseStr);
            if (proxyError.proxy_code != 0)
            {
                throw new HiWiFiApiAccessDeniedException($"[HiWiFiAPI]Proxy Error, code: {proxyError.proxy_code}, msg: {proxyError.proxy_msg}");
            }
        }

        public static IEnumerable<DeviceInfo> GetDeviceStatus(string host, LoginInfoEx loginInfo)
        {
            string url = string.Format(DEVICES_STATUS_URL_FORMAT, host, loginInfo.Info.stok);
            string args = string.Format(DEVICE_STATUS_REQUEST_PAYLOAD_FORMAT, DateTime.Now.ToString("yyyyMMdd"));
            string responseStr = HttpUtils.Post(url, args, HttpUtils.RequestType.Json, HttpUtils.DefaultUserAgent, loginInfo.Cookies);

            CheckResponse(responseStr);

            try
            {
                CommonResponse<MuticallResultCollection<DeviceMethodResultData>> response = JsonConvert.DeserializeObject<CommonResponse<MuticallResultCollection<DeviceMethodResultData>>>(responseStr);
                IEnumerable<DeviceInfo> result = response.data.results[0].result.data.list;
                return result;
            }
            catch (Exception ex)
            {
                throw new HiWiFiApiException($"[HiWiFiAPI]Unkown Error, Message: {ex.Message}\r\nResponse: {responseStr}");
            }
        }

        public static NetSpeedInfo GetDeviceNetSpeed(string host, LoginInfoEx loginInfo)
        {
            string url = string.Format(DEVICE_NETSPEED_URL_FORMAT, host, loginInfo.Info.stok);
            string args = DEVICE_NETSPEED_REQUEST_PAYLOAD_FORMAT;
            string responseStr = HttpUtils.Post(url, args, HttpUtils.RequestType.Json, HttpUtils.DefaultUserAgent, loginInfo.Cookies);

            CheckResponse(responseStr);

            try
            {
                CommonResponse<NetSpeedInfo> response = JsonConvert.DeserializeObject<CommonResponse<NetSpeedInfo>>(responseStr);
                return response.data;
            }
            catch (Exception ex)
            {
                throw new HiWiFiApiException($"[HiWiFiAPI]Unkown Error, Message: {ex.Message}\r\nResponse: {responseStr}");
            }
        }
    }
}
