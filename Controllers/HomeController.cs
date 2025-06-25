using Microsoft.AspNetCore.Mvc;
using WhistleblowingApp.Data;

namespace WhistleblowingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DownloadPDF()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdf", "whistleblowing.pdf");
            return PhysicalFile(filePath, "application/pdf", "whistleblowing.pdf");
        }
    }
}