using HospitalMVC.Data;
using HospitalMVC.Models;

namespace HospitalMVC.Services
{
	public class PatientScreen
	{
		private readonly UserContext _db;

		public PatientScreen(UserContext db)
		{
			_db = db;
		}

		public List<LabOrder> GetPatientReports(int patientId)
		{
			return _db.LabOrders
					  .Where(r => r.PatientId == patientId && r.IsReleased)
					  .OrderByDescending(r => r.Id)
					  .ToList();
		}
	}
}
