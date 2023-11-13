using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;


internal class StartMenu
{
    private IntPtr Renderer => Engine.Renderer2;
    private List<Button> buttons = new List<Button>();
    private TextRenderer textRenderer = new TextRenderer();




    public StartMenu()
    {
        // Create buttons
        Button startButton = new Button("Start Game", new Vector2(200, 200), new Vector2(50, 50));
        Button scoreboardButton = new Button("Scoreboard", new Vector2(200, 250), new Vector2(50, 50));
        Button rulesButton = new Button("Rules", new Vector2(200, 300), new Vector2(50, 50));
        Button creditsButton = new Button("Credits", new Vector2(200, 350), new Vector2(50, 50));

        // Add buttons to the list
        buttons.Add(startButton);
        buttons.Add(scoreboardButton);
        buttons.Add(rulesButton);
        buttons.Add(creditsButton);
    }

    public void Update()
    {
        // Handle button click events
        foreach (Button button in buttons)
        {
            if (button.IsClicked())
            {
                HandleButtonClick(button);
            }
        }
    }

    public void Draw(Font font)
    {
        // Draw background
        SDL.SDL_SetRenderDrawColor(Renderer, 255, 255, 255, 255); // Set background color to white
        SDL.SDL_RenderClear(Renderer);

        // Draw buttons
        foreach (Button button in buttons)
        {
            // Draw button rectangle
            SDL.SDL_SetRenderDrawColor(Renderer, 0, 0, 255, 255); // Set button color to blue
            SDL.SDL_Rect rect = new SDL.SDL_Rect()
            {
                x = (int)button.getPosition().X,
                y = (int)button.getPosition().Y,
                w = (int)button.getSize().X,
                h = (int)button.getSize().Y
            };

            SDL.SDL_RenderFillRect(Renderer, ref rect);

            // Draw button text
            Vector2 textPosition = new Vector2(button.getPosition().X + 10, button.getPosition().Y + 10); // Adjust text position for padding
            textRenderer.displayText(button.Text, textPosition, Color.Black, font);
        }

        // Present renderer
        SDL.SDL_RenderPresent(Renderer);
    }


    private void HandleButtonClick(Button button)
    {
        // Implement logic for button click events here
        if (button.Text == "Start Game")
        {
            // Start the game (placeholder logic)
            Game game = new Game();
            game.Update();
        }
        else if (button.Text == "Scoreboard")
        {
            // Open scoreboard view (placeholder logic)
            Console.WriteLine("Opening Scoreboard");
        }
        else if (button.Text == "Rules")
        {
            // Open rules view (placeholder logic)
            Console.WriteLine("Opening Rules");
        }
        else if (button.Text == "Credits")  
        {
            // Open game credits view (placeholder logic)
            Console.WriteLine("Opening Credits");
        }
    }
}
