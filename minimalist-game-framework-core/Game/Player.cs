using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;
using Game.Game.Collision;

namespace Game.Game
{
    internal class Player : Entity
    {
        private IntPtr Renderer => Engine.Renderer2;  // Gets the SDL Renderer from the Engine class

        //private IntPtr Renderer;  // SDL Renderer
        //private IntPtr Font;      // SDL Font (using SDL_ttf)

        private const float PLAYER_WIDTH = 50f;
        private const float PLAYER_HEIGHT = 70f;
        private const int BLOCK_SIZE = 50;
        private float GRAVITY = 0.25f;// 0.5f; //lowr the gravity.
        private float NORMALF = -0.25f;
        private const float JUMP_STRENGTH = -9.0f;
        private Vector2 playerPosition;
        private Vector2 playerVelocity;
        //private Game.Game.Collision.CollisionManager collisions;
        //to test
        
        public Player(Vector2 playerPosition, Vector2 playerVelocity)
        {
            this.playerPosition = playerPosition;
            this.playerVelocity = playerVelocity;
        }

        protected override Rectangle CalculateBound()
        {
            return new Rectangle((int)playerPosition.X, (int)playerPosition.Y, (int)(PLAYER_WIDTH + 0.1f), (int)(PLAYER_HEIGHT + 0.1f));
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

            HandleInput();

            // Apply gravity
            /*if (detectCollision(entities, new Vector2(playerVelocity.X, playerVelocity.Y + 1f)))
            {
                System.Console.WriteLine("hi");
                NORMALF = -0.25f;
                playerVelocity.Y = 0;
                //playerPosition.Y -= 0.5f;
            }
            else
            {
                playerVelocity.Y += (GRAVITY - NORMALF);
            }*/

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

            Render();
        }

        public void getHandleInput()
        {
            HandleInput();
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
                Vector2 prospectiveVelocity = new Vector2(-2.0f, 0);
                if (CollisionManager.) 
                {
                    System.Console.WriteLine("Hi");
                    playerVelocity.X = 0;
                    //playerPosition.X += 0.1f;
                }
                else
                {
                    playerVelocity.X = -2.0f;// -5.0f;
                }
            }
            // Check RIGHT arrow key.
            else if (keys[(int)SDL.SDL_Scancode.SDL_SCANCODE_D] == 1)
            {
                Vector2 prospectiveVelocity = new Vector2(2.0f, 0);
                if (detectCollision(entities, prospectiveVelocity))
                {
                    System.Console.WriteLine("hi");
                    playerVelocity.X = 0;
                    //playerPosition.X -= 0.1f;
                }
                else
                {
                    playerVelocity.X = 2.0f;// -5.0f;
                }
            }

            // You can also add other key checks here, e.g., for jumping:
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
                NORMALF = 0;
                playerVelocity.Y = JUMP_STRENGTH;
            }
        }

        public void Render()
        {
            int numKeys;
            IntPtr keyStatePtr = SDL.SDL_GetKeyboardState(out numKeys);
            DrawRectangle(playerPosition, new Vector2(PLAYER_WIDTH, PLAYER_HEIGHT), GameColor.Player);
        }



        public void DrawRectangle(Vector2 position, Vector2 size, GameColor color)
        {
            // Set the drawing color based on the enum (placeholder RGB values)
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

        public Boolean detectCollision(List<Entity> entities, Vector2 prospectiveVelocity)
        {
            float thisLeft = (int)playerPosition.X + prospectiveVelocity.X;
            float thisRight = (int)playerPosition.X + prospectiveVelocity.X + PLAYER_WIDTH;
            float thisTop = (int)playerPosition.Y + prospectiveVelocity.Y;
            float thisBottom = (int)playerPosition.Y + prospectiveVelocity.Y + PLAYER_HEIGHT;
            for (int i = 1; i < entities.Count; i++)
            {
                float otherLeft = (int)entities[i].getCoordinates()[0].X;
                float otherRight = (int)entities[i].getCoordinates()[0].X + entities[i].getCoordinates()[1].X;
                float otherTop = (int)entities[i].getCoordinates()[0].Y;
                float otherBottom = (int)entities[i].getCoordinates()[0].Y + entities[i].getCoordinates()[1].Y;

                if (thisRight < otherLeft || thisLeft > otherRight || thisBottom < otherTop || thisTop > otherBottom)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

internal enum GameColor
{
    Background, Player, Block1, White
}
