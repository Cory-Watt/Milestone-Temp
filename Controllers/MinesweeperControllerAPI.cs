using Microsoft.AspNetCore.Mvc;
using Milestone.Models;
using Milestone.Services;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;

namespace Milestone.Controllers
{
    [ApiController]
    [Route("api")]
    public class MinesweeperControllerAPI : ControllerBase
    {
        GameDAO _gameService =  new GameDAO();

        public MinesweeperControllerAPI()
        {
            _gameService = new GameDAO();
        }

        [HttpGet("showSavedGames")]
        [ResponseType(typeof(List<GameDTO>))]
        public ActionResult<IEnumerable<GameDTO>>ShowSavedGames()
        {

            List<GameDTO> gameDTOs = _gameService.GetSavedGames();

            return gameDTOs;
        }

        [HttpGet("showSavedGames/{gameId}")]
        [ResponseType(typeof(GameDTO))]
        public ActionResult <GameDTO> ShowSavedGame(int gameId)
        {
            GameDTO game = _gameService.GetGameById(gameId);
            if (game == null)
            {
                return NotFound();
            }
            return game;
        }

        [HttpDelete("deleteOneGame/{gameId}")]
        public void DeleteOneGame(int gameId)
        {
            _gameService.DeleteGame(gameId);

        }

        [HttpPost("saveGame")]
        public IActionResult SaveGame(GameDTO game)
        {
            //game.SaveDateTime = DateTime.Now;
            _gameService.SaveGame(game);
            return Ok();
        }
    }
}