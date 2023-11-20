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
    private const float PLAYER_WIDTH = 50f;
    private const float PLAYER_HEIGHT = 70f;
    private const int BLOCK_SIZE = 50;
    private float GRAVITY = 0.25f;// 0.5f; //lowr the gravity.
    private float NORMALF = -0.25f;
    private const float JUMP_STRENGTH = -9.0f;
    public Vector2 playerPosition;
    public Vector2 playerVelocity;
    public Vector2 playerSize = new Vector2(PLAYER_WIDTH, PLAYER_HEIGHT);
    private TextRenderer text;
    private Font font;
    private Collidable player;

    public Player(Vector2 playerPosition, Vector2 playerVelocity, TextRenderer text, Font font)
    {
        this.playerPosition = playerPosition;
        this.playerVelocity = playerVelocity;
        this.text = text;
        this.font = font;
        this.player = new Collidable(this, "player");
    }


    protected override Rectangle CalculateBound()
    {
        return new Rectangle((int)playerPosition.X, (int)playerPosition.Y, (int)(PLAYER_HEIGHT), (int)(PLAYER_WIDTH));
    }

    public List<Vector2> getCoordinates()
    {

        List<Vector2> result = new List<Vector2>();
        result.Add(playerPosition);
        result.Add(new Vector2(50, 70));
        return result;
    }

    public void playerLoop()
    {
        CollisionObject collisionDetected = CollisionManager.checkBlockCollision(this, playerVelocity);
        
        HandleInput();
        
        // Apply gravity

        HandleJump();
        if (collisionDetected.getCollidedHorizontal()) 
        {
            playerVelocity.Y = 0;
            playerVelocity.Y -= (GRAVITY);
            //playerPosition.Y -= 1f;
           // System.Console.WriteLine("down");
        } else if (collisionDetected.getCollidedHorizontal())
        {
           // System.Console.WriteLine("up");
            playerVelocity.Y = playerVelocity.Y * -1;
        }
        
        //collisionDetected = "";
        playerVelocity.Y += (GRAVITY);
        

        // Update player position
        playerPosition += playerVelocity;

        

        // Collision detection for the floor
        if (playerPosition.Y > 400) // Assuming 500 is ground level
        {
            playerPosition.Y = 400;
            playerVelocity.Y = 0; // Stop downward movement
        }

        // Collision detection for the walls (placeholder logic)
        if (playerPosition.X < 0 || playerPosition.X + PLAYER_WIDTH > 800) // Assuming screen width is 800
        {
            playerVelocity.X = 0; // Stop horizontal movement
        }

        Render(Game.localCamera);
    }

    private void HandleInput()
    {
        int numKeys;
        IntPtr keyboardStatePtr = SDL.SDL_GetKeyboardState(out numKeys);

        // Convert IntPtr to byte array
        byte[] keys = new byte[numKeys];
        Marshal.Copy(keyboardStatePtr, keys, 0, numKeys);

        // Reset the horizontal velocity to 0.
        playerVelocity.X = 0.0f;


        // Check LEFT arrow key.
        if (keys[(int)SDL.SDL_Scancode.SDL_SCANCODE_A] == 1)
        {
            text.displayText("left", new Vector2(10, 30), Color.Black, font);     
            Vector2 prospectiveVelocity = new Vector2(-2.0f, 0);
            CollisionObject collisionDetected = CollisionManager.checkBlockCollision(this, prospectiveVelocity);
            if (collisionDetected.getCollidedHorizontal()) 
            {
                //System.Console.WriteLine("left");
                playerVelocity.X = 0;
            }
            else
            {
                playerVelocity.X = -2.0f;
            }
        }    
        // Check RIGHT arrow key.
        // Check RIGHT arrow key.
        else if (keys[(int)SDL.SDL_Scancode.SDL_SCANCODE_D] == 1)
        {
            text.displayText("right", new Vector2(10, 30), Color.Black, font);
            Vector2 prospectiveVelocity = new Vector2(2.0f, 0);
            CollisionObject collisionDetected = CollisionManager.checkBlockCollision(this, prospectiveVelocity);
            if (collisionDetected.getCollidedHorizontal())
            {
                System.Console.WriteLine("right");
                playerVelocity.X = 0;
            }
            else
            {
                playerVelocity.X = 2.0f;
            }
        }

        // You can also add other key checks here, e.g., for jumping:
    }

    private bool keyPressed()
    {
        int numKeys;
        IntPtr keyboardStatePtr = SDL.SDL_GetKeyboardState(out numKeys);

        // Convert IntPtr to byte array
        byte[] keys = new byte[numKeys];
        Marshal.Copy(keyboardStatePtr, keys, 0, numKeys);
        if (keys[(int)SDL.SDL_Scancode.SDL_SCANCODE_W] == 1)
        {

            return true;
        }
        return false;
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
            //GRAVITY = 0.25f;
            //NORMALF = 0;
            playerVelocity.Y = JUMP_STRENGTH;
        }
    }



    protected override void Render(Camera camera)
    {
        int numKeys;
        IntPtr keyStatePtr = SDL.SDL_GetKeyboardState(out numKeys);
        Vector2 localPosition = camera.globalToLocal(playerPosition);
        Draw(localPosition, playerSize);
    }

    protected override void Draw(Vector2 position, Vector2 size)
    {
        SDL.SDL_SetRenderDrawColor(Renderer, 255, 0, 0, 255); // red

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
