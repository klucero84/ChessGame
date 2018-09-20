using System.Linq;
using AutoMapper;
using ChessGameAPI.Dtos;
using ChessGameAPI.Models;

namespace ChessGameAPI.Helpers
{
    /// <summary>
    /// profile for automapper utility
    /// </summary>
    public class AutoMapperProfiles : Profile
    {
        /// <summary>
        /// profiles for mapping models to dtos
        /// </summary>
        public AutoMapperProfiles()
        {
            //model to dto
            CreateMap<User, UserForListDto>().ForMember(dest => dest.PhotoUrl, opt => {
                opt.MapFrom(srs => srs.Photos.FirstOrDefault( p => p.IsMain).URL);
            });
            CreateMap<User, UserForDetailDto>().ForMember(dest => dest.PhotoUrl, opt => {
                opt.MapFrom(srs => srs.Photos.FirstOrDefault( p => p.IsMain).URL);
            });
            CreateMap<Photo, PhotosForDetailDto>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<Move, MoveDto>();
            CreateMap<Piece, PieceDto>();
            CreateMap<Game, GameDto>();


            //dto to model
            CreateMap<UserForUpdateDto, User>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<MoveForAddMoveDto, Move>();
        }    
    }
}