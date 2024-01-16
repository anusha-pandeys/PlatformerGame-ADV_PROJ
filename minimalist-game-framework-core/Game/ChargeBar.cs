﻿using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

internal class ChargeBar : UIElements
{
    private string name;
    private Vector2 position;
    private Vector2 size;
    public int charge;
    private IntPtr Renderer => Engine.Renderer2;
    private Texture chargeBackgroundTexture;
    private ResizableTexture chargeTexture;
    public ChargeBar(string name, Vector2 position, int charge, Vector2 size)
    {
        setName(name);
        setPosition(position);
        this.charge = charge;
        this.size = size;
        string relativePath = "Assets\\chargeBarEmpty.png";
        string absolutePath = System.IO.Path.GetFullPath(relativePath);
        chargeBackgroundTexture = Engine.LoadTexture(absolutePath);

        relativePath = "Assets\\chargeBar.png";
        absolutePath = System.IO.Path.GetFullPath(relativePath);
        chargeTexture = Engine.LoadResizableTexture(absolutePath, 0, 0,0,0);
    }

    public void setCharge(int charge)
    {
        this.charge = charge;
    }

    public int getCharge()
    {
        return charge;
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
        Draw(position, getSize());
    }
    public override void setSize(Vector2 size)
    {
        this.size = size;
    }

    public override Vector2 getSize()
    {
        return size;
    }

    protected override void Draw(Vector2 position, Vector2 size)
    {
        SDL.SDL_SetRenderDrawColor(Renderer, 255, 255, 51, 100);
        Engine.DrawTexture(chargeBackgroundTexture, position, scaleMode: TextureScaleMode.Nearest);
        float sizeX = ((float)size.X * (float)(charge / 100.0));
        Console.WriteLine(sizeX);
        position.X += 97;
        position.Y += 33;
        Engine.DrawResizableTexture(chargeTexture, new Bounds2(position, new Vector2(sizeX+25, size.Y-20)), scaleMode: TextureScaleMode.Nearest);
        /*SDL.SDL_Rect rect = new SDL.SDL_Rect()
        {
            x = (int)position.X,
            y = (int)position.Y,
            w = (int)size.X,
            h = (int)size.Y
        };

        SDL.SDL_RenderFillRect(Renderer, ref rect);
        Vector2 healthSize = getSize() - new Vector2(20, 20);
        SDL.SDL_SetRenderDrawColor(Renderer, 187, 51, 255, 100);
        int offset = 30;
        
        SDL.SDL_Rect rect2 = new SDL.SDL_Rect()
        {

            x = (int)(position.X + 10),
            y = (int)(position.Y + 10),
            w = (int)sizeX,
            h = (int)(healthSize.Y)
        };

        SDL.SDL_RenderFillRect(Renderer, ref rect2);*/
    }


}
