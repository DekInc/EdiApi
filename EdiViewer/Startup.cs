using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdiViewer.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EdiViewer
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/";
                });
            services.AddMemoryCache();            
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(25);
                options.Cookie.IsEssential = true;
            });
            services.AddDistributedMemoryCache();
            services.AddMvc(options => {
                options.InputFormatters.Insert(0, new RawJsonBodyInputFormatter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            ApplicationSettings.ApiUri = (string)Configuration.GetSection("EdiWebApi").GetValue(typeof(string), "ApiUri");
            services.AddSingleton<Utility.Scheduling.Interfaces.IScheduledTask, Utility.Scheduling.GetEdi830Task>();
            services.AddSingleton<Utility.Scheduling.Interfaces.IScheduledTask, Utility.Scheduling.AutoSendInventary830Task>();
            services.AddScheduler();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSession();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseDeveloperExceptionPage();
                app.UseHsts();
            }            
            app.UseHttpsRedirection();
            app.UseStaticFiles();            
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
