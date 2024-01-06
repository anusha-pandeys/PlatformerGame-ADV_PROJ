using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System.Drawing;

internal class Flower : Entity
{
    private IntPtr Renderer => Engine.Renderer2;
    private Vector2 position;
    private Vector2 size;
    private bool isCollected = false;

    public Flower(Vector2 position)
    {
        this.position = position;
        this.size = new Vector2(20, 20); 
    }

    public bool IsCollected => isCollected;

    public void Collect()
    {
        isCollected = true;
    }

    public override void Render(Camera camera)
    {
        if (!isCollected)
        {
            Draw(position, size);
        }
    }

    protected override Rectangle CalculateBound()
    {
        return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }

    protected override void Draw(Vector2 position, Vector2 size)
    {
        SDL.SDL_SetRenderDrawColor(Renderer, 255, 105, 180, 255); // Pink color
        SDL.SDL_Rect flowerRect = new SDL.SDL_Rect()
        {
            x = (int)position.X,
            y = (int)position.Y,
            w = (int)size.X,
            h = (int)size.Y
        };
        SDL.SDL_RenderFillRect(Renderer, ref flowerRect);
    }
}

