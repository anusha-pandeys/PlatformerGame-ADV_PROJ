using SDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

internal class Pits : Entity
{
    private IntPtr Renderer => Engine.Renderer2;
    private Vector2 position;
    private Vector2 size;
    private Collidable pits;
    private Color originalColor;
    private Color pitColor;
    private float collisionCooldown = 0.1f;
    private float timeSinceCollision = 0.0f;

    public bool playerDeath = false;

    public Pits(Vector2 position, Vector2 size)
    {
        this.position = position;
        this.size = size;
        this.pits = new Collidable(this, "pit");
        this.originalColor = new Color(0, 255, 0, 255); // Original color (green)
        this.pitColor = originalColor;
        Game.entities.Add(this);
    }

    public bool getPlayerDeath()
    {
        return playerDeath;
    }

    public void pitsLoop()
    {
        //Render(new Camera());
        if (checkCollision())
        {

            HandleCollision();
            //playerDeath = true;
            Game.player.setHealth(50);
        }
    }

    public bool checkCollision()
    {
        CollisionObject collisionDetected = CollisionManager.checkCollisions("player", "pit", new Vector2(0, 0));
        if (collisionDetected.getCollided())
        {
            return true;
        }

        return false;
    }

    private void HandleCollision()
    {
        // Check if the collision is with the player
        if (checkCollision())
        {
            // Handle collision logic here for Pits colliding with the player
            // Change the Pits' color to red
            pitColor = new Color(255, 0, 0, 255); // Red color
        }
    }

    private void UpdateCollisionCooldown()
    {
        // Update the elapsed time since the last collision
        timeSinceCollision += Engine.TimeDelta;

        // Check if enough time has passed since the last collision to revert to the original color
        if (timeSinceCollision >= collisionCooldown)
        {
            pitColor = originalColor;
        }
    }

    protected override Rectangle CalculateBound()
    {
        return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }

    public override void Render(Camera camera)
    {
        Draw(position, size);
    }

    protected override void Draw(Vector2 position, Vector2 size)
    {

        SDL.SDL_SetRenderDrawColor(Renderer, pitColor.R, pitColor.G, pitColor.B, pitColor.A);
        SDL.SDL_Rect rect = new SDL.SDL_Rect()
        {
            x = (int)position.X,
            y = (int)position.Y,
            w = (int)size.X,
            h = (int)size.Y
        };

        SDL.SDL_RenderFillRect(Renderer, ref rect);
    }
}
