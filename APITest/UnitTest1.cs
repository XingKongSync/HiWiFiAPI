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
            Cookies = new CookieCollection() { new Cookie() { Name = "sysauth", Value = "5d2e37e377d398cf2865531084e4a31a", Domain = "192.168.199.1" } },
            Info = new LoginInfo() { stok = "/;stok=042852cd98f0199b96b5f08ad02e8096" }
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

        [TestMethod]
        public void TestGetNetSpeed()
        {
            var devList = HiWiFiAPI.HiWiFiApi.GetDeviceNetSpeed(host, loginInfo);
        }
    }
}
