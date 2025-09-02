using Microsoft.AspNetCore.Mvc;
using HospitalMVC.Data;
using HospitalMVC.Models;
using System.Linq;
using HospitalMVC.Services;
using Hangfire;

namespace HospitalMVC.Controllers
{
	public class DoctorController : Controller
	{
		private readonly UserContext _db;
		public DoctorController(UserContext db) => _db = db;

		[HttpGet]
		public IActionResult Patients()
		{
			var vm = new DoctorPageViewModel
			{
				Patients = _db.Users
							  .Where(u => u.Role != null && u.Role.ToLower() == "patient")
							  .OrderBy(u => u.Id)
							  .ToList(),
				AllOrders = _db.LabOrders
							   .OrderByDescending(o => o.Id)
							   .ToList()
			};
			return View(vm);
		}
		[HttpGet]
		public IActionResult Orders(int patientId)
		{
			var patient = _db.Users.FirstOrDefault(u => u.Id == patientId);

			var vm = new DoctorPageViewModel
			{
				Patients = _db.Users	
							  .Where(u => u.Role != null && u.Role.ToLower() == "patient")
							  .OrderBy(u => u.Id)
							  .ToList(),
				AllOrders = _db.LabOrders
							   .OrderByDescending(o => o.Id)
							   .ToList(),

				SelectedPatientId = patientId,
				SelectedPatientName = patient?.Name ?? $"Patient #{patientId}",
				SelectedPatientOrders = _db.LabOrders
					.Where(o => o.PatientId == patientId && o.IsReleased)
					.OrderByDescending(o => o.Id)
					.ToList(),	

				AddOrder = new AddOrderViewModel { PatientId = patientId }
			};

			return View("Patients", vm);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CreateOrder([Bind(Prefix = "AddOrder")] AddOrderViewModel m)
		{
			if (string.IsNullOrWhiteSpace(m.TestName) || string.IsNullOrWhiteSpace(m.Results))
				return RedirectToAction("Orders", new { patientId = m.PatientId });

			bool releaseNow = !m.Date.HasValue;

			var order = new LabOrder
			{
				PatientId = m.PatientId,
				TestName = m.TestName.Trim(),
				Results = m.Results.Trim(),
				IsReleased = releaseNow,
				ReleaseAt = m.Date?.Date
			};

			_db.LabOrders.Add(order);
			_db.SaveChanges();

			if (!releaseNow)
			{
 				var runAt = m.Date!.Value.Date.AddHours(9);
				var delay = runAt - DateTime.Now;
				if (delay < TimeSpan.Zero) delay = TimeSpan.Zero;

				BackgroundJob.Schedule<IOrderPublisher>(
					s => s.ReleaseOrder(order.Id),
					delay);
			}

			return RedirectToAction("Orders", new { patientId = m.PatientId });
		}


	}
}
