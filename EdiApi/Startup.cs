using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EdiApi
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<Models.EdiDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EdiDB"), opt => opt.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)));
            services.AddDbContext<Models.wmsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("wmsDB"), opt => opt.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)));
            services.AddDbContext<Models.Remps_globalDB.Remps_globalContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Remps_globalDB"), opt => opt.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)));
            services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
