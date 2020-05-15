
using System;
using LokalestyringUWP.Handler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesterForUWP
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestLogIn()
        {

            LoginHandler.UserName = "Hammer";
            LoginHandler.Password = "123";

            LoginHandler.OnLogin();

            Assert.IsTrue(LoginHandler.LoginSuccesfulTest);



        }
    }
}
