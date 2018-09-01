using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWiFiAPI.Entity
{

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

    public class DeviceInfoEx
    {
        private DeviceInfo _info;

        /// <summary>
        /// 数据发送速度（字节/秒）
        /// </summary>
        public long UploadSpeed;

        /// <summary>
        /// 数据接收速度（字节/秒）
        /// </summary>
        public long DownloadSpeed;

        public DeviceInfo Info { get => _info; private set => _info = value; }

        /// <summary>
        /// 设备的连线、断线状态变化通知事件
        /// </summary>
        public event Action<DeviceInfo> DeviceOnlineStatusChanged;

        /// <summary>
        /// 更新设备状态
        /// </summary>
        /// <param name="dev"></param>
        public void Update(DeviceInfo dev)
        {
            if (dev == null)
            {
                return;
            }
            //第一次更新数据时不计算网速，不触发连线断线事件
            if (_info != null)
            {
                //判断连线状态是否变化
                if (_info.online != dev.online)
                {
                    //触发连线状态变化事件
                    DeviceOnlineStatusChanged?.Invoke(dev);
                }
            }
            //更新数据
            _info = dev;
        }

        /// <summary>
        /// 更新设备网速
        /// </summary>
        /// <param name="dev"></param>
        public void Update(DeviceSpeed dev)
        {
            if (dev == null)
            {
                return;
            }
            UploadSpeed = dev.upload_speed;
            DownloadSpeed = dev.download_speed;
        }
    }
}
