using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


class Game
{
    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(640, 480);

    private IntPtr Renderer => Engine.Renderer2;  // Gets the SDL Renderer from the Engine class

    //private IntPtr Renderer;  // SDL Renderer
    private IntPtr Font;      // SDL Font (using SDL_ttf)

    private const int PLAYER_WIDTH = 50;
    private const int PLAYER_HEIGHT = 70;
    private const int BLOCK_SIZE = 50;
    private const float GRAVITY = 0.25f;// 0.5f; //lowr the gravity.
    private const float JUMP_STRENGTH = -15.0f;

    private Vector2 playerPosition;
    private Vector2 playerVelocity;

    //public Game()
    //{

    //}

    //public void Update() { }


    public Game()
    {
        playerPosition = new Vector2(100, 100); // Initial position
        playerVelocity = new Vector2(0, 0);     // Initial velocity
    }

    public void Update()
    {
        HandleInput();

        // Apply gravity
        playerVelocity.Y += GRAVITY;

        // Update player position
        playerPosition += playerVelocity;

        // Collision detection for the floor
        if (playerPosition.Y > 500) // Assuming 500 is ground level
        {
            playerPosition.Y = 500;
            playerVelocity.Y = 0; // Stop downward movement
        }

        // Collision detection for the walls (placeholder logic)
        if (playerPosition.X < 0 || playerPosition.X + PLAYER_WIDTH > 800) // Assuming screen width is 800
        {
            playerVelocity.X = 0; // Stop horizontal movement
        }

        Render();
    }


    //to test
    public void TestDrawText()
    {
        DrawText("Hello, SDL!", new Vector2(100, 100), Color.Player);
    }

    //to test
    public void TestDrawRectangle()
    {
        DrawRectangle(new Vector2(100, 100), new Vector2(100, 100), Color.Player);
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
        if (keys[(int)SDL.SDL_Scancode.SDL_SCANCODE_LEFT] == 1)
        {
            playerVelocity.X = -2.0f;// -5.0f;
        }
        // Check RIGHT arrow key.
        else if (keys[(int)SDL.SDL_Scancode.SDL_SCANCODE_RIGHT] == 1)
        {
            playerVelocity.X = 2.0f;// 5.0f;
        }

        // You can also add other key checks here, e.g., for jumping:
        if (keys[(int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE] == 1)
        {
            Jump();
        }
    }

    private void Jump()
    {
        // Only allow jumping if the player is on the ground.
        if (playerPosition.Y == 500)
        {
            playerVelocity.Y = JUMP_STRENGTH;
        }
    }

    private void Render()
    {
        int numKeys;
        IntPtr keyStatePtr = SDL.SDL_GetKeyboardState(out numKeys);

        // Draw background
        DrawRectangle(new Vector2(0, 0), new Vector2(800, 600), Color.Background);

        // Draw player
        DrawRectangle(playerPosition, new Vector2(PLAYER_WIDTH, PLAYER_HEIGHT), Color.
            Player);

        // Display player X, Y coordinates and inputs
        DrawText($"X: {playerPosition.X}, Y: {playerPosition.Y}", new Vector2(10, 10), Color.White);
        if (Marshal.ReadByte(keyStatePtr, (int)SDL.SDL_Scancode.SDL_SCANCODE_LEFT) == 1)
            DrawText("Left Arrow Pressed", new Vector2(10, 30), Color.White);
        if (Marshal.ReadByte(keyStatePtr, (int)SDL.SDL_Scancode.SDL_SCANCODE_RIGHT) == 1)
            DrawText("Right Arrow Pressed", new Vector2(10, 50), Color.White);
        if (Marshal.ReadByte(keyStatePtr, (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE) == 1)
            DrawText("Space Key Pressed", new Vector2(10, 70), Color.White);

        // Placeholder logic for drawing blocks (assuming they're spread out horizontally for simplicity)
        for (int i = 0; i < 800; i += BLOCK_SIZE * 2)
        {
            DrawRectangle(new Vector2(i, 500 - BLOCK_SIZE), new Vector2(BLOCK_SIZE, BLOCK_SIZE), Color.Block1);
        }
    }



    private void DrawRectangle(Vector2 position, Vector2 size, Color color)
    {
        // Set the drawing color based on the enum (placeholder RGB values)
        switch (color)
        {
            case Color.Background:
                SDL.SDL_SetRenderDrawColor(Renderer, 50, 50, 50, 255); // dark gray
                break;
            case Color.Player:
                SDL.SDL_SetRenderDrawColor(Renderer, 255, 0, 0, 255); // red
                break;
            case Color.Block1:
                SDL.SDL_SetRenderDrawColor(Renderer, 0, 255, 0, 255); // green
                break;
            case Color.White:
                SDL.SDL_SetRenderDrawColor(Renderer, 255, 255, 255, 255); // white
                break;
        }

        SDL.SDL_Rect rect = new SDL.SDL_Rect()
        {
            x = (int)position.X,
            y = (int)position.Y,
            w = (int)size.X,
            h = (int)size.Y
        };

        SDL.SDL_RenderFillRect(Renderer, ref rect);
    }

    private void DrawText(string text, Vector2 position, Color color)
    {
        if (Font == IntPtr.Zero)
        {
            Console.WriteLine("Font is not loaded. Error: {0}", SDL.SDL_GetError());
            return;
        }

        if (Renderer == IntPtr.Zero)
        {
            Console.WriteLine("Renderer is not initialized. Error: {0}", SDL.SDL_GetError());
            return;
        }

        SDL.SDL_Color sdlColor;

        switch (color)
        {
            case Color.Background:
                sdlColor = new SDL.SDL_Color { r = 50, g = 50, b = 50 };
                break;
            case Color.Player:
                sdlColor = new SDL.SDL_Color { r = 255, g = 0, b = 0 };
                break;
            case Color.Block1:
                sdlColor = new SDL.SDL_Color { r = 0, g = 255, b = 0 };
                break;
            default:
                sdlColor = new SDL.SDL_Color { r = 255, g = 255, b = 255 };
                break;
        }

        IntPtr surface = SDL_ttf.TTF_RenderText_Solid(Font, text, sdlColor);
        if (surface == IntPtr.Zero)
        {
            Console.WriteLine("Failed to render text. Error: {0}", SDL.SDL_GetError());
            return;
        }

        IntPtr texture = SDL.SDL_CreateTextureFromSurface(Renderer, surface);
        if (texture == IntPtr.Zero)
        {
            SDL.SDL_FreeSurface(surface); // Clean up the surface before exiting
            Console.WriteLine("Failed to create texture from surface. Error: {0}", SDL.SDL_GetError());
            return;
        }

        int textWidth, textHeight;
        SDL.SDL_QueryTexture(texture, out _, out _, out textWidth, out textHeight);
        SDL.SDL_Rect renderQuad = new SDL.SDL_Rect()
        {
            x = (int)position.X,
            y = (int)position.Y,
            w = textWidth,
            h = textHeight
        };

        SDL.SDL_RenderCopy(Renderer, texture, IntPtr.Zero, ref renderQuad);

        SDL.SDL_DestroyTexture(texture);
        SDL.SDL_FreeSurface(surface);
    }



    enum Color
    {
        Background, Player, Block1, White // Placeholder colors
    }
}