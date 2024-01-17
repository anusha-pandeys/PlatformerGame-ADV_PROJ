using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;
//just make velocity negative so it can jump up but not down
internal class LevelSeperator : Entity
{
    private IntPtr Renderer => Engine.Renderer2;
    public Vector2 position;
    public Vector2 size;
    private Player player;
    private Collidable oneWay;
    private Texture levelSepTexture;
    private float collisionCooldown = 0.1f;
    private float timeSinceCollision = 0.0f;

    public LevelSeperator(Vector2 position, Vector2 size)
    {
        this.position = position;
        this.size = size;
        this.oneWay = new Collidable(this, "oneWay");
        Game.entities.Add(this);

        string relativePath = "Assets\\seperator.png";
        string absolutePath = System.IO.Path.GetFullPath(relativePath);
        levelSepTexture = Engine.LoadTexture(absolutePath);
    }

    public void Update()
    {
        //Render(new Camera());
        checkCollision();
    }

    public bool checkCollision()
    {
        CollisionObject collisionDetected = CollisionManager.checkCollisions("player", "oneWay", new Vector2(0, 10f));
        if (collisionDetected.getCollided())
        {
            Game.player.jumps = 0;
            return true;
        }

        return false;
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
        Engine.DrawTexture(levelSepTexture, position, null, size);
        
    }


}