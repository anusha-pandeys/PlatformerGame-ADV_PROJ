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
    private CreditScreen creditScreen = new CreditScreen();
    private RulesMenu rulesMenu = new RulesMenu();
    private Scoreboard scoreboard = new Scoreboard();
    Font yFont = Engine.LoadFont("Retro Gaming.ttf", 11);
    private bool showRulesScreen;
    private bool showCreditScreen;
    private bool showStartMenu;
    private bool showScoreboardScreen;
    private Button startButton = new Button("Start Game", new Vector2(200, 200), new Vector2(50, 50));
    private Button scoreboardButton = new Button("Scoreboard", new Vector2(200, 250), new Vector2(50, 50));
    private Button rulesButton = new Button("Rules", new Vector2(200, 300), new Vector2(50, 50));
    private Button creditsButton = new Button("Credits", new Vector2(200, 350), new Vector2(50, 50));
    public StartMenu()
    {
        // Create buttons
        
        

        // Add buttons to the list
        buttons.Add(startButton);
        buttons.Add(scoreboardButton);
        buttons.Add(rulesButton);
        buttons.Add(creditsButton);
    }

    public bool IsStartButtonClicked()
    {
        // Check if the "Start Game" button is clicked
        return startButton.IsClicked();
    }

    public bool IsScoreBoardClicked()
    {
        return scoreboardButton.IsClicked();
    }

    public void Update()
    {
        // Poll for events
        SDL.SDL_PumpEvents();

        // Handle button click events
        foreach (Button button in buttons)
        {
            if (button.IsClicked())
            {
                HandleButtonClick(button);
            }
        }

        // Handle back button click event
        if (showStartMenu && IsStartButtonClicked())
        {
            // Handle back button click logic (e.g., go back to the main menu)
            showRulesScreen = false;
            showCreditScreen = false;
            showScoreboardScreen = false;
        } else if (showScoreboardScreen && scoreboard.IsBackButtonClicked())
        {
            showRulesScreen = false;
            showCreditScreen = false;
            showScoreboardScreen = false;
        }
        else if (showRulesScreen)
        {
            // Check if back button is clicked in RulesMenu
            if (rulesMenu.IsBackButtonClicked())
            {
                showRulesScreen = false;
                showCreditScreen = false;
                showScoreboardScreen = false;
            }
        }
        else if (showCreditScreen)
        {
            // Check if back button is clicked in CreditScreen
            if (creditScreen.IsBackButtonClicked())
            {
                showRulesScreen = false;
                showCreditScreen = false;
                showScoreboardScreen = false;
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


        if (showRulesScreen)
        {
            rulesMenu.Draw(yFont);
        }
        else if (showCreditScreen)
        {
            creditScreen.Draw(yFont);
        }
        else if (showScoreboardScreen)
        {
            scoreboard.Draw(yFont);
        }
        // Present renderer
        SDL.SDL_RenderPresent(Renderer);
    }

    private void HandleButtonClick(Button button)
    {
        //Console.WriteLine($"Button Clicked: {button.Text}");

        if (button.Text == "Start Game")
        {
            // Start the game (placeholder logic)
            showRulesScreen = false;
            showCreditScreen = false;
        }
        else if (button.Text == "Scoreboard")
        {
            // Open scoreboard view (placeholder logic)
            Console.WriteLine("Opening Scoreboard");
            showRulesScreen = false;
            showCreditScreen = false;
            showScoreboardScreen = true;
        }
        else if (button.Text == "Rules")
        {
            // Show rules menu
            showRulesScreen = true;
            showCreditScreen = false;
            showScoreboardScreen = false;
        }
        else if (button.Text == "Credits")
        {
            // Show credit screen
            showCreditScreen = true;
            showRulesScreen = false;
            showScoreboardScreen = false;
        }
    }
}
