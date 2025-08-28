using HospitalMVC.Data;

namespace HospitalMVC.Services
{
	public class ResultSender
	{
		private readonly UserContext _db;

		public ResultSender(UserContext db)
		{
			_db = db;
		}

		public void SendResult(int patientId, int orderId)
		{
			var user = _db.Users.FirstOrDefault(u => u.Id == patientId);
			var order = _db.LabOrders.FirstOrDefault(o => o.Id == orderId);
			if (user == null || order == null) return;

			Console.WriteLine($"[Hangfire] {user.Email} | Test={order.TestName} | Result={order.Results}");
		}
	}
}
