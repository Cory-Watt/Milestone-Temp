using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Milestone.Models;
using Milestone.Services;
using Milestone.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System.Text.Json;

namespace Milestone.Controllers
{
    public class MinesweeperController : Controller
    {

        private readonly IGameService _gameService;
        private readonly ILogger<MinesweeperController> _logger;

        public MinesweeperController(IGameService gameService, ILogger<MinesweeperController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        static List<ButtonModel> buttons = new List<ButtonModel>();
        const int GRID_SIZE = 25;
        const int BOMB_SIZE = 5;
        const int ROW_SIZE = 5;
        public ButtonModel[,] grid { get; set; }
        GameDAO gameDAO = new GameDAO();

        [CustomAuthorization]
        public IActionResult Index()
        {
            if (buttons.Count == 0)
            {
                grid = new ButtonModel[5, 5];
                //Will loop thorugh the row
                int id = 0;
                for (int r = 0; r < 5; r++)
                {
                    //will loop through the coulmns of the above row
                    for (int c = 0; c < 5; c++)
                    {
                        // create new cell object and add in every postinon in the grid
                        ButtonModel button = new ButtonModel(id, 0);
                        grid[r, c] = button;
                        id++;
                    }
                }
                setupLiveNeighbors();
                calculateLiveNeigbors();
                buttons.Clear();
                buttons.AddRange(grid.Cast<ButtonModel>());
                buttons = grid.Cast<ButtonModel>().ToList();

                // Log the initial state of the game board
                _logger.LogInformation("Initial game board state: {GameBoard}", JsonConvert.SerializeObject(buttons));
              
            }
            //This is to check live bombs location for cheating/testing purposes
            for (int r = 0; r < buttons.Count; r++)
            {
                if (buttons.ElementAt(r).Live)
                {
                    Console.WriteLine("button is live: " + r);
                    _logger.LogDebug("Button at index {ButtonIndex} is live", r);
                }
            }
            
            return View("Index", buttons);
        }

        public IActionResult HandleButtonClick(string buttonNumber)
        {
            // Log the selected button
            _logger.LogInformation("User clicked button: {ButtonNumber}", buttonNumber);

            if (!string.IsNullOrEmpty(buttonNumber))
            {
                if (int.TryParse(buttonNumber, out int bN))
                {
                    if (bN >= 0 && bN < buttons.Count)
                    {
                        buttons[bN].ButtonState = (buttons[bN].ButtonState + 1) % 4;
                        return PartialView(buttons[bN]);
                    }
                    else
                    {
                        // Handle the case where the button number is out of range
                        return BadRequest("Invalid button number");
                    }
                }
                else
                {
                    // Handle the case where the button number is not in a valid format
                    return BadRequest("Invalid button number format");
                }
            }
            else
            {
                // Handle the case where the button number is null or empty
                return BadRequest("Button number is required");
            }
        }
        /* public IActionResult HandleButtonClick(int buttonNumber)
         {
             ButtonModel clickedButton = buttons.ElementAt(buttonNumber);
             {
                     clickedButton.ButtonState++;
             }
             return PartialView(buttons.ElementAt(buttonNumber));
         } */

        public IActionResult ShowOneButton(int buttonNumber)
        {

            // Log the selected button
            _logger.LogInformation("User clicked button: {ButtonNumber}", buttonNumber);

            if (!buttons.ElementAt(buttonNumber).Flagged)
            {
               
                buttons.ElementAt(buttonNumber).ButtonState = (buttons.ElementAt(buttonNumber).ButtonState + 1) % 4;
             }

            // Log the current game state after button click
            _logger.LogInformation("Game board state after button click: {GameBoard}", JsonConvert.SerializeObject(buttons));

            int didIwin = didIWin();
            string messageString = "";
            string buttonString = RenderRazorViewToString(this, "ShowOneButton", buttons.ElementAt(buttonNumber));
            if (didIwin != 0)
            {
                
                if(didIwin == 1)
                {
                     messageString = "<p>Game Over. Sorry you Loss</p>";
                }
                else
                {
                     messageString = "<p>Game Over. Congrats you won</p>";
                }
                
            }
            var package = new { part1 = buttonString, part2 = messageString };
            return Json(package);

        }

        public IActionResult RightClickShowOneButton(int buttonNumber)
        {
            // Log the selected button
            _logger.LogInformation("User right-clicked button: {ButtonNumber}", buttonNumber);

            buttons.ElementAt(buttonNumber).Flagged = !buttons.ElementAt(buttonNumber).Flagged;
            if (buttons.ElementAt(buttonNumber).Flagged)
            {
                buttons.ElementAt(buttonNumber).ImageName = "flag.png";
            }
            else
            {
                buttons.ElementAt(buttonNumber).ImageName = "unopenedsquare.png";
            }
            int didIwin = didIWin();
            string messageString = "";
            string buttonString = RenderRazorViewToString(this, "ShowOneButton", buttons.ElementAt(buttonNumber));
            if (didIwin != 0)
            {

                if (didIwin == 1)
                {
                    messageString = "<p>Game Over. Sorry you Loss</p>";
                }
                else
                {
                    messageString = "<p>Game Over. Congrats you won</p>";
                }

            }
            var package = new { part1 = buttonString, part2 = messageString };
            return Json(package);
        }

        public int didIWin()
        {
            int vistedButtons =0;
            // 0 indicates game is not over, 1 indicates loss, 2 indicates victory
            for(int i=0; i<buttons.Count; i++)
            {
                if (buttons.ElementAt(i).ButtonState !=0 & buttons.ElementAt(i).Live)
                {
                    return 1;
                }
                else if(buttons.ElementAt(i).ButtonState !=0 && buttons.ElementAt(i).Live == false)
                {
                    vistedButtons++;
                }
            }
            if (vistedButtons == 20) 
            {
                return 2;
            }
            return 0;

        }

        public static string RenderRazorViewToString(Controller controller, string viewName, object model = null)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                IViewEngine viewEngine =
                    controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as
                        ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    sw,
                    new HtmlHelperOptions()
                );
                viewResult.View.RenderAsync(viewContext);
                return sw.GetStringBuilder().ToString();
            }
        }

        //THis method will set up the board with random live members (w/ bombs)
        public void setupLiveNeighbors()
        {
            //Calculate number of bombs and convert to int
            int count = 0;
            //Set random live cells
            while (count < BOMB_SIZE)
            {
                //Get Random Postion
                Random random = new Random();
                int randRow = random.Next(0, 5);
                int randCol = random.Next(0, 5);
                //Link cell to new refernce
                ButtonModel button = grid[randRow, randCol];
                //Check if postion is not already live
                if (button.Live == false)
                {
                    // live property to true
                    button.Live = true;
                    count++;
                }
                else
                {
                    continue;
                }
            }
        }
        //Will calcualte the live neighbors for cells on the grid
        public void calculateLiveNeigbors()
        {
            int arrSize = ROW_SIZE - 1;

            //loop through the grid
            for (int r = 0; r < ROW_SIZE; r++)
            {
                for (int c = 0; c < ROW_SIZE; c++)
                {
                    int liveNeighbors = 0;
                    ButtonModel button = grid[r, c];
                    //Check If cell has down neighbor
                    if (r != arrSize)
                    {
                        ButtonModel down = grid[r + 1, c];
                        if (down.Live)
                        {
                            liveNeighbors++;
                        }
                    }
                    //check if cell has up neighbor
                    if (r != 0)
                    {
                        ButtonModel up = grid[r - 1, c];
                        if (up.Live)
                        {
                            liveNeighbors++;
                        }
                    }
                    //check if cell has right neighbor
                    if (c != ROW_SIZE - 1)
                    {
                        ButtonModel right = grid[r, c + 1];
                        if (right.Live)
                        {
                            liveNeighbors++;
                        }
                    }
                    //check if cell has left neighbor
                    if (c != 0)
                    {
                        ButtonModel left = grid[r, c - 1];
                        if (left.Live)
                        {

                            liveNeighbors++;
                        }
                    }
                    //check if cell has topleft neighbor
                    if (r != 0 && c != 0)
                    {
                        ButtonModel topleft = grid[r - 1, c - 1];
                        if (topleft.Live)
                        {
                            liveNeighbors++;
                        }
                    }
                    //check if cell has bottom right neighbor
                    if (r != ROW_SIZE - 1 && c != ROW_SIZE - 1)
                    {
                        ButtonModel bottomright = grid[r + 1, c + 1];
                        if (bottomright.Live)
                        {
                            liveNeighbors++;
                        }
                    }
                    //check if cell has top right neighbor
                    if (r != 0 && c != ROW_SIZE - 1)
                    {
                        ButtonModel topRight = grid[r - 1, c + 1];
                        if (topRight.Live)
                        {
                            liveNeighbors++;
                        }
                    }
                    //check if cell has bottom left neighbor
                    if (r != ROW_SIZE - 1 && c != 0)
                    {
                        ButtonModel bottomleft = grid[r + 1, c - 1];
                        if (bottomleft.Live)
                        {
                            liveNeighbors++;
                        }
                    }
                    //if cell is live and has 8 neighbors, set to 9 live
                    if (button.Live && liveNeighbors == 8)
                    {
                        button.Neighbors = 9;
                    }
                    else
                    {
                        //set neighbor property
                        button.Neighbors = liveNeighbors;
                    }
                }
            }
        }

        public IActionResult saveGame(UserModel user)
        {
            GameDTO game = new GameDTO();
            game.date = DateTime.Now.ToString("MM / dd / yyyy");
            game.time = DateTime.Now.ToString("H: mm");
            game.UserId = "test";
            string gameData = "";
            //foreach (ButtonModel button in buttons)
            //{
              string jsonString = System.Text.Json.JsonSerializer.Serialize(buttons);
              gameData = jsonString;
            //}
            game.gameData = gameData;
             Console.WriteLine(gameData);

            gameDAO.SaveGame(game);


            return View("SavedGames", gameDAO.GetSavedGames());
        }

        public IActionResult resumeGame(string gameData)
        {
           
            buttons.Clear();
            
            var jsonArray = JArray.Parse(gameData);
            foreach(var data in jsonArray)
            {
                var id = (int)data["Id"];
                var buttonState = (int)data["ButtonState"];
                var live = (bool)data["Live"];
                var visited = (bool)data["Visited"];
                var neighbours = (int)data["Neighbors"];
                var image = (String)data["ImageName"];
                var flagged = (bool)data["Flagged"];
                buttons.Add(new ButtonModel(id, buttonState, live, visited, neighbours, image, flagged));
            }
            return View("Index", buttons);


        }
        public IActionResult ShowGames()
        {
            return View("SavedGames", gameDAO.GetSavedGames());
        }

        public IActionResult deleteGame(int GameId) 

        {
            gameDAO.DeleteGame(GameId);
            return View("SavedGames", gameDAO.GetSavedGames());
        }

    }
}
