using Microsoft.AspNetCore.Mvc;
using HospitalMVC.Data;
using HospitalMVC.Models;
using System.Linq;

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
										   .Where(o => o.PatientId == patientId)
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

			var order = new LabOrder { PatientId = m.PatientId, TestName = m.TestName.Trim(), Results = m.Results.Trim() };
			_db.LabOrders.Add(order);
			_db.SaveChanges();

			return RedirectToAction("Orders", new { patientId = m.PatientId });
		}

	}
}
