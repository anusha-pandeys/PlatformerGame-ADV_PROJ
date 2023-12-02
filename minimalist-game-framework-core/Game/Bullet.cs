using SDL2;
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
    public Enemy Source { get; set; }

    public Bullet(Vector2 position, Vector2 velocity, Vector2 size, GameColor color)
    {
        this.position = position;
        this.velocity = velocity;
        this.size = size;
        this.color = color;
        Game.entities.Add(this);
    }

    public void BulletLoop(Camera camera)
    {
        float fixedTimeStep = 0.016f; // Adjust this value as needed
        float accumulatedTime = 0.0f;

        // Update the bullet's position based on the velocity
        accumulatedTime += Engine.TimeDelta;

        while (accumulatedTime >= fixedTimeStep)
        {
            position += velocity * fixedTimeStep;
            accumulatedTime -= fixedTimeStep;
        }

        Render(camera);
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
        SDL.SDL_SetRenderDrawColor(Renderer, 255, 0, 0, 255); // red
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
