using System.Collections.Generic;


class Game
{
    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(640, 480);
    private List<Entity> entities = new List<Entity>();
    private TextRenderer textRenderer;
    Font font = Engine.LoadFont("Retro Gaming.ttf", 11);

    Player x;
    Map map;
    Blocks floor;
    public Game()
    {
        Font font = Engine.LoadFont("Retro Gaming.ttf", 11);
        Vector2 playerPosition = new Vector2(100, 100); // Initial position
        Vector2 playerVelocity = new Vector2(0, 0);     // Initial velocity
        map = new Map();
        x = new Player(playerPosition, playerVelocity, entities);
        floor = new Blocks(new Vector2(300, 400), new Vector2(50, 50), GameColor.Block1);
        entities.Add(x);
        entities.Add(floor);
        textRenderer = new TextRenderer();
    }

    public void Update()
    {
        map.setBackgroundColor();
        floor.Render();
        x.playerLoop();

        DisplayPlayerCoordinates();
    }

    public void DisplayPlayerCoordinates()
    {
        string playerCoordinates = string.Format("{0}, {1}", x.getCoordinates()[0].X, x.getCoordinates()[0].Y);
        textRenderer.displayText(playerCoordinates, new Vector2(0, 0), Color.Black, font);
    }
}