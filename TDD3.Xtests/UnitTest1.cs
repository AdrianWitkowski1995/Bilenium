using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;




namespace TDD3.tests
{
    public class UnitTest1
    {
      

        [Fact]
        public void ShouldReturnListOfUsers()
        {
            var users = new List<User>() {
                new User { Login="asd",Email="asd@m.pl", Password="asd",Id=1},
                new User{Login="afsd",Email="asfd@m.pl", Password="asfd",Id= 2} };

            var expected = new List<UserDto> {
                new UserDto { Login="asd",Email="asd@m.pl",Id=1},
                new UserDto{Login="afsd",Email="asfd@m.pl",Id= 2} };
            var _userRepository = new Mock<IRepository<User>>();
            _userRepository.Setup(x => x.GetAll()).Returns(users.AsQueryable());
            var _userService = new UserService(_userRepository.Object);

            var userController = new UserController(_userService);

            //act
            List<UserDto> result = userController.Get();

            //assert
            Assert.Equal<UserDto>(expected, result);
        }


    }

    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public List<UserDto> GetAll()
        {
            IQueryable<User> users = _userRepository.GetAll();
            var usersToReturn = new List<UserDto>();
            users.ToList().ForEach(x => {
                usersToReturn.Add(new UserDto
                {
                    Email = x.Email,
                    Id = x.Id,
                    Login = x.Login
                });
            });
            
            return usersToReturn;
        }
    }
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public List<UserDto> Get()
        {
            var users = _userService.GetAll();
           
            return users;
        }
    }

    
    public interface IUserService
    {
        List<UserDto> GetAll();
    }

    public interface IRepository<T> where T : Entity
    {
        IQueryable<User> GetAll();
    }
    public class UserDto
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }

        public override bool Equals(object obj)
        {
            var user = (UserDto)obj;

            return user.Email == Email && user.Login == Login && Id == user.Id;

        }

        public override int GetHashCode()
        {
            return new { Login, Email,Id }.GetHashCode();
        }
    }
    public class Entity
    {
        public long Id { get; set; }
    }

    public class User : Entity
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
