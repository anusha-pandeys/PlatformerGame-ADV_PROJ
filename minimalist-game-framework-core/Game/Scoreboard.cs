using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Linq; // Add this using directive

internal class Scoreboard
{
    private IntPtr Renderer => Engine.Renderer2;
    private TextRenderer textRenderer = new TextRenderer();
    private Button backButton;
    private FileIO fileIO = new FileIO();

    // Define playerScores and playerNames as class members
    private static List<string> playerNames = new List<string>();
    private static Dictionary<string, int> playerScores = new Dictionary<string, int>();

    public Scoreboard()
    {
        backButton = new Button("Back", new Vector2(10, 430), new Vector2(80, 40));
    }

    public static void SetPlayerName(string name)
    {
        if (!playerNames.Contains(name))
        {
            playerNames.Add(name);
            playerScores[name] = 0; // Initialize score for the new player
        }
    }

    // Method to update player score (call this method whenever a player's score changes)
    public static void UpdatePlayerScore(string name, int score)
    {
        if (playerNames.Contains(name))
        {
            playerScores[name] = score;
        }
    }

    // Method to get the top 5 players
    public static List<KeyValuePair<string, int>> GetTopPlayers()
    {
        var topPlayers = playerScores.ToList().OrderByDescending(pair => pair.Value).ToList();
        return topPlayers;
    }

    public void Draw(Font font)
    {
        // Draw background
        SDL.SDL_SetRenderDrawColor(Renderer, 255, 255, 255, 255); // Set background color to white
        SDL.SDL_RenderClear(Renderer);

        // Draw other elements (if any) for the credit screen or rules menu

        // Draw instructions text
        string[] scores = fileIO.readFromFile();

        // Set color for text
        SDL.SDL_Color textColor = new SDL.SDL_Color() { r = 0, g = 0, b = 0, a = 255 }; // Black color

        // Display instructions text on separate lines
        Vector2 basePosition = new Vector2(50, 50); // Adjust the starting position
        int lineHeight = 60; // Adjust the line height as needed

        var topPlayers = GetTopPlayers();

        for (int i = 1; i <= topPlayers.Count; i++)
        {
            if (i <= 10)
            {
                Vector2 linePosition = new Vector2(basePosition.X, basePosition.Y + i * lineHeight);
                string scoreText = $"{i}. {topPlayers[i - 1].Key}: {topPlayers[i - 1].Value} Enemies Killed";
                textRenderer.displayText(scoreText, linePosition, Color.Black, font);
            }
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
        Vector2 backTextPosition = new Vector2(backButton.getPosition().X, backButton.getPosition().Y + 30);
        textRenderer.displayText(backButton.Text, backTextPosition, Color.Black, font);

        SDL.SDL_RenderPresent(Renderer);
    }

    public bool IsBackButtonClicked()
    {
        return backButton.IsClicked();
    }
}
