using System;
using AutoMapper;
using Logic.Domain;
using Logic.Domain.Moves;
using MultiplayerChessApi.Response;

namespace MultiplayerChessApi.Mapping;

public class ChessGameProfile : Profile
{
    public ChessGameProfile()
    {
        CreateMap<ChessGame, GameCreatedResponse>()
            .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.Id));

        CreateMap<ChessGame, GameByIdResponse>()
            .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Board, opt => opt.MapFrom(src => src.Board.ToSmallFen()))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.ToString()))
            .ForMember(dest => dest.CurrentPlayer, opt => opt.MapFrom(src => src.CurrentPlayer.Username))
            .ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.Players));

        CreateMap<ChessGame, AllGamesResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.ToString()))
            .ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.Players));

        CreateMap<ChessGame, GameJoinedResponse>()
            .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.Id));
    }
}
