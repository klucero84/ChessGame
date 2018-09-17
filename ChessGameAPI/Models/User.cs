using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGameAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        [Required]
        public DateTime DateJoined { get; set; }

        [Required]
        public DateTime LastActive { get; set; }

        public ICollection<Photo> Photos { get; set; }

        //public bool IsInCheck { get; set; }
    }

    //public enum UserColor
    //{
        //WHITE = 0,
        //BLACK = 1
    //}
}
