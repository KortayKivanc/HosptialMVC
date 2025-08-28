using HospitalMVC.Data;
using HospitalMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HospitalMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly UserContext _db;

		public HomeController(ILogger<HomeController> logger, UserContext db)
        {
            _logger = logger;
			_db = db;
		}

		

		public IActionResult Index()
        {
            return View();
        }
		

		public IActionResult Privacy()
        {
            return View();
        }

		[HttpGet("register")]
		public IActionResult Register() => View(new RegisterViewModel());



		[HttpPost("register")]
        public IActionResult Register(RegisterViewModel model)
        {
			if (!ModelState.IsValid) return View(model);
			if (_db.Users.Any(u => u.Email == model.Email))
			{
				ViewBag.Error = "Email is already in use.";
				return View(model);
			}
			var user = new User
			{

				Name = model.Name.Trim(),
				Role = model.Role.Trim(),
				Email = model.Email.Trim(),
				Password = model.Password  
			};
			_db.Users.Add(user);
			_db.SaveChanges();
			return RedirectToAction("Login");
		}

		[HttpGet("login")]
		public IActionResult Login() => View(new LoginViewModel());
        [HttpPost("login")]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _db.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password";
                return View(model);
            }
            TempData["LoggedInUser"] = user.Name;
			if (user.Role.Equals("Patient", StringComparison.OrdinalIgnoreCase))
			{
				return RedirectToAction("Reports", "Patient", new { patientId = user.Id });
			}
			else if (user.Role.Equals("Doctor", StringComparison.OrdinalIgnoreCase))
			{
				return RedirectToAction("Patients", "Doctor"); 
			}
			else
			{
				ViewBag.Error = "Unknown role.";
				return View(model);
			}
		}

		


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
