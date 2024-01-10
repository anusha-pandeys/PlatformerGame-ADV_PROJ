using SDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

internal class Blocks : Entity
{
    private IntPtr Renderer => Engine.Renderer2; // Gets the SDL Renderer from the Engine class
    public Vector2 position;
    public static Vector2 size = new Vector2 (50,50);
    private GameColor color;
    private Collidable blocks;
    private string sidesInContact;
    private Vector2 velocity; // New property to store the velocity
    public Boolean slide;
    public void SetVelocity(Vector2 velocity)
    {
        CollisionObject collision = CollisionManager.checkBlockCollision(this, velocity, 1);
        if (collision.getCollided())
        {
            this.velocity *= -1;
        }
    }

    public Vector2 getVelcoity()
    {
        return velocity;
    }

    public void blockLoop()
    {
        // Update the block's position based on the velocity
        SetVelocity(velocity);
        position += velocity;

        // Render(Game.localCamera);
    }

    private Texture texture;
    public Blocks(Vector2 position, GameColor color, Vector2 velocity)
    {
        this.position = position;
        this.color = color;
        sidesInContact = "";
        this.velocity = velocity;
        Game.entities.Add(this);
        string relativePath = "Assets\\block4.png";
        string absolutePath = System.IO.Path.GetFullPath(relativePath);
        texture = Engine.LoadTexture(absolutePath);
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
        Engine.DrawTexture(texture, position, null, size);
    }
}