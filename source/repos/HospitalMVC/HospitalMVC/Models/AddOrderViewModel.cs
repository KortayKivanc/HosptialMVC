namespace HospitalMVC.Models
{
	public class AddOrderViewModel
	{
		public int PatientId { get; set; }
		public string TestName { get; set; } = "";
		public string Results { get; set; } = "";
	}
}
