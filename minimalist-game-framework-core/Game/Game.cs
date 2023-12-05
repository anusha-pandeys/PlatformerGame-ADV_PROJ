using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;


class Game
{

    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(640, 480);
    public static List<Entity> entities = new List<Entity>();
    private TextRenderer textRenderer;
    Font font = Engine.LoadFont("Retro Gaming.ttf", 11);
    private StartMenu StartMenu;
    private RulesMenu rulesMenu;
    private WinScreen winScreen = new WinScreen();
    private LoseScreen loseScreen = new LoseScreen();
    private CreditScreen creditScreen;
    private bool showStartMenu = true;
    public  static Player player;
    private Map map;
    private NPC redNPC;
    private NPC greyNPC;
    //private Blocks floor;
    //private Blocks floor2;
    private List<Blocks> levelBlocks;
    private List<Blocks> levelBlocks2;
    public static Camera localCamera;
    private List<Checkpoint> checkpoints;
    private List<Pits> pits;
    private List<Ladder> ladders;
    private static int currLevel = 1;
    private Spear spear;
    //private Ladder ladder;

    public Game()
    {
        Vector2 playerPosition = new Vector2(100, 300); // Initial position
        Vector2 playerVelocity = new Vector2(0, 0);     // Initial velocity
        map = new Map();
        textRenderer = new TextRenderer();
        StartMenu = new StartMenu();
        rulesMenu = new RulesMenu();
        creditScreen = new CreditScreen();
        //slide = new Slides(new Vector2(300,100), new Vector2(100,100));
        //entities.Add(moving);
        player = new Player(playerPosition, playerVelocity, textRenderer, font);
        spear = new Spear();
        Vector2 redNPCPosition = new Vector2(400, 300); // Set the red NPC's initial position
        redNPC = new NPC(redNPCPosition, new Vector2(50, 50), player, Color.Red, 500f, 1.5f);

        Vector2 greyNPCPosition = new Vector2(500, 300); // Set the grey NPC's initial position
        greyNPC = new NPC(greyNPCPosition, new Vector2(50, 50), player, Color.Gray, 300f, 1.0f);

        levelBlocks = LevelLoader.LoadLevel("Game\\levelPractice.txt", 50); // Replace with the correct path
                                                                            // levelBlocks2 = LevelLoader.LoadLevel("Game\\levelPractice2.txt", 50); // Replace with the correct path
                                                                            //Font font = Engine.LoadFont("Retro Gaming.ttf", 11);        
                                                                            //startMenu = new StartMenu();

        pits = LevelLoader.loadPits("Game\\levelPractice.txt", 50);
        //ladders = LevelLoader.loadLadder("Game\\levelPractice.txt", 50);
        //loading checkpoints
        //checkpoints = LevelLoader.LoadCheckpoints("Game\\levelPractice.txt", 50); // Use the correct path and size
        //ladder = new Ladder(new Vector2(100, 200), new Vector2(50, 100));
        //CollisionManager.AddObj("pit", pit);
        loadEntities();
        CollisionManager.AddObj("player", player);
        

        //CollisionManager.AddObj("player", player);
        //CollisionManager.AddObj("slide", slide);
        localCamera = new Camera();
    }
    //

