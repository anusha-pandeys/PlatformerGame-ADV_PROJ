using SDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

internal class Blocks : Entity
{
    private IntPtr Renderer => Engine.Renderer2; // Gets the SDL Renderer from the Engine class
    private Vector2 position;
    public Vector2 size;
    private GameColor color;
    private Collidable blocks;
    private string sidesInContact;
    private Vector2 velocity; // New property to store the velocity

    public void SetVelocity(Vector2 velocity)
    {
        this.velocity = velocity;
    }

    public void blockLoop()
    {
        // Update the block's position based on the velocity
        position += velocity * Engine.TimeDelta;

        // Render(Game.localCamera);
    }

    private Texture playerTexture;
    public Blocks(Vector2 position, Vector2 size, GameColor color)
    {
        this.position = position;
        this.size = size;
        this.color = color;
        sidesInContact = "";
        Game.entities.Add(this);
        string relativePath = "Assets\\blocks.png";
        string absolutePath = System.IO.Path.GetFullPath(relativePath);
        playerTexture = Engine.LoadTexture(absolutePath);
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