using SDL2;
using System;
using System.Collections.Generic;
using System.Text;


internal class Blocks : Entity
{
    private IntPtr Renderer => Engine.Renderer2; // Gets the SDL Renderer from the Engine class
    private Vector2 position;
    private Vector2 size;
    private GameColor color;
    public Blocks(Vector2 position, Vector2 size, GameColor color)
    {
        this.position = position;
        this.size = size;
        this.color = color;
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
