using SDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

internal class Spear : Entity
{
    private IntPtr Renderer => Engine.Renderer2;
    private Vector2 position;
    private Vector2 size;
    private Collidable spear;

    public Spear()
    {
        this.position = Game.player.Position + new Vector2(Game.player.size.X + 5f, Game.player.size.Y/2);
        this.size = new Vector2(50, 5);
        this.spear = new Collidable(this, "spear");
        Game.entities.Add(this);
    }

    public void spearLoop()
    {
        this.position = Game.player.Position + new Vector2(Game.player.size.X + 5f, Game.player.size.Y / 2); 
    }
    public override void Render(Camera camera)
    {
        Draw(Game.localCamera.globalToLocal(this.position), this.size);
    }

    protected override Rectangle CalculateBound()
    {
        return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }

    protected override void Draw(Vector2 position, Vector2 size)
    {
        SDL.SDL_SetRenderDrawColor(Renderer, 116, 86, 75, 100); // green
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
