using HospitalMVC.Data;
using HospitalMVC.Models;

namespace HospitalMVC.Services
{
	public class OrderPublisher : IOrderPublisher
	{
		private readonly UserContext _db;

		public OrderPublisher(UserContext db)
		{
			_db = db;
		}

		public void ReleaseOrder(int orderId)
		{
			var order = _db.LabOrders.Find(orderId);
			if (order == null) return;

			order.IsReleased = true;
			_db.SaveChanges();
		}
	}
}