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

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


        //public bool IsInCheck { get; set; }
    }

    //public enum UserColor
    //{
        //WHITE = 0,
        //BLACK = 1
    //}
}
