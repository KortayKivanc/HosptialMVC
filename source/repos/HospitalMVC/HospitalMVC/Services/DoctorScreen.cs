using Hangfire;
using HospitalMVC.Data;
using HospitalMVC.Models;

namespace HospitalMVC.Services
{
	public class DoctorScreen
	{
		private readonly UserContext _db;
		private readonly IBackgroundJobClient _jobs;

		public DoctorScreen(UserContext db, IBackgroundJobClient jobs)
		{
			_db = db;
			_jobs = jobs;
		}

		public List<User> GetPatients()
		{
			return _db.Users
					  .Where(u => u.Role != null && u.Role.ToLower() == "patient")
					  .OrderBy(u => u.Id)
					  .ToList();
		}

		public List<LabOrder> GetOrders()
		{
			return _db.LabOrders
					  .OrderBy(o => o.Id)
					  .ToList();
		}

		public void CreateOrder(int patientId, string testName, string result)
		{
			var order = new LabOrder
			{
				PatientId = patientId,
				TestName = testName?.Trim(),
				Results = result?.Trim()
			};
			_db.LabOrders.Add(order);
			_db.SaveChanges();
		}

		public void QueuePatientReport(int patientId, int orderId, int delayMinutes)
		{
			_jobs.Schedule<ResultSender>(s => s.SendResult(patientId, orderId),
										 TimeSpan.FromMinutes(delayMinutes));
		}
	}
}
