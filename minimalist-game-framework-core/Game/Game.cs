using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


class Game
{
    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(640, 480);

    

    //public Game()
    //{

    //}

    //public void Update() { }


    public Game()
    {
        playerPosition = new Vector2(100, 100); // Initial position
        playerVelocity = new Vector2(0, 0);     // Initial velocity
    }

    public void Update()
    {
        HandleInput();

        // Apply gravity
        playerVelocity.Y += GRAVITY;

        // Update player position
        playerPosition += playerVelocity;

        // Collision detection for the floor
        if (playerPosition.Y > 500) // Assuming 500 is ground level
        {
            playerPosition.Y = 500;
            playerVelocity.Y = 0; // Stop downward movement
        }

        // Collision detection for the walls (placeholder logic)
        if (playerPosition.X < 0 || playerPosition.X + PLAYER_WIDTH > 800) // Assuming screen width is 800
        {
            playerVelocity.X = 0; // Stop horizontal movement
        }

        Render();
    }



}