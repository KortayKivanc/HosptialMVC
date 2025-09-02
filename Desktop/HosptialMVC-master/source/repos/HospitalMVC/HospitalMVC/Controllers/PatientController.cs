using HospitalMVC.Data;
using HospitalMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HospitalMVC.Controllers
{
	public class PatientController : Controller
	{
		private readonly UserContext _db;
		public PatientController(UserContext db) => _db = db;

		[HttpGet] 
		public IActionResult Reports(int patientId)
		{
			var p = _db.Users.FirstOrDefault(u => u.Id == patientId);
			var tests = _db.LabOrders
	.Where(x => x.PatientId == patientId && x.IsReleased)
	.ToList();

			var vm = new PatientPageViewModel
			{
				PatientId = patientId,
				PatientName = p?.Name ?? "Unknown",
				PatientEmail = p?.Email ?? "",
				PatientRole = p?.Role ?? "",
				Tests = tests
			};

			return View("PatientPage", vm);  
		}

		[HttpPost]
		public IActionResult PatientPage(int patientId)
		{
			var p = _db.Users.FirstOrDefault(u => u.Id == patientId);
			var tests = _db.LabOrders
	.Where(x => x.PatientId == patientId && x.IsReleased)
	.ToList();

			var vm = new PatientPageViewModel
			{
				PatientId = patientId,
				PatientName = p?.Name ?? "Unknown",
				PatientEmail = p?.Email ?? "",
				PatientRole = p?.Role ?? ""
				,
				Tests = tests
			};

			return View(vm); 
		}
	}
}
