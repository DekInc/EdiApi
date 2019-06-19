using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdiApi.Models.EdiDB;
using EdiApi.Models.WmsDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
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
            services.AddDbContext<EdiDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EdiDB")));
            services.AddDbContext<EdiDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EdiDBLong")));
            services.AddDbContext<WmsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("wmsdb")));
            services.AddDbContext<WmsContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("wmsdbLong")));
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Hilmer Campos",
                        Email = "Hilmer.Campos@GlcAmerica.com"
                    }
                });
            });
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
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api.docs/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                //Include virtual directory if site is configured so
                c.RoutePrefix = "api.docs";
                c.SwaggerEndpoint("./v1/swagger.json", "Api v1");
            });
        }
    }
}
