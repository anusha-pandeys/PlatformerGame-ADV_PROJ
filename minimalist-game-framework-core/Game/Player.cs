using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Security.Cryptography;
using static SDL2.SDL;
using System.IO;

internal class Player : Entity
{
    private IntPtr Renderer => Engine.Renderer2;  // Gets the SDL Renderer from the Engine class
    private const int BLOCK_SIZE = 50;
    private float GRAVITY = 0.25f;// 0.5f; //lowr the gravity.
    private float NORMALF = -0.25f;
    private const float JUMP_STRENGTH = -6f;
    //public Vector2 position;
    public Vector2 playerVelocity = new Vector2 (0, 0);
    private TextRenderer text;
    private Font font;
    private Color originalColor = new Color(255, 0, 0, 255); // Original color (red)
    private Color playerColor;
    private float collisionCooldown = 0.1f; // Time in seconds before reverting to the original color
    private float timeSinceCollision = 0.0f;
    private bool blockBelow;
    private Collidable player;
    public ChargeBar chargeBar;
    private Texture playerTexture;
    private Texture walkingTexture1;
    private Texture walkingTexture2;
    private Texture originalTexture;
    private Texture originalTexture2;
    private Texture originalTexture3;
    private Texture jumping;
    private Boolean run = false;
    private Boolean direction = false;
    private float timeOrig = 0.0f;
    private float animationTime = 0.2f;
    private Boolean onGround = true;
    
    private Boolean jumped = false;
    public float floorY;
    public int level = 1;
    public Player(TextRenderer text, Font font)
    {

        size = new Vector2(30, 30);
        this.text = text;
        this.font = font;
        this.playerColor = originalColor;
        this.player = new Collidable(this, "player");
        chargeBar = new ChargeBar("playerChargeBar", new Vector2(220,50), 10, new Vector2(100, 50));
        Game.entities.Add(this);
        blockBelow = false;
        chargeBar.setCharge(50);
        size = new Vector2(64f, 64f);
        ///var path =
        
        string relativePath = "Assets\\persephoneStanding.png";
        string absolutePath = System.IO.Path.GetFullPath(relativePath);
        originalTexture = Engine.LoadTexture(absolutePath);

        relativePath = "Assets\\persephoneStanding2.png";
        absolutePath = System.IO.Path.GetFullPath(relativePath);
        originalTexture2 = Engine.LoadTexture(absolutePath);

        relativePath = "Assets\\persephoneStanding3.png";
        absolutePath = System.IO.Path.GetFullPath(relativePath);
        originalTexture3 = Engine.LoadTexture(absolutePath);

        relativePath = "Assets\\persephoneWalking1.png";
        absolutePath = System.IO.Path.GetFullPath(relativePath);
        walkingTexture1 = Engine.LoadTexture(absolutePath);

        relativePath = "Assets\\persephoneWalking2.png";
        absolutePath = System.IO.Path.GetFullPath(relativePath);
        walkingTexture2 = Engine.LoadTexture(absolutePath);

        relativePath = "Assets\\jumping.png";
        absolutePath = System.IO.Path.GetFullPath(relativePath);
        jumping = Engine.LoadTexture(absolutePath);

        playerTexture = originalTexture;
    }

    public void setCharge(int charge)
    {
        chargeBar.setCharge(charge);
    }
    internal Rectangle GetPlayerBounds()
    {
        return CalculateBound();
    }


    protected override Rectangle CalculateBound()
    {
        return new Rectangle((int)position.X, (int)position.Y, (int)(size.X), (int)(size.Y));
    }

    public List<Vector2> getCoordinates()
    {

        List<Vector2> result = new List<Vector2>();
        result.Add(position);
        result.Add(new Vector2(50, 70));
        return result;
    }

    /*public void translateUpLadder()
    {
        while(CollisionManager.checkCollisions("player", "ladder"))
        {
            playerVelocity.X = 0;
            playerVelocity.Y = 0.1f;
            position.Y -= playerVelocity.Y;
           
            Render(Game.localCamera);
        }
    }*/

    public void playerLoop()
    {
        long startTime = DateTime.Now.Ticks;
        HandleInput();
        HandleJump();
        double secondsElapsed = new TimeSpan(DateTime.Now.Ticks - startTime).TotalSeconds;
        HandleCollisionY(secondsElapsed);
        HandleCollisionX(secondsElapsed);
        CollisionObject obj = CollisionManager.checkCollisions("player", "slide", new Vector2(0, 30));
        if (obj.getCollided())
        {
            Console.WriteLine("hi");
            position.Y += obj.getDistanceY();
            playerVelocity.Y = 0;
            playerVelocity.X = 2f;
        }
        position += playerVelocity;
        // Collision detection for the floor
        
        
        if (position.Y > floorY - size.Y)
        {
            position.Y = floorY - size.Y;
            playerVelocity.Y = 0; // Stop downward movement
        }
        
        chargeBar.Render();
    }

