using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;

internal class Enemy : Entity
{
    private IntPtr Renderer => Engine.Renderer2;
    private Vector2 position;
    private Vector2 size;
    private Color enemyColor;
    private float shootCooldown = 0.5f;
    private float timeSinceLastShot = 0.0f;
    private float bulletSpeed = 50.0f;


    public Enemy(Vector2 spawnPosition, Vector2 size)
    {
        this.position = spawnPosition;
        this.size = size;
        this.enemyColor = Color.Blue;
        Game.entities.Add(this);
    }

    public void EnemyLoop()
    {
        Console.WriteLine("EnemyLoop called");
        UpdateShooting();

        // Update and render bullets
        foreach (var entity in Game.entities.ToArray())
        {
            if (entity is Bullet bullet && bullet.Source == this)
            {
                bullet.BulletLoop(Game.localCamera); // Pass the camera here
            }
        }
    }



    private void UpdateShooting()
    {
        Console.WriteLine("UpdateShooting called");
        Console.WriteLine($"Time Delta: {Engine.TimeDelta}");
        
        timeSinceLastShot += Engine.TimeDelta;

        if (timeSinceLastShot >= shootCooldown)
        {
            Console.WriteLine("Shooting at player");
            ShootAtPlayer();
            timeSinceLastShot = 0.0f;
        }
    }


    private void ShootAtPlayer()
    {
        Console.WriteLine("Shooting at player");

        Vector2 playerPosition = Game.player.playerPosition;
        Console.WriteLine($"Player Position: {playerPosition}");

        Vector2 direction = (playerPosition - position).Normalized();
        Console.WriteLine($"Direction: {direction}");
        Console.WriteLine("Creating bullet");
        Bullet bullet = new Bullet(position, direction * bulletSpeed, new Vector2(10, 10), GameColor.White);


        // Set the source of the bullet
        bullet.Source = this;

        Game.entities.Add(bullet);
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
        SDL.SDL_SetRenderDrawColor(Renderer, enemyColor.R, enemyColor.G, enemyColor.B, enemyColor.A);

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
