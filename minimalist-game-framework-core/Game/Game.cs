using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;


class Game
{

    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(640, 480);
    private List<Entity> entities = new List<Entity>();
    private TextRenderer textRenderer;
    Font font = Engine.LoadFont("Retro Gaming.ttf", 11);
    StartMenu startMenu;
    private StartMenu StartMenu;
    private RulesMenu rulesMenu;
    private CreditScreen creditScreen;
    private bool showStartMenu = true;

    Player x;
    Map map;
    Blocks floor;
    MovingBlock moving;
    public Game()
    {
        Font font = Engine.LoadFont("Retro Gaming.ttf", 11);
        Vector2 playerPosition = new Vector2(100, 100); // Initial position
        Vector2 playerVelocity = new Vector2(0, 0);     // Initial velocity
        map = new Map();
        x = new Player(playerPosition, playerVelocity, entities);
        floor = new Blocks(new Vector2(300, 400), new Vector2(50, 50), GameColor.Block1);
        moving = new MovingBlock(new Vector2(100, 100), new Vector2(50, 50), GameColor.Block1);
        entities.Add(x);
        entities.Add(floor);
        textRenderer = new TextRenderer();
        startMenu = new StartMenu();
        rulesMenu = new RulesMenu();
        creditScreen = new CreditScreen();
        //entities.Add(moving);
    }

    public void Update()
    {
        // Poll for events
        SDL.SDL_PumpEvents();

        // Update game logic based on the current state
        if (showStartMenu)
        {
            startMenu.Update();
            startMenu.Draw(font);

            // If start button is clicked, hide the start menu and start the game
            if (startMenu.IsStartButtonClicked())
            {
                showStartMenu = false;
            }
        }
        else
        {
            // Update game logic here (same as before)
            map.setBackgroundColor();
            floor.Render();
            x.playerLoop();
            DisplayPlayerCoordinates();
            moving.updateCoordinates();

            // Check if back button is clicked in RulesMenu or CreditScreen
            if (rulesMenu.IsBackButtonClicked() || creditScreen.IsBackButtonClicked())
            {
                showStartMenu = true;
            }
        }

        // Present renderer
        SDL.SDL_RenderPresent(Engine.Renderer2);
    }




    public void DisplayPlayerCoordinates()
    {
        string playerCoordinates = string.Format("{0}, {1}", x.getCoordinates()[0].X, x.getCoordinates()[0].Y);
        textRenderer.displayText(playerCoordinates, new Vector2(0, 0), Color.Black, font);
    }
}