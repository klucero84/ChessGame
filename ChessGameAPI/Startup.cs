using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChessGameAPI.Data;
using ChessGameAPI.Helpers;
using ChessGameAPI.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ChessGameAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IHostingEnvironment env { get; } 

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            env = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("connString"))
                    .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.IncludeIgnoredWarning)));
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.BuildServiceProvider().GetService<DataContext>().Database.Migrate();
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins("http://localhost:4200");
            }));
            services.AddSignalR();
            services.AddAutoMapper();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters =new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context => 
                    {
                        /// <summary>
                        /// log all user activity
                        /// todo: make async and faster since this will be getting called on every token validation
                        /// </summary>
                        int userId = 0;
                        var dbContext = context.HttpContext.RequestServices.GetRequiredService<DataContext>();
                        var userIdClaim = context.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                        var userIdValue = userIdClaim?.Value;
                        int.TryParse(userIdValue, out userId);
                        var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
                        user.LastActive = DateTime.Now;
                        dbContext.SaveChanges();
                        return Task.CompletedTask;

                        
                    }
                };
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(opt => {
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder => {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
                // app.UseHsts();
            }
            app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
            {
                routes.MapHub<MoveHub>("/api/move/ws");
            });
            
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes => {
                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { Controller = "Fallback", action = "Index" }
                );
            });
        }
    }
}
