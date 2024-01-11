﻿using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;

internal class Fire : Entity
{
    private IntPtr Renderer => Engine.Renderer2;
    private Collidable fireCollidable;
    private Texture fireTexture;
    private float damageCooldown = 1.0f; // Time in seconds before applying damage again
    private float damageDuration = 5.0f; // Total duration for the damage effect
    private float timeSinceDamage = 0.0f;
    private float chargeDecreaseInterval = 1.0f; // Interval to decrease charge (adjust as needed)
    private float chargeDecreaseTimer = 0.0f;
    private bool playerInContact = false;

    public Fire(Vector2 position)
    {
        this.position = position;
        size = Blocks.size;
        this.fireCollidable = new Collidable(this, "fire");
        Game.entities.Add(this);
    }

    public void FireLoop()
    {
        Console.WriteLine("FireLoop() called");
        HandleCollision();

        // Update the cooldown timer
        timeSinceDamage += Engine.TimeDelta;

        // Update the charge decrease timer
        chargeDecreaseTimer += Engine.TimeDelta;

        // Check if the player is in contact with the fire and decrease charge at the specified interval
        if (chargeDecreaseTimer >= chargeDecreaseInterval && playerInContact)
        {
            HandleChargeDecrease();
            chargeDecreaseTimer = 0.0f;
        }

        // Render the fire entity
        Render(Game.localCamera);
    }

    private void HandleCollision()
    {
        // Check if the player collides with the fire
        CollisionObject collisionDetected = CollisionManager.checkCollisions("player", "fire", new Vector2(0, 0));
        if (collisionDetected.getCollided())
        {
            Console.WriteLine("Player collided with fire");
            // Apply damage to the player only if enough time has passed since the last damage
            if (timeSinceDamage >= damageCooldown)
            {
                // Decrease charge at the specified interval
                playerInContact = true;
                timeSinceDamage = 0.0f; // Reset the cooldown timer
                HandleChargeDecrease(); // Decrease charge immediately upon collision
            }
        }
        else
        {
            playerInContact = false;
        }
    }


    private void HandleChargeDecrease()
    {
        Console.WriteLine("Handling charge decrease");
        int currentCharge = Game.player.chargeBar.getCharge();
        int newCharge = currentCharge - 1; // Decrease charge by 1 (adjust as needed)

        // Gradually decrease charge over 3 seconds
        double duration = 3.0; // in seconds
        double progress = timeSinceDamage / duration;
        newCharge = Math.Max(newCharge, (int)(currentCharge - progress));

        Game.player.chargeBar.setCharge(newCharge); // Update player's charge
    }



    public override void Render(Camera camera)
    {
        Vector2 localPosition = camera.globalToLocal(position);
        Draw(localPosition, size);
    }

    protected override Rectangle CalculateBound()
    {
        return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }

    protected override void Draw(Vector2 position, Vector2 size)
    {
        // Implement drawing logic for the fire entity (e.g., fill rectangle with a fire color)
        SDL.SDL_SetRenderDrawColor(Engine.Renderer2, 255, 0, 0, 255);
        SDL.SDL_Rect rect = new SDL.SDL_Rect()
        {
            x = (int)position.X,
            y = (int)position.Y,
            w = (int)size.X,
            h = (int)size.Y
        };
        SDL.SDL_RenderFillRect(Engine.Renderer2, ref rect);
    }
}
