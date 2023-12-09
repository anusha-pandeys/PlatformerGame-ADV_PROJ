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
        

    }

}