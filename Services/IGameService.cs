using System;
using System.Collections.Generic;
using System.Linq;

namespace Milestone.Models
{
    public interface IGameService
    {
        List<GameDTO> GetSavedGames();
        GameDTO GetGameById(int gameId);
        void SaveGame(GameDTO game);
        void DeleteGame(int gameId);
    }

    public class GameService : IGameService
    {
        private readonly List<GameDTO> _savedGames;

        public GameService()
        {
            _savedGames = new List<GameDTO>();
        }

        public List<GameDTO> GetSavedGames()
        {
            return _savedGames;
        }

        public GameDTO GetGameById(int gameId)
        {
            return null;
            // return _savedGames.FirstOrDefault(g => g.Id == gameId);
        }

        public void SaveGame(GameDTO game)
        {
            // Generate a unique ID for the game
            //int gameId = _savedGames.Count > 0 ? _savedGames.Max(g => g.Id) + 1 : 1;
            //game.Id = gameId;

            // Set the current date and time
            //game.SaveDateTime = DateTime.Now;

            //_savedGames.Add(game);
            
        }

        public void DeleteGame(int gameId)
        {
            //GameDTO game = _savedGames.FirstOrDefault(g => g.Id == gameId);
            //if (game != null)
            //{
            //    _savedGames.Remove(game);
            //}
        }
    }
}