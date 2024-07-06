using Hr.System.PL.Models;
using HrSystem.DAL.Entities;
using HrSystem.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hr.System.PL.Controllers
{
    [Authorize]

    public class RoleController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RoleController> logger;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public RoleController(UserManager<ApplicationUser> userManager,
            ILogger<RoleController> logger,RoleManager<ApplicationRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.roleManager = roleManager;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> GetRoles(string SearchValue = "")
        {
            List<ApplicationRole> roles;
            if (String.IsNullOrEmpty(SearchValue))
            {
                roles = await roleManager.Roles.Include(x=>x.CreationRoles).Include(x=>x.ModificarionRoles).ToListAsync();
            }
            else
            {
                roles = await roleManager.Roles.Where(x => x.Name.ToLower().Trim()
                .Contains(SearchValue.ToLower().Trim())).ToListAsync();
            }
            return View(roles);
        }
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(ApplicationRole role, List<string> Permissions)
        {

            var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role.CreatorId=userId;
            role.CreatedAt = DateTime.Now;
            var result = await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                foreach (var permission in Permissions)
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
                return RedirectToAction("GetRoles");
            }
            return View(role);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateRole(string id)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(id);
                var claims = await roleManager.GetClaimsAsync(role);
                var permissions = claims.Where(c => c.Type == "Permission").Select(c => c.Value).ToList();

                RoleViewModel roleViewModel = new RoleViewModel()
                {
                    Id = id,
                    Name = role.Name,
                    Permissions=permissions
                };
                return View(roleViewModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }
        [HttpPost]
        public async Task<IActionResult> UpdateRole(RoleViewModel model)
        {
            var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = await roleManager.FindByIdAsync(model.Id);
            role.Name = model.Name;
            role.ModificationTime = DateTime.Now;
            role.ModifiedBy = userId;
            var result = await roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                var existingClaims = await roleManager.GetClaimsAsync(role);
                var existingPermissions = existingClaims.Where(c => c.Type == "Permission").Select(c => c.Value).ToList();

                var permissionsToRemove = existingPermissions.Except(model.Permissions).ToList();
                var permissionsToAdd = model.Permissions.Except(existingPermissions).ToList();

                foreach (var permission in permissionsToRemove)
                {
                    await roleManager.RemoveClaimAsync(role, new Claim("Permission", permission));
                }

                foreach (var permission in permissionsToAdd)
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
                return RedirectToAction("GetRoles");
            }

            foreach (var error in result.Errors)
            {
                logger.LogError(error.Description);
                ModelState.AddModelError("", error.Description);
            }
            return View(model);

        }
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role= await roleManager.FindByIdAsync(id);
            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("GetRoles");
            }
            return View(role);

        }
        [HttpGet]
        public async Task<IActionResult> GetRoleUsers(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role is null)
            {
                return NotFound();
            }

            ViewBag.RoleId = role.Id;
            var usersInRole = new List<UserInRoleViewModel>();
            var users = await userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var userInRole = new UserInRoleViewModel()
                {
                    UserId=user.Id,   
                    UserName = user.UserName,
                    Email=user.Email,
                    PhoneNumber=user.PhoneNumber
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                    userInRole.IsSelected = true;
                else
                    userInRole.IsSelected = false;
                usersInRole.Add(userInRole);
            }
            return View(usersInRole);

        }
        [HttpPost]
        public async Task<IActionResult> DeleteUserInRole(string roleId,string userId)
        {
            var user=await userManager.FindByIdAsync(userId);
            var role = await roleManager.FindByIdAsync(roleId);
            var result = await userManager.RemoveFromRoleAsync(user,role.Name);
            if (user == null || role == null)
            {
                return NotFound();
            }
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(GetRoleUsers), new { id = roleId });
            }
            foreach (var error in result.Errors)
            {
                logger.LogError(error.Description);
                ModelState.AddModelError("", error.Description);
            }
            return RedirectToAction(nameof(GetRoleUsers), new { id = roleId });
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
