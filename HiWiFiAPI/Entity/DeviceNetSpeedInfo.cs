using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWiFiAPI.Entity
{
    public class DeviceSpeed
    {
        public string mac;
        public int download_speed;
        public int upload_speed;
    }

    public class TotalSpeed
    {
        public int upload_speed;
        public int download_speed;
    }

    public class NetSpeedInfo
    {
        public List<DeviceSpeed> details;
        public TotalSpeed total;
    }
}