    private void HandleCollisionY(double secondsElapsed)
    {
        
        CollisionObject collisionDetected = CollisionManager.checkBlockCollision(this, new Vector2(0, playerVelocity.Y+2f), secondsElapsed);
        if (collisionDetected.getCollided())
        {
            position.Y += collisionDetected.getDistanceY();
            if (collisionDetected.getBlock().slide)
            {
                playerVelocity.X = 10f;
            }
            else
            {
                playerVelocity.X += collisionDetected.getBlock().getVelcoity().X;
            }
            playerVelocity.Y = 0;
        } else if (!CollisionManager.checkBlockCollision(this, new Vector2(0, 2), secondsElapsed).getCollided())
        {
            playerVelocity.Y += (GRAVITY);
        }
        
    }
    private void HandleCollisionX(double secondsElapsed)
    {
        float horizontalMovement = playerVelocity.X;
        CollisionObject collisionDetected = CollisionManager.checkBlockCollision(this, new Vector2(2f, 0), secondsElapsed);
        if (collisionDetected.getCollided())
        {
            position.X += collisionDetected.getDistanceX();
        } else
        {
            collisionDetected = CollisionManager.checkBlockCollision(this, new Vector2(-2f, 0), secondsElapsed);
            if (collisionDetected.getCollided())
            {
                position.X += collisionDetected.getDistanceX();
            }
        }
    }


    private void HandleInput()
    {
        int numKeys;
        IntPtr keyboardStatePtr = SDL.SDL_GetKeyboardState(out numKeys);
        byte[] keys = new byte[numKeys];
        Marshal.Copy(keyboardStatePtr, keys, 0, numKeys);
        playerVelocity.X = 0.0f;


        // Check LEFT arrow key.
        if (keys[(int)SDL.SDL_Scancode.SDL_SCANCODE_A] == 1)
        {
            timeOrig += Engine.TimeDelta;
            if (timeOrig < animationTime)
            {
                playerTexture = walkingTexture1;
                direction = false;
            }
            else if ((timeOrig >= animationTime) && (timeOrig <= animationTime*2))
            {
                playerTexture = walkingTexture2;
                direction = false;
            } else
            {
                timeOrig = 0.0f;
            }
            text.displayText("left", new Vector2(10, 30), Color.Black, font);     
            playerVelocity.X = -2.0f;
            if (keys[(int)SDL.SDL_Scancode.SDL_SCANCODE_LSHIFT] == 1)
            {
                if(chargeBar.getCharge() > 10)
                {
                    playerVelocity.X -= 2.0f;
                    chargeBar.setCharge(chargeBar.getCharge() - 1);
                }
                
            }
        }
        //timeOrig = 0;
        // Check RIGHT arrow key.
        else if (keys[(int)SDL.SDL_Scancode.SDL_SCANCODE_D] == 1)
        {
            timeOrig += Engine.TimeDelta;
            if (timeOrig < animationTime)
            {
                playerTexture = walkingTexture1;
                direction = true;
            }
            else if ((timeOrig >= animationTime) && (timeOrig <= animationTime * 2))
            {
                playerTexture = walkingTexture2;
                direction = true;
            }
            else
            {
                timeOrig = 0.0f;
            }
            text.displayText("right", new Vector2(10, 30), Color.Black, font);
            playerVelocity.X = 2.0f;

            if (keys[(int)SDL.SDL_Scancode.SDL_SCANCODE_LSHIFT] == 1)
            {
                if (chargeBar.getCharge() > 10)
                {
                    playerVelocity.X += 2.0f;
                    chargeBar.setCharge(chargeBar.getCharge() - 1);
                }
            }
        } 
        else
        {
            timeOrig += Engine.TimeDelta;
            if (timeOrig < animationTime)
            {
                playerTexture = originalTexture;
            } else if (timeOrig >= animationTime && timeOrig < animationTime * 2)
            {
                playerTexture = originalTexture2;
            } else if (timeOrig >= animationTime*2 && (timeOrig < animationTime * 3))
            {
                playerTexture = originalTexture3;
            } else if (timeOrig >= animationTime*3)
            {
                playerTexture = originalTexture;
                timeOrig = 0.0f; 
            }
        }
    }

    private void HandleJump()
    {
        int numKeys;
        IntPtr keyboardStatePtr = SDL.SDL_GetKeyboardState(out numKeys);

        // Convert IntPtr to byte array
        byte[] keys = new byte[numKeys];
        Marshal.Copy(keyboardStatePtr, keys, 0, numKeys);

        if (keys[(int)SDL.SDL_Scancode.SDL_SCANCODE_W] == 1)
        {
            Jump();
            timeOrig += Engine.TimeDelta;
            if (timeOrig < 1)
            {
                playerTexture = jumping;
            }
            else
            {
                timeOrig = 0;
                playerTexture = originalTexture;
            }
            jumped = true;
        }
    }
    private void Jump()
    {
        
        if (playerVelocity.Y == 0)
        {
            playerVelocity.Y = JUMP_STRENGTH;
        }
        
    }


    public void renderOutside(Camera camera)
    {
        Render(camera);
    }
    public override void Render(Camera camera)
    {
        //Game.localCamera.updateGlobalCy(position,size,playerVelocity);
        Vector2 localPosition = Game.localCamera.globalToLocal(position);
        Draw(localPosition, size);
        //Draw(position, size);
    }

    protected override void Draw(Vector2 position, Vector2 size)
    {
        if (!direction)
        {
            Engine.DrawTexture(playerTexture, position, null, size, 0, null, TextureMirror.Horizontal, scaleMode: TextureScaleMode.Nearest);
        }
        else
        {
            Engine.DrawTexture(playerTexture, position, null, size, scaleMode: TextureScaleMode.Nearest);
        }

    }

}


internal enum GameColor
{
    Background, Player, Block1, White
}
