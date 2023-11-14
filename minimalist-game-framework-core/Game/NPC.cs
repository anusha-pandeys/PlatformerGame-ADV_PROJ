using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;

internal class NPC : Entity
{
    private IntPtr Renderer => Engine.Renderer2; // Gets the SDL Renderer from the Engine class
    private Vector2 position;
    private Vector2 size;
    private float followRadius = 300.0f; // Set the follow radius
    private float speed = 1.0f; // Set the NPC's speed
    private Player player;
    private float distance; // Declare distance as a field

    public NPC(Vector2 position, Vector2 size, Player player)
    {
        this.position = position;
        this.size = size;
        this.player = player;
    }

    public void Update()
    {
        if (IsPlayerInRadius())
        {
            FollowPlayer();
        }

        // Implement collision detection for NPC
        string collisionDetected = CollisionManager.checkBlockCollision(this, new Vector2(speed, 0));

        // Update NPC's position based on collision detection
        if (collisionDetected.Contains("left"))
        {
            position.X -= speed;
        }
        else if (collisionDetected.Contains("right"))
        {
            position.X += speed;
        }

        Render();
    }

    private bool IsPlayerInRadius()
    {
        // Update the distance field
        distance = CalculateDistance(position, player.Position);
        return distance <= followRadius;
    }

    private void FollowPlayer()
    {
        // Calculate direction from NPC to player
        Vector2 direction = CalculateDirection(player.Position - position);

        // Update NPC's position only if it doesn't collide with any blocks
        string collisionDetected = CollisionManager.checkBlockCollision(this, direction * speed);
        if (collisionDetected == "na")
        {
            position += direction * speed;
        }
        Console.WriteLine($"NPC Position: {position}, Player Position: {player.Position}, Distance: {distance}");
    }




    private float CalculateDistance(Vector2 point1, Vector2 point2)
    {
        float dx = point1.X - point2.X;
        float dy = point1.Y - point2.Y;
        return (float)Math.Sqrt(dx * dx + dy * dy);
    }

    private Vector2 CalculateDirection(Vector2 vector)
    {
        float length = CalculateLength(vector);
        if (length == 0)
        {
            return Vector2.Zero;
        }
        else
        {
            return vector / length;
        }
    }

    private float CalculateLength(Vector2 vector)
    {
        return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
    }

    protected override Rectangle CalculateBound()
    {
        return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }

    protected override void Render()
    {
        Draw(position, size);
    }

    protected override void Draw(Vector2 position, Vector2 size)
    {
        SDL.SDL_SetRenderDrawColor(Renderer, 0, 0, 255, 255); // blue

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
