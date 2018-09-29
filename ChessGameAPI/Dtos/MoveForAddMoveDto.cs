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

        public bool IsWhite { get; set; }
        
        public bool IsCastle { get; set; }

        public bool IsCapture { get; set; }

        // public bool Concede { get; set; }

        // public bool OfferDraw { get; set; }

        public string connId { get; set; }

    
    }
}