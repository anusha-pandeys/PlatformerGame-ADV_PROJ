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
        Stopwatch s = new Stopwatch();
        s.Start();
        while (s.Elapsed < TimeSpan.FromSeconds(5))
        {
            // Draw background
            SDL.SDL_SetRenderDrawColor(Renderer, 0, 0, 0, 255); // Set background to white
            SDL.SDL_RenderClear(Renderer);

            Vector2 textPosition = new Vector2(50, 240);
            textRenderer.displayText("You lose :(", textPosition, Color.Red, font);

            SDL.SDL_RenderPresent(Renderer);
        }
    }

}