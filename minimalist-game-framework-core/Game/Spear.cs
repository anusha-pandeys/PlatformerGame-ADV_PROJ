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
    private Collidable spear;
    private int[,] stateTransitions = { { 1, 1, 0 }, { 0, 1, 1 }, { 1, 0, 1 } };
    private int currentState = 0; // 0 = notDrawn; 1 = drawing; 2 = pullign back
    private float dx = 0;
    private Sound spearMusic;
    public Spear()
    {

        this.position = Game.player.Position + new Vector2(0, Game.player.size.Y/2);
        this.size = new Vector2(50, 5);
        this.spear = new Collidable(this, "spear");
        Game.entities.Add(this);
        spearMusic = Engine.LoadSound("Spear 1.mp3");
    }

    public void spearLoop()
    {

        if (IsClicked())
        {
            
            if (currentState == 0 && canTransition(1))
            {
                Engine.PlaySound(spearMusic, false, 0);
                dx = 10f;
            }
        } 
        else if (this.position.X >= (Game.player.position.X + Game.player.size.X)
            && currentState == 1 && canTransition(2))
        {
            dx = -10f;
        } 
        else if (this.position.X < (Game.player.position.X + Game.player.size.X/2))
        {
            this.position.X = Game.player.position.X; 
            dx = 0f;
            currentState = 0;
        }
        //this.position.X = Game.player.Position.X;
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
    
    public override void Render(Camera camera)
    {
        Draw(Game.localCamera.globalToLocal(this.position), this.size);
    }

    protected override Rectangle CalculateBound()
    {
        return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }

    protected override void Draw(Vector2 position, Vector2 size)
    {
        SDL.SDL_SetRenderDrawColor(Renderer, 116, 86, 75, 100); // green
        SDL.SDL_Rect rect = new SDL.SDL_Rect()
        {
            x = (int)position.X,
            y = (int)position.Y,
            w = (int)size.X,
            h = (int)size.Y
        };
        SDL.SDL_RenderFillRect(Renderer, ref rect);
    }

    public bool IsClicked()
    {

        int mouseX, mouseY;
        SDL.SDL_GetMouseState(out mouseX, out mouseY);

        bool isClicked = (mouseX >= 0 && mouseX <= Game.Resolution.X &&
                          mouseY >= 0 && mouseY <= Game.Resolution.Y &&
                          SDL.SDL_GetMouseState(IntPtr.Zero, IntPtr.Zero) == SDL.SDL_BUTTON(SDL.SDL_BUTTON_LEFT));

        return isClicked;
    }
}
