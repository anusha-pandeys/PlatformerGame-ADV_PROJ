using SDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static SDL2.SDL;

internal class Spear : Entity
{
    private IntPtr Renderer => Engine.Renderer2;
    private Vector2 position;
    private Vector2 size;
    private Collidable spear;//
    private int[,] stateTransitions = { { 1, 1, 0 }, { 0, 1, 1 }, { 1, 0, 1 } };
    public static int currentState = 0; // 0 = notDrawn; 1 = drawing; 2 = pullign back
    private float dx = 0;
    private Sound spearMusic;
    private float shootCooldown = 1.25f; // Set the desired cooldown time in seconds
    private float timeSinceLastShot = 0.0f;
    private bool ableToDamage = true;
    private Texture spearTexture;
    public int degree = 0;
    private int direction = 1;
    public Spear()
    {
        string relativePath = "Assets\\spear.png";
        string absolutePath = System.IO.Path.GetFullPath(relativePath);
        spearTexture = Engine.LoadTexture(absolutePath);

        this.position = Game.player.Position + new Vector2(0, Game.player.size.Y/2);
        this.size = new Vector2(80, 10);
        ableToDamage = true;
        this.spear = new Collidable(this, "spear");
        Game.entities.Add(this);
        spearMusic = Engine.LoadSound("Spear 1.mp3");
    }
    
    public void spearLoop()
    {
        timeSinceLastShot += Engine.TimeDelta;
        if(!Game.player.direction)
        {
            direction = -1;
        } else if (Game.player.direction)
        {
            direction = 1;
        }
        if(IsClickedRight() && timeSinceLastShot < shootCooldown)
        {
            degree -= 10*direction;   
        } 
        if(timeSinceLastShot > shootCooldown)
        {
            degree = 0;
        }
        if (IsClickedRight() && (timeSinceLastShot >= shootCooldown))
        {
            degree = 0;
            timeSinceLastShot = 0;
            for (int i = 0; i < Game.entities.Count; i++)
            {
                if (Game.entities[i] is NPC)
                {
                    NPC npc = (NPC)Game.entities[i];

                    double distance = Math.Sqrt(Math.Pow((Game.player.position.X - npc.position.X), 2) + Math.Pow((Game.player.position.Y - npc.position.Y), 2));
                    if (distance < 100)//
                    {
                        npc.healthBar.setHealth(npc.healthBar.getHealth() - 75);
                        break;
                    }
                }
            }
        }
        if (IsClickedLeft() && (timeSinceLastShot >= shootCooldown))
        {
            timeSinceLastShot = 0;

            if (currentState == 0 && canTransition(1))
            {
                Engine.PlaySound(spearMusic, false, 0);
                dx = 10f*direction;
            }
            this.position.X += dx;
            this.position.Y = Game.player.Position.Y + Game.player.size.Y / 2;
        }
        if (((this.position.X >= (Game.player.position.X + 100f))
            && currentState == 1 && canTransition(2)))
        {
            dx = -10f;
            
        }
        if (((this.position.X <= (Game.player.position.X - 100f))
            && currentState == 1 && canTransition(2)))
        {
            dx = 10f;

        } 

        else if ((this.position.X <= (Game.player.position.X)) && Game.player.direction)
        {
            this.position.X = Game.player.position.X;
            dx = 0f;
            currentState = 0;
            ableToDamage = true;
        } else if ((this.position.X >= (Game.player.position.X)) && !Game.player.direction)
        {
            this.position.X = Game.player.position.X;
            dx = 0f;
            currentState = 0;
            ableToDamage = true;
        }
        //this.position.X = Game.player.Position.X;
        if (currentState == 0 && this.position.X != Game.player.Position.X)
        {
            this.position.X = Game.player.position.X;
        }
        if (currentState == 0)
        {
            ableToDamage = true;
        }

        if(currentState == 1 && ableToDamage)
        {
            for (int i = 0; i < Game.entities.Count; i++)
            {
                if (Game.entities[i] is NPC)
                {
                    NPC npc = (NPC)Game.entities[i];
                    if (CollisionManager.checkCollisions("spear", npc.tag, new Vector2(0, 0)).getCollided())
                    {
                        if (npc.tag.Contains("greynpc"))
                        {
                            npc.healthBar.setHealth(npc.healthBar.getHealth() - 30);
                            ableToDamage = false;
                        }
                    }
                }
            }
        } 
        this.position.X += dx;
        this.position.Y = Game.player.Position.Y + Game.player.size.Y / 2;
    }

    private bool canTransition(int transitionTo)
    {
        if (stateTransitions[currentState, transitionTo] == 1)
        {
            currentState = transitionTo;
            return true;
        }
        return false;
    }
    
    protected override Rectangle CalculateBound()
    {
        return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }

    public override void Render(Camera camera)
    {
        Vector2 localPosition = camera.globalToLocal(position);
        Draw(localPosition, size);
    }

    protected override void Draw(Vector2 position, Vector2 size)
    {
        if(!Game.player.direction)
        {
            Engine.DrawTexture(spearTexture, position, null, size, scaleMode: TextureScaleMode.Nearest, rotation: degree, 
                mirror: TextureMirror.Horizontal);
        } else
        {
            Engine.DrawTexture(spearTexture, position, null, size, scaleMode: TextureScaleMode.Nearest, rotation: degree);
        }
        
    }

    public bool IsClickedLeft()
    {

        int mouseX, mouseY;
        SDL.SDL_GetMouseState(out mouseX, out mouseY);

        bool isClicked = (mouseX >= 0 && mouseX <= Game.Resolution.X &&
                          mouseY >= 0 && mouseY <= Game.Resolution.Y &&
                          SDL.SDL_GetMouseState(IntPtr.Zero, IntPtr.Zero) == SDL.SDL_BUTTON(SDL.SDL_BUTTON_LEFT));

        return isClicked;
    }

    public bool IsClickedRight()
    {

        int mouseX, mouseY;
        SDL.SDL_GetMouseState(out mouseX, out mouseY);

        bool isClicked = (mouseX >= 0 && mouseX <= Game.Resolution.X &&
                          mouseY >= 0 && mouseY <= Game.Resolution.Y &&
                          SDL.SDL_GetMouseState(IntPtr.Zero, IntPtr.Zero) == SDL.SDL_BUTTON(SDL.SDL_BUTTON_RIGHT));

        return isClicked;
    }
}
