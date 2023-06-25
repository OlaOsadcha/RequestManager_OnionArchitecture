using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RequestManager.Core.Domain.Entities;
using RequestManager.Infrastructure.Manager;
using RequestManager.Web.Models.Enitities;
using System.Data;

namespace RequestManager.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class SubDepartmentController : Controller
    {
        private readonly DataManager DataManager;
        private IMapper _mapper;

        public SubDepartmentController(DataManager dataManager, IMapper mapper)
        {
            DataManager = dataManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await GetAllSubDepartment();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(SubDepartmentDTO model)
        {
            if (model != null)
            {
                var subDepartment = _mapper.Map<SubDepartmentDTO, SubDepartment>(model);

                var isAlreadyExists = await this.DataManager.SubDepartmentRepository.IsSubDepartmentAlreadyExists(model.CabNumber);
                if (!isAlreadyExists)
                {
                    var result = await this.DataManager.SubDepartmentRepository.AddAsync(subDepartment);
                }
                else
                {
                    ModelState.AddModelError(nameof(SubDepartmentDTO.CabNumber), $"Subdepartment with name {model.CabNumber} is already exist!");
                }
            }

            await GetAllSubDepartment();
            return View();
        }

        public async Task<IActionResult> DeleteSubDepartment(int id)
        {
            if (ModelState.IsValid)
            {
                await this.DataManager.SubDepartmentRepository.DeleteAsync(new SubDepartment() { Id = id });
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }

        private async Task GetAllSubDepartment()
        {
            var subDepartmentsDB = await this.DataManager.SubDepartmentRepository.GetAllAsync();
            var subDepartmentsDto = _mapper.Map<List<SubDepartment>, List<SubDepartmentDTO>>(subDepartmentsDB.ToList());

            ViewBag.SubDepartments = subDepartmentsDto;

            var departmentsDB = await this.DataManager.DepartmentRepository.GetAllAsync();
            var departmentsDto = _mapper.Map<List<Department>, List<DepartmentDTO>>(departmentsDB.ToList());

            ViewBag.Departments = new SelectList(departmentsDto, "Id", "Name");
        }
    }
}