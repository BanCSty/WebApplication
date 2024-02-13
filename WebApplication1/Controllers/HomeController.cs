using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebShop.DAL.Interfaces;

namespace WebApplication.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {       
        public IActionResult Index()
        {
            return View();
        }
    }
}
