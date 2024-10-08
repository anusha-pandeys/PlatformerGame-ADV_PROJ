using SDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

internal class Pits : Entity
{
    private IntPtr Renderer => Engine.Renderer2;
    public Vector2 position;
    public static Vector2 size = new Vector2(50, 50);
    private Collidable pits;
    private Color originalColor;
    private Color pitColor;
    private float collisionCooldown = 0.1f;
    private float timeSinceCollision = 0.0f;
    private Texture playerTexture;
    public bool playerDeath = false;
    public float deathCounter = 0;

    public Pits(Vector2 position)
    {
        this.position = position;
        this.pits = new Collidable(this, "pit");
        this.originalColor = new Color(0, 255, 0, 255); // Original color (green)
        this.pitColor = originalColor;
        Game.entities.Add(this);
        string relativePath = "Assets\\pits.png";
        string absolutePath = System.IO.Path.GetFullPath(relativePath);
        playerTexture = Engine.LoadTexture(absolutePath);
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
            Game.player.chargeBar.setCharge(0);
        }
    }

    public bool checkCollision()
    {
        CollisionObject collisionDetected = CollisionManager.checkCollisions("player", "pit", new Vector2(0, 2));
        if (collisionDetected.getCollided())
        {
            deathCounter += Engine.TimeDelta;//
            Game.player.position.Y += 0.2f;
            if(deathCounter > 2)
            {
                deathCounter = 0;
                return true;
            }
        }
        if (!collisionDetected.getCollided())
        {
            
            deathCounter = 0;
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
        Vector2 localPosition = camera.globalToLocal(position);
        Draw(localPosition, size);



    }

    protected override void Draw(Vector2 position, Vector2 size)
    {
        Engine.DrawTexture(playerTexture, position, null, size);
    }
}
