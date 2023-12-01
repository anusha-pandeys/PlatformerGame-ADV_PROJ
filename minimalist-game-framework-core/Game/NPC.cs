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
    private IntPtr Renderer => Engine.Renderer2;
    private Vector2 position;
    private Vector2 size;
    private float followRadius;
    private float speed;
    private Player player;
    private float distance;
    private Color originalColor;
    private Color npcColor;
    private float collisionCooldown = 0.1f;
    private float timeSinceCollision = 0.0f;
    private Collidable npc;

    public NPC(Vector2 position, Vector2 size, Player player, Color npcColor, float followRadius, float speed)
    {
        this.position = position;
        this.size = size;
        this.player = player;
        this.originalColor = npcColor; // Set the original color
        this.npcColor = originalColor;
        this.followRadius = followRadius;
        this.speed = speed;
        Game.entities.Add(this);
        this.npc = new Collidable(this, "npc");
    }

    private void UpdateCollisionCooldown()
    {
        // Update the elapsed time since the last collision
        timeSinceCollision += Engine.TimeDelta;

        // Check if enough time has passed since the last collision to revert to the original color
        if (timeSinceCollision >= collisionCooldown)
        {
            npcColor = originalColor;
        }
    }

    //

    public void Update()
    {
        if (IsPlayerInRadius())
        {
            FollowPlayer();
        }

        // Check for collisions with the player
        //string collisionDetected = CollisionManager.checkBlockCollision(player, new Vector2(speed, 0), 1);

        // Update NPC's position based on collision detection
        if (CollisionManager.checkBlockCollision(player, new Vector2(-1*speed, 0), 1).getCollided())
        {
            Console.WriteLine("Collision Left");
            position.X -= speed;
        }
        else if (CollisionManager.checkBlockCollision(player, new Vector2(speed, 0), 1).getCollided())
        {
            Console.WriteLine("Collision Right");
            position.X += speed;
        }


        // Handle collision and update color
        HandleCollision();
        UpdateCollisionCooldown();  // Update collision cooldown

        // Reset the NPC color if enough time has passed since the last collision
        if (timeSinceCollision >= collisionCooldown)
        {
            npcColor = originalColor;
        }


        //Render(Game.localCamera);
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
        //string collisionDetected = CollisionManager.checkBlockCollision(this, direction * speed);
        if (!CollisionManager.checkBlockCollision(player, new Vector2(speed, 0), 1).getCollided())
        {
            position += direction * speed;
        }
    }

    private bool playerCollided()
    {
        Rectangle npcBounds = CalculateBound();
        Rectangle playerBounds = player.GetPlayerBounds();
        return npcBounds.IntersectsWith(playerBounds);
    }


    private void HandleCollision()
    {
        // Check if the collision is with the player
        if (playerCollided())
        {
            // Handle collision logic here for NPC colliding with the player
            // For example, change the NPC's color to black
            npcColor = new Color(0, 0, 0, 255);
            timeSinceCollision = 0.0f; // Reset the timer
        }
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

    public override void Render(Camera camera)
    {
        Vector2 localPosition = camera.globalToLocal(position);
        Draw(localPosition, size);
    }

    protected override void Draw(Vector2 position, Vector2 size)
    {
        SDL.SDL_SetRenderDrawColor(Renderer, npcColor.R, npcColor.G, npcColor.B, npcColor.A);

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
