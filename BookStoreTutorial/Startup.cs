using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreTutorial.Models.DataLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookStoreTutorial
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
            //Enable MVC
            services.AddControllersWithViews();

            //Enable MVC with NewtonSoftJson (use Json to serialize/deserialize objects in sessions)
            services.AddControllersWithViews().AddNewtonsoftJson();

            //Lowercase routing
            services.AddRouting(options => options.LowercaseUrls = true);

            //Enable web browser caching
            services.AddMemoryCache();

            //Enable sessions
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(60 * 5);     //5 minute timeout
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = false;
            });

            //Add DbContext
            services.AddDbContext<BookstoreContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BookstoreContext"))
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Order matters
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();           //Enable sessions
            app.UseAuthorization();

            //endpoints order matters. Go from specific to general
            app.UseEndpoints(endpoints =>
            {
                //Admin
                endpoints.MapAreaControllerRoute(
                    name: "admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Book}/{action=Index}/{id?}"
                );

                //Paging, sorting, filtering
                endpoints.MapControllerRoute(
                    name: "",
                    pattern: "{controller}/{action}/page/{pagenumber}/size/{pagesize}/sort/{sortfield}/{sortdirection}/filter/{author}/{genre}/{price}"
                );

                //Paging, sorting
                endpoints.MapControllerRoute(
                    name: "",
                    pattern: "{controller}/{action}/page/{pagenumber}/size/{pagesize}/sort/{sortfield}/{sortdirection}"
                );

                //Default
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}/{slug?}");
            });
        }
    }
}
