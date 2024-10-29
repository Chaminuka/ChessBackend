using Chess;
using ChessBackende8.Controllers.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata.Ecma335;

namespace ChessBackende8.Services
{
    public class GamesService
    {



        private readonly DataContext _context;
        private readonly Logic _logic;
        public GamesService(DataContext context, Logic logic)
        {
            _logic = logic;
            _context = context;
        }


















        private int PlayerIsWhite(string pgn, string name)
        {
            for (int i = 0; i < pgn.Length; i++)
            {
                if (pgn[i] == 'W')
                {
                    bool isWhite = (pgn.Substring(i + 7, name.Length) == name);
                    if (isWhite)
                    {
                        return 0;
                    }
                    return 1;
                }

            }
            return -1;
        }



        private void AddPositions(GameMe game)
        {

            var games = _logic.GetFens(game.positions);

            game.currPosition = games;

        }

        private bool WhiteWon(string pgn, string whiteName)
        {
            string searchTerm = "Termination \"";
            for (int i = 0; i <= pgn.Length - searchTerm.Length; i++)
            {
                if (pgn.Substring(i, searchTerm.Length) == searchTerm)
                {

                    int terminationIndex = i + searchTerm.Length;
                    if (terminationIndex + whiteName.Length <= pgn.Length)
                    {
                        return pgn.Substring(terminationIndex, whiteName.Length) == whiteName;
                    }
                }
            }
            return false;
        }

        private bool BlackeWon(string pgn, string whiteName)
        {
            string searchTerm = "Termination ";
            for (int i = 0; i <= pgn.Length - searchTerm.Length; i++)
            {
                if (pgn.Substring(i, searchTerm.Length) == searchTerm)
                {


                    int terminationIndex = i + searchTerm.Length;
                    if (terminationIndex + whiteName.Length <= pgn.Length)
                    {
                        return pgn.Substring(terminationIndex, whiteName.Length) != whiteName;
                    }
                }
            }
            return false;
        }



        public async Task<double> GetWinRatio(string position)
        {

            var games = await _context.Games.ToListAsync();
            var retGames = new List<GameMe>();
            foreach (var game in games)
            {

                if (game.currPosition.Contains(_logic.FenNormalizer(position)))
                {

                    retGames.Add(game);
                }
            }
            double wins = 0.0;
            double total = 0.0;
            foreach (var game in retGames)
            {

                if (game.whiteWon)
                {
                    wins++;
                }
                total++;


            }
            if(total < 1)
            {
                total = 1;
            }
            double result = wins / total;
            return result;

        }

        public async Task<List<GameMe>> AddGame(string gamePgn, string name)
        {
            int isWhiteInt = PlayerIsWhite(gamePgn, name);
            bool isWhite = false;
            switch (isWhiteInt)
            {
                case 0:
                    isWhite = true;
                    break;
                case 1:
                    isWhite = false;
                    break;
                case -1:
                    throw new Exception("Invalid pgn!");

                default:
                    break;
            }



            GameMe game = new GameMe();




            game.positions = gamePgn;
            game.isWhite = isWhite;
            if (isWhite)
            {
                game.whiteWon = WhiteWon(gamePgn, name);
            }
            else
            {
                game.whiteWon = !BlackeWon(gamePgn, name);
            }


            try
            {
                AddPositions(game);
            }
            catch(Exception e) 
            {
                Console.WriteLine(e);
            }

            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            if (isWhite)
            {
                return await GetGamesWhite();
            }
            return await GetGamesBlack();





        }


        public async Task<string> ToFEN(string pgn)
        {

            List<string> moves = new List<string>();
            string currMove = string.Empty;

            for (int i = 0; i < pgn.Length; i++)
            {
                if (pgn[i] != ' ')
                {
                    currMove += pgn[i];
                }
                else
                {
                    if (currMove != string.Empty)
                    {
                        moves.Add(currMove);
                        currMove = string.Empty;
                    }
                }
            }

            // Capture the last move if currMove is not empty after the loop
            if (currMove != string.Empty)
            {
                moves.Add(currMove);
            }

            ChessBoard board = new ChessBoard();

            foreach (var move in moves)
            {
                bool valid =  board.IsValidMove(move);
                if (valid)
                {
                    board.Move(move);
                }
                
            }

            return board.ToFen();
        }



        private Task<bool> Validator(string pgn)
        {
            return Task.Run(() =>
            {
                List<string> moves = new List<string>();
                string currMove = string.Empty;

                for (int i = 0; i < pgn.Length; i++)
                {
                    if (pgn[i] != ' ')
                    {
                        currMove += pgn[i];
                    }
                    else
                    {
                        if (currMove != string.Empty)
                        {
                            moves.Add(currMove);
                            currMove = string.Empty;
                        }
                    }
                }

                // Capture the last move if currMove is not empty after the loop
                if (currMove != string.Empty)
                {
                    moves.Add(currMove);
                }

                ChessBoard board = new ChessBoard();

                foreach (var move in moves)
                {
                    bool valid = board.IsValidMove(move);
                    if (valid)
                    {
                        board.Move(move);
                    }
                    else
                    {
                        return false; // Return false if an invalid move is found
                    }
                }

                return true; // Return true if all moves are valid
            });
        }


        public async Task<bool> Validate(string pgn)
        {
            return await Validator(pgn);
        }




        public async Task<List<GameMe>> GetGamesBlack()
        {
            var games = await _context.Games.ToListAsync();
            var retGames = new List<GameMe>();

            for (int i = 0; i < games.Count; i++)
            {
                if (!games[i].isWhite)
                {
                    retGames.Add(games[i]);
                }
            }
            return retGames;
        }

        public async Task<List<GameMe>> GetGamesWhite()
        {
            var games = await _context.Games.ToListAsync();
            var retGames = new List<GameMe>();

            for (int i = 0; i < games.Count; i++)
            {
                if (games[i].isWhite)
                {
                    retGames.Add(games[i]);
                }
            }
            return retGames;
        }


    }
}

