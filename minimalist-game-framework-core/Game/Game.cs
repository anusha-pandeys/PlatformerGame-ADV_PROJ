using System.Collections.Generic;
using System;
using SDL2;
using System.Drawing;

class Game
{
    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(640, 480);
    private List<Entity> entities = new List<Entity>();
    private TextRenderer textRenderer;
    private Font font = Engine.LoadFont("Retro Gaming.ttf", 11);
    private StartMenu StartMenu;
    private RulesMenu rulesMenu;
    private CreditScreen creditScreen;
    private bool showStartMenu = true;
    private Player player;
    private Map map;
    private List<Blocks> levelBlocks;
    public static Camera localCamera;
    private List<Checkpoint> checkpoints = new List<Checkpoint>();
    private string currentLevelPath = "Game\\levelPractice.txt"; // Initialize first lev

    public Game()
    {
        Vector2 playerPosition = new Vector2(100, 300);
        Vector2 playerVelocity = new Vector2(0, 0);
        map = new Map();
        textRenderer = new TextRenderer();
        StartMenu = new StartMenu();
        rulesMenu = new RulesMenu();
        creditScreen = new CreditScreen();
        player = new Player(playerPosition, playerVelocity, textRenderer, font);

        // Load initial level
        LoadNewLevel(currentLevelPath);

        localCamera = new Camera();
    }

    public void Update()
    {
        SDL.SDL_PumpEvents();

        if (showStartMenu)
        {
            StartMenu.Update();
            StartMenu.Draw(font);
            if (StartMenu.IsStartButtonClicked())
            {
                showStartMenu = false;
            }
        }
        else
        {
            map.setBackgroundColor();
            foreach (var block in levelBlocks)
            {
                block.blockLoop();
                CollisionManager.addBlock(block);
            }

            player.playerLoop();
            localCamera.UpdateGlobalCy(player.playerPosition, player.playerSize, player.playerVelocity);
            DisplayPlayerCoordinates();

            foreach (var checkpoint in checkpoints)
            {
                checkpoint.Update(localCamera);
            }

            if (rulesMenu.IsBackButtonClicked() || creditScreen.IsBackButtonClicked())
            {
                showStartMenu = true;
            }

            foreach (var checkpoint in checkpoints)
            {
                if (CollisionManager.checkCheckpointCollision(player, checkpoint.Bound))
                {
                    LoadNewLevel(currentLevelPath);
                    player.playerPosition = new Vector2(100, 300);
                    break;
                }
            }
        }

        SDL.SDL_RenderPresent(Engine.Renderer2);
    }

    private void DisplayPlayerCoordinates()
    {
        string playerCoordinates = string.Format("{0}, {1}", player.getCoordinates()[0].X, player.getCoordinates()[0].Y);
        textRenderer.displayText(playerCoordinates, new Vector2(0, 0), Color.Black, font);
    }

    private void LoadNewLevel(string currentLevel)
    {
        // Determine the next level
        string nextLevelPath;
        if (currentLevel == "Game\\levelPractice.txt")
        {
            nextLevelPath = "Game\\level2.txt";
        }
        else if (currentLevel == "Game\\level2.txt")
        {
            nextLevelPath = "Game\\level3.txt";
        }
        else
        {
            nextLevelPath = "Game\\levelPractice.txt"; // back to beginging
        }

        // Update the current level path
        currentLevelPath = nextLevelPath;

        // Clear existing checkpoints and load new ones
        if (checkpoints != null)
        {
            checkpoints.Clear();
        }
        else
        {
            checkpoints = new List<Checkpoint>();
        }
        levelBlocks = LevelLoader.LoadLevel(nextLevelPath, 50);
        checkpoints = LevelLoader.LoadCheckpoints(nextLevelPath, 50);
    }
}
