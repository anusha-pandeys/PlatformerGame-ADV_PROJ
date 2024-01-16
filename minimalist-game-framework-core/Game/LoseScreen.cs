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
    private Music loseMusic;
    
    public LoseScreen()
    {
        textRenderer = new TextRenderer();
        loseMusic = Engine.LoadMusic("loseMusic.mp3");

    }

    public void show()
    {

        // Draw background
        Engine.PlayMusic(loseMusic, true, 0);
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