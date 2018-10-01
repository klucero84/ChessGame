using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using ChessGameAPI.Controllers;
using ChessGameAPI.Data;
using ChessGameAPI.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

        public static string GetNotation(this MoveController controller, MoveForAddMoveDto dto) {
            string[] letters = {"a", "b", "c", "d", "e", "f", "g", "h"};
            StringBuilder sb = new StringBuilder();

            if (dto.IsCastle) {
                if (dto.EndX == 2) {
                    sb.Append("0-0-0");
                } else {
                    sb.Append("0-0");
                }
                return sb.ToString();
            }
            if (dto.PieceDiscriminator == "Knight") {
                sb.Append("N");
            } else if (!String.IsNullOrEmpty(dto.PieceDiscriminator) && dto.PieceDiscriminator != "Pawn") {
                sb.Append(dto.PieceDiscriminator[0].ToString());
            }
            sb.Append(letters[dto.StartX] + (dto.StartY + 1).ToString());
            if (dto.IsCapture) {
                sb.Append("x");
            } else {
                sb.Append("-");
            }
            sb.Append(letters[dto.EndX] + (dto.EndY + 1).ToString());
            if (!String.IsNullOrEmpty(dto.PromoteTo)) {
                if (dto.PromoteTo == "Knight") {
                sb.Append("=N");
                } else {
                    sb.Append("=" + dto.PromoteTo[0].ToString());
                }
            }
            return sb.ToString();
        }
    }
}