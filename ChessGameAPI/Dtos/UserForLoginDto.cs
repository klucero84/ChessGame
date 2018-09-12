using System.ComponentModel.DataAnnotations;

namespace ChessGameAPI.Dtos
{
    /// <summary>
    /// dto for the login process
    /// </summary>
    public class UserForLoginDto
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [Required]
        public string Password { get; set; }
    }
}