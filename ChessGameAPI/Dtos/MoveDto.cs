namespace ChessGameAPI.Dtos
{
    /// <summary>
    /// dto for historic move infomation
    /// </summary>
    public class MoveDto
    {
        public int Id { get; set; }

        public PieceDto Piece { get; set; }

        public UserForDetailDto User { get; set; }

        public int StartX { get; set; }

        public int StartY { get; set; }

        public int EndX { get; set; }

        public int EndY { get; set; }
    }
}