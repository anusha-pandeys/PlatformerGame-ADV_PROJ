﻿using SDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;


internal class Bullet : Entity
{
    private IntPtr Renderer => Engine.Renderer2;
    private Vector2 position;
    private Vector2 size;
    private GameColor color;
    private Vector2 velocity;
    public Entity Source { get; set; }
    private Texture bulletTexture;

    public Bullet(Vector2 position, Vector2 velocity, Vector2 size, GameColor color)
    {
        this.position = position;
        this.velocity = velocity;
        this.size = size;
        this.color = color;
        Game.entities.Add(this);
        string relativePath = "Assets\\Bullet.png";
        string absolutePath = System.IO.Path.GetFullPath(relativePath);
        bulletTexture = Engine.LoadTexture(absolutePath);
    }

    public void BulletLoop(Camera camera)
    {
        float fixedTimeStep = 0.016f; // Adjust this value as needed
        float accumulatedTime = 0.0f;

        // Update the bullet's position based on the velocity
        accumulatedTime += Engine.TimeDelta;
        // Check for collision with the player
        if (CheckPlayerCollision())
        {
            // Handle player collision logic here
            Game.player.chargeBar.setCharge(0);

            // Remove the bullet from the entities list
            Game.entities.Remove(this);
        }
        while (accumulatedTime >= fixedTimeStep)
        {
            position += velocity * fixedTimeStep;
            accumulatedTime -= fixedTimeStep;
        }

        // Remove the bullet if it's off-screen
        if (!IsOnScreen(camera))
        {
            Game.entities.Remove(this);
        }

        Render(camera);
    }

    private bool CheckPlayerCollision()
    {
        Rectangle bulletBounds = CalculateBound();
        Rectangle playerBounds = Game.player.GetPlayerBounds();
        return bulletBounds.IntersectsWith(playerBounds);
    }

    private bool IsOnScreen(Camera camera)
    {
        // Replace these values with your screen dimensions
        int screenWidth = (int)Game.Resolution.X;
        int screenHeight = (int)Game.Resolution.Y;

        // Check if the bullet is within the visible area
        return position.X >= 0 && position.X <= screenWidth &&
               position.Y >= 0 && position.Y <= screenHeight;
    }

    protected override Rectangle CalculateBound()
    {
        return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }

    public override void Render(Camera camera)
    {
        Vector2 localPosition = camera.globalToLocal(position);
        Draw(localPosition, size);
    }

    protected override void Draw(Vector2 position, Vector2 size)
    {
        
        Engine.DrawTexture(bulletTexture, position, null, size);
    }
}
