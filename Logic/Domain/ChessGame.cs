using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Domain.BoardUtil;
using Logic.Domain.Exceptions;
using Logic.Domain.Moves;
using Logic.Domain.Pieces;
using Logic.Domain.Players;

namespace Logic.Domain
{
    public class ChessGame
    {
    
        public string Id { get; init; }
        public Board Board { get; init; }
        public Player CurrentPlayer { get; private set;}
        public ChessGameState State { get; private set; } = ChessGameState.Waiting;
        public Result? Result { get; private set; } = null;

        public IEnumerable<string> Players { get
            {
                if (_playerBlack == null)
                {
                    return new List<string> { _playerWhite.Username };
                }
                return new List<string> { _playerWhite.Username, _playerBlack.Username };
            }
        }

        private Player _playerWhite;
        private Player? _playerBlack;

        public Player PlayerWhite => _playerWhite;
        public Player? PlayerBlack => _playerBlack;


        public ChessGame(string username)
        {
            Id = Guid.NewGuid().ToString();
            Board = new Board();
            _playerWhite = new Player(username, PlayerColor.White);
            CurrentPlayer = _playerWhite;
        }

        public void JoinGame(string username)
        {
            if (_playerBlack != null || State != ChessGameState.Waiting)
            {
                throw new ChessIllegalStateException("Game has already started");
            }

            if (_playerWhite.Username == username)
            {
                throw new ChessIllegalStateException("Player with the same name already in game");
            }

            _playerBlack = new Player(username, PlayerColor.Black);
            State = ChessGameState.InProgress;
        }

        public IEnumerable<Move> LegalMovesForPiece(Position startPosition)
        {
            if (Board.IsEmpty(startPosition) || Board[startPosition]!.Color != CurrentPlayer.Color)
            {
                return Enumerable.Empty<Move>();
            }

            Piece piece = Board[startPosition]!;
            IEnumerable<Move> moveCandidates = piece.GetMoves(startPosition, Board);
            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        public void MakeMove(Move move)
        {
            ValidateMove(move);

            Board.SetPawnSkipPosition(CurrentPlayer.Color, null);
            move.Execute(Board);
            CurrentPlayer = CurrentPlayer == _playerWhite ? _playerBlack! : _playerWhite;
            CheckForGameOver();
        }

        public IEnumerable<Move> AllLegalMovesFor(PlayerColor color)
        {
            IEnumerable<Move> moveCandidates = Board.PiecePositionsFor(color).SelectMany(pos => {
                Piece? piece = Board[pos];
                return piece?.GetMoves(pos, Board) ?? Enumerable.Empty<Move>();
            });

            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        // called after each move after player swap
        private void CheckForGameOver()
        {
            if (!AllLegalMovesFor(CurrentPlayer.Color).Any())
            {
                State = ChessGameState.Finished;

                if (Board.IsInCheck(CurrentPlayer.Color))
                {
                    Player opponent = CurrentPlayer == _playerWhite ? _playerBlack! : _playerWhite;
                    Result = Result.Win(opponent);
                }
                else
                {
                    Result = Result.Draw(EndReason.Stalemate);
                }
            }
            else if (Board.InsufficientMaterial())
            {
                State = ChessGameState.Finished;
                Result = Result.Draw(EndReason.InsufficientMaterial);
            }
        }

        private void ValidateMove(Move move)
        {
            if (State != ChessGameState.InProgress)
            {
                throw new ChessIllegalStateException("Game is not in progress");
            }

            IEnumerable<Move> legalMoves = LegalMovesForPiece(move.Start);
            if (!legalMoves.Contains(move))
            {
                throw new ChessIllegalStateException("Illegal move");
            }
        }
    }
}
