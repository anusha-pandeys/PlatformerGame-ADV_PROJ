using System.Collections.Generic;

class Game
{
    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(640, 480);

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
        floor2 = new Blocks(new Vector2(200, 250), new Vector2(50, 50), GameColor.Block1);
        CollisionManager.addBlock(floor);
        CollisionManager.addBlock(floor2);
    }

    public void Update()
    {
        map.setBackgroundColor();
        floor.blockLoop();
        floor2.blockLoop();
        player.playerLoop();
    }
}