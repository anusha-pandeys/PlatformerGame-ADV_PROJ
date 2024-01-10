using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Text;

internal class CollisionManager
{
    public static List<ICollidable> collidables { get; set; } = new List<ICollidable>();
    public static List<Blocks> blocks { get; set; } = new List<Blocks>();
    public static CollisionObject collision = new CollisionObject();
    public static ICollidable AddObj(string tag, Entity entity)
    {
        ICollidable collidable = new Collidable(entity, tag);
        collidables.Add(collidable);
        return collidable;
    }

    public static void addBlock(Blocks block)
    {
        blocks.Add(block);
    }


    public static CollisionObject checkBlockCollision(Entity entity, Vector2 vel, double time)
    { 
        Rectangle rectangle = entity.Bound;
        Rectangle bound = new Rectangle((int)(rectangle.X + (vel.X*time)), (int)(rectangle.Y + (vel.Y*time)), rectangle.Width, rectangle.Height);
        for (int i = 0; i < blocks.Count; i++)
        {
            collision = isCollided(bound, blocks[i].Bound , new CollisionObject());
            if(collision.getCollided() && entity != blocks[i]) 
            {
                collision.setBlock(blocks[i]);
                return collision;
            }
        }
        return new CollisionObject();
    }

    public static CollisionObject checkCollisions(string objA, string objB, Vector2 velocity) 
    {
        for (int i = 0; i < collidables.Count; i++)
        {
            var obj1 = collidables[i];
            for (int j = i + 1; j < collidables.Count; j++)
            {
                var obj2 = collidables[j];
                if (obj1.Tag == obj2.Tag)
                {
                    continue;
                }
                else if (((objA == obj1.Tag) && (objB == obj2.Tag)) || ((objA == obj2.Tag) && (objB == obj1.Tag)))
                {
                    CollisionObject obj = isCollided(obj1, obj2, velocity);
                    //obj.setBlock(blocks[j]);
                    if(obj.getCollided())
                    {
                        return obj;
                    }
                    
                }
            }
        }
        return new CollisionObject();
    }
    public static CollisionObject isCollided(ICollidable entityA, ICollidable entityB, Vector2 vel)
    {

        Entity a = entityA.GameObject;
        Entity b = entityB.GameObject;

        Rectangle rectA = a.Bound;
        Rectangle rectB = b.Bound;

        Rectangle bound = new Rectangle((int)(rectA.X + vel.X), (int)(rectA.Y + vel.Y), rectA.Width, rectA.Height);
        return isCollided(bound, rectB, new CollisionObject());
    }
      
    public static CollisionObject isCollided(Rectangle rectA, Rectangle rectB, CollisionObject sideCollided)
    {
        CollisionObject collisions = sideCollided;
        
        if (rectA.IntersectsWith(rectB))
        {
            float overlapX = Math.Min(rectA.X + rectA.Width, rectB.X + rectB.Width) - Math.Max(rectA.X, rectB.X);
            float overlapY = Math.Min(rectA.Y + rectA.Height, rectB.Y + rectB.Height) - Math.Max(rectA.Y, rectB.Y);

            // Determine the axis of least penetration
            if (overlapX < overlapY)
            {
                // Resolve along the X axis
                if (rectA.X < rectB.X)
                    collisions.setDistanceX(-1 * overlapX);
                else
                    collisions.setDistanceX(overlapX);
            }
            else
            {
                // Resolve along the Y axis
                if (rectA.Y < rectB.Y)
                    collisions.setDistanceY(-1 * overlapY);
                else
                    collisions.setDistanceY(overlapY);
            }
            collisions.setCollided(true);
        }
        return collisions;
    }

    //handle collisions between the player and checkpoints:
    public static bool checkCheckpointCollision(Entity entity, Rectangle checkpointBound)
    {
        Rectangle entityBound = entity.Bound;
        // Check if entityBound intersects with checkpointBound
        if (entityBound.IntersectsWith(checkpointBound))
        {
            return true;
        }
        return false;
    }

    public static bool checkOneWayCollision(Entity entity, Entity oneWay)
    {
        Rectangle entityBound = entity.Bound;
        Rectangle oneWayBound = oneWay.Bound;
        // Check if entityBound intersects with checkpointBound
        if (entityBound.Bottom <= oneWayBound.Top && entityBound.IntersectsWith(oneWay.Bound))
        {
            return true;
        }
        return false;
    }
}