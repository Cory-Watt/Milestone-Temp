namespace Milestone.Models
{
    public class ButtonModel
    {
        public int Id { get; set; }
        public int ButtonState { get; set; }
        public bool Live { get; set; }
        int Row { get; set; }
        int Column { get; set; }
        public bool Visited { get; set; }
        public int Neighbors { get; set; }
        public string ImageName { get; set; }
        public bool Flagged { get; set; }


        public ButtonModel() { }

        public ButtonModel(int id, int buttonState)
        {
            //Setting default values
            Id = id;
            Row = -1;
            Column = -1;
            Visited = false;
            Live = false;
            Neighbors = 0;
            ButtonState = buttonState;
        }

        public ButtonModel(int id, int buttonState, bool live, bool visited, int neighbors, string imageName, bool flagged) 
        {
            Id = id;
            ButtonState = buttonState;
            Live = live;
            Visited = visited;
            Neighbors = neighbors;
            ImageName = imageName;
            Flagged = flagged;
        }
    }
}
