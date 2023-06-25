using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RequestManager.Core.Domain.Entities;
using RequestManager.Infrastructure.Manager;
using RequestManager.Web.Models.Enitities;

namespace RequestManager.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class CategoryController : Controller
    {
        private readonly DataManager DataManager;
        private IMapper _mapper;

        public CategoryController(DataManager dataManager, IMapper mapper)
        {
            DataManager = dataManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await GetAllCategories();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CategoryDTO categoryDto)
        { 
            if(ModelState.IsValid)
            {
                var categoryToDb = _mapper.Map<CategoryDTO, Category>(categoryDto);
                var isAlreadyExists = await this.DataManager.CategoryRepository.IsCategoryAlreadyExists(categoryDto.Name);
                if (!isAlreadyExists)
                {
                    var result = await this.DataManager.CategoryRepository.AddAsync(categoryToDb);
                }
                else
                {
                    ModelState.AddModelError(nameof(CategoryDTO.Name), $"Category with name {categoryDto.Name} is already exist!");
                }
            }

            await GetAllCategories();
            return View();
        }

        private async Task GetAllCategories()
        {
            var categoriesDB = await this.DataManager.CategoryRepository.GetAllAsync();
            var categoriesDto = _mapper.Map<List<Category>, List<CategoryDTO>>(categoriesDB.ToList());

            ViewBag.Categories = categoriesDto;
        }
    }
}