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
              //Aspnet Core'un Connection String baðlantýsý yapabilmesi için 
            //servislerine dbcontext eklenmesi gerekir.
            services.AddDbContext<MyContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));
            }) ;

            //IUnitOfWork gördüðün zaman bana UnitOfWork nesnesi üret!
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //IEmailSender gördüðün zaman bana EmailSender nesnesi üret!
            services.AddScoped<IEmailSender, EmailSender>();

           
            services.AddControllersWithViews().AddRazorRuntimeCompilation();//calýþýrken razor sayfasýnda yapýlan deðiþikliklerin sayfaya yansýmasý için ekledik.


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

            app.UseRouting(); //rooting mekanizmasý için

            app.UseSession();

            app.UseAuthentication(); //login logout kullanabilmek için 
            app.UseAuthorization(); //authorization attiribute kullanabilmek için


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
