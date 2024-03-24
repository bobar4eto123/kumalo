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
