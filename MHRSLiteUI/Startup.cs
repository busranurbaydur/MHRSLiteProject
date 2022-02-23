using MHRSLiteBusiness.Contracts;
using MHRSLiteBusiness.EmailService;
using MHRSLiteBusiness.Implementations;
using MHRSLiteDataAccess;
using MHRSLiteEntity.IdentityModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHRSLiteUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
              //Aspnet Core'un Connection String ba�lant�s� yapabilmesi i�in 
            //servislerine dbcontext eklenmesi gerekir.
            services.AddDbContext<MyContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));
            }) ;

            //IUnitOfWork g�rd���n zaman bana UnitOfWork nesnesi �ret!
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //IEmailSender g�rd���n zaman bana EmailSender nesnesi �ret!
            services.AddScoped<IEmailSender, EmailSender>();

           
            services.AddControllersWithViews().AddRazorRuntimeCompilation();//cal���rken razor sayfas�nda yap�lan de�i�ikliklerin sayfaya yans�mas� i�in ekledik.


            services.AddRazorPages();
            services.AddMvc();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(60);
            });

            services.AddIdentity<AppUser, AppRole>(opts=> 
            {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireLowercase = false;
                opts
                .Password.RequireDigit = false;
                opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<MyContext>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
       
            app.UseStaticFiles();

            app.UseRouting(); //rooting mekanizmas� i�in

            app.UseSession();

            app.UseAuthentication(); //login logout kullanabilmek i�in 
            app.UseAuthorization(); //authorization attiribute kullanabilmek i�in


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    "management",
                    "management",
                    "management/{controller=Admin}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
