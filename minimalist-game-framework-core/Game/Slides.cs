using SDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

internal class Slides : Entity
{
    private IntPtr Renderer => Engine.Renderer2; // Gets the SDL Renderer from the Engine class
    public Vector2 position;
    public static Vector2 size = Blocks.size;
    private GameColor color;
    private Collidable blocks;
    private string sidesInContact;



    private Texture playerTexture;
    public Slides(Vector2 position, GameColor color, Vector2 velocity)
    {
        this.position = position;
        this.color = color;
        sidesInContact = "";
        Game.entities.Add(this);
        string relativePath = "Assets\\blocks.png";
        string absolutePath = System.IO.Path.GetFullPath(relativePath);
        playerTexture = Engine.LoadTexture(absolutePath);
    }
    public void slideLoop()
    {
        // Update the block's position based on the velocity


        // Render(Game.localCamera);
    }
    public List<Vector2> getCoordinates()
    {
        List<Vector2> result = new List<Vector2>();
        result.Add(position);
        result.Add(size);
        return result;
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
        Engine.DrawTexture(playerTexture, position, null, size);
    }
}