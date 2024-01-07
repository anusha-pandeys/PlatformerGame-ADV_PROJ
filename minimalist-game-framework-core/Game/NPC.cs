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
    public Vector2 position;
    public Vector2 size;
    private float followRadius;
    private float speed;
    private Player player;
    private float distance;
    private Color originalColor;
    private Color npcColor;
    private float collisionCooldown = 0.1f;
    private float timeSinceCollision = 0.0f;
    private Collidable npc;
    public HealthBar healthBar;
    private Texture npcTexture;
    public string tag;
    
    public NPC(Vector2 position, Vector2 size, Player player, Color npcColor, float followRadius, float speed, string filePath, string tag)
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
        healthBar = new HealthBar("npcHealthBar", new Vector2(this.position.X, this.position.Y), 100,
            new Vector2(70f, 30f));
        this.tag = tag;
        string absolutePath = System.IO.Path.GetFullPath(filePath);
        npcTexture = Engine.LoadTexture(absolutePath);
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
        if (healthBar.getHealth() > 0)
        {
            healthBar.setPosition(new Vector2(this.position.X, this.position.Y - 40f));
            if (IsPlayerInRadius())
            {
                FollowPlayer();
            }
            CollisionObject obj = checkCollision("player");
            if (obj.getCollided())
            {
                Game.player.chargeBar.setCharge(0);
            }
            
            // Check for collisions with the player
            //string collisionDetected = CollisionManager.checkBlockCollision(player, new Vector2(speed, 0), 1);

            // Update NPC's position based on collision detection
            if (CollisionManager.checkBlockCollision(player, new Vector2(-1 * speed, 0), 1).getCollided())
            {//
                position.X -= speed;
            }
            else if (CollisionManager.checkBlockCollision(player, new Vector2(speed, 0), 1).getCollided())
            {
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

            healthBar.Render();
            //Render(Game.localCamera);
        } else
        {
            Game.entities.Remove(this);
        }
    }
    private CollisionObject checkCollision(string target)
    {
        CollisionObject obj = CollisionManager.checkCollisions(tag, target, new Vector2(1, 0));
        if (obj.getCollided())
        {
            return obj;
        }
        return new CollisionObject();
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
        Engine.DrawTexture(npcTexture, position, null, size);
    }

}
