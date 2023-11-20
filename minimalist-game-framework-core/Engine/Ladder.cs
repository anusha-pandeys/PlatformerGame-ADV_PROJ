using SDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

internal class Ladder : Entity
{
    private IntPtr Renderer => Engine.Renderer2;
    private Vector2 position;
    private Vector2 size;
    private Collidable ladders;
    private bool translate = false;

    public Ladder(Vector2 position, Vector2 size)
    {
        this.position = position;
        this.size = size;
        this.ladders = new Collidable(this, "ladder");
    }

    public bool getTranslate()
    {
        return translate;
    }
    public void ladderLoop()
    {
        Render(new Camera());
        if(checkCollision())
        {
            translate = true;
        } else
        {
            translate = false;
        }
    }
    public bool checkCollision()
    {
        //Dictionary<string, bool> ret = CollisionManager.checkCollisions("pit", "player");
        if (CollisionManager.checkCollisions("player", "ladder"))
        {
            return true;
        }
        return false;
    }
    protected override Rectangle CalculateBound()
    {
        return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }

    protected override void Draw(Vector2 position, Vector2 size)
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
        //Draw(position, size);
        Vector2 localPosition = camera.globalToLocal(position);
        Draw(localPosition, size);
    }
}

