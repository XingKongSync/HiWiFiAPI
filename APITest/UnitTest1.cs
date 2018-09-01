using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Net;
using HiWiFiAPI.Entity;
using Newtonsoft.Json;

namespace APITest
{
    [TestClass]
    public class UnitTest1
    {
        LoginInfoEx loginInfo = new LoginInfoEx()
        {
            Cookies = new CookieCollection() { new Cookie() { Name = "sysauth", Value = "c1a4007a5e92feb5d1fb799becc66118", Domain = "192.168.199.1" } },
            Info = new LoginInfo() { stok = "/;stok=6886d4c48ab0d187d67ea2ae793b968d" }
        };

        string host = "192.168.199.1";
        string password = "a1b2c3d4";

        [TestMethod]
        public void TestLogin()
        {
            var loginInfo = HiWiFiAPI.HiWiFiApi.Login(host, password);

            Debug.WriteLine($"token: {loginInfo.Info.stok}");
            Assert.IsFalse(string.IsNullOrWhiteSpace(loginInfo.Info.stok));

            Assert.IsNotNull(loginInfo.Cookies);
            Debug.WriteLine($"Cookie key:{loginInfo.Cookies[0].Name}, value: {loginInfo.Cookies[0].Value}");
        }

        [TestMethod]
        public void TestGetDevices()
        {
            var devList = HiWiFiAPI.HiWiFiApi.GetDeviceStatus(host, loginInfo);
            Debug.WriteLine(devList);
        }
    }
}
