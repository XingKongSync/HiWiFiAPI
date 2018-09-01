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

        private long _uploadSpeed;
        private long _downloadSpeed;

        private Dictionary<string, DeviceInfoEx> _deviceMap;

        /// <summary>
        /// 局域网设备列表
        /// </summary>
        public Dictionary<string, DeviceInfoEx> DeviceMap
        {
            get
            {
                if (_deviceMap == null)
                {
                    _deviceMap = new Dictionary<string, DeviceInfoEx>();
                }
                return _deviceMap;
            }
            private set => _deviceMap = value;
        }

        /// <summary>
        /// 发送速度（字节/秒）
        /// </summary>
        public long UploadSpeed { get => _uploadSpeed; private set => _uploadSpeed = value; }

        /// <summary>
        /// 接收速度（字节/秒）
        /// </summary>
        public long DownloadSpeed { get => _downloadSpeed; private set => _downloadSpeed = value; }

        /// <summary>
        /// 设备的连线、断线状态变化通知事件
        /// </summary>
        public event Action<DeviceInfo> DeviceOnlineStatusChanged;

        public HiWiFiService(string ip, string password)
        {
            _ip = ip;
            _password = password;

            //登陆
            Login();
            //初始化数据
            InitDeviceInfo();
        }

        /// <summary>
        /// 登陆
        /// </summary>
        private void Login()
        {
            if (string.IsNullOrWhiteSpace(_ip) || string.IsNullOrEmpty(_password))
            {
                throw new HiWiFiApiException("[HiWiFiService]Invalid IP or Password");
            }

            _loginInfo = HiWiFiApi.Login(_ip, _password);
        }

        /// <summary>
        /// 重试登陆
        /// </summary>
        public void Relogin() => Login();

        /// <summary>
        /// 初始化设备信息
        /// </summary>
        private void InitDeviceInfo()
        {
            DeviceMap.Clear();

            var devList = HiWiFiApi.GetDeviceStatus(_ip, _loginInfo);
            foreach (var dev in devList)
            {
                DeviceInfoEx infoEx = new DeviceInfoEx();
                infoEx.DeviceOnlineStatusChanged += InfoEx_DeviceOnlineStatusChanged;
                infoEx.Update(dev);
                DeviceMap[dev.mac] = infoEx;
            }
        }

        /// <summary>
        /// 心跳函数，依赖外部触发
        /// 触发后自动更新设备信息，并且计算网速
        /// </summary>
        public void DoHeartBeat()
        {
            //更新设备状态，并判断连线状态变化
            var devList = HiWiFiApi.GetDeviceStatus(_ip, _loginInfo);
            foreach (var dev in devList)
            {
                if (DeviceMap.TryGetValue(dev.mac, out DeviceInfoEx devEx))
                {
                    devEx.Update(dev);
                }
                else
                {
                    DeviceInfoEx infoEx = new DeviceInfoEx();
                    infoEx.DeviceOnlineStatusChanged += InfoEx_DeviceOnlineStatusChanged;
                    infoEx.Update(dev);
                    DeviceMap[dev.mac] = infoEx;
                }
            }
            //更新设备网速信息
            var devSpeedList = HiWiFiApi.GetDeviceNetSpeed(_ip, _loginInfo);
            UploadSpeed = devSpeedList.total.upload_speed;
            DownloadSpeed = devSpeedList.total.download_speed;
            foreach (var speed in devSpeedList.details)
            {
                if (DeviceMap.TryGetValue(speed.mac, out DeviceInfoEx devEx))
                {
                    devEx.Update(speed);
                }
            }
        }

        /// <summary>
        /// 设备连线状态发生编号
        /// 将事件转发给外部
        /// </summary>
        /// <param name="obj"></param>
        private void InfoEx_DeviceOnlineStatusChanged(DeviceInfo obj)
        {
            DeviceOnlineStatusChanged?.Invoke(obj);
        }
    }
}
