using Castle.Core.Logging;
using FakeItEasy;
using FluentAssertions;
using kumalo.Controllers;
using kumalo.Data;
using kumalo.Data.Entities;
using kumalo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace kumaloWebAppTests.ControllerTest
{

    public class HomeControllerTest
    {
        /*
        [Fact]
        public void EditAccount_ReturnsViewResultWithEditAccountModel()
        {
            // Arrange
            var loggedUserId = "loggedUserId";
            var loggedUser = new User("Test", "Test", "Supplier")
            {
                Id = loggedUserId,
                FirstName = "John",
                LastName = "Doe",
                Age = "32",
                City = "New York",
                PhoneNumber = "123456789",
                Description = "Banica"
            };

            // Create a fake DbContextOptions<AppDbContext>
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Pass the options to the constructor when creating the fake AppDbContext
            var fakeContext = A.Fake<AppDbContext>(x => x.WithArgumentsForConstructor(() => new AppDbContext(options)));

            A.CallTo(() => fakeContext.Users.FirstOrDefault(u => u.Id == loggedUserId)).Returns(loggedUser);

            var controller = new HomeController(null, fakeContext, null);
            var expectedModel = new EditAccountModel
            {
                FirstName = loggedUser.FirstName,
                LastName = loggedUser.LastName,
                Age = loggedUser.Age,
                City = loggedUser.City,
                PhoneNumber = loggedUser.PhoneNumber,
                Description = loggedUser.Description
            };

            // Act
            var result = controller.EditAccount();

            // Assert
            result.Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeEquivalentTo(expectedModel);
        }
        */
        [Fact]
        public void Login_ReturnsViewResult()
        {
            // Arrange
            var controller = new HomeController(null, null, null);

            // Act
            var result = controller.Login();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }
        [Fact]
        public void Register_ReturnsViewResult()
        {
            // Arrange
            var controller = new HomeController(null, null, null);

            // Act
            var result = controller.Register();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void EditAccount_InvalidModelState_ReturnsViewResult()
        {
            // Arrange
            var controller = new HomeController(null, null, null);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = controller.EditAccount(new EditAccountModel());

            // Assert
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public void Register_InvalidModelState_ReturnsViewResult()
        {
            // Arrange
            var controller = new HomeController(null, null, null);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = controller.Register(new UserRegisterModel());

            // Assert
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public void Login_WithInvalidModelState_ReturnsViewResult()
        {
            // Arrange
            var logger = A.Fake<ILogger<HomeController>>();
            var httpContextAccessor = A.Fake<IWebHostEnvironment>();
            var controller = new HomeController(logger, null, null);
            controller.ModelState.AddModelError("AnyKey", "ErrorMessage");

            // Act
            var result = controller.Login(new UserLoginModel());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }


    }


}
