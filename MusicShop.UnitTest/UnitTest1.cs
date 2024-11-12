using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Music_Instrumet_Online_Shop.Areas.Customer.Controllers;
using Music_Instrumet_Online_Shop.Models;
using MusicShop.Models;
using MusicShop.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Music_Instrumet_Online_Shop.Tests
{
    public class HomeControllerTest
    {
        private readonly Mock<ILogger<HomeController>> _loggerMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly HomeController _controller;

        public HomeControllerTest()
        {
            _loggerMock = new Mock<ILogger<HomeController>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _controller = new HomeController(_loggerMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public void Index_ReturnsViewResult_WithListOfCategories()
        {
            // Arrange
            var categories = new List<Category> { new Category { Id = 1, Name = "Guitars" } };
            _unitOfWorkMock.Setup(u => u.Category.GetAll(null, null)).Returns(categories.AsQueryable());

            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Category>>(viewResult.ViewData.Model);
            Assert.Single(model);
        }

        [Fact]
        public void Instruments_ReturnsNotFound_WhenCategoryIsNull()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Category.GetFirstOrDefault(It.IsAny<System.Linq.Expressions.Expression<System.Func<Category, bool>>>(), null, false))
              .Returns((Category)null);

            // Act
            var result = _controller.Instruments(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Details_ReturnsViewResult_WithShoppingCart()
        {
            // Arrange
            var product = new Product { Id = 1, Title = "Acoustic Guitar", Category = new Category { Id = 1, Name = "Guitars" } };
            _unitOfWorkMock.Setup(u => u.Product.GetFirstOrDefault(It.IsAny<System.Linq.Expressions.Expression<System.Func<Product, bool>>>(), null, false))
              .Returns(product);

            // Act
            var result = _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ShoppingCart>(viewResult.ViewData.Model);
            Assert.Equal(1, model.ProductId);
        }

    }
}