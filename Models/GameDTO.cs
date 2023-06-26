namespace Milestone.Models
{
    public class GameDTO
    {
        public int GameId { get; set; }
        public string UserId { get; set; }

        public string time { get; set; }

        public string date { get; set; }
       public  string gameData { get; set; }
        

        public GameDTO() { }

        public GameDTO(string UserId, string time, string date, string gameData)
        {
            this.UserId = UserId;
            this.time = time;
            this.date = date;
            this.gameData = gameData;
        }

    }
}
