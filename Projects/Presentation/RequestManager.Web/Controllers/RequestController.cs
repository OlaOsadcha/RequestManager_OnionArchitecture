using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RequestManager.Core.Domain.Entities;
using RequestManager.Infrastructure.Manager;
using RequestManager.Web.Models.Enitities;
using System;
using System.Data;
using System.IO;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RequestManager.Web.Controllers
{
    [Authorize(Roles = "admin, moderator, user, executor")]
    public class RequestController : Controller
    {
        private readonly DataManager DataManager;
        private readonly IWebHostEnvironment _environment;
        private IMapper _mapper;
        private readonly UserManager<RequestUser> userManager;

        public RequestController(DataManager dataManager, IMapper mapper, IWebHostEnvironment environment,
            UserManager<RequestUser> userManager)
        {
            DataManager = dataManager;
            _mapper = mapper;
            _environment = environment;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await GetUser();
            if (user != null)
            {
                var requestsForUser = await this.DataManager.RequestRepository.GetAllRequestForUserAsync(user.Id);
                var requestsDto = _mapper.Map<List<Request>, List<RequestDto>>(requestsForUser);

                return View(requestsDto);
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [HttpGet]
        [Authorize(Roles = "executor")]
        public async Task<ActionResult> ChangeStatusExecutor()
        {
            var user = await GetUser();
            if (user != null)
            {
                var requests = await this.DataManager.RequestRepository.GetAllRequestForExecutorAsync(user.Id);
                var requestsDto = this._mapper.Map<List<Request>, List<RequestDto>>(requests);

                return View(requestsDto);
            }

            return RedirectToAction("Logout", "Account");
        }

        [HttpPost]
        [Authorize(Roles = "executor")]
        public async Task<ActionResult> ChangeStatusExecutor(int requestId, int status)
        {
            var user = await GetUser();

            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }            

            var request = await this.DataManager.RequestRepository.GetByIdAsync(requestId);
            if (request != null)
            {
                request.Status = status;
                Lifecycle lifecycle = await this.DataManager.LifecycleRepository.GetByIdAsync(request.LifecycleId);

                if (lifecycle != null)
                {
                    if (status == (int)RequestStatus.Proccesing)
                    {
                        lifecycle.Proccesing = DateTime.Now;
                    }
                    else if (status == (int)RequestStatus.Checking)
                    {
                        lifecycle.Checking = DateTime.Now;
                    }
                    else if (status == (int)RequestStatus.Closed)
                    {
                        lifecycle.Closed = DateTime.Now;
                    }

                    await this.DataManager.LifecycleRepository.UpdateAsync(lifecycle);                   
                }
            }

            return RedirectToAction("ChangeStatusExecutor");
        }

        [HttpGet]
        [Authorize(Roles = "moderator")]
        public async Task<ActionResult> Distribute()
        {
            var requests = await this.DataManager.RequestRepository.GetAllRequestForModeratorAsync();
            var requestsDto = this._mapper.Map<List<Request>, List<RequestDto>>(requests);

            var executors = (await this.userManager.GetUsersInRoleAsync("moderator")).ToList();
            var executorsDto = this._mapper.Map<List<RequestUser>, List<UserDTO>>(executors);

            ViewBag.Executors = new SelectList(executorsDto, "Id", "UserName");
            return View(requestsDto);
        }

        [HttpPost]
        [Authorize(Roles = "moderator")]
        public async Task<ActionResult> Distribute(int? requestId, int? executorId)
        {
            if (requestId == null && executorId == null)
            {
                return RedirectToAction("Distribute");
            }
            Request? req = await this.DataManager.RequestRepository.GetByIdAsync(requestId ?? 0);
            var executor = await this.DataManager.RequestUserRepository.GetByIdAsync(executorId ?? 0);

            if (req == null && executor == null)
            {
                return RedirectToAction("Distribute");
            }
            
            req.ExecutorId = executorId;
            req.Status = (int)RequestStatus.Distributed;

            Lifecycle lifecycle = await DataManager.LifecycleRepository.GetByIdAsync(req.LifecycleId);
            lifecycle.Distributed = DateTime.Now;
           
            await DataManager.LifecycleRepository.UpdateAsync(lifecycle);

            return RedirectToAction("Distribute");
        }

        public async Task<IActionResult> Executor(int id)
        {
            var request = await this.DataManager.RequestRepository.GetByIdAsync(id);            

            if (request != null)
            {
                bool existsExecutor = await this.DataManager.RequestRepository.IsExecutorExists(request.Id);
                if (existsExecutor)
                {
                    var executor = await this.DataManager.RequestUserRepository.GetByIdAsync(request.ExecutorId ?? 0);
                    var executorDto = this._mapper.Map<RequestUser, UserDTO>(executor);
                    return PartialView("RequestExecutor", executorDto);
                }

                return PartialView("RequestExecutor", null);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Lifecycle(int id)
        {

            var lifecycle = await this.DataManager.LifecycleRepository.GetByIdAsync(id);
            if (lifecycle != null)
            {
                var lifecycleDto = this._mapper.Map<Lifecycle, LifecycleDTO>(lifecycle);
                return PartialView("RequestLifecycle", lifecycleDto);
            }

            else
            {
                return PartialView("RequestLifecycle", null);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var request = await this.DataManager.RequestRepository.GetByIdAsync(id);

            if (request != null)
            {
                var requestDto = this._mapper.Map<Request, RequestDto>(request);

                var subDepartment = await this.DataManager.SubDepartmentRepository.GetByIdAsync(requestDto.SubDepartmentId ?? 0);

                if (subDepartment != null)
                {
                    var subDepDto = this._mapper.Map<SubDepartment, SubDepartmentDTO>(subDepartment);
                    requestDto.SubDepartment = subDepDto;
                }

                var category = await this.DataManager.CategoryRepository.GetByIdAsync(requestDto.CategoryId ?? 0);
                if (category != null)
                {
                    var categoryDto = this._mapper.Map<Category, CategoryDTO>(category);
                    requestDto.Category = categoryDto;
                }

                return PartialView("RequestDetails", requestDto);
               // return View(requestDto);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        [Authorize(Roles = "admin")]
        public async Task<ActionResult> RequestList()
        {
            var requests = await this.DataManager.RequestRepository.GetAllAsync();
            var requestsDto = _mapper.Map<List<Request>, List<RequestDto>>(requests.ToList());

            return View(requestsDto.ToList());
        }

        public async Task<ActionResult> Delete(int id)
        {
            var request = await this.DataManager.RequestRepository.GetByIdAsync(id);

            var user = await GetUser();

            if (user != null)
            {
                if (request != null && request.UserId == user.Id)
                {
                    await this.DataManager.LifecycleRepository.DeleteAsync(new Core.Domain.Entities.Lifecycle() { Id = request.LifecycleId });
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Download(int id)
        {
            var request = await this.DataManager.RequestRepository.GetByIdAsync(id);
            if (request != null)
            {
                string fileName = this._environment.ContentRootPath + $"/Files/{request.File}";
                string contentType = "image/jpeg";

                string ext = request.File.Substring(request.File.LastIndexOf('.'));
                switch (ext)
                {
                    case "txt":
                        contentType = "text/plain";
                        break;
                    case "png":
                        contentType = "image/png";
                        break;
                    case "tiff":
                        contentType = "image/tiff";
                        break;
                }
                return File(fileName, contentType, fileName);
            }

            return Content("File is not found.");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
           var user = await GetUser();

            if (user != null)
            {
                await GetDictionaries(user);
                return PartialView("Create");           
            }

            return RedirectToAction("Logout", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestDto requestDto, IFormFile error)
        {
            var user = await GetUser();
            if (user != null)
            {             
                //requestDto.Status = (int)RequestStatus.Open;
                DateTime current = DateTime.Now;

                Lifecycle lifecycle = new Lifecycle() { Opened = current };
                LifecycleDTO lifecycleDTO = this._mapper.Map<Lifecycle, LifecycleDTO>(lifecycle);

                requestDto.Lifecycle = lifecycleDTO;

                await this.DataManager.LifecycleRepository.AddAsync(lifecycle);

                requestDto.UserId = user.Id;

                if (error != null)
                {
                    string ext = error.FileName.Substring(error.FileName.LastIndexOf('.'));
                    string path = current.ToString("dd/MM/yyyy H:mm:ss").Replace(":", "_").Replace("/", ".") + ext;

                    using (Stream fileStream = new FileStream("~/Files/"+path, FileMode.Create))
                    {
                        await error.CopyToAsync(fileStream);
                        requestDto.File = path;
                    }
                }
                else
                {
                    requestDto.File = string.Empty;
                }

                var request = this._mapper.Map<RequestDto, Request>(requestDto);
                var result = await this.DataManager.RequestRepository.AddAsync(request);

                if (result != null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        private async Task<RequestUser?> GetUser()
        {
            var currentUser = this.User.Identity?.Name ?? string.Empty;
            var user = await this.DataManager.RequestUserRepository.FindByNameAsync(currentUser);

            return user;
        }

        private async Task GetDictionaries(RequestUser user)
        {            

            if (user != null)
            {
                var subdepartments = await this.DataManager.SubDepartmentRepository.GetAllAsync();
                var subDepartmentsDto = _mapper.Map<List<SubDepartment>, List<SubDepartmentDTO>>(subdepartments.ToList());

                var usersSubdepartments = subDepartmentsDto.Where(x => x.DepartmentId == user.DepartmentId).ToList();

                var categories = await this.DataManager.CategoryRepository.GetAllAsync();
                var categoriesDto = _mapper.Map<List<Category>, List<CategoryDTO>>(categories.ToList());

                ViewBag.SubDepartments = new SelectList(usersSubdepartments, "Id", "CabNumber");
                ViewBag.Categories = new SelectList(categoriesDto, "Id", "Name");             
            }        
        }
    }
}
