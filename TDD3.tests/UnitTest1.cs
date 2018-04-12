using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using System.Web.Http;



namespace TDD3.tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void ShouldReturnListOfUsers()
        {
            var users = new List<User>(){new User(),new User()};
            var _userRepository = new Mock<IRepository<User>>();

            var _userService = new Mock<IUserService>();
            _userService.Setup(x => x.GetAll()).Returns(users);

            var userController = new UserController(_userService.Object);

            //act
            List<User> result = userController.Get();

            //assert
            Assert.AreEqual(users,result);
        }

        
    }
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public List<User> Get()
        {
            var users=_userService.GetAll();
            return users;
        }
    }

    public interface IUserService
    {
        List<User> GetAll();
    }

    public interface IRepository<T> where T : Entity
    {

    }

    public class Entity
    {
        public long Id { get; set; }
    }

    public class User : Entity
    {

    }
}
