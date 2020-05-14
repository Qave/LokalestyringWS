using Windows.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LokalestyringUWP;
using LokalestyringUWP.Annotations;
using LokalestyringUWP.Handler;
using LokalestyringUWP.View;

namespace UnitTesterUWP.Tests
{
    [TestClass]
    public class UnitTestLogin
    {
        [TestMethod]
        public void TestLogin()
        {
            //This Test will see if you are able to login

            //Arrange
            LoginHandler.UserName = "Hammer";
            LoginHandler.Password = "123";
            //Act
            LoginHandler.OnLogin();
            //Assert
            Assert.IsTrue(LoginHandler.LoginSuccesfulTest);
        }

        [TestMethod]
        public void TestLogOut()
        {
            //Arrange
            //Act
            LoginHandler.OnLogout();
            //Assert
            Assert.IsTrue(LoginHandler.LogOutSuccesfulTest);
        }
    }
}
