using System.Net.Http;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAS.BLL;
using NAS.Controllers;
using NAS.Model;

namespace NAS.UnitTests
{
    [TestClass]
    public class UserControllerTest
    {
        private UserController _controller = new UserController(new NasBLLstub());

        [TestMethod]
        public void RegistrerUser_Post_OK()
        {
            // Arrange
            var user = new UserViewModel()
            {
                Username = "testuser",
                Password = "123"
            };

            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            // Act
            var response = _controller.Post(user);
            string responseMessage;

            // Assert
            Assert.IsTrue(response.TryGetContentValue(out responseMessage));
            Assert.AreEqual("Success", responseMessage);
     
        }

        [TestMethod]
        public void RegistrerUser_Post_Fail()
        {
            // Arrange
            var user = new UserViewModel()
            {
                Username = "dbFail",
                Password = "123"
            };

            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            // Act
            var response = _controller.Post(user);
            string responseMessage;

            // Assert
            Assert.IsTrue(response.TryGetContentValue(out responseMessage));
            Assert.AreEqual("Det har oppstått en feil, vennligst prøv igjen senere", responseMessage);

        }

        [TestMethod]
        public void RegistrerUser_Post_NoUserFound()
        {
            // Arrange
            var user = new UserViewModel()
            {
                Username = "NoUserFound",
                Password = "123"
            };

            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            // Act
            var response = _controller.Post(user);
            string responseMessage;

            // Assert
            Assert.IsTrue(response.TryGetContentValue(out responseMessage));
            Assert.AreEqual("Det er ingen aktive medlemmer med den eposten", responseMessage);

        }

        [TestMethod]
        public void RegistrerUser_Post_UserTaken()
        {
            // Arrange
            var user = new UserViewModel()
            {
                Username = "userTaken",
                Password = "123"
            };

            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            // Act
            var response = _controller.Post(user);
            string responseMessage;

            // Assert
            Assert.IsTrue(response.TryGetContentValue(out responseMessage));
            Assert.AreEqual("En bruker er allerede registrert med den eposten", responseMessage);

        }

        [TestMethod]
        public void Get()
        {
            // Arrange
            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            // Act
            var response = _controller.Get();
            UserApiModel user;

            // Assert
            Assert.IsTrue(response.TryGetContentValue(out user));
            Assert.AreEqual("testuser", user.Username);
            Assert.AreEqual("test", user.Name);
            Assert.AreEqual(true, user.ValidPayment);
            Assert.AreEqual("Membership", user.Membership);

        }
    }
}
