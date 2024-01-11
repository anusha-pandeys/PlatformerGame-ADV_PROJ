using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;

internal class LoseScreen
{
    private IntPtr Renderer => Engine.Renderer2;
    private TextRenderer textRenderer;
    private Font font = Engine.LoadFont("Retro Gaming.ttf", 11);
    
    public LoseScreen()
    {
        textRenderer = new TextRenderer();
        
    }

    public void show()
    {
        
        // Draw background
        SDL.SDL_SetRenderDrawColor(Renderer, 0, 0, 0, 255); // Set background to white
        SDL.SDL_RenderClear(Renderer);
        Stopwatch s = new Stopwatch();
        s.Start();
        while (s.Elapsed < TimeSpan.FromSeconds(3))
        {
            //Engine.DrawTexture(loseScreen, new Vector2(0, 0), size: new Vector2(640, 480));
        }
        SDL.SDL_RenderPresent(Renderer);
        
    }

}