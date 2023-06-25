using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RequestManager.Core.Domain.Entities;
using RequestManager.Infrastructure.Manager;
using RequestManager.Web.Models;
using RequestManager.Web.Models.Enitities;
using System.Collections.Generic;
using System.Data;

namespace RequestManager.Web.Controllers
{
    [Authorize(Roles = "admin, moderator, user, executor")]
    public class UserController : Controller
    {
        private readonly DataManager DataManager;
        private readonly UserManager<RequestUser> UserManager;
        private readonly RoleManager<RequestRole> RoleManager;
        private IMapper _mapper;

        public UserController(DataManager dataManager, UserManager<RequestUser> userManager, 
            RoleManager<RequestRole> roleManager, IMapper mapper)
        {
            this.DataManager = dataManager;
            this.UserManager = userManager;
            this.RoleManager = roleManager;
            this._mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin, moderator, executor")]
        public async Task<IActionResult> Index()
        {
            List<UserViewModel> userViewModels = new List<UserViewModel>();           

            await this.FilterLists();
            userViewModels = await FillTheListOfUsers();
            
            return View(userViewModels);
        }     

        [HttpPost]
        [Authorize(Roles = "admin, moderator, executor")]
        public async Task<IActionResult> Index(int department, int role)
        {
            var allUsers = new List<UserViewModel>();
            var users = await FillTheListOfUsers();

            if (role == 0 && department == 0)
            {
                return RedirectToAction("Index");
            }
            if (role == 0 && department != 0)
            {
                var filteredUsers = users.Where(x => x.User.DepartmentId == department).ToList();
                allUsers = filteredUsers;
            }
            else if (role != 0 && department == 0)
            {
                var rolesDB = await this.RoleManager.Roles.ToListAsync();//.Where(x => x.Id == role).ToList().FirstOrDefault();
                var roleDB = rolesDB.Where(x => x.Id == role).FirstOrDefault();
                if (roleDB != null)
                {
                    allUsers = users.Where(x => x.Role == roleDB.Name).ToList();
                }
            }
            else
            {
                var filteredUsers = users.Where(x => x.User.DepartmentId == department).ToList();

                var rolesDB = await this.RoleManager.Roles.ToListAsync();//.Where(x => x.Id == role).ToList().FirstOrDefault();
                var roleDB = rolesDB.Where(x => x.Id == role).FirstOrDefault();
                if (roleDB != null)
                {
                    allUsers = filteredUsers.Where(x => x.Role == roleDB.Name).ToList();
                }
            }
            await this.FilterLists();
            return View(allUsers);
        }       

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
            if (this.User.IsInRole("admin"))
            {
                await SelectedListFilling();
                return View();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User cannot be created, because of lack of authorization permissions!");
                await SelectedListFilling();

                return RedirectToAction("Index");
            }            
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(UserDTO model)
        {
            if (ModelState.IsValid)
            {
                var requestUser = _mapper.Map<UserDTO, RequestUser>(model);

                if (!UserManager.Users.Any(x => x.UserName == requestUser.UserName))
                {
                    var result = await this.UserManager.CreateAsync(requestUser, model.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                        await SelectedListFilling();

                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User Is Already Exists!");
                    await SelectedListFilling();

                    return View(model);
                }

                await this.UserManager.AddToRoleAsync(requestUser, model.Role);
                return RedirectToAction("Index");
            }

           await SelectedListFilling();

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (this.User.IsInRole("admin"))
            {
                var user = await this.UserManager.FindByIdAsync(id.ToString());
                if (user != null)
                {
                    var userDTO = this._mapper.Map<RequestUser, UserDTO>(user);
                    await SelectedListFilling();
                    return View(userDTO);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error! User cannot be found!");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User cannot be created, because of lack of authorization permissions!");
            }

            await SelectedListFilling();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(UserDTO user)
        {
            if (ModelState.IsValid)
            {
                var userToDb = this._mapper.Map<UserDTO, RequestUser>(user);
                await this.UserManager.UpdateAsync(userToDb);
                return RedirectToAction("Index");
            }

            await SelectedListFilling();
            return View(user);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await this.UserManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                var result = await this.UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(id);
            }

            return NotFound();
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await this.UserManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                var userDTO = this._mapper.Map<RequestUser, RequestUserDto>(user);

                var department = await this.DataManager.DepartmentRepository.GetByIdAsync(userDTO.DepartmentId ?? 0);

                if (department != null)
                {
                    var departmetnDto = this._mapper.Map<Department, DepartmentDTO>(department);
                    userDTO.Department = departmetnDto;
                }

                return PartialView("UserDetails", userDTO);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        private async Task SelectedListFilling()
        {
            var departments = await DataManager.DepartmentRepository.GetAllAsync();
            var roles = RoleManager.Roles;

            ViewBag.Departments = new SelectList(departments, "Id", "Name");
            ViewBag.Status = new SelectList(roles, "Name", "Name");
        }

        private async Task FilterLists()
        {
            var departmentsDB = await this.DataManager.DepartmentRepository.GetAllAsync();

            List<DepartmentDTO> departments = _mapper.Map<List<Department>, List<DepartmentDTO>>(departmentsDB.ToList());
            departments.Insert(0, new DepartmentDTO() { Name = "All", Id = 0 });
            ViewBag.DepartmentsFilter = new SelectList(departments, "Id", "Name");

            var rolesDB = RoleManager.Roles;
            List<RoleDTO> roles = _mapper.Map<List<RequestRole>, List<RoleDTO>>(rolesDB.ToList());
            roles.Insert(0, new RoleDTO() { Name = "All", Id = 0});
            ViewBag.RolesFilter = new SelectList(roles, "Id", "Name");
        }

        private async Task<List<UserViewModel>> FillTheListOfUsers()
        {
            List<UserViewModel> userViewModels = new List<UserViewModel>();
            var users = await this.DataManager.RequestUserRepository.GetAllAsync();

            foreach (var user in users)
            {
                var userTask = await GetUserWithRoles(user);
                userViewModels.Add(userTask);
            }
            return userViewModels;
        }

        private async Task<UserViewModel> GetUserWithRoles(RequestUser user)
        {
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.User = user;
            var roles = await this.UserManager.GetRolesAsync(user);
            userViewModel.Role = roles.FirstOrDefault() ?? string.Empty;

            return userViewModel;
        }
    }
}