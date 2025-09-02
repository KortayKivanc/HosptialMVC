namespace HospitalMVC.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Email { get; set; } = "";
		public string Password { get; set; } = "";
		public string Role { get; set; } = "Patient";
		public string Name { get; set; } = "";
	}
}