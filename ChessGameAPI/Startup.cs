using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChessGameAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChessGameAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IHostingEnvironment CurrentEnvironment { get; } 

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            CurrentEnvironment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string env = CurrentEnvironment.EnvironmentName;
            string connString;
            if (env == "Production")
            {
                connString = Configuration.GetConnectionString("Production");
            } else
            {
                connString = Configuration.GetConnectionString("Development");
            }

            services.AddDbContext<DataContext>(options => options.UseSqlServer(connString));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
