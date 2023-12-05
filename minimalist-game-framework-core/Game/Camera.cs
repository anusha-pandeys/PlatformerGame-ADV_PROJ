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
    public Camera()
    {
        screen = new Vector2(width / 2, height / 2);
        globalCy = Game.player.Position.Y;
    }

    public Vector2 globalToLocal(Vector2 global)
    {
        Vector2 local = new Vector2(global.X, global.Y - offset);
        //Vector2 local = new Vector2(global.X, Math.Abs(global.Y - globalCy) - height);
        //System.Console.WriteLine(local.X + "  " + local.Y);
        return local;
        
    }
    public void UpdateGlobalCy(Vector2 globalPlayer, Vector2 playerSize, Vector2 playerVelocity)
    {
        offset += globalPlayer.Y - globalCy;
        globalCy = globalPlayer.Y;
        
        
        /*
        float localPlayerY = height - Math.Abs(globalPlayer.Y - globalCy);
        if (localPlayerY < playerSize.Y/2 && playerVelocity.Y == 0)
        {
            globalCy -= height / 2;
        } else if (localPlayerY > height-playerSize.Y/2 && playerVelocity.Y == 0)
        {
            globalCy += height / 2;
        }
        */


        /*
        int newCy = (int) playerPosition.Y - height / 2;
        globalCy = Math.Max(480, newCy);
        /*
        if (playerPosition.Y >= 50)
        {
            globalCy += (int) (0.75 * height);
        }
        */

    }

}