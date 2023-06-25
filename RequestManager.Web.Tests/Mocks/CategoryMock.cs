using Moq;
using RequestManager.Core.Application.Interfaces;
using RequestManager.Core.Domain.Entities;
using RequestManager.Infrastructure.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestManager.Web.Tests.Mocks
{
    public static class CategoryMock
    {
        public static DataManager GetDataManagerRepository()
        {
            var categoryElements = GetCategoryElements();
            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categoryElements);

            //var dataManagerMock = new Mock<DataManager>();
            var dataManagerMock = new DataManager(null, null, null, null, categoryRepository.Object, null);

            return dataManagerMock;
        }

        private static List<Category> GetCategoryElements()
        {
            var c1 = new Category() { Id = 1, Name = "Category1" };
            var c2 = new Category() { Id = 2, Name = "Category2" };
            var c3 = new Category() { Id = 3, Name = "Category3" };
            var c4 = new Category() { Id = 4, Name = "Category4" };

            List<Category> result = new List<Category>();
            result.Add(c1);
            result.Add(c2);
            result.Add(c3);
            result.Add(c4);
            return result;
        }
    }
}
