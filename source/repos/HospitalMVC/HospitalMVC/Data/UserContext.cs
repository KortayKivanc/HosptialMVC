using HospitalMVC.Models; 

using Microsoft.EntityFrameworkCore;





namespace HospitalMVC.Data
{
	public class UserContext : DbContext
	{
		public UserContext(DbContextOptions<UserContext> options) : base(options) { }  


		public DbSet<User> Users { get; set; }
		public DbSet<LabOrder> LabOrders { get; set; }

		
	}
}



