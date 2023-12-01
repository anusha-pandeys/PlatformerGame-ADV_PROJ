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
            if(collision.getCollided()) 
            {
                collision.setBlock(blocks[i]);
                return collision;
            }
        }
        return new CollisionObject();
    }

    public static bool getClosestBlock(Player player)
    {
        float playerCenterX = player.Position.X + player.playerSize.X / 2;
        float playerCenterY = player.Position.Y + player.playerSize.Y / 2;
        int max = 0;
        double closestDist = double.MaxValue;
        for(int i = 0; i < blocks.Count; i++)
        {
            Rectangle curr = blocks[i].Bound;
            float currCenterX = curr.X + curr.Width / 2;
            float currCenterY = curr.Y + curr.Height / 2;
            double dist = Math.Sqrt(Math.Pow(playerCenterX - currCenterX, 2) + Math.Pow(playerCenterY - currCenterY, 2));
            if(dist < closestDist)
            {
                closestDist = dist;
                max = i;
            }
        }
        Rectangle closest = blocks[max].Bound;
        if(Math.Abs((closest.Y - playerCenterY) - ((player.playerSize.Y/2) + (closest.Width/2))) < 10)
        {
            return true;
        }
        return false;
    }

    public static CollisionObject checkCollisions(string objA, string objB) 
    {
        for (int i = 0; i < collidables.Count - 1; i++)
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
                    return isCollided(obj1, obj2);
                }
            }
        }
        return null;
    }
    public static CollisionObject isCollided(ICollidable entityA, ICollidable entityB)
    {

        Entity a = entityA.GameObject;
        Entity b = entityB.GameObject;

        Rectangle rectA = a.Bound;
        Rectangle rectB = b.Bound;
        return isCollided(rectA, rectB, new CollisionObject());
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
}