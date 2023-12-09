using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using SDL2;

internal class Slides : Entity
{
    private IntPtr Renderer => Engine.Renderer2;
    private Vector2 position;
    private Vector2 size;
    private Collidable slide;

    public Slides(Vector2 position, Vector2 size)
    {
        this.position = position;
        this.size = size;
        this.slide = new Collidable(this, "slide");
        Game.entities.Add(this);
    }

    public void slidesLoop()
    {
        Render(Game.localCamera);
        //Dictionary<string, bool> collided = CollisionManager.checkCollisions("player", "slide");
        /*if (collided.)
        {
            System.Console.WriteLine("collided with slide");
        }*/
    }

    protected override Rectangle CalculateBound()
    {
        return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }

    protected override void Draw(Vector2 position, Vector2 size)
    {
        int partitions = 100;
        Vector2 currPos = position;
        Vector2 currSize = size;
        currSize.X = currSize.X/partitions;

        while (!(currSize.Y <= 0)) 
        {
            drawRect(currPos, currSize);
            currPos.X += currSize.X;
            currPos.Y += 1f;
            currSize.Y -= 1f;
            //currSize.X -= size.X / partitions;
        }
    }


    private void drawRect(Vector2 position, Vector2 size)
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

    public override void Render(Camera camera)
    {
        Draw(this.position, this.size);
    }
}
