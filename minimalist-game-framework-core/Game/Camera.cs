using System;
using System.Collections.Generic;
using System.Text;

internal class Camera
{
    private static readonly int width = 640;
    public static readonly int height = 480;
    public static float globalCy;
    private float offset = Game.player.size.Y / 4;

    float layer1Pos;
    private Texture columnLayer1Left;
    private Texture columnLayer1Right;

    public Camera()
    {
        globalCy = Game.player.Position.Y + 60;
        layer1Pos = 0;
        columnLayer1Left = Engine.LoadTexture(System.IO.Path.GetFullPath("Assets\\greek-column.png"));
        columnLayer1Right = Engine.LoadTexture(System.IO.Path.GetFullPath("Assets\\greek-column.png"));
    }

    public Vector2 globalToLocal(Vector2 global)
    {
        return new Vector2(global.X, global.Y - offset);
      
    }
    public void updateGlobalCy(Vector2 globalPlayer)
    {
        offset += globalPlayer.Y - globalCy;
        globalCy = globalPlayer.Y;

    }

    public float updateParallaxLayer1(Vector2 globalPlayer)
    {
        return (globalPlayer.Y - globalCy) / 2;
    }

    public void parallaxLayer1(float change)
    {
        layer1Pos -= change;
        Engine.DrawTexture(columnLayer1Left, new Vector2 (10, layer1Pos), null, new Vector2(130, 480));
        Engine.DrawTexture(columnLayer1Right, new Vector2(500, layer1Pos), null, new Vector2(130, 480));

        if (480 - layer1Pos > 0)
        {
            float tile2 = -(480 - layer1Pos);
            Engine.DrawTexture(columnLayer1Left, new Vector2(10, tile2), null, new Vector2(130, 480));
            Engine.DrawTexture(columnLayer1Right, new Vector2(500, tile2), null, new Vector2(130, 480));
        }
        else
        {
            layer1Pos = 0;
        }


    }

}