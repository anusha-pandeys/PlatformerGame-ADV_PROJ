using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;

class NameScreen
{
    private string playerName = "";
    private TextRenderer textRenderer = new TextRenderer();
    private Button enterButton;
    private Font bodyFont = Engine.LoadFont("Retro Gaming.ttf", 16); // Use your desired font

    public NameScreen()
    {
        enterButton = new Button("Enter", new Vector2(300, 300), new Vector2(100, 40));
    }

    public void Update()
    {
        playerName += Engine.TypedText;

        if (Engine.GetKeyDown(Key.Backspace) && playerName != null && playerName.Length > 0)
            playerName = playerName.Remove(playerName.Length - 1, 1);

        // Handle other input if needed
    }

    public void Render()
    {
        // Draw background and other elements for the name screen

        // Draw instructions text
        string instructions = "Enter Your Name";
        textRenderer.displayText(instructions, new Vector2(250, 15), Color.White, bodyFont);

        // Draw player name input
        textRenderer.displayText(playerName, new Vector2(250, 250), Color.White, bodyFont);

        // Draw enter button
        SDL.SDL_SetRenderDrawColor(Engine.Renderer2, 0, 255, 0, 255); // Set button color to green
        SDL.SDL_Rect enterRect = new SDL.SDL_Rect()
        {
            x = (int)enterButton.getPosition().X,
            y = (int)enterButton.getPosition().Y,
            w = (int)enterButton.getSize().X,
            h = (int)enterButton.getSize().Y
        };
        SDL.SDL_RenderFillRect(Engine.Renderer2, ref enterRect);

        // Draw enter button text
        Vector2 enterTextPosition = new Vector2(enterButton.getPosition().X, enterButton.getPosition().Y + 10);
        textRenderer.displayText(enterButton.Text, enterTextPosition, Color.Black, bodyFont);
    }


    public bool IsEnterButtonClicked()
    {
        return enterButton.IsClicked();
    }

    public string GetPlayerName()
    {
        return playerName;
    }
}
