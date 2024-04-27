using ShopSavvy.Models;
using Microsoft.EntityFrameworkCore;

namespace ShopSavvy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15); // Set session timeout
            });

            builder.Services.AddDbContext<ToyStoreDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ToyDBConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=UserLogin}/{id?}");

            app.Run();
        }
    }
}