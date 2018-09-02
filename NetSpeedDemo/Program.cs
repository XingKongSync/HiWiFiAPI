using HiWiFiAPI;
using HiWiFiAPI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSpeedDemo
{
    class Program
    {
        static HiWiFiService service = null;
        static void Main(string[] args)
        {
            string ip = "192.168.199.1";
            string pwd = "a1b2c3d4";

            try
            {
                service = new HiWiFiService(ip, pwd);
                service.DeviceOnlineStatusChanged += Service_DeviceOnlineStatusChanged;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create service failed, message:" + ex.Message);
                Console.ReadKey();
                return;
            }

            PrintSummary(service.DeviceMap.Values);
            Console.WriteLine();
            Console.WriteLine("Press any key to show net speed...");
            Console.ReadKey();

            while (true)
            {
                try
                {
                    service.DoHeartBeat();

                    if (_messageDelayCount > 0)
                    {
                        _messageDelayCount--;
                        if (_messageDelayCount <= 0)
                        {
                            _mesasge = "Nothing Happenend.";
                            _messageColor = _defaultColor;
                        }
                    }

                    PrintNetSpeed(service.DeviceMap.Values);
                }
                catch (HiWiFiApiAccessDeniedException)
                {
                    //登陆信息失效，重新登陆
                    service.Relogin();
                }
                catch (Exception ex)
                {
                    //发生了其他错误
                    Console.WriteLine("Unkown Error, message:" + ex.Message);
                    Console.ReadKey();
                    return;
                }
                Task.Delay(2000).Wait();
            }
        }

        static string GetGoodDevName(DeviceInfo dev)
        {
            return string.IsNullOrWhiteSpace(dev.name) ? $"{dev.ip} {dev.mac}" : dev.name;
        }

        static string FormatTime(DateTime time)
        {
            return time.ToString("dd HH:mm:ss");
        }

        static void PrintSummary(IEnumerable<DeviceInfoEx> devExList)
        {
            Console.Clear();
            foreach (var devEx in devExList)
            {
                string devName = GetGoodDevName(devEx.Info);
                if (devName.Length > _padRight) _padRight = devEx.Info.name.Length;
            }

            foreach (var devEx in devExList)
            {
                string devName = GetGoodDevName(devEx.Info);
                string nowStatus = devEx.Info.online == 1 ? "Online " : "Offline";
                string lastOffline = FormatTime(TimestampHelper.GetTime(devEx.Info.last_offline));
                string lastOnline = FormatTime(TimestampHelper.GetTime(devEx.Info.last_online));
                Console.WriteLine($"Device: {devName.PadRight(_padRight)}\t {nowStatus} lastOffline: {lastOffline}\t lastOnline: {lastOnline}");
            }
        }

        #region PrintNetSpeed
        private static ConsoleColor _defaultColor = ConsoleColor.Gray;
        private static ConsoleColor _messageColor = ConsoleColor.Gray;
        private static string _mesasge = "Nothing Happenend.";
        private static int _messageDelayCount = 0;

        private static void Service_DeviceOnlineStatusChanged(DeviceInfo obj)
        {
            if (obj.online == 1)
            {
                _mesasge = obj.name + " connected.";
                _messageColor = ConsoleColor.Green;
            }
            else
            {
                _mesasge = obj.name + " disconnected.";
                _messageColor = ConsoleColor.Red;
            }
            _messageDelayCount = 10;
        }

        private static int _padRight = 5;

        static void PrintNetSpeed(IEnumerable<DeviceInfoEx> devExList)
        {
            Console.Clear();
            //输出局域网整体速度
            string totalUploadSpeed = (int)(service.UploadSpeed / 8000f) + "KB/S";
            string totalDownloadSpeed = (int)(service.DownloadSpeed / 8000f) + "KB/S";
            Console.WriteLine($"Total Upload: {totalUploadSpeed}, Download: {totalDownloadSpeed}\r\n");

            //输出连线状态变化事件
            Console.ForegroundColor = _messageColor;
            Console.WriteLine("Recent Message: " + _mesasge + "\r\n");
            Console.ForegroundColor = _defaultColor;

            //输出设备网速
            int lineLimit = 25;
            for (int i = 0; i < lineLimit && i < devExList.Count(); i++)
            {
                var devEx = devExList.ElementAt(i);

                string devName = GetGoodDevName(devEx.Info);
                string txSpeed = (int)(devEx.UploadSpeed / 8000f) + "KB/S";
                string rxSpeed = (int)(devEx.DownloadSpeed / 8000f) + "KB/S";

                if (devName.Length > _padRight) _padRight = devEx.Info.name.Length;

                Console.WriteLine($"Device: {devName.PadRight(_padRight)}\t mac: {devEx.Info.mac}\t up: {txSpeed.PadLeft(5)}\t down: {rxSpeed.PadLeft(5)}");
            }
        }
        #endregion

    }
}
