using IdentityApp.Models;
using IdentityApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IdentityTestDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(IdentityTestDbContext db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        { 
            return View();
        }

        public IActionResult GetUsers()
        {
            var r = _userManager.Users.Count();
            return View(_userManager.Users.ToList());
        }

        public IActionResult GetRoles()
        {
            return View(_roleManager.Roles.ToList());
        }

        public async Task<IActionResult> CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(IdentityRole role)
        {
            await _roleManager.CreateAsync(role);

            return View();
        }

    }
}
