using SDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

internal class Pits : Entity
{
    private IntPtr Renderer => Engine.Renderer2;
    private Vector2 position;
    private Vector2 size;
    private Collidable pits;
    public bool playerDeath = false;
    //private bool first;
    public Pits(Vector2 position, Vector2 size)
    {
        this.position = position;
        this.size = size;
        this.pits = new Collidable(this, "pit");
        //first = false;
        Game.entities.Add(this);
    }
    public bool getPlayerDeath()
    {
        return playerDeath; 
    }
    public void pitsLoop()
    {
        //Render(new Camera());
        if (checkCollision())
        {
            playerDeath = true;
        }
    }
    public bool checkCollision()
    {
        //Dictionary<string, bool> ret = CollisionManager.checkCollisions("pit", "player");
        if(CollisionManager.checkCollisions("player", "pit"))
        {
            return true;
        }
        return false;
    }

    protected override Rectangle CalculateBound()
    {
        return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }

    public override void Render(Camera camera)
    {
        Draw(position, size);
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
}
