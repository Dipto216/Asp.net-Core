using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    public class DashboardController : Controller
    {
        public readonly LoginContext _context;

        public DashboardController(LoginContext context)
        {
            _context = context;
        }
        [AllowAnonymous]

        public IActionResult Index()
        {
            IEnumerable<PersonModel> objCatlist = _context.Persons;
            return View(objCatlist);
        }
        public IActionResult Person()
        {
            return View();
        }

        
    }
}
