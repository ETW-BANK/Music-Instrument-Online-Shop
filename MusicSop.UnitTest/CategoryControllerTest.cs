using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Music_Instrumet_Online_Shop.Areas.Admin.Controllers;
using MusicShop.Models;
using MusicShop.Repository.IRepository;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MusicSop.UnitTest
{
    [TestClass]
    public class CategoryControllerTest
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private CategoryController _categoryController;
        private List<Category> _categoryList;

        //[TestInitialize]
        //public void TestInitialize()
        //{
        //    //Arrange
          
        //    _mockCategoryRepository = new Mock<ICategoryRepository>();

            
        //    _mockUnitOfWork = new Mock<IUnitOfWork>();

         
        //    _categoryList = new List<Category>
        //    {
        //        new Category { Id = 1, Name = "Guitar", DisplayOrder = 1, ImageUrl = "/image/gitar.jpg" },
        //        new Category { Id = 2, Name = "Piano", DisplayOrder = 2, ImageUrl = "/image/piano.jpg" }
        //    };

          
        //    _mockCategoryRepository.Setup(repo => repo.GetAll(null, null)).Returns(_categoryList);

          
        //    _mockUnitOfWork.Setup(uow => uow.Category).Returns(_mockCategoryRepository.Object);

           
        //    _categoryController = new CategoryController(_mockUnitOfWork.Object);

        //    _categoryController.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        //}


        //[TestMethod]
        //public void Get_All_Categories_From_The_DAtaBase()
        //{

        //    // Act
        //    var result = _categoryController.Index() as ViewResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.IsInstanceOfType(result.Model, typeof(List<Category>));
        //    var categories = result.Model as List<Category>;
        //    Assert.AreEqual(2, categories.Count); 
        //    Assert.AreEqual("Guitar", categories[0].Name);
        //    Assert.AreEqual("Piano", categories[1].Name);

        //}

        //[TestMethod]
        //public void Create_Post_Returns_Redirect_When_Valid_Model()
        //{
        //    // Arrange
        //    var newCategory = new Category { Name = "Drums", DisplayOrder = 3, ImageUrl = "/image/drums.jpg" };

        //    // Act
        //    var result = _categoryController.Create(newCategory) as RedirectToActionResult;

        //    // Assert
        //    _mockUnitOfWork.Verify(uow => uow.Category.Add(newCategory), Times.Once); 
        //    _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once); 
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("Index", result.ActionName); 
        //    Assert.AreEqual("Category Created successfully", _categoryController.TempData["success"]); 
        //}

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

        //[TestMethod]
        //public void Delete_Post_Returns_Redirect_When_Valid_Category()
        //{
        //    // Arrange
        //    var categoryToDelete = new Category { Id = 1, Name = "Guitar", DisplayOrder = 1, ImageUrl = "/image/guitar.jpg" };

        //    _mockCategoryRepository.Setup(repo => repo.GetFirstOrDefault(
        //        It.IsAny<Expression<Func<Category, bool>>>(),  
        //        It.Is<string>(s => s == null),  
        //        It.Is<bool>(b => b == false)    
        //    )).Returns(categoryToDelete); 

        //    // Act
        //    var result = _categoryController.DeletePost(categoryToDelete.Id) as RedirectToActionResult;

        //    // Assert
        //    _mockUnitOfWork.Verify(uow => uow.Category.Remove(categoryToDelete), Times.Once);  
        //    _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);  

           
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("Index", result.ActionName);  

        //    Assert.AreEqual("Category Deleted successfully", _categoryController.TempData["success"]);
        //}

    }
}