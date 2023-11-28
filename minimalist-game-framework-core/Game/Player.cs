using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Security.Cryptography;

internal class Player : Entity
{
    private IntPtr Renderer => Engine.Renderer2;  // Gets the SDL Renderer from the Engine class
    private const int BLOCK_SIZE = 50;
    private float GRAVITY = 0.25f;// 0.5f; //lowr the gravity.
    private float NORMALF = -0.25f;
    private const float JUMP_STRENGTH = -3f;
    public Vector2 playerPosition;
    public Vector2 playerVelocity;
    public Vector2 globalPos;
    public Vector2 playerSize;
    private TextRenderer text;
    private Font font;
    private Color originalColor = new Color(255, 0, 0, 255); // Original color (red)
    private Color playerColor;
    private float collisionCooldown = 0.1f; // Time in seconds before reverting to the original color
    private float timeSinceCollision = 0.0f;
    private bool blockBelow;
    private Collidable player;
    public Player(Vector2 playerPosition, Vector2 playerVelocity, TextRenderer text, Font font)
    {
        this.playerPosition = playerPosition;
        this.playerVelocity = playerVelocity;
        this.text = text;
        this.font = font;
        this.playerColor = originalColor;
        this.player = new Collidable(this, "player");
        Game.entities.Add(this);
        blockBelow = false;
        playerSize = new Vector2(50f, 70f);
    }


    internal Rectangle GetPlayerBounds()
    {
        return CalculateBound();
    }


    protected override Rectangle CalculateBound()
    {
        return new Rectangle((int)playerPosition.X, (int)playerPosition.Y, (int)(playerSize.X), (int)(playerSize.Y));
    }




    public Vector2 Position
    {
        get { return playerPosition; }
    }

    

    public List<Vector2> getCoordinates()
    {

        List<Vector2> result = new List<Vector2>();
        result.Add(playerPosition);
        result.Add(new Vector2(50, 70));
        return result;
    }

    /*public void translateUpLadder()
    {
        while(CollisionManager.checkCollisions("player", "ladder"))
        {
            playerVelocity.X = 0;
            playerVelocity.Y = 0.1f;
            playerPosition.Y -= playerVelocity.Y;
           // Render(Game.localCamera);
        }
    }*/

    public void playerLoop()
    {
        long startTime = DateTime.Now.Ticks;
        HandleInput();
        HandleJump();
        double secondsElapsed = new TimeSpan(DateTime.Now.Ticks - startTime).TotalSeconds;
        HandleCollisionX(secondsElapsed);
        HandleCollisionY(secondsElapsed);
        playerPosition += playerVelocity;
        // Collision detection for the floor
        if (playerPosition.Y > 400) // Assuming 500 is ground level
        {
            playerPosition.Y = 400;
            playerVelocity.Y = 0; // Stop downward movement
        }

        Render(Game.localCamera);
    }

    private void HandleCollisionY(double secondsElapsed)
    {
        if (!CollisionManager.checkBlockCollision(this, new Vector2(0, 2), 1).getCollided())
        {
            playerVelocity.Y += (GRAVITY);
        }
        CollisionObject collisionDetected = CollisionManager.checkBlockCollision(this, new Vector2(0, playerVelocity.Y+1f), secondsElapsed);
        if (collisionDetected.getCollided())
        {
            playerVelocity.Y = 0;
            if (collisionDetected.getDistanceY() > 0 && collisionDetected.getDistanceX() < CollisionManager.blocks[0].size.X/2)
            {
                playerPosition -= new Vector2(collisionDetected.getDistanceX(), collisionDetected.getDistanceY());
                //playerPosition.Y -= collisionDetected.getDistanceY();
            }
            
        }
    }
    private void HandleCollisionX(double secondsElapsed)
    {
        float horizontalMovement = playerVelocity.X;
        CollisionObject collisionDetected = CollisionManager.checkBlockCollision(this, new Vector2(playerVelocity.X, 0), secondsElapsed);
        if (collisionDetected.getCollided())
        {
            // Handle collision when moving in the original direction
            //playerVelocity.X = 0;
            playerPosition -= new Vector2(collisionDetected.getDistanceX(), collisionDetected.getDistanceY());
        } else
        {
            collisionDetected = CollisionManager.checkBlockCollision(this, new Vector2(playerVelocity.X, 0), secondsElapsed);
            if (collisionDetected.getCollided())
            {
                playerPosition += new Vector2(collisionDetected.getDistanceX(), collisionDetected.getDistanceY());
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
            text.displayText("left", new Vector2(10, 30), Color.Black, font);     
            playerVelocity.X = -2.0f;
        }    
        // Check RIGHT arrow key.
        else if (keys[(int)SDL.SDL_Scancode.SDL_SCANCODE_D] == 1)
        {
            text.displayText("right", new Vector2(10, 30), Color.Black, font);
            playerVelocity.X = 2.0f;
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
        //Vector2 localPosition = camera.globalToLocal(playerPosition);
        //Draw(localPosition, playerSize);
        Draw(playerPosition, playerSize);
    }

    protected override void Draw(Vector2 position, Vector2 size)
    {

        SDL.SDL_SetRenderDrawColor(Renderer, playerColor.R, playerColor.G, playerColor.B, playerColor.A);

        SDL.SDL_Rect rect = new SDL.SDL_Rect()
        {
            x = (int)position.X,
            y = (int)position.Y,
            w = (int)size.X,
            h = (int)size.Y
        };

        SDL.SDL_RenderFillRect(Renderer, ref rect);
    }
}


internal enum GameColor
{
    Background, Player, Block1, White
}
