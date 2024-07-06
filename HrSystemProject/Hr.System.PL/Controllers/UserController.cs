using HrSystem.DAL.Entities;
using HrSystem.DAL.ViewModel;
using HrSystem.PL.Models;
using HRSystem.BLL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Hr.System.PL.Controllers
{
    [Authorize]

    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<UserController> logger;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(UserManager<ApplicationUser> userManager,ILogger<UserController> logger , IHttpContextAccessor httpContextAccessor
            ,RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> GetUsers(string SearchValue = "")
        {
            List<ApplicationUser> users;
            if (String.IsNullOrEmpty(SearchValue))
            {
                users = await userManager.Users.Include(c=>c.CreationUsers).Include(c => c.ModificarionUsers).ToListAsync();
            }
            else
            {
                users=await userManager.Users.Where(x=>x.UserName.ToLower().Trim()
                .Contains(SearchValue.ToLower().Trim())).ToListAsync();
            }
            return View(users);
        }
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var user = await userManager.Users
                            .Include(u => u.CreationUsers)
                            .Include(u => u.ModificarionUsers)
                            .FirstOrDefaultAsync(u => u.Id == id);
                return View(user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateUser()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
            SignUpViewModel model=new SignUpViewModel();
            model.Roles= await roleManager.Roles.ToListAsync();
            model.CreatorId = userId;
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(SignUpViewModel input, string[] selectedRoles)
        {
            var user = new ApplicationUser
            {
                Email= input.Email,
                UserName= input.UserName,
                PhoneNumber= input.PhoneNumber,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatorId= input.CreatorId,
            };
            var result = await userManager.CreateAsync(user, input.Password);
            
            if (result.Succeeded)
            {
                foreach (var role in selectedRoles)
                {
                    var roleResult = await userManager.AddToRoleAsync(user, role);
                    if (!roleResult.Succeeded)
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            logger.LogError(error.Description);
                            ModelState.AddModelError("", error.Description);
                        }
                    }

                }
                return RedirectToAction("GetUsers");
            }

            foreach (var error in result.Errors)
            {
                logger.LogError(error.Description);
                ModelState.AddModelError("", error.Description);
            }
            input.Roles = await roleManager.Roles.ToListAsync();
            return View(input);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(string id)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var user = await userManager.FindByIdAsync(id);
                var allRoles = await roleManager.Roles.ToListAsync();
                var userRoles = await userManager.GetRolesAsync(user);
                var userRolesDto = allRoles.Select(role => new UserRolesDto
                {
                    UserId = user.Id,
                    RoleId = role.Id,
                    value = userRoles.Contains(role.Name)
                }).ToList();
                SignUpViewModel model = new SignUpViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    IsActive=user.IsActive,
                    ModifierId= userId,
                    Roles = allRoles,
                    UserRoles = userRolesDto
                };
               
                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(SignUpViewModel model, string[] selectedRoles)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.IsActive = model.IsActive;
            user.ModifiedBy = model.ModifierId; //modifier
            user.ModificationTime=DateTime.Now;
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var currentRoles = await userManager.GetRolesAsync(user);
                var rolesToAdd = selectedRoles.Except(currentRoles).ToList();
                var rolesToRemove = currentRoles.Except(selectedRoles).ToList();
                if (rolesToAdd.Any())
                {
                    await userManager.AddToRolesAsync(user, rolesToAdd);
                }

                if (rolesToRemove.Any())
                {
                    await userManager.RemoveFromRolesAsync(user, rolesToRemove);
                }
                return RedirectToAction("GetUsers");
            }

            foreach (var error in result.Errors)
            {
                logger.LogError(error.Description);
                ModelState.AddModelError("", error.Description);
            }
            model.Roles = await roleManager.Roles.ToListAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
           
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("GetUsers");
            }

            foreach (var error in result.Errors)
            {
                logger.LogError(error.Description);
                ModelState.AddModelError("", error.Description);
            }
            return View(user);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
