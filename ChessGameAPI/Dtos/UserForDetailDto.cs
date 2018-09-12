using System;
using System.Collections.Generic;

namespace ChessGameAPI.Dtos
{
    /// <summary>
    /// dto for user detail 
    /// </summary>
    public class UserForDetailDto
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

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public ICollection<PhotosForDetailDto> Photos { get; set; }
    }
}