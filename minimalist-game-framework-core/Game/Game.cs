using System.Collections.Generic;


class Game
{

    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(640, 480);
    private List<Entity> entities = new List<Entity>();
    private TextRenderer textRenderer;
    Font font = Engine.LoadFont("Retro Gaming.ttf", 11);
    StartMenu startMenu;
    private Player player;
    private Map map;
    //private Blocks floor;
    //private Blocks floor2;
    private List<Blocks> levelBlocks;
    private List<Blocks> levelBlocks2;
    public Game()
    {
        Vector2 playerPosition = new Vector2(100, 300); // Initial position
        Vector2 playerVelocity = new Vector2(0, 0);     // Initial velocity
        map = new Map();
        textRenderer = new TextRenderer();
        startMenu = new StartMenu();
        //entities.Add(moving);
        player = new Player(playerPosition, playerVelocity, textRenderer, font);
        //floor = new Blocks(new Vector2(100, 250), new Vector2(50, 50), GameColor.Block1);
        //floor2 = new Blocks(new Vector2(200, 250), new Vector2(50, 50), GameColor.Block1);
        //CollisionManager.addBlock(floor);
        //CollisionManager.addBlock(floor2);

        levelBlocks = LevelLoader.LoadLevel("Game\\levelPractice.txt", 50); // Replace with the correct path
       // levelBlocks2 = LevelLoader.LoadLevel("Game\\levelPractice2.txt", 50); // Replace with the correct path
        //Font font = Engine.LoadFont("Retro Gaming.ttf", 11);        
        //startMenu = new StartMenu();
    }

    public void Update()
    {
        if (true)  // Add a condition to check when the start menu should be visible
        {
            startMenu.Update();
            startMenu.Draw(font);
        }
        else
        {
            // Update game logic here (e.g., player movement, collisions, etc.)
            map.setBackgroundColor();
            floor.Render();
            x.playerLoop();
            DisplayPlayerCoordinates();
            moving.updateCoordinates();
        }
    }

    private void DisplayPlayerCoordinates()
    {
        string playerCoordinates = string.Format("{0}, {1}", player.getCoordinates()[0].X, player.getCoordinates()[0].Y);
        textRenderer.displayText(playerCoordinates, new Vector2(0, 0), Color.Black, font);
    }
}
