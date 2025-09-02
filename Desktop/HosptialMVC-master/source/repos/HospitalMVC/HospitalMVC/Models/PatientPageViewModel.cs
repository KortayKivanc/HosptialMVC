namespace HospitalMVC.Models
{
	public class PatientPageViewModel
	{
		public int PatientId { get; set; }
		public string PatientName { get; set; } = "";
		public string PatientEmail { get; set; } = "";
		public string PatientRole { get; set; } = "";
		public IEnumerable<LabOrder> Tests { get; set; } = Enumerable.Empty<LabOrder>();
	}
}
