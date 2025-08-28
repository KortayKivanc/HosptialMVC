using Hangfire;
using Hangfire.MySql;
using HospitalMVC.Data;
using HospitalMVC.Models;
using HospitalMVC.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Mail;
using System.Transactions;


namespace HospitalMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			var conn = "server=127.0.0.1;Allow User Variables=True;port=3306;database=hospitaldb;user=hospuser;password=StrongPass123!;";

			builder.Services.AddDbContext<UserContext>(opt =>
							opt.UseMySql(conn, ServerVersion.AutoDetect(conn)));

			var options = new MySqlStorageOptions
			{
				TransactionIsolationLevel = IsolationLevel.ReadCommitted,
				QueuePollInterval = TimeSpan.FromSeconds(15),
				JobExpirationCheckInterval = TimeSpan.FromHours(1),
				CountersAggregateInterval = TimeSpan.FromMinutes(5),
				PrepareSchemaIfNecessary = true
			};

			builder.Services.AddScoped<DoctorScreen>();
			builder.Services.AddScoped<ResultSender>();
			builder.Services.AddScoped<IBackgroundJobClient, BackgroundJobClient>();

			builder.Services.AddHangfire(cfg =>
				cfg.UseSimpleAssemblyNameTypeSerializer()
				   .UseRecommendedSerializerSettings()
				   .UseStorage(new MySqlStorage(conn, options)));

			builder.Services.AddHangfireServer();
			builder.Services.AddControllersWithViews();

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
			app.UseRouting();
			app.UseHangfireDashboard("/hangfire");
			app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
			app.MapGet("/", context =>
			{
				context.Response.Redirect("/login");
				return Task.CompletedTask;
			});

			app.Run();
        }
    }
}
