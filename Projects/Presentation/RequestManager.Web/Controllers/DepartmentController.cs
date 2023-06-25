using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RequestManager.Core.Domain.Entities;
using RequestManager.Infrastructure.Manager;
using RequestManager.Web.Models;
using RequestManager.Web.Models.Enitities;

namespace RequestManager.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class DepartmentController : Controller
    {
        private readonly DataManager DataManager;
        private IMapper _mapper;

        public DepartmentController(DataManager dataManager, IMapper mapper)
        {
            DataManager = dataManager;
            _mapper = mapper;   
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await GetAllDepartment();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(DepartmentDTO departmentDto)
        {
            if (ModelState.IsValid)
            {
                var departmentToDb = _mapper.Map<DepartmentDTO, Department>(departmentDto);
                var isAlreadyExists = await this.DataManager.DepartmentRepository.IsDepartmentAlreadyExists(departmentDto.Name);
                if (!isAlreadyExists)
                {
                    var result = await this.DataManager.DepartmentRepository.AddAsync(departmentToDb);
                }
                else
                {
                    ModelState.AddModelError(nameof(DepartmentDTO.Name), $"Department with name {departmentDto.Name} is already exist!");
                }
            }
            
            await GetAllDepartment();   
            return View();
        }
      
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            if (ModelState.IsValid)
            {
                await this.DataManager.DepartmentRepository.DeleteAsync(new Department() { Id = id });
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }           
        }

        private async Task GetAllDepartment()
        {
            var departmentsDB = await this.DataManager.DepartmentRepository.GetAllAsync();
            var departmentsDto = _mapper.Map<List<Department>, List<DepartmentDTO>>(departmentsDB.ToList());

            ViewBag.Departments = departmentsDto;
        }
    }
}
