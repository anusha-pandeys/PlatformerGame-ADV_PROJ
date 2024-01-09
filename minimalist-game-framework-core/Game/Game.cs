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
    public static Camera localCamera;//
    private List<Checkpoint> checkpoints;
    private List<Pits> pits;
    private List<LevelSeperator> levelSeperators;
    private List<Ladder> ladders;
    private static int currLevel = 1;
    private Enemy enemy;
    private Boss boss;
    private Spear spear;
    private Fire fire;
    //private Ladder ladder;
    private Music bgMusic;
    private List<Flower> flowers = new List<Flower>();
    List<Flower> flowersToRemove = new List<Flower>();
    List<Slides> slides = new List<Slides>();
    public static int enemiesKilled = 0;
    public Boolean playerDeath = false;
    public Game()
    {
        map = new Map();
        textRenderer = new TextRenderer();
        StartMenu = new StartMenu();
        rulesMenu = new RulesMenu();
        creditScreen = new CreditScreen();
        //slide = new Slides(new Vector2(300,100), new Vector2(100,100));
        //entities.Add(moving);
        player = new Player(textRenderer, font);
        spear = new Spear();
        Vector2 redNPCPosition = new Vector2(400, 300); // Set the red NPC's initial position
        redNPC = new NPC(redNPCPosition, new Vector2(50, 50), player, Color.Red, 500f, 0.5f, "Assets\\redGhost.png", "npc1");

        Vector2 greyNPCPosition = new Vector2(500, 300); // Set the grey NPC's initial position
        greyNPC = new NPC(greyNPCPosition, new Vector2(50, 50), player, Color.Gray, 500f, 1.0f, "Assets\\greyGhost.png", "npc2");

        fire = new Fire(new Vector2(100, 100), new Vector2(50, 50));                                                



        Vector2 enemySpawnPosition = new Vector2(400, 100); // Set the desired spawn position
        Enemy enemy = new Enemy(enemySpawnPosition, new Vector2(50, 50)); // Adjust size as needed

        Vector2 bossPosition = new Vector2(500, 300); // Set the boss's initial position
        Boss boss = new Boss(bossPosition, new Vector2(50, 50), player, 150f, 1.0f);

        levelBlocks = LevelLoader.LoadLevel("Game\\levelPractice.txt", 50); // Replace with the correct path\
                                                                            // levelBlocks2 = LevelLoader.LoadLevel("Game\\levelPractice2.txt", 50); // Replace with the correct path
                                                                            //Font font = Engine.LoadFont("Retro Gaming.ttf", 11);        
                                                                            //startMenu = new StartMenu();

        bgMusic = Engine.LoadMusic("bg music.mp3");

        pits = LevelLoader.loadPits("Game\\levelPractice.txt", new Vector2(50, 20));
        levelSeperators = LevelLoader.loadLevelSeperator("Game\\levelPractice.txt", 50);
        slides = LevelLoader.LoadSlides("Game\\levelPractice.txt", new Vector2(50, 50));
        //ladders = LevelLoader.loadLadder("Game\\levelPractice.txt", 50);
        //loading checkpoints
        //checkpoints = LevelLoader.LoadCheckpoints("Game\\levelPractice.txt", 50); // Use the correct path and size
        //ladder = new Ladder(new Vector2(100, 200), new Vector2(50, 100));
        //CollisionManager.AddObj("pit", pit);
        loadEntities();
        CollisionManager.AddObj("player", player);
        CollisionManager.AddObj("boss", boss);
        CollisionManager.AddObj("npc1", redNPC);
        CollisionManager.AddObj("npc2", greyNPC);
        CollisionManager.AddObj("spear", spear);
        CollisionManager.AddObj("fire", fire);
        //CollisionManager.AddObj("player", player);
        //CollisionManager.AddObj("slide", slide);
        localCamera = new Camera();
        flowers = LevelLoader.LoadFlowers("Game\\levelPractice.txt", 50);
    }
    //

    public void Update()
    {
        // Poll for events
        //Engine.PlayMusic(bgMusic, true, 0);



        SDL.SDL_PumpEvents();
        // Measure the time elapsed between one frame and the next

        // Update game logic based on the current state
        if (showStartMenu)
        {
            StartMenu.Update();
            StartMenu.Draw(font);

            // If start button is clicked, hide the start menu and start the game
            if (StartMenu.IsStartButtonClicked())
            {
                showStartMenu = false;
                // Start the background music when the game starts and loop it
                //Engine.PlayMusic(bgMusic, true, 0);
            }
        }
        else
        {
            if (!playerDeath)
            {
                //map.setBackgroundColor();
                Texture background = Engine.LoadTexture(System.IO.Path.GetFullPath("Assets\\background.png"));
                Engine.DrawTexture(background, new Vector2(0, 0), null, new Vector2(640, 480));

            foreach (var block in levelBlocks)
            {
                block.blockLoop();
                //CollisionManager.addBlock(block);
            }
            foreach (var levelSep in levelSeperators)
            {
                levelSep.Update();
            }
            foreach (var pit in pits)
            {
                pit.pitsLoop();
                if (pit.checkCollision())
                {
                    //implement death/game over

                        Game.player.chargeBar.setCharge(0);



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

            
                foreach (var flower in flowers)
                {
                    flower.FlowerLoop(player);
                
                }



                player.playerLoop();
                localCamera.parallaxLayer1(localCamera.updateParallaxLayer1(player.position));
                localCamera.updateGlobalCy(player.position, player.size, player.playerVelocity);
                DisplayPlayerCoordinates();
                redNPC.Update();
                greyNPC.Update();
                fire.FireLoop();
                spear.spearLoop();

                foreach (var entity in Game.entities.ToArray())
                {
                    if (entity is Enemy enemyEntity)  // Rename the variable to 'enemyEntity' or any other suitable name
                    {
                        enemyEntity.EnemyLoop();
                    }
                    if (entity is Boss bossEntity)  // Rename the variable to 'enemyEntity' or any other suitable name
                    {
                        bossEntity.Update();
                    }
                }
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

            foreach (var sep in levelSeperators)
            {
                if (player.position.Y < sep.position.Y - player.size.Y && player.floorY > sep.position.Y)
                {
                    player.floorY = sep.position.Y;
                    break;
                }
            }

                if (Game.player.chargeBar.getCharge() <= 0)

                {
                    FileIO file = new FileIO();
                    file.writeToFile();
                    playerDeath = true;
                    loseScreen.show();

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
            
                }
                */
            }


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
        pits = LevelLoader.loadPits(levelPath, new Vector2(50, 20));
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
        
        foreach (var flower in flowers)
        {
            CollisionManager.AddObj("flower", flower);
        }

        foreach (var slide in slides)
        {
            CollisionManager.AddObj("slide", slide);
        }
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

    public void increaseEnemiesKilled()
    {
        enemiesKilled++;
    }
}

