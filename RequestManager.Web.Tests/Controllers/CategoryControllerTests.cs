using AutoMapper;
using Moq;
using RequestManager.Core.Application.Interfaces;
using RequestManager.Infrastructure.Manager;
using RequestManager.Web.Controllers;
using RequestManager.Web.Mappings;
using RequestManager.Web.Models.Enitities;
using RequestManager.Web.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestManager.Web.Tests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly DataManager _dataManagerMock;

        public CategoryControllerTests()
        {
            _dataManagerMock = CategoryMock.GetDataManagerRepository();
        }

        [Fact]
        public async Task GetAllDataTest()
        {
            //Arrange
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ModelMappings());
            });
            var mapper = mockMapper.CreateMapper();

            CategoryController categoryController = new CategoryController(_dataManagerMock, mapper);

            //Act
            var indexInizial = await categoryController.Index();
            var result = categoryController.ViewBag.Categories;

            //Assert 
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
        }
    }
}