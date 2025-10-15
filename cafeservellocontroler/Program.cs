using cafeservellocontroler.Data;
using cafeservellocontroler.Repositorio.ProdutoRepositorio;
using cafeservellocontroler.Repositorio.RevendedorRepositorio;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace cafeservellocontroller
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            // Configura o DbContext + SQL Server
            builder.Services.AddDbContext<BancoContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
            builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();

            builder.Services.AddDbContext<BancoContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
            builder.Services.AddScoped<IRevendedorRepositorio, RevendedorRepositorio>();

            var app = builder.Build(); // só depois de registrar tudo

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
