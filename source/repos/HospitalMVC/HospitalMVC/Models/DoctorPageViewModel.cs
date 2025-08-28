namespace HospitalMVC.Models
{
	public class DoctorPageViewModel
	{
		public IEnumerable<User> Patients { get; set; } = new List<User>();
		public IEnumerable<LabOrder> AllOrders { get; set; } = new List<LabOrder>();

		public int? SelectedPatientId { get; set; }
		public string? SelectedPatientName { get; set; }
		public IEnumerable<LabOrder> SelectedPatientOrders { get; set; } = new List<LabOrder>();

		public AddOrderViewModel AddOrder { get; set; } = new AddOrderViewModel();
	}
}
