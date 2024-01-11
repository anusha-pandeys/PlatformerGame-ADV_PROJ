using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;

internal class CreditScreen
{
    private IntPtr Renderer => Engine.Renderer2;
    private TextRenderer textRenderer = new TextRenderer();
    private Button backButton;

    private Texture bg; // Add a background texture

    public CreditScreen()
    {
        backButton = new Button("Back", new Vector2(10, 430), new Vector2(80, 40));

        // Load the background texture for the credit screen
        string relativePath = "Assets\\startscreen.png";
        string absolutePath = System.IO.Path.GetFullPath(relativePath);
        bg = Engine.LoadTexture(absolutePath);
    }

    public void Draw(Font font)
    {
        // Draw background
        Engine.DrawTexture(bg, new Vector2(0, 0));

        // Draw white rectangle in the middle of the screen
        SDL.SDL_SetRenderDrawColor(Renderer, 255, 255, 255, 255); // Set color to white
        SDL.SDL_Rect rect = new SDL.SDL_Rect()
        {
            x = 100, // Center horizontally
            y = 200, // Center vertically
            w = 417,
            h = 200
        };
        SDL.SDL_RenderFillRect(Renderer, ref rect);

        // Display credits information
        string[] creditsLines = {
            "Created by -",
            "Tejas Pakalapati",
            "Anusha P",
            "Abhigna",
            "Vaibhav Attre"
        };

        // Set color for text
        SDL.SDL_Color textColor = new SDL.SDL_Color() { r = 0, g = 0, b = 0, a = 255 }; // Black color

        // Display credits text on separate lines
        Vector2 basePosition = new Vector2(100, 200); 
        int lineHeight = 15; // Adjust the line height as needed

        for (int i = 0; i < creditsLines.Length; i++)
        {
            Vector2 linePosition = new Vector2(basePosition.X, basePosition.Y + i * lineHeight);
            textRenderer.displayText(creditsLines[i], linePosition, Color.Black, font);
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
