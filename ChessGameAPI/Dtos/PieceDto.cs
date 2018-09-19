namespace ChessGameAPI.Dtos
{
    /// <summary>
    /// dto for piece info
    /// </summary>
    public class PieceDto
    {
        public int Id { get; set; }

        public string Discriminator { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public UserForDetailDto OwnedBy { get; set; }

    }
}