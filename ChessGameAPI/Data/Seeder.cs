using System.Collections.Generic;
using ChessGameAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ChessGameAPI.Data
{
    public class Seeder
    {
        private readonly DataContext _context;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context">Datacontext to seed with data</param>
        public Seeder(DataContext context)
        {
            _context = context;
            
        }

        /// <summary>
        /// Main seed process
        /// </summary>
        public void SeedUsers(){
            var userData = System.IO.File.ReadAllText("Data/generated.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            foreach (var user in users)
            {
                byte[] passwordHash, passwordSalt; 
                CreatePasswordHash("password", out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                _context.Users.Add(user);
            }
            _context.SaveChanges();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            };
        }
    }
}