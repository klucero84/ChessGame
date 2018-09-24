using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using ChessGameAPI.Controllers;
using ChessGameAPI.Data;
using ChessGameAPI.WebSocketManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ChessGameAPI.Helpers
{
    /// <summary>
    /// Utility class for Extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Adds headers for application error to the http response
        /// Adds Access control allow origin headers
        /// </summary>
        /// <param name="response">http response to add the headers to</param>
        /// <param name="message">error message to include in application-error header</param>
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static int GetCurrentUserId(this ControllerBase baseController)
        {
            int userId = 0;
            if (baseController.User.Identity.IsAuthenticated)
            {
                var claimsIdentity = baseController.User.Identity as ClaimsIdentity;
                var userIdClaim = claimsIdentity?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                var userIdValue = userIdClaim?.Value;
                int.TryParse(userIdValue, out userId);
            }
            return userId;
        }  


        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app, PathString path, WebSocketHandler handler)
        {
            return app.Map(path, (_app) => _app.UseMiddleware<WebSocketManagerMiddleware>(handler));
        }

        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<WebSocketConnectionManager>();

            foreach(var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if(type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
                {
                    services.AddSingleton(type);
                }
            }

            return services;
        }
    }
}