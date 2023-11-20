using System;
using System.Collections.Generic;
using System.Text;

internal class Camera
{
    public Vector2 screen;
    private static readonly int width = 640;
    private static readonly int height = 480;
    private int globalCy = height;
    public Camera()
    {
        screen = new Vector2(width / 2, height / 2);
    }

    public Vector2 globalToLocal(Vector2 globalPlayer)
    {
        //return globalPlayer;
        float yPos = globalPlayer.Y;
        yPos -= Game.player.playerVelocity.Y;
        return new Vector2(globalPlayer.X, yPos);
        //return new Vector2(globalPlayer.X, height - Math.Abs(globalPlayer.Y - globalCy));

    }
    public void UpdateGlobalCy(Vector2 globalPlayer, Vector2 playerSize, Vector2 playerVelocity)
    {
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