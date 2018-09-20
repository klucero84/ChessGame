using System;
using System.Collections.Generic;

namespace ChessGameAPI.Dtos
{
    /// <summary>
    /// dto for game info
    /// </summary>
    public class GameDto
    {

        /// <summary>
        /// unique identifier of game
        /// </summary>
        /// <value>integer</value>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DateTime DateCompleted { get; set; }
        
        /// <summary>
        /// IEnumerable collection of dtos for moves
        /// </summary>
        /// <value>IEnumerable collection of dtos for moves.</value>
        public IEnumerable<MoveDto> Moves { get; set; }

        /// <summary>
        /// IEnumerable collection of dtos for pieces.
        /// </summary>
        /// <value>IEnumerable of PieceDto objects</value>
        public IEnumerable<PieceDto> Pieces { get; set; }

        /// <summary>
        /// dto of details for white user.
        /// </summary>
        /// <value>UserForDetailDto object</value>
        public UserForDetailDto WhiteUser { get; set; }

        /// <summary>
        /// dto of details for the black user.
        /// </summary>
        /// <value>UserForDetailDto object</value>
        public UserForDetailDto BlackUser { get; set; }
    }
}