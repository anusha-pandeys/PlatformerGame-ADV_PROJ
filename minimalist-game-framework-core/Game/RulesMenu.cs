using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;

internal class RulesMenu
{
    private IntPtr Renderer => Engine.Renderer2;
    private TextRenderer textRenderer = new TextRenderer();
    private Button backButton;

    public RulesMenu()
    {
        backButton = new Button("Back", new Vector2(10, 430), new Vector2(80, 40));
    }

    public void Draw(Font font)
    {
        // Draw background
        SDL.SDL_SetRenderDrawColor(Renderer, 255, 255, 255, 255); // Set background color to white
        SDL.SDL_RenderClear(Renderer);

        // Draw other elements (if any) for the credit screen or rules menu

        // Draw instructions text
        string[] instructionLines = {
            "You are Persephone,",
            "you want to escape hell,",
            "jump up with W key,",
            "move left with A key,",
            "move right with D key"
        };

        // Set color for text
        SDL.SDL_Color textColor = new SDL.SDL_Color() { r = 0, g = 0, b = 0, a = 255 }; // Black color

        // Display instructions text on separate lines
        Vector2 basePosition = new Vector2(50, 150); // Adjust the starting position
        int lineHeight = 30; // Adjust the line height as needed

        for (int i = 0; i < instructionLines.Length; i++)
        {
            Vector2 linePosition = new Vector2(basePosition.X, basePosition.Y + i * lineHeight);
            textRenderer.displayText(instructionLines[i], linePosition, Color.Black, font);
        }

        // Draw back button
        SDL.SDL_SetRenderDrawColor(Renderer, 0, 255, 0, 255); // Set button color to green
        SDL.SDL_Rect backRect = new SDL.SDL_Rect()
        {
            x = (int)backButton.getPosition().X,
            y = (int)backButton.getPosition().Y,
            w = (int)backButton.getSize().X,
            h = (int)backButton.getSize().Y
        };
        SDL.SDL_RenderFillRect(Renderer, ref backRect);

        // Draw back button text
        Vector2 backTextPosition = new Vector2(backButton.getPosition().X + 10, backButton.getPosition().Y + 10);
        textRenderer.displayText(backButton.Text, backTextPosition, Color.Black, font);

        SDL.SDL_RenderPresent(Renderer);
    }

    public bool IsBackButtonClicked()
    {
        return backButton.IsClicked();
    }
}
