using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessGame.Server.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public UserColor Color { get; set; }

        public bool IsInCheck { get; set; }
    }

    public enum UserColor
    {
        WHITE = 0,
        BLACK = 1
    }
}
