# HiWiFiAPI
极路由后台API封装

## 简介/Introduction
支持通过获取全局网速、设备网速、设备连线消息、设备断线消息

## 示例/Usage

```C#
class Program
{
	static HiWiFiService service = null;
	static void Main(string[] args)
	{
		string ip = "192.168.199.1";
		string pwd = "123456";

		service = new HiWiFiService(ip, pwd);
		service.DeviceOnlineStatusChanged += Service_DeviceOnlineStatusChanged;

		while (true)
		{
			service.DoHeartBeat();
			Print(service.DeviceMap.Values);
			Task.Delay(2000).Wait();
		}
	}

	private static void Service_DeviceOnlineStatusChanged(DeviceInfo obj)
	{
		string _mesasge = "Device: " + obj.name + (obj.online == 1 ? " connected." : " disconnected.");
		Console.WriteLine("Recent Message: " + _mesasge);
	}

	static void Print(IEnumerable<DeviceInfoEx> devExList)
	{
		Console.Clear();
		//输出局域网整体速度
		string totalUploadSpeed = (int)(service.UploadSpeed / 8000f) + "KB/S";
		string totalDownloadSpeed = (int)(service.DownloadSpeed / 8000f) + "KB/S";
		Console.WriteLine($"Total Upload: {totalUploadSpeed}, Download: {totalDownloadSpeed}\r\n");

		//输出设备网速
		int lineLimit = 25;
		for (int i = 0; i < lineLimit && i < devExList.Count(); i++)
		{
			var devEx = devExList.ElementAt(i);

			string devName = string.IsNullOrWhiteSpace(devEx.Info.name) ? "<Unkown>" : devEx.Info.name;
			string txSpeed = (int)(devEx.UploadSpeed / 8000f) + "KB/S";
			string rxSpeed = (int)(devEx.DownloadSpeed / 8000f) + "KB/S";

			if (devName.Length > _padLeft) _padLeft = devEx.Info.name.Length;

			Console.WriteLine($"Device: {devName}\t up: {txSpeed}\t down: {rxSpeed.}");
		}
	}
}
```
