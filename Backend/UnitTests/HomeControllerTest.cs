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
    public class HomeControllerTest
    {

        private HomeController _controller = new HomeController(new NasBLL(new NasDALstub()));

        public List<User> CreateList()
        {
            var users = new List<User>();

            var user1 = new User()
            {
                Name = "Chris",
                Username = "chris@mail.com",
                ValidPayment = true,
                Membership = "Enkeltmedlemskap"
            };

            var user2 = new User()
            {
                Name = "Sara",
                Username = "sara@mail.com",
                ValidPayment = false,
                Membership = "Livsvarig"
            };

            var user3 = new User()
            {
                Name = "Paul",
                Username = "paul@mail.com",
                ValidPayment = true,
                Membership = "Studentmedlemskap"
            };

            users.Add(user1);
            users.Add(user2);
            users.Add(user3);

            return users;
        }

        [TestMethod]
        public void UserList_Specific_User()
        {      
            var userListExpected = CreateList().ToPagedList(1, 10);

            var actionResult = (ViewResult)_controller.Index("Chris", "","", 1);

            var result = (PagedList<User>)actionResult.Model;

            Assert.AreEqual(actionResult.ViewName, "");

            Assert.IsTrue(result.Count == 1);

            for (var i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(userListExpected[i].Name, result[i].Name);
                Assert.AreEqual(userListExpected[i].Username, result[i].Username);
                Assert.AreEqual(userListExpected[i].ValidPayment, result[i].ValidPayment);
                Assert.AreEqual(userListExpected[i].Membership, result[i].Membership);
            }
        }

        [TestMethod]
        public void UserList_SearchString_Empty()
        {
            var userListExpected = CreateList().OrderBy(s => s.Name).ToPagedList(1,10);

            var actionResult = (ViewResult)_controller.Index("", "", "", 1);

            var result = (PagedList<User>)actionResult.Model;

            Assert.AreEqual(actionResult.ViewName, "");

            for (var i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(userListExpected[i].Name, result[i].Name);
                Assert.AreEqual(userListExpected[i].Username, result[i].Username);
                Assert.AreEqual(userListExpected[i].ValidPayment, result[i].ValidPayment);
                Assert.AreEqual(userListExpected[i].Membership, result[i].Membership);
            }
        }

        [TestMethod]
        public void UserList_SearchString_Null()
        {
            var userListExpected = CreateList().OrderBy(s => s.Name).ToPagedList(1, 10);

            var actionResult = (ViewResult)_controller.Index(null, "", "", 1);

            var result = (PagedList<User>)actionResult.Model;

            Assert.AreEqual(actionResult.ViewName, "");

            for (var i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(userListExpected[i].Name, result[i].Name);
                Assert.AreEqual(userListExpected[i].Username, result[i].Username);
                Assert.AreEqual(userListExpected[i].ValidPayment, result[i].ValidPayment);
                Assert.AreEqual(userListExpected[i].Membership, result[i].Membership);
            }
        }

        [TestMethod]
        public void UserList_Sort_Name_desc()
        {
            var userListExpected = CreateList().OrderByDescending(s => s.Name).ToPagedList(1, 10);

            var actionResult = (ViewResult)_controller.Index("", "Name_desc", "", 1);

            var result = (PagedList<User>)actionResult.Model;

            Assert.AreEqual(actionResult.ViewName, "");

            for (var i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(userListExpected[i].Name, result[i].Name);
                Assert.AreEqual(userListExpected[i].Username, result[i].Username);
                Assert.AreEqual(userListExpected[i].ValidPayment, result[i].ValidPayment);
                Assert.AreEqual(userListExpected[i].Membership, result[i].Membership);
            }
        }

        [TestMethod]
        public void UserList_Sort_Username_desc()
        {
            var userListExpected = CreateList().OrderByDescending(s => s.Username).ToPagedList(1, 10);

            var actionResult = (ViewResult)_controller.Index("", "Username_desc", "", 1);

            var result = (PagedList<User>)actionResult.Model;

            Assert.AreEqual(actionResult.ViewName, "");

            for (var i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(userListExpected[i].Name, result[i].Name);
                Assert.AreEqual(userListExpected[i].Username, result[i].Username);
                Assert.AreEqual(userListExpected[i].ValidPayment, result[i].ValidPayment);
                Assert.AreEqual(userListExpected[i].Membership, result[i].Membership);
            }
        }

        [TestMethod]
        public void UserList_Sort_Username()
        {
            var userListExpected = CreateList().OrderBy(s => s.Username).ToPagedList(1, 10);

            var actionResult = (ViewResult)_controller.Index("", "Username", "", 1);

            var result = (PagedList<User>)actionResult.Model;

            Assert.AreEqual(actionResult.ViewName, "");

            for (var i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(userListExpected[i].Name, result[i].Name);
                Assert.AreEqual(userListExpected[i].Username, result[i].Username);
                Assert.AreEqual(userListExpected[i].ValidPayment, result[i].ValidPayment);
                Assert.AreEqual(userListExpected[i].Membership, result[i].Membership);
            }
        }

        [TestMethod]
        public void UserList_Sort_ValidPayment()
        {
            var userListExpected = CreateList().OrderBy(s => s.ValidPayment).ToPagedList(1, 10);

            var actionResult = (ViewResult)_controller.Index("", "ValidPayment", "", 1);

            var result = (PagedList<User>)actionResult.Model;

            Assert.AreEqual(actionResult.ViewName, "");

            for (var i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(userListExpected[i].Name, result[i].Name);
                Assert.AreEqual(userListExpected[i].Username, result[i].Username);
                Assert.AreEqual(userListExpected[i].ValidPayment, result[i].ValidPayment);
                Assert.AreEqual(userListExpected[i].Membership, result[i].Membership);
            }
        }

        [TestMethod]
        public void UserList_Sort_ValidPayment_desc()
        {
            var userListExpected = CreateList().OrderByDescending(s => s.ValidPayment).ToPagedList(1, 10);

            var actionResult = (ViewResult)_controller.Index("", "ValidPayment_desc", "", 1);

            var result = (PagedList<User>)actionResult.Model;

            Assert.AreEqual(actionResult.ViewName, "");

            for (var i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(userListExpected[i].Name, result[i].Name);
                Assert.AreEqual(userListExpected[i].Username, result[i].Username);
                Assert.AreEqual(userListExpected[i].ValidPayment, result[i].ValidPayment);
                Assert.AreEqual(userListExpected[i].Membership, result[i].Membership);
            }
        }

        [TestMethod]
        public void UserList_Sort_Membership()
        {
            var userListExpected = CreateList().OrderBy(s => s.Membership).ToPagedList(1, 10);

            var actionResult = (ViewResult)_controller.Index("", "Membership", "", 1);

            var result = (PagedList<User>)actionResult.Model;

            Assert.AreEqual(actionResult.ViewName, "");

            for (var i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(userListExpected[i].Name, result[i].Name);
                Assert.AreEqual(userListExpected[i].Username, result[i].Username);
                Assert.AreEqual(userListExpected[i].ValidPayment, result[i].ValidPayment);
                Assert.AreEqual(userListExpected[i].Membership, result[i].Membership);
            }
        }

        [TestMethod]
        public void UserList_Sort_Membership_desc()
        {
            var userListExpected = CreateList().OrderByDescending(s => s.Membership).ToPagedList(1, 10);

            var actionResult = (ViewResult)_controller.Index("", "Membership_desc", "", 1);

            var result = (PagedList<User>)actionResult.Model;

            Assert.AreEqual(actionResult.ViewName, "");

            for (var i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(userListExpected[i].Name, result[i].Name);
                Assert.AreEqual(userListExpected[i].Username, result[i].Username);
                Assert.AreEqual(userListExpected[i].ValidPayment, result[i].ValidPayment);
                Assert.AreEqual(userListExpected[i].Membership, result[i].Membership);
            }
        }

        [TestMethod]
        public void UserList_Sort_ActiveUser()
        {
            var userListExpected = CreateList().OrderBy(s => s.ActiveUser).ToPagedList(1, 10);

            var actionResult = (ViewResult)_controller.Index("", "ActiveUser", "", 1);

            var result = (PagedList<User>)actionResult.Model;

            Assert.AreEqual(actionResult.ViewName, "");

            for (var i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(userListExpected[i].Name, result[i].Name);
                Assert.AreEqual(userListExpected[i].Username, result[i].Username);
                Assert.AreEqual(userListExpected[i].ValidPayment, result[i].ValidPayment);
                Assert.AreEqual(userListExpected[i].Membership, result[i].Membership);
            }
        }

        [TestMethod]
        public void UserList_Sort_ActiveUser_desc()
        {
            var userListExpected = CreateList().OrderByDescending(s => s.ActiveUser).ToPagedList(1, 10);

            var actionResult = (ViewResult)_controller.Index("", "ActiveUser_desc", "", 1);

            var result = (PagedList<User>)actionResult.Model;

            Assert.AreEqual(actionResult.ViewName, "");

            for (var i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(userListExpected[i].Name, result[i].Name);
                Assert.AreEqual(userListExpected[i].Username, result[i].Username);
                Assert.AreEqual(userListExpected[i].ValidPayment, result[i].ValidPayment);
                Assert.AreEqual(userListExpected[i].Membership, result[i].Membership);
            }
        }

        [TestMethod]
        public void EditUser_Get_User()
        {
            int id = 1;

            var result = (ViewResult)_controller.EditUser(id);

            var expectedUser = new UserViewModel()
            {
                Name = "Name",
                Username = "Username",
                ValidPayment = true,
                Membership = "Membership"
            };

            var actionResult = (UserViewModel)result.Model;

            Assert.AreEqual(result.ViewName, "");

            Assert.AreEqual(actionResult.Name, expectedUser.Name);
            Assert.AreEqual(actionResult.Username, expectedUser.Username);
            Assert.AreEqual(actionResult.ValidPayment, expectedUser.ValidPayment);
            Assert.AreEqual(actionResult.Membership, expectedUser.Membership);

        }

        [TestMethod]
        public void EditUser_Get_User_Not_Found()
        {
            int id = 0;

            var actionResult = (ViewResult)_controller.EditUser(id);

            var result = (UserViewModel)actionResult.Model;

            Assert.AreEqual(actionResult.ViewName, "");
            Assert.AreEqual(result.Name, "Error");

        }

        [TestMethod]
        public void EditUser_Post_Ok()
        {

            var newUser = new UserViewModel()
            {
                Name = "NewUser"
            };

            var result = (RedirectToRouteResult)_controller.EditUser(newUser);

            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "Index");

        }

        [TestMethod]
        public void EditUser_Post_DB_Fail()
        {

            var newUser = new UserViewModel()
            {
                Name = "Fail",
            };

            var actionResult = (ViewResult)_controller.EditUser(newUser);

            Assert.AreEqual(actionResult.ViewName, "");
        }

        [TestMethod]
        public void EditUser_Post_Model_fail()
        {
            var innUser = new UserViewModel();

            _controller.ViewData.ModelState.AddModelError("Password", "Passwords given do not match");

            var actionResult = (ViewResult)_controller.EditUser(innUser);

            Assert.IsTrue(actionResult.ViewData.ModelState.Count == 1);
            Assert.AreEqual(actionResult.ViewName, "");
        }

        [TestMethod]
        public void Error_Action()
        {

            var actionResult = (ViewResult) _controller.Error();

            Assert.AreEqual(actionResult.ViewName, "");
        }
    }
}
