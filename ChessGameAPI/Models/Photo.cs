using System;
using System.ComponentModel.DataAnnotations;

namespace ChessGameAPI.Models
{
    public class Photo
    {
        public int Id { get; set; }

        [Required]
        public string URL { get; set; }

        public string Caption { get; set; }

        public DateTime DateAdded { get; set; }

        [Required]        
        public bool IsMain { get; set; }
    }
}