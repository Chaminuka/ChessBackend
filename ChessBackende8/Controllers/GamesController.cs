using Chess;
using ChessBackende8.Controllers.Entities;
using ChessBackende8.Services;
using Microsoft.AspNetCore.Mvc;


namespace ChessBackende8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GamesService _gamesService;

        public GamesController(GamesService gamesService)
        {
            _gamesService = gamesService;
        }

        // POST api/games
        [HttpPost]
        public async Task<ActionResult<List<GameMe>>> NewGame(string gamePgn, string name)
        {
            return Ok(await _gamesService.AddGame(gamePgn, name));
        }

        // GET api/games/white
        [HttpGet("white")]
        public async Task<ActionResult<List<GameMe>>> GetWhiteGames()
        {
            var games = await _gamesService.GetGamesWhite();
            return Ok(games);
        }

        // GET api/games/black
        [HttpGet("black")]
        public async Task<ActionResult<List<GameMe>>> GetBlackGames()
        {
            var games = await _gamesService.GetGamesBlack();
            return Ok(games);
        }


        [HttpGet("Validator")]
        public async Task<ActionResult<ChessBoard>> Validator(string moves)
        {
            var ret =  await _gamesService.Validate(moves);
            return Ok(ret);
        }
        [HttpGet("FEN")]
        public async Task<ActionResult<string>> GetFEN(string moves)
        {
            var ret = await _gamesService.ToFEN(moves);
            return Ok(ret);
        }


        [HttpGet("currposition")]
        public async Task<ActionResult<double>> GetPositionResults(string position)
        {
           
           
            var winRatio = await _gamesService.GetWinRatio(position);
            return Ok(winRatio);
            

        }
    }
}
