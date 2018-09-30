using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGameAPI.Models
{
    public class User
    {
        public User()
        {
            _lastActive = DateTime.Now;
        }
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        private DateTime _dateJoined;
        [Required]
        public DateTime DateJoined {
            get 
            { 
                return _dateJoined == null ? 
                DateTime.Now : _dateJoined;
            }
            set { _dateJoined = value; }
        }

        private DateTime _lastActive;
        
        [Required]
        public DateTime LastActive {
            get 
            {
                return _lastActive == null ?
                DateTime.Now : _lastActive;
            }
            set { _lastActive = value; }
        }

        public DateTimeOffset? utcOffset { 
            get; 
            set; 
        }

        public IEnumerable<Photo> Photos { get; set; }

        public IEnumerable<Piece> Pieces { get; set; }

        public IEnumerable<Move> Moves { get; set; }

        public ICollection<Message> MessagesSent { get; set; }
        
        public ICollection<Message> MessagesReceived { get; set; }
    }
}
