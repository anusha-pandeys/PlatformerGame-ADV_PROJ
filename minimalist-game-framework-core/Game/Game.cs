using System.Collections.Generic;
using Game.Game.Collision;

namespace Game.Game
{
    class Game
    {
        public static readonly string Title = "Minimalist Game Framework";
        public static readonly Vector2 Resolution = new Vector2(640, 480);

        Player player;
        Map map;
        Blocks floor;
        public Game()
        {
            Vector2 playerPosition = new Vector2(100, 100); // Initial position
            Vector2 playerVelocity = new Vector2(0, 0);     // Initial velocity
            map = new Map();
            player = new Player(playerPosition, playerVelocity);
            floor = new Blocks(new Vector2(300, 400), new Vector2(50, 50), GameColor.Block1);
            CollisionManager.addBlock(floor);
        }

        public void Update()
        {
            map.setBackgroundColor();
            floor.blockLoop();
            player.playerLoop();
        }
    }
}