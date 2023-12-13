using SDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;



internal class Checkpoint : Entity
{
    private IntPtr Renderer => Engine.Renderer2; // Gets the SDL Renderer from the Engine class

    private Vector2 position;
    private Vector2 size;
    private Texture checkpointTexture;
    public Checkpoint(Vector2 position, Vector2 size)
    {
        this.position = position;
        this.size = size;
        Game.entities.Add(this);
        string relativePath = "Assets\\checkpoint.png";
        string absolutePath = System.IO.Path.GetFullPath(relativePath);
        checkpointTexture = Engine.LoadTexture(absolutePath);
    }
    

    public Rectangle Bound
    {
        get
        {
            return CalculateBound();
        }
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
        // Set the checkpoint color to yellow
        Engine.DrawTexture(checkpointTexture, this.position, null, this.size);
        //;
        //SDL.SDL_RenderFillRect(Renderer, ref rect);
    }

    public void Update(Camera camera)
    {
        Render(camera);
    }


}

