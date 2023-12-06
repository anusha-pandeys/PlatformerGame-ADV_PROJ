using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;


internal class Boss : Entity
{
    private IntPtr Renderer => Engine.Renderer2;
    private Vector2 position;
    private Vector2 size;
    private float followRadius;
    private Vector2 velocity;
    private float speed;
    private Player player;
    private const float GRAVITY = 0.25f;
    private float accumulatedGravity = 0.0f;
    private float shootCooldown = 2.0f; // Set the desired cooldown time in seconds
    private float timeSinceLastShot = 0.0f;
    private float bulletSpeed = 100.0f;

    public Boss(Vector2 position, Vector2 size, Player player, float followRadius, float speed)
    {
        this.position = position;
        this.size = size;
        this.player = player;
        this.followRadius = followRadius;
        this.speed = speed;
        this.velocity = Vector2.Zero;
        Game.entities.Add(this);
    }

    public void Update()
    {
        if (IsPlayerInRadius())
        {
            MoveTowardsPlayer();
        }
        else
        {
            velocity = Vector2.Zero;
        }

        // Apply gravity to the boss
        ApplyGravity();

        // Update boss position based on velocity
        position += velocity;

        // Collision detection to adjust the position if needed
        string collisionDetected = CollisionManager.checkBlockCollision(this, velocity);

        if (collisionDetected != "na")
        {
            AdjustPositionOnCollision(collisionDetected);
            accumulatedGravity = 0.0f; // Reset accumulated gravity on collision
        }

        // Check if the boss is hitting the left or right boundary
        if (position.X < 0 || position.X + size.X > 800) // Assuming screen width is 800
        {
            velocity.X = 0; // Stop horizontal movement
        }

        // Check if the boss is hitting the ground
        if (position.Y > 400) // Assuming 500 is ground level
        {
            position.Y = 400;
            velocity.Y = 0; // Stop downward movement
            velocity.X = 0; // Stop horizontal movement
        }

        UpdateShooting();
    }

    private void UpdateShooting()
    {
        timeSinceLastShot += Engine.TimeDelta;

        if (timeSinceLastShot >= shootCooldown)
        {
            ShootAtPlayer();
            timeSinceLastShot = 0.0f;
        }
    }

    private void ShootAtPlayer()
    {
        Vector2 playerPosition = Game.player.playerPosition;
        Vector2 initialPlayerPosition = playerPosition; // Store initial player position

        Vector2 direction = (initialPlayerPosition - position).Normalized();
        Fireball fireball = new Fireball(position, direction * bulletSpeed, new Vector2(10, 10), GameColor.White);

        // Set the source of the fireball
        fireball.Source = this;
    }

    private void ApplyGravity()
    {
        // Accumulate gravity over time
        accumulatedGravity += GRAVITY;
        velocity.Y += accumulatedGravity;
        Console.WriteLine($"Velocity Y: {velocity.Y}");
    }


    private void MoveTowardsPlayer()
    {
        Vector2 direction = CalculateDirection(player.Position - position);

        // Update boss's position only if it doesn't collide with any blocks
        string collisionDetected = CollisionManager.checkBlockCollision(this, direction * speed);

        if (collisionDetected == "na")
        {
            // Adjust the boss's velocity based on the direction towards the player
            velocity = direction * speed;
        }
    }

    private void AdjustPositionOnCollision(string collisionDetected)
    {
        Console.WriteLine($"Collision Detected: {collisionDetected}");
        // Handle collision logic here
        // For example, change the boss's color to black
        SDL.SDL_SetRenderDrawColor(Renderer, 0, 0, 0, 255); // Change color if needed

        // Adjust the boss's position based on collision direction
        if (position.Y <= 400) // Check if the boss is not on the ground
        {
            if (collisionDetected.Contains("left"))
            {
                // Move the boss to the right of the block
                position.X += 1;
            }
            else if (collisionDetected.Contains("right"))
            {
                // Move the boss to the left of the block
                position.X -= 1;
            }
            else if (collisionDetected.Contains("up"))
            {
                // Move the boss downwards
                position.Y += 1;
            }
            else if (collisionDetected.Contains("down"))
            {
                // Move the boss upwards
                position.Y -= 1;
            }
        }
    }

    private bool IsPlayerInRadius()
    {
        float distance = CalculateDistance(position, player.Position);
        return distance <= followRadius;
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

    private float CalculateDistance(Vector2 point1, Vector2 point2)
    {
        float dx = point1.X - point2.X;
        float dy = point1.Y - point2.Y;
        return (float)Math.Sqrt(dx * dx + dy * dy);
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
        SDL.SDL_SetRenderDrawColor(Renderer, 255, 0, 0, 255); // Customize color if needed

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
