using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System.Drawing;

internal class Flower : Entity
{
    private IntPtr Renderer => Engine.Renderer2;
    private Vector2 position;
    private Vector2 size;
    private bool isCollected = false;
    private Texture flowerTexture;
    private Texture flowerTexture2;
    private Texture finalTexture;
    private float origTime = 0;
    public Flower(Vector2 position)
    {
        this.position = position;
        this.size = new Vector2(20, 20);
        // Register the flower with the CollisionManager
        Game.entities.Add(this);
        CollisionManager.AddObj("flower", this);

        string relativePath = "Assets\\flower.png";
        string absolutePath = System.IO.Path.GetFullPath(relativePath);
        flowerTexture = Engine.LoadTexture(absolutePath);

        relativePath = "Assets\\flowerMove.png";
        absolutePath = System.IO.Path.GetFullPath(relativePath);
        flowerTexture2 = Engine.LoadTexture(absolutePath);

        finalTexture = flowerTexture;
    }

    public bool IsCollected => isCollected;

    public void FlowerLoop(Player player)
    {
        if (!isCollected)
        {
            origTime += Engine.TimeDelta;
            if (origTime < .5f)
            {
                finalTexture = flowerTexture;
            }
            else if (origTime >= .5f && origTime <= 1f)
            {
                finalTexture = flowerTexture2;
            }
            else
            {
                finalTexture = flowerTexture;
                origTime = 0;
            }
            CollisionObject obj = CollisionManager.checkCollisions("flower", "player", new Vector2(0, 0));
            if (obj.getCollided())
            {

                Collect();

                player.chargeBar.setCharge(Game.player.chargeBar.getCharge() + 50);


                Game.entities.Remove(this);
            }
        }
    }
    public void Collect()
    {
        isCollected = true;
    }

    public override void Render(Camera camera)
    {
        Vector2 localPosition = camera.globalToLocal(position);
        Draw(localPosition, size);
    }

    protected override Rectangle CalculateBound()
    {
        return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
    }

    protected override void Draw(Vector2 position, Vector2 size)
    {
        Engine.DrawTexture(finalTexture, position, null, size);
    }

    /*
    public void FlowerLoop(Player player)
    {
        if (!isCollected)
        {
            // Check for collisions with the player using the CollisionManager
            CollisionObject obj = CollisionManager.checkCollisions("flower", "player", new Vector2(0, 0));
            if (obj.getCollided())
            {
                // Collect the flower
                this.Collect();

                // Remove the flower from the game entities list
                Game.entities.Remove(this);

                // Increase the player's charge
                player.IncreaseCharge(10);
            }
        }
    }
    */
}

