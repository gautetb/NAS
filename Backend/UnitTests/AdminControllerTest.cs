using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using NAS.BLL;
using NAS.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NAS.Controllers;
using NAS.DAL;
using PagedList;


namespace NAS.UnitTests
{
    [TestClass]
    public class AdminControllerTest
    {

        private AdminController _controller = new AdminController(new NasBLL(new NasDALstub()));

        [TestMethod]
        public void Login_Get_View()
        {
            var actionResult = (ViewResult)_controller.Login();

            Assert.AreEqual(actionResult.ViewName, "");
        }

        [TestMethod]
        public void Login_Not_Valid()
        {
            var testAdmin = new AdminViewModel()
            {
                Username = "Not valid",
                Password = "Credentials"
            };

            var actionResult = (ViewResult) _controller.Login(testAdmin, "ReturnUrl");

            Assert.AreEqual(actionResult.ViewName, "");
        }

        [TestMethod]
        public void Login_Not_ReturnUrl_NULL()
        {
            var testAdmin = new AdminViewModel()
            {
                Username = "username",
                Password = "password"
            };

            var actionResult = (RedirectToRouteResult)_controller.Login(testAdmin, null);

            Assert.AreEqual("Index", actionResult.RouteValues["action"]);
            Assert.AreEqual("Home", actionResult.RouteValues["controller"]);
        }

        [TestMethod]
        public void Login_Not_ReturnUrl_Not_NULL()
        {
            var testAdmin = new AdminViewModel()
            {
                Username = "username",
                Password = "password"
            };

            var actionResult = (RedirectResult)_controller.Login(testAdmin, "Something");

            Assert.AreEqual("Something", actionResult.Url);

        }

        [TestMethod]
        public void RegisterAdmin()
        {
            var actionResult = (ViewResult)_controller.RegisterAdmin();

            Assert.AreEqual(actionResult.ViewName, "");

        }

        [TestMethod]
        public void RegistrerAdmin_Post_OK()
        {

            var newAdmin = new AdminViewModel()
            {
                Username = "DbSuccess",
            };

            var actionResult = (RedirectToRouteResult)_controller.RegisterAdmin(newAdmin);

            Assert.AreEqual("Index", actionResult.RouteValues["action"]);
            Assert.AreEqual("Home", actionResult.RouteValues["controller"]);

        }

        [TestMethod]
        public void RegistrerAdmin_Post__DB_Fail()
        {
            var newAdmin = new AdminViewModel()
            {
                Username = "DbFail",
            };

            var actionResult = (ViewResult)_controller.RegisterAdmin(newAdmin);

            Assert.AreEqual(actionResult.ViewName, "");

        }

        [TestMethod]
        public void RegistrerAdmin_Post_Model_Fail()
        {
            var newAdmin = new AdminViewModel();

            _controller.ViewData.ModelState.AddModelError("Username", "Ugyldig epost");

            var actionResult = (ViewResult)_controller.RegisterAdmin(newAdmin);

            Assert.IsTrue(actionResult.ViewData.ModelState.Count == 1);
            Assert.AreEqual(actionResult.ViewName, "");
        }

        [TestMethod]
        public void Logout()
        {
            var actionResult = (RedirectToRouteResult) _controller.Logout();

            Assert.AreEqual("Index", actionResult.RouteValues["action"]);
            Assert.AreEqual("Home", actionResult.RouteValues["controller"]);
        }
    }
}

