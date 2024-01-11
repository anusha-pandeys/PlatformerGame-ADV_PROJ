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
using System.IO;



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
    private List<NPC> redNPCs;
    private List<NPC> greyNPCs;
    private List<Fire> fires;
    //private Blocks floor;
    //private Blocks floor2;
    private List<Blocks> levelBlocks;
    public static Camera localCamera;
    //private List<Checkpoint> checkpoints;
    //private List<Ladder> ladders;
    private List<Pits> pits;
    private List<LevelSeperator> levelSeperators;
    private static int currLevel = 1;
    //private Enemy enemy;
    private Boss boss;
    private Spear spear;
    //private Ladder ladder;
    private Music bgMusic;
    private List<Flower> flowers;
    List<Flower> flowersToRemove = new List<Flower>();
    List<Slides> slides;
    public static int enemiesKilled = 0;
    public Boolean playerDeath = false;
    Texture background;
    private int numGreyNPC;

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

        loadLevel("Game\\levelPractice.txt");
        
        //Font font = Engine.LoadFont("Retro Gaming.ttf", 11);        
        //startMenu = new StartMenu();

        bgMusic = Engine.LoadMusic("bgm.mp3");
        
        //CollisionManager.AddObj("pit", pit);

        CollisionManager.AddObj("player", player);
        CollisionManager.AddObj("boss", boss);
        int i = 0;
        foreach (var r in redNPCs)
        {
            CollisionManager.AddObj("npc1", r);
        }
        i = 0;
        foreach (var g in greyNPCs)
        {
            i++;
            string tags = "greynpc" + i.ToString();
            CollisionManager.AddObj(tags, g);
        }
        foreach (var f in fires)
        {
            CollisionManager.AddObj("fire", f);
        }

        CollisionManager.AddObj("spear", spear);
        //CollisionManager.AddObj("slide", slide);
        localCamera = new Camera();
        background = Engine.LoadTexture(System.IO.Path.GetFullPath("Assets\\background.png"));
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
                Engine.DrawTexture(background, new Vector2(0, 0), null, new Vector2(640, 480));

                



                player.playerLoop();
                localCamera.parallaxLayer1(localCamera.updateParallaxLayer1(player.position));
                localCamera.updateGlobalCy(player.position, player.size, player.playerVelocity);
                DisplayPlayerCoordinates();
                spear.spearLoop();
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


                foreach (var flower in flowers)
                {
                    flower.FlowerLoop(player);

                }
                foreach (var entity in Game.entities.ToArray())
                {
                    /*
                    if (entity is Enemy enemyEntity)  // Rename the variable to 'enemyEntity' or any other suitable name
                    {
                        enemyEntity.EnemyLoop();
                    }
                    */
                    if (entity is Boss bossEntity)  // Rename the variable to 'enemyEntity' or any other suitable name
                    {
                        bossEntity.Update();
                    }
                }
                //moving.updateCoordinates();

                /* 
                //Render checkpoints - DELETE

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
                        player.level++;
                        break;
                    }
                }

                foreach (var r in redNPCs)
                {
                    r.Update();
                } 
                foreach (var g in greyNPCs)
                {
                    g.Update();
                }
                foreach (var f in fires)
                {
                    f.FireLoop();
                }
            
                if(Game.player.chargeBar.getCharge() > 100)
                {
                    Game.player.setCharge(100);
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

    private void loadLevel(string filePath)
    {

        /*
        KEY
        # = block
        m = moving block
        s = slide
        * = flower
        f = fire
        r = red npc
        g = gray npc
        p = pit

        S = levelSeperator
        b = boss
        Q = player
         */

        levelBlocks = new List<Blocks>();
        //checkpoints = new List<Checkpoint>();
        pits = new List<Pits>();
        slides = new List<Slides>();
        levelSeperators = new List<LevelSeperator>();
        redNPCs = new List<NPC>();
        greyNPCs = new List<NPC>();
        fires = new List<Fire>();
        flowers = new List<Flower>();
        string[] lines = File.ReadAllLines(filePath);
        int level = 1;
        double highestFloor = 0;

        for (int y = lines.Length-1; y >= 0; y--)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {

                float newY = lines.Length - y;
                if (lines[y][x] == '#') // block
                {
                    Vector2 position = new Vector2(x * Blocks.size.X, Resolution.Y - (newY * Blocks.size.Y));
                    levelBlocks.Add(new Blocks(position, GameColor.Block1, new Vector2(0, 0)));
                    if (y == lines.Length - 1) // floor
                    {
                        Game.player.floorY = Resolution.Y - (newY * Blocks.size.Y);
                        highestFloor = Resolution.Y - (newY * Blocks.size.Y);
                    }
                }
                else if (lines[y][x] == 'p') //pits
                {
                    Vector2 position = new Vector2(x * Pits.size.X, Resolution.Y - (newY * Pits.size.Y));
                    pits.Add(new Pits(position));
                }
                else if (lines[y][x] == 'm') // moving block
                {
                    Vector2 position = new Vector2(x * Blocks.size.X, Resolution.Y - (newY * Blocks.size.Y));
                    levelBlocks.Add(new Blocks(position, GameColor.Block1, new Vector2(2f, 0)));
                }
                else if (lines[y][x] == 's') //slide
                {
                    Vector2 position = new Vector2(x * Blocks.size.X, Resolution.Y - (newY * Blocks.size.Y));
                    Blocks slideBlock = new Blocks(position, GameColor.White, new Vector2(0, 0));
                    slideBlock.slide = true;
                    slideBlock.setSlideTexture();
                    levelBlocks.Add(slideBlock);
                    //slides.Add(new Slides(position, GameColor.White, new Vector2(0, 0)));
                }
                else if (lines[y][x] == 'Q') //player
                {
                    Game.player.position = new Vector2(x * Game.player.size.X, Resolution.Y - (newY * Game.player.size.Y));

                }
                else if (lines[y][x] == '*') // flower
                {
                    Vector2 position = new Vector2(x * Blocks.size.X, Resolution.Y - (newY * Blocks.size.Y));
                    flowers.Add(new Flower(position));
                }
                else if (lines[y][x] == 'S')
                {
                    Vector2 position = new Vector2(x * Blocks.size.X, Resolution.Y - (newY * Blocks.size.Y));
                    levelSeperators.Add(new LevelSeperator(position, Blocks.size));
                    if (x+1 >= lines[y].Length || lines[y][x+1] !='S')
                    {
                        level++;
                    }
                    highestFloor = Resolution.Y - (newY * Blocks.size.Y);
                }
                else if (lines[y][x] == 'f')
                {
                    Vector2 position = new Vector2(x * Blocks.size.X, Resolution.Y - (newY * Blocks.size.Y));
                    fires.Add(new Fire(position));
                }
                else if (lines[y][x] == 'g')
                {
                    Vector2 position = new Vector2(x * Blocks.size.X, Resolution.Y - (newY * Blocks.size.Y));
                    numGreyNPC++;
                    string tag = "greynpc" + numGreyNPC.ToString();
                    greyNPCs.Add(new NPC(level, position, player, Color.Gray, 200f, 1.0f, "Assets\\greyGhost.png", tag));
                }
                else if (lines[y][x] == 'r')
                {
                    Vector2 position = new Vector2(x * Blocks.size.X, Resolution.Y - (newY * Blocks.size.Y));
                    redNPCs.Add(new NPC(level, position, player, Color.Red, 200f, 0.5f, "Assets\\redGhost.png", "npc1"));
                } else if (lines[y][x] == 'b')
                {
                    Vector2 position = new Vector2 (x * Blocks.size.X, Resolution.Y - (newY * Blocks.size.Y));
                    boss = new Boss(level, highestFloor, position, player, 150f, 1.0f);
                }
                else continue;
            }
        }
        loadEntities();
        //DIFF SIXES OF SLIDE LADDER??

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

