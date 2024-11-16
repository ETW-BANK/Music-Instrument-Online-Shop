using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Music_Instrumet_Online_Shop.Areas.Admin.Controllers;
using MusicShop.Models;
using MusicShop.Repository.IRepository;
using MusicShop.Repository.Repository.IRepository;

using System.Linq.Expressions;



namespace MusicSop.UnitTest
{
    [TestClass]
    public class CompanyControllerTest
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ICompanyRepository> _mockCompanyyRepository;
        private CompanyController _companyController;
        private List<Companies> _companyList;

        [TestInitialize]
        public void TestInitialize()
        {
            //Arrange
            _mockCompanyyRepository = new Mock<ICompanyRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _companyList = new List<Companies>
            {
                new Companies { Id = 1, Name = "Lukas",StreetAddress=null,City=null,State=null,PostalCode=null, Country=null,Email=null,PhoneNumber=null },
                new Companies { Id = 2, Name = "Chas",StreetAddress=null,City=null,State=null,PostalCode=null, Country=null,Email=null,PhoneNumber=null },
            };
            _mockCompanyyRepository.Setup(repo => repo.GetAll(null, null)).Returns(_companyList);
            _mockUnitOfWork.Setup(uow => uow.Company).Returns(_mockCompanyyRepository.Object);
            _companyController = new CompanyController(_mockUnitOfWork.Object);
            _companyController.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        }

        [TestMethod]
        public void Get_All_Companies_From_The_DataBase()
        {
            // Act
            var result = _companyController.Index() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(List<Companies>));
            var companies = result.Model as List<Companies>;
            Assert.AreEqual(2, companies.Count);
            Assert.AreEqual("Lukas", companies[0].Name);
            Assert.AreEqual("Chas", companies[1].Name);
        }
        [TestMethod]
        public void Update_Company_When_Valid_Model()
        {
            // Arrange
            var newComapny = new Companies { Id = 1, Name = "Lukas", StreetAddress = null, City = null, State = null, PostalCode = null, Country = null, Email = null, PhoneNumber = null };
            // Act
            var result = _companyController.Upsert(newComapny) as RedirectToActionResult;
            // Assert
            _mockUnitOfWork.Verify(uow => uow.Company.Upadate(newComapny), Times.Once());
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Company Created Successfully", _companyController.TempData["success"]);
        }
        [TestMethod]
        public void Create_Company_When_Valid_Model()
        {
            // Arrange
            var newComapny = new Companies { Id = 0, Name = "Lukas", StreetAddress = null, City = null, State = null, PostalCode = null, Country = null, Email = null, PhoneNumber = null };
            // Act
            var result = _companyController.Upsert(newComapny) as RedirectToActionResult;

            // Assert
            _mockUnitOfWork.Verify(uow => uow.Company.Add(newComapny), Times.Once());
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Company Created Successfully", _companyController.TempData["success"]);
        }
        [TestMethod]
        public void Delete_Returns_Json_When_Valid_Company()
        {
            // Arrange
            int validCompanyId = 1;

            _mockUnitOfWork.Setup(u => u.Company.GetFirstOrDefault(It.IsAny<Expression<Func<Companies, bool>>>(), null, false))
                .Returns(new Companies { Id = validCompanyId });

            _mockUnitOfWork.Setup(u => u.Company.Remove(It.IsAny<Companies>()));
            _mockUnitOfWork.Setup(u => u.Save());

            //Act
            var result = _companyController.Delete(validCompanyId) as JsonResult;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result.Value, "Result.Value should not be null");

        }
        [TestMethod]
        public void Delete_Returns_Json_When_Invalid_Company()
        {
            // Arrange
            int invalidCompanyId = 999;

            _mockUnitOfWork.Setup(u => u.Company.GetFirstOrDefault(It.IsAny<Expression<Func<Companies, bool>>>(), null, false))
                .Returns(new Companies { Id = invalidCompanyId });

            // Act
            var result = _companyController.Delete(invalidCompanyId) as JsonResult;

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result.Value, "Result.Value should not be null");
  
        }
    }


}

