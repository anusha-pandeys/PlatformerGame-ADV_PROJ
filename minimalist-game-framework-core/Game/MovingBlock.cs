using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

internal class MovingBlock : Entity
{
    private IntPtr Renderer => Engine.Renderer2; // Gets the SDL Renderer from the Engine class
    private Vector2 position;
    private Vector2 size;
    private Vector2 velocity;
    private GameColor color;
    private int direction;

    public MovingBlock(Vector2 position, Vector2 size, GameColor color)
    {
        this.position = position;
        this.size = size;
        this.color = color;
        this.direction = 1;
        this.velocity = new Vector2(1, 0);
    }
    public List<Vector2> getCoordinates()
    {
        List<Vector2> result = new List<Vector2>();
        result.Add(position);
        result.Add(size);
        return result;
    }
    public void Render()
    {
        DrawRectangle(position, size, GameColor.Block1);
    }

    public void updateCoordinates()
    {
        if (position.X > 400)
        {
            this.direction = -1;
        } 
        if (position.X < 100)
        {
            this.direction = 1;
        }
        position.X += velocity.X * this.direction;
        Render();
    }
    public void DrawRectangle(Vector2 position, Vector2 size, GameColor color)
    {
        SDL.SDL_SetRenderDrawColor(Renderer, 0, 255, 0, 255); // green
        SDL.SDL_Rect rect = new SDL.SDL_Rect()
        {
            x = (int)position.X,
            y = (int)position.Y,
            w = (int)size.X,
            h = (int)size.Y
        };
        SDL.SDL_RenderFillRect(Renderer, ref rect);
    }

    public Boolean detectCollision(List<Entity> entities, Vector2 prospectiveVelocity)
    {
        return true;
    }
}