    public void Update()
    {
        // Poll for events
        SDL.SDL_PumpEvents();
        spear.spearLoop();
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
            
            map.setBackgroundColor();
            
            
            /*
            foreach (var block in levelBlocks)
            {
                block.blockLoop();
                //CollisionManager.addBlock(block);
            }
            */
            foreach (var pit in pits)
            {
                pit.pitsLoop();
                if (pit.getPlayerDeath())
                {
                    //implement death/game over
                    System.Console.WriteLine("dead");
                    loseScreen.show();
                }
                //CollisionManager.addBlock(block);
            }

            //foreach (var ladder in ladders)
            //{
                /*ladder.ladderLoop();
                if (ladder.getTranslate())
                {
                    player.translateUpLadder();
                }*/
                //CollisionManager.addBlock(block);
           // }
            

            player.playerLoop();
            localCamera.UpdateGlobalCy(player.position, player.size, player.playerVelocity);
            //localCamera.UpdateGlobalCy(player.position, player.size, player.playerVelocity);
            DisplayPlayerCoordinates();
            redNPC.Update();
            greyNPC.Update();
            
            //moving.updateCoordinates();

            // Render checkpoints
            
            /*
            foreach (var checkpoint in checkpoints)
            {
                checkpoint.Update(localCamera);
            }
            */

            foreach (Entity i in entities)
            {
                /*
                if (i.Position.Y + i.size.Y > Camera.globalCy - Camera.height/2 && i.Position.Y - i.size.Y < Camera.globalCy - Camera.height / 2)
                {
                    i.Render(localCamera);
                }
                */
                i.Render(localCamera);


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
                    currLevel++;
                    checkpoints.Clear();
                    CollisionManager.blocks.Clear();
                    CollisionManager.collidables.Clear();
                    winScreen.show(); 
                    string path = "Game\\level" + currLevel.ToString() + ".txt";
                    LoadNewLevel(path);
                    player.position = new Vector2(100, 300); // Reset position
                    
                    break;
                }
            }*/

        }
        SDL.SDL_RenderPresent(Engine.Renderer2);

        RenderGrid(Engine.Renderer2);
    }

    private void DisplayPlayerCoordinates()
    {
        string playerCoordinates = string.Format("{0}, {1}", player.getCoordinates()[0].X, player.getCoordinates()[0].Y);
        textRenderer.displayText(playerCoordinates, new Vector2(0, 0), Color.Black, font);

        //string redNPCCoordinates = string.Format("Red NPC: {0}, {1}", redNPC.Position.X, redNPC.Position.Y);
        //textRenderer.displayText(redNPCCoordinates, new Vector2(0, 20), Color.Black, font);

        //string greyNPCCoordinates = string.Format("Grey NPC: {0}, {1}", greyNPC.Position.X, greyNPC.Position.Y);
        //textRenderer.displayText(greyNPCCoordinates, new Vector2(0, 40), Color.Black, font);

        string floorCoordinates = string.Format("Floor: {0}, {1}", levelBlocks[0].Position.X, levelBlocks[0].Position.Y);
        textRenderer.displayText(floorCoordinates, new Vector2(0, 20), Color.Black, font);

    }

    private void LoadNewLevel(string levelPath)
    {
        // Clear existing checkpoints
        levelBlocks = LevelLoader.LoadLevel(levelPath, 50);
        pits = LevelLoader.loadPits(levelPath, 50);
        ladders = LevelLoader.loadLadder(levelPath, 50);
        checkpoints = LevelLoader.LoadCheckpoints(levelPath, 50);
        loadEntities();
    }

    public void loadEntities()
    {
        foreach (var block in levelBlocks)
        {
            //block.blockLoop();
            CollisionManager.addBlock(block);
        }
        
        foreach (var pit in pits)
        {
            //pit.pitsLoop();
            CollisionManager.AddObj("pit", pit);
        }
        //foreach (var ladder in ladders)
        //{
            //pit.pitsLoop();
        //    CollisionManager.AddObj("ladder", ladder);
       // }
    }
    public void RenderGrid(IntPtr renderer)
    {
        for (int row = 0; row < 32; row++)
        {
            for (int col = 0; col < 32; col++)
            {
                SDL.SDL_Rect tileRect = new SDL.SDL_Rect
                {
                    x = col * 21,
                    y = row * 21,
                    w = 21,
                    h = 21,
                };

                
                SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);  

                
                SDL.SDL_RenderDrawRect(renderer, ref tileRect);
            }
        }
    }

    public Player getPlayer()
    {
        return player;
    }
}

