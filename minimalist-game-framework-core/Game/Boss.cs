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
    private float shootCooldown = 10000.0f;
    private float timeSinceLastShot = 0.0f;
    private float bulletSpeed = 30.0f;
    private Texture bossTexture;
    private Collidable bossCollidable;  // New collidable for the boss
    private int level;
    private double floor;
    public HealthBar healthBar;

    public Boss(int level, double highestFloor, Vector2 position, Player player, float followRadius, float speed)
    {
        this.position = position;
        this.level = level;
        floor = highestFloor;
        size = new Vector2((float)(Blocks.size.X * 1.5), (float)(Blocks.size.Y * 1.5));
        this.player = player;
        this.followRadius = followRadius;
        this.speed = speed;
        this.velocity = Vector2.Zero;

        // Create a new Collidable for the boss
        bossCollidable = new Collidable(this, "boss");
        CollisionManager.AddObj("boss", this);  // Register the boss with the collision manager

        Game.entities.Add(this);

        string relativePath = "Assets\\HADES.png";
        string absolutePath = System.IO.Path.GetFullPath(relativePath);
        bossTexture = Engine.LoadTexture(absolutePath);

       healthBar = new HealthBar("bossHealthBar", new Vector2(this.position.X, this.position.Y), 100,
           new Vector2(70f, 30f));


    }

    public void Update()
    {


        if (healthBar.getHealth() > 0)
        {

            healthBar.setPosition(new Vector2(this.position.X, this.position.Y - 40f));

            foreach (var entity in Game.entities.ToArray())
            {
                if (entity is Bullet bullet && bullet.Source == this && Game.player.level == level)
                {
                    bullet.BulletLoop(Game.localCamera);
                }
            }

            long startTime = DateTime.Now.Ticks;
            double secondsElapsed = 0;

            if (IsPlayerInRadius() && Game.player.level == level)
            {
                secondsElapsed = new TimeSpan(DateTime.Now.Ticks - startTime).TotalSeconds;
                MoveTowardsPlayer(secondsElapsed);
            }
            else
            {
                velocity = Vector2.Zero;
            }

            ApplyGravity();

            CollisionObject playerCollision = checkCollision("player");
            if (playerCollision.getCollided())
            {
                // Handle collision logic with the player
                Game.player.chargeBar.setCharge(0);
            }

            // Check for collisions with spears
            CollisionObject spearCollision = checkCollision("spear");
            if (spearCollision.getCollided())
            {
                // Handle collision logic with spears
                healthBar.setHealth(healthBar.getHealth() - 1); // Adjust damage as needed
            }


            // Check collisions along the Y-axis before updating the position
            HandleCollisionY(secondsElapsed);

            // Update the position here
            position += velocity;

            // Handle collisions along the X-axis
            HandleCollisionX(secondsElapsed);

            // Collision detection
            secondsElapsed = new TimeSpan(DateTime.Now.Ticks - startTime).TotalSeconds;
            CollisionObject collisionDetected = CollisionManager.checkBlockCollision(this, velocity, secondsElapsed);

            if (collisionDetected.getCollided())
            {
                AdjustPositionOnCollision(collisionDetected);
                accumulatedGravity = 0.0f;
            }

            // Check if hitting the left or right boundary
            if (position.X < 0 || position.X + size.X > 640)
            {
                velocity.X = 0;
            }

            // Check if hitting the ground
            if (position.Y + size.Y >= floor)
            {
                position.Y = (float)floor - size.Y;
                velocity.Y = 0;
            }

            UpdateShooting();
            healthBar.Render();
        }
        else
        {

            Game.bossDeath = true;
            Game.entities.Remove(this);
            if (Game.bossDeath)
            {
                Game.increaseBossKilled();
            }
            
            //
        }
    }

    private CollisionObject checkCollision(string target)
    {
        CollisionObject obj = CollisionManager.checkCollisions("boss", target, new Vector2(1, 0));
        if (obj.getCollided())
        {
            return obj;
        }
        return new CollisionObject();
    }

    private void HandleCollisionY(double secondsElapsed)
    {

        CollisionObject collisionDetected = CollisionManager.checkBlockCollision(this, new Vector2(0, velocity.Y + 2f), secondsElapsed);
        if (collisionDetected.getCollided())
        {
            position.Y += collisionDetected.getDistanceY();
            velocity.Y = 0;
        }
        else if (!CollisionManager.checkBlockCollision(this, new Vector2(0, 2), secondsElapsed).getCollided())
        {
            velocity.Y += (GRAVITY);
        }
    }
    private void HandleCollisionX(double secondsElapsed)
    {
        float horizontalMovement = velocity.X;
        CollisionObject collisionDetected = CollisionManager.checkBlockCollision(this, new Vector2(2f, 0), secondsElapsed);
        if (collisionDetected.getCollided())
        {
            position.X += collisionDetected.getDistanceX();
        }
        else
        {
            collisionDetected = CollisionManager.checkBlockCollision(this, new Vector2(-2f, 0), secondsElapsed);
            if (collisionDetected.getCollided())
            {
                position.X += collisionDetected.getDistanceX();
            }
        }
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
        Vector2 playerPosition = Game.player.position;
        Vector2 initialPlayerPosition = playerPosition;

        Vector2 direction = (initialPlayerPosition - position).Normalized();
        Bullet bullet = new Bullet(position, direction * bulletSpeed, new Vector2(10, 10), GameColor.White);
        bullet.Source = this;
    }

    private void ApplyGravity()
    {
        if (position.Y - size.Y >= floor)  // Adjust the value if the ground level is different
        {
            accumulatedGravity = 0.0f;
            velocity.Y = 0;
        }
        else
        {
            accumulatedGravity += GRAVITY;
            velocity.Y += accumulatedGravity;
            //Console.WriteLine($"Velocity Y: {velocity.Y}");
        }
    }

    private void MoveTowardsPlayer(double time)
    {
        Vector2 direction = CalculateDirection(player.Position - position);
        CollisionObject collisionDetected = CollisionManager.checkBlockCollision(this, direction * speed, time);

        if (!collisionDetected.getCollided())
        {
            velocity.X = direction.X * speed;
        }
    }




    private void AdjustPositionOnCollision(CollisionObject collisionDetected)
    {
        //Console.WriteLine($"Collision Detected: Left={collisionDetected.getLeft()}, Right={collisionDetected.getRight()}, Up={collisionDetected.getUp()}, Down={collisionDetected.getDown()}");

        SDL.SDL_SetRenderDrawColor(Renderer, 0, 0, 0, 255);

        if (position.Y <= floor)
        {
            if (collisionDetected.getLeft())
            {
                position.X += collisionDetected.getDistanceX();
            }
            else if (collisionDetected.getRight())
            {
                position.X -= collisionDetected.getDistanceX();
            }
            else if (collisionDetected.getUp())
            {
                position.Y += collisionDetected.getDistanceY();
            }
            else if (collisionDetected.getDown())
            {
                position.Y -= collisionDetected.getDistanceY();
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
        Engine.DrawTexture(bossTexture, position, null, size);
    }
}

