using System;

namespace ChessGameAPI.Dtos
{
    /// <summary>
    /// dto for photo in user detail
    /// </summary>
    public class PhotosForDetailDto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Caption { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DateTime DateAdded { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public bool IsMain { get; set; }
    }
}