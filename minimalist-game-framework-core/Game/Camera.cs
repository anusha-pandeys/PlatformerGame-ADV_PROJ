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

    private Vector2 layer1Pos;
    private Texture parallaxBricks = Engine.LoadTexture(System.IO.Path.GetFullPath("Assets\\parallaxBricks.png"));

    public Camera()
    {
        screen = new Vector2(width / 2, height / 2);
        globalCy = Game.player.Position.Y;
        layer1Pos = new Vector2(0, 0);
    }

    public Vector2 globalToLocal(Vector2 global)
    {
        Vector2 local = new Vector2(global.X, global.Y - offset);
        //Vector2 local = new Vector2(global.X, Math.Abs(global.Y - globalCy) - height);
        //System.Console.WriteLine(local.X + "  " + local.Y);
        return local;
        
    }
    public void updateGlobalCy(Vector2 globalPlayer, Vector2 playerSize, Vector2 playerVelocity)
    {
        offset += globalPlayer.Y - globalCy;
        globalCy = globalPlayer.Y;

    }

    public float updateParallaxLayer1 (Vector2 globalPlayer)
    {
        return (globalPlayer.Y - globalCy)/2;
    }

    public void parallaxLayer1(float change)
    {
        layer1Pos = new Vector2(layer1Pos.X, layer1Pos.Y - change);
        Engine.DrawTexture(parallaxBricks, layer1Pos, null, new Vector2(640, 480));

        if (480 - layer1Pos.Y > 0)
        {
            Vector2 tile2 = new Vector2(layer1Pos.X,  - (480-layer1Pos.Y));
            Engine.DrawTexture(parallaxBricks, tile2, null, new Vector2(640, 480));
        } else
        {
            layer1Pos = new Vector2(layer1Pos.X, 0);
        }


    }

}