using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

internal class HealthBar : UIElements
{
    private string name;
    private Vector2 position;
    private int health;
    private IntPtr Renderer => Engine.Renderer2;
    public HealthBar(string name, Vector2 position, int health)
    {
        setName(name);
        setPosition(position);
        this.health = health;
    }

    public void setHealth(int health)
    {
        this.health = health;
    }

    public int getHealth()
    {
        return health;
    }
    public override string getName()
    {
        return name;
    }
    public override void setName(string name)
    {
        this.name = name;
    }
    public override Vector2 getPosition()
    {
        return position;
    }
    public override void setPosition(Vector2 position)
    {
        this.position = position;
    }

    public override void Render()
    {
        Draw(position, new Vector2(200, 50));
    }


    protected override void Draw(Vector2 position, Vector2 size)
    {
        SDL.SDL_SetRenderDrawColor(Renderer, 255, 255, 51, 100);

        SDL.SDL_Rect rect = new SDL.SDL_Rect()
        {
            x = (int)position.X,
            y = (int)position.Y,
            w = (int)size.X,
            h = (int)size.Y
        };

        SDL.SDL_RenderFillRect(Renderer, ref rect);

        SDL.SDL_SetRenderDrawColor(Renderer, 255, 51, 51, 100);
        int offset = 30;
        double sizeX =  ((double)size.X * (health / 100.0))-20;
        SDL.SDL_Rect rect2 = new SDL.SDL_Rect()
        {
            
            x = (int)(position.X+ 10),
            y = (int)(position.Y+ 10),
            w = (int)sizeX,
            h = (int)(size.Y-20)
        };

        SDL.SDL_RenderFillRect(Renderer, ref rect2);
    }
}
