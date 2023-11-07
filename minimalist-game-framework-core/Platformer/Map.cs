using SDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

public class Map
{
    private IntPtr Renderer => Engine.Renderer2;  // Gets the SDL Renderer from the Engine class
    public void setBackgroundColor()
    {
        SDL.SDL_SetRenderDrawColor(Renderer, 255, 255, 255, 255); // white
        SDL.SDL_Rect rect = new SDL.SDL_Rect()
        {
            x = 0,
            y = 0,
            w = 800,
            h = 800
        };

        SDL.SDL_RenderFillRect(Renderer, ref rect);
    }
}
