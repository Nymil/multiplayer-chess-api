using System;
using AutoMapper;
using Logic.Domain;
using Logic.Domain.Moves;
using Logic.Domain.Pieces;
using Logic.Domain.Players;
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
            .ForMember(dest => dest.Board, opt => opt.MapFrom(src => src.Board.ToFen(src.CurrentPlayer.Color)))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.ToString()))
            .ForMember(dest => dest.CurrentPlayer, opt => opt.MapFrom(src => src.CurrentPlayer.Username))
            .ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.Players))
            .ForMember(dest => dest.LastMove, opt => opt.MapFrom(src => src.LastMove != null ? src.LastMove.ToString() : null))
            .ForMember(dest => dest.Result, opt => opt.ConvertUsing(new GameResultResponseConverter()))
            .ForMember(dest => dest.CapturedPieces, opt => opt.MapFrom(src => new CapturesResponse
                {
                    White = src.Board.CapturedPieces.Where(p => p.Color == PlayerColor.White).Select(p => p.Type.ToString()).ToList(),
                    Black = src.Board.CapturedPieces.Where(p => p.Color == PlayerColor.Black).Select(p => p.Type.ToString()).ToList()
                }));

        CreateMap<ChessGame, AllGamesResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.ToString()))
            .ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.Players));

        CreateMap<ChessGame, GameJoinedResponse>()
            .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.Id));
    }
}

public class GameResultResponseConverter : IValueConverter<Result?, GameResultResponse?>
{
    public GameResultResponse? Convert(Result? source, ResolutionContext context)
    {
        if (source == null)
        {
            return null;
        }

        return new GameResultResponse
        {
            Winner = source.Winner?.Username,
            Reason = source.Reason.ToString()
        };
    }
}
