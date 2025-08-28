
namespace HospitalMVC.Models
{
	public class LabOrder
	{
		public int Id { get; set; }
		public int PatientId { get; set; }
		public string Results { get; set; } = "";
		public string TestName { get; set; } = "";
	}
}