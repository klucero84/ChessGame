namespace ChessGameAPI.Dtos
{
    /// <summary>
    /// dto for adding a move
    /// </summary>
    public class MoveForAddMoveDto
    {
        public int Id { get; set; }
        
        public string PieceDiscriminator { get; set; }

        public int StartX { get; set; }

        public int StartY { get; set; }

        public int EndX { get; set; }

        public int EndY { get; set; }

        public int GameId { get; set; }

        public int PieceId { get; set; }

        public int UserId { get; set; }

    
    }
}