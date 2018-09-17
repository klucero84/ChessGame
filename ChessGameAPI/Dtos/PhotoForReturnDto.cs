namespace ChessGameAPI.Dtos
{
    /// <summary>
    /// Return information for photo post cloudinary creation
    /// </summary>
    public class PhotoForReturnDto : PhotosForDetailDto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string PublicId { get; set; }
        
    }
}