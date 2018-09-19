using System.Collections.Generic;

namespace ChessGameAPI.Dtos
{
    public class GameDto
    {

        public int Id { get; set; }
        
        public IEnumerable<MoveDto> Moves { get; set; }

        public IEnumerable<PieceDto> Pieces { get; set; }

        public UserForDetailDto WhiteUser { get; set; }

        public UserForDetailDto BlackUser { get; set; }
    }
}