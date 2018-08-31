using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace APITest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestLogin()
        {
            string host = "192.168.199.1";
            string password = "a1b2c3d4";
            HiWiFiAPI.HiWiFiAPI.Login(host, password);
        }
    }
}
