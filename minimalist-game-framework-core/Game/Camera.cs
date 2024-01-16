using System;
using System.Collections.Generic;
using System.Text;

internal class Camera
{
    public Vector2 screen;
    private static readonly int width = 640;
    public static readonly int height = 480;
    public static float globalCy;
    private float offset = Game.player.size.Y/4;

    float layer1Y;
    private Texture columnLeft1;
    private Texture columnRight1;


    public Camera()
    {
        screen = new Vector2(width / 2, height / 2);
        globalCy = Game.player.Position.Y + 60;
        layer1Y = 0;
        columnLeft1 = Engine.LoadTexture(System.IO.Path.GetFullPath("Assets\\greek-column-copy.png"));
        columnRight1 = Engine.LoadTexture(System.IO.Path.GetFullPath("Assets\\greek-column-copy.png"));
    }

    public Vector2 globalToLocal(Vector2 global)
    {
        return new Vector2(global.X, global.Y - offset);
        
    }
    public void updateCamera(Vector2 playerPos)
    {
        offset += playerPos.Y - globalCy;
        globalCy = playerPos.Y;

        parallaxLayer1(playerPos);
    }

    public void parallaxLayer1(Vector2 playerPos)
    {
        float change = (playerPos.Y - globalCy) / 3;

        layer1Y -= change;
        Engine.DrawTexture(columnLeft1, new Vector2(10, layer1Y), null, new Vector2(130, 480));
        Engine.DrawTexture(columnLeft1, new Vector2(500, layer1Y), null, new Vector2(130, 480));

        if (480 - layer1Y > 0)
        {
            float aboveTile = - (480- layer1Y);
            Engine.DrawTexture(columnLeft1, new Vector2(10, aboveTile), null, new Vector2(130, 480));
            Engine.DrawTexture(columnLeft1, new Vector2(500, aboveTile), null, new Vector2(130, 480));
        } else
        {
            layer1Y = 0;
        }


    }

}