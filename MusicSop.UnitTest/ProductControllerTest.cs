using ClothShop.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Music_Instrumet_Online_Shop.Areas.Admin.Controllers;
using MusicShop.Models;
using MusicShop.Repository.IRepository;
using MusicShop.ViewModels;
using System.Linq.Expressions;

namespace MusicSop.UnitTest
{
    [TestClass]
    public class ProductControllerTest
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IProductRepository> _mockProductRepository;
        private ProductController _ProductController;
        private Mock< IWebHostEnvironment> webhost;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            _mockProductRepository = new Mock<IProductRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            webhost = new Mock<IWebHostEnvironment>();

            List<Product> productList = new List<Product>
         {
            new Product { Id = 1, Title = "Ibanez", Description = "Bass guitar", Category = new Category { Id = 1, Name = "Electric" }, Price = 200, ImageUrl = "/image/gitar.jpg" },
            new Product { Id = 2, Title = "Yamaha", Description = "Key instrument", Category = new Category { Id = 2, Name = "Key Instrument" }, Price = 2000, ImageUrl = "/image/piano.jpg" }
         };

            _mockProductRepository .Setup(repo => repo.GetAll(null, "Category")) .Returns(productList);
            _mockUnitOfWork.Setup(uow => uow.Product).Returns(_mockProductRepository.Object);
            _ProductController = new ProductController(_mockUnitOfWork.Object, webhost.Object);
            _ProductController.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        }

        [TestMethod]
        public void Get_All_Products_From_The_DataBase()
        {

            // Act
            var result = _ProductController.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(List<Product>));
            var products= result.Model as List<Product>;
            Assert.AreEqual(2, products.Count); 
            Assert.AreEqual("Ibanez", products[0].Title);
            Assert.AreEqual("Yamaha", products[1].Title);

        }

        [TestMethod]
        public void Upsert_Adds_New_Product()
        {
            // Arrange
            var newProduct = new ProductVM
            {
                Product = new Product { Id = 0, Title = "New Product", CategoryId = 1 },
                CategoryList = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Category1", Value = "1" }
                }
            };

            IFormFile file = null;

            // Act
            var result = _ProductController.Upsert(newProduct, file);

            // Assert

            _mockUnitOfWork.Verify(u => u.Product.Add(It.IsAny<Product>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
        }

        //[TestMethod]
        //public void Edit_Post_Returns_Redirect_When_Valid_Model()
        //{
        //    // Arrange
        //    var categoryToEdit = new Category { Id = 1, Name = "Updated Guitar", DisplayOrder = 1, ImageUrl = "/image/updatedguitar.jpg" };

        //    // Act
        //    var result = _categoryController.Edit(categoryToEdit) as RedirectToActionResult;

        //    // Assert
        //    _mockUnitOfWork.Verify(uow => uow.Category.Update(categoryToEdit), Times.Once); 
        //    _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once); 
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("Index", result.ActionName); 
        //}

        [TestMethod]
        public void Delete_Returns_Json_With_Success_When_Product_Exists()
        {
            // Arrange
            var productToDelete = new Product
            {
                Id = 1,
                Title = "Guitar",
                Description = "Electric Guitar",
                Price = 100,
                ImageUrl = "/image/guitar.jpg"
            };

            _mockProductRepository.Setup(repo => repo.GetFirstOrDefault(
                It.IsAny<Expression<Func<Product, bool>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
                .Returns(productToDelete);

            webhost.Setup(env => env.WebRootPath).Returns("C:\\MockWebRoot");

            // Act
            var result = _ProductController.Delete(productToDelete.Id) as JsonResult;

            // Assert
            var data = result.Value as dynamic;
         
            _mockProductRepository.Verify(repo => repo.Remove(productToDelete), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);

            var expectedImagePath = Path.Combine("C:\\MockWebRoot", productToDelete.ImageUrl.Trim('\\'));
            Assert.IsNotNull(data);
            Assert.IsNotNull(result.Value);
        }


    }
}