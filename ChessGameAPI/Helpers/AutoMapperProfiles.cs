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
            CreateMap<Move, MoveDto>().ForMember(dest => dest.Notation, 
                opt => opt.MapFrom(srs => srs.AlgebraicNotation));
            CreateMap<Piece, PieceDto>();
            CreateMap<Game, GameDto>().ForMember(dest => dest.StatusCode, option => {
                option.MapFrom(source => source.StatusCode); });
            CreateMap<MessageForCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageForReturnDto>()
                .ForMember(m => m.SenderPhotoUrl, opt => opt
                    .MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).URL))
                .ForMember(m => m.RecipientPhotoUrl, opt => opt
                    .MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).URL));

            //dto to model
            CreateMap<UserForUpdateDto, User>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<MoveForAddMoveDto, Move>().ForMember(dest => dest.AlgebraicNotation, 
                opt => opt.MapFrom(srs => srs.Notation));
        }    
    }
}