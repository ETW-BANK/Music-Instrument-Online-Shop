using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Music_Instrumet_Online_Shop.Areas.Admin.Controllers;
using MusicShop.Models;
using MusicShop.Repository.IRepository;
using MusicShop.Repository.Repository.IRepository;


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

         
            _companyList= new List<Companies>
            {
                new Companies { Id = 1, Name = "Lukas",StreetAddress=null,City=null,State=null,PostalCode=null, Country=null,Email=null,PhoneNumber=null },
                new Companies { Id = 2, Name = "Chas",StreetAddress=null,City=null,State=null,PostalCode=null, Country=null,Email=null,PhoneNumber=null },
            };

          
            _mockCompanyyRepository.Setup(repo => repo.GetAll(null, null)).Returns(_companyList);

          
            _mockUnitOfWork.Setup(uow => uow.Company).Returns(_mockCompanyyRepository.Object);

           
            _companyController = new CompanyController(_mockUnitOfWork.Object);

            _companyController.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        }


        }

    }
