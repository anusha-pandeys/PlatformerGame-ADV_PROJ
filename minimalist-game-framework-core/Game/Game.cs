using System.Collections.Generic;


class Game
{
    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(640, 480);
    private List<Entity> entities = new List<Entity>();

    Player x;
    Map map;
    Blocks floor;
    MovingBlock moving;
    public Game()
    {
        Vector2 playerPosition = new Vector2(100, 100); // Initial position
        Vector2 playerVelocity = new Vector2(0, 0);     // Initial velocity
        map = new Map();
        x = new Player(playerPosition, playerVelocity, entities);
        floor = new Blocks(new Vector2(300, 400), new Vector2(50, 50), GameColor.Block1);
        moving = new MovingBlock(new Vector2(100, 100), new Vector2(50, 50), GameColor.Block1);
        entities.Add(x);
        entities.Add(floor);
        entities.Add(moving);
    }

    public void Update()
    {
        map.setBackgroundColor();
        floor.Render();
        x.playerLoop();
        List<Vector2> updated = moving.getCoordinates();
        moving.updateCoordinates(new Vector2(updated[0].X + 1f, updated[0].Y));
    }
}