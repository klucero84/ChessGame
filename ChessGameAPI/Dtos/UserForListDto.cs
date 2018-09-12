using System;

namespace ChessGameAPI.Dtos
{
    /// <summary>
    /// dto for listing users
    /// </summary>
    public class UserForListDto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public int Id {get; set;}

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DateTime Created { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DateTime LastActive { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string PhotoUrl { get; set; }
    }
}