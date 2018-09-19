namespace ChessGameAPI.Dtos
{
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