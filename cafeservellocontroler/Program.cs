using cafeservellocontroler.Data;
using cafeservellocontroler.Helper;
using cafeservellocontroler.Repositorio.FornecedorRepositorio;
using cafeservellocontroler.Repositorio.ItensVendaRepositorio;
using cafeservellocontroler.Repositorio.ProdutoRepositorio;
using cafeservellocontroler.Repositorio.RevendedorRepositorio;
using cafeservellocontroler.Repositorio.UsuarioRepositorio;
using cafeservellocontroler.Repositorio.VendaRepositorio;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Globalization;

namespace cafeservellocontroller
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

                  

            // DbContext
            builder.Services.AddDbContext<BancoContext>(options =>
                options.UseMySQL(builder.Configuration.GetConnectionString("Database")));

            // Repositórios
            builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
            builder.Services.AddScoped<IRevendedorRepositorio, RevendedorRepositorio>();
            builder.Services.AddScoped<IFornecedorRepositorio, FornecedorRepositorio>();
            builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            builder.Services.AddScoped<IVendaRepositorio, VendaRepositorio>();
            builder.Services.AddScoped<IItensVendaRepositorio, ItensVendaRepositorio>();

            // Sessão
            builder.Services.AddHttpContextAccessor();     // CORRETO
            builder.Services.AddScoped<ISessao, Sessao>();

            // Email
            builder.Services.AddScoped<IEmail, Email>();

            builder.Services.AddSession(o =>
            {
                o.Cookie.HttpOnly = true;
                o.Cookie.IsEssential = true;
            });



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

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
