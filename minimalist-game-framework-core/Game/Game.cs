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
    private StartMenu StartMenu;
    private RulesMenu rulesMenu;
    private CreditScreen creditScreen;
    private bool showStartMenu = true;
    private Player player;
    private Map map;
    private NPC redNPC;
    private NPC greyNPC;
    //private Blocks floor;
    //private Blocks floor2;
    private List<Blocks> levelBlocks;
    private List<Blocks> levelBlocks2;
    public static Camera localCamera;
    private List<Checkpoint> checkpoints;
    private Pits pit;
    public Game()
    {
        Vector2 playerPosition = new Vector2(100, 300); // Initial position
        Vector2 playerVelocity = new Vector2(0, 0);     // Initial velocity
        map = new Map();
        textRenderer = new TextRenderer();
        StartMenu = new StartMenu();
        rulesMenu = new RulesMenu();
        creditScreen = new CreditScreen();
        //entities.Add(moving);
        player = new Player(playerPosition, playerVelocity, textRenderer, font);
        //floor = new Blocks(new Vector2(100, 250), new Vector2(50, 50), GameColor.Block1);
        //floor2 = new Blocks(new Vector2(200, 250), new Vector2(50, 50), GameColor.Block1);
        //CollisionManager.addBlock(floor);
        //CollisionManager.addBlock(floor2);

        Vector2 redNPCPosition = new Vector2(400, 300); // Set the red NPC's initial position
        redNPC = new NPC(redNPCPosition, new Vector2(50, 50), player, Color.Red);

        Vector2 greyNPCPosition = new Vector2(500, 300); // Set the grey NPC's initial position
        greyNPC = new NPC(greyNPCPosition, new Vector2(50, 50), player, Color.Gray);

        levelBlocks = LevelLoader.LoadLevel("Game\\levelPractice.txt", 50); // Replace with the correct path
                                                                            // levelBlocks2 = LevelLoader.LoadLevel("Game\\levelPractice2.txt", 50); // Replace with the correct path
                                                                            //Font font = Engine.LoadFont("Retro Gaming.ttf", 11);        
                                                                            //startMenu = new StartMenu();
        pit = new Pits(new Vector2(300, 200), new Vector2(50, 20));
        //loading checkpoints
        checkpoints = LevelLoader.LoadCheckpoints("Game\\levelPractice.txt", 50); // Use the correct path and size
        CollisionManager.AddObj("pit", pit);
        CollisionManager.AddObj("player", player);

        localCamera = new Camera();
    }

    public void Update()
    {
        // Poll for events
        SDL.SDL_PumpEvents();

        // Update game logic based on the current state
        if (showStartMenu)
        {
            StartMenu.Update();
            StartMenu.Draw(font);

            // If start button is clicked, hide the start menu and start the game
            if (StartMenu.IsStartButtonClicked())
            {
                showStartMenu = false;
            }
           
        }//
        else
        {
            // Update game logic here (same as before)
            map.setBackgroundColor();
            foreach (var block in levelBlocks)
            {
                block.blockLoop();
                CollisionManager.addBlock(block);
            }
            player.playerLoop();
            localCamera.UpdateGlobalCy(player.playerPosition, player.playerSize, player.playerVelocity);
            DisplayPlayerCoordinates();
            redNPC.Update();
            greyNPC.Update();
            pit.pitsLoop();
            if(pit.getPlayerDeath())
            {
                //implement death/game over
                System.Console.WriteLine("dead");
            }
            //moving.updateCoordinates();

            // Render checkpoints
            foreach (var checkpoint in checkpoints)
            {
                checkpoint.Update(localCamera);
            }


            // Check if back button is clicked in RulesMenu or CreditScreen
            if (rulesMenu.IsBackButtonClicked() || creditScreen.IsBackButtonClicked())
            {
                showStartMenu = true;
            }

            // Checkpoint collision detection
            /*foreach (var checkpoint in checkpoints)
            {
                if (CollisionManager.checkCheckpointCollision(player, checkpoint.Bound))
                {
                    LoadNewLevel("Game\\level2.txt");
                    player.playerPosition = new Vector2(100, 300); // Reset position
                    break;
                }
            }*/

        }

        // Present renderer
        SDL.SDL_RenderPresent(Engine.Renderer2);
    }

    private void DisplayPlayerCoordinates()
    {
        string playerCoordinates = string.Format("{0}, {1}", player.getCoordinates()[0].X, player.getCoordinates()[0].Y);
        textRenderer.displayText(playerCoordinates, new Vector2(0, 0), Color.Black, font);

        string redNPCCoordinates = string.Format("Red NPC: {0}, {1}", redNPC.Position.X, redNPC.Position.Y);
        textRenderer.displayText(redNPCCoordinates, new Vector2(0, 20), Color.Black, font);

        string greyNPCCoordinates = string.Format("Grey NPC: {0}, {1}", greyNPC.Position.X, greyNPC.Position.Y);
        textRenderer.displayText(greyNPCCoordinates, new Vector2(0, 40), Color.Black, font);
    }

    private void LoadNewLevel(string levelPath)
    {
        // Clear existing checkpoints
        checkpoints.Clear();

        levelBlocks = LevelLoader.LoadLevel(levelPath, 50);
    }
}
