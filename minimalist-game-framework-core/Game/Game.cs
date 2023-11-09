using System.Collections.Generic;

class Game
{

    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(640, 480);

    
    private TextRenderer textRenderer;
    Font font = Engine.LoadFont("Retro Gaming.ttf", 11);
    StartMenu startMenu;


    Player player;
    Map map;
    Blocks floor;
    Blocks floor2;
    public Game()
    {

        Vector2 playerPosition = new Vector2(100, 300); // Initial position
        Vector2 playerVelocity = new Vector2(0, 0);     // Initial velocity
        map = new Map();
        player = new Player(playerPosition, playerVelocity);
        floor = new Blocks(new Vector2(100, 250), new Vector2(50, 50), GameColor.Block1);
        floor2 = new Blocks(new Vector2(300, 250), new Vector2(50, 50), GameColor.Block1);
        CollisionManager.addBlock(floor);
        CollisionManager.addBlock(floor2);

        Font font = Engine.LoadFont("Retro Gaming.ttf", 11);
        
        textRenderer = new TextRenderer();
        startMenu = new StartMenu();
        //entities.Add(moving);

    }

    public void Update()
    {

        map.setBackgroundColor();
        floor.blockLoop();
        floor2.blockLoop();
        player.playerLoop();

        /*if (true)  // Add a condition to check when the start menu should be visible
        {
            startMenu.Update();
            startMenu.Draw(font);
        }
        else
        {
            // Update game logic here (e.g., player movement, collisions, etc.)
                       
            
            DisplayPlayerCoordinates();
            //moving.updateCoordinates();
        }*/
    }

    public void DisplayPlayerCoordinates()
    {
        string playerCoordinates = string.Format("{0}, {1}", player.getCoordinates()[0].X, player.getCoordinates()[0].Y);
        textRenderer.displayText(playerCoordinates, new Vector2(0, 0), Color.Black, font);

    }
}