using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;


internal class CollisionManager
{
    private static List<ICollidable> collidables = new List<ICollidable>();
    public static List<Blocks> blocks { get; set; } = new List<Blocks>();

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


    public static CollisionObject checkBlockCollision(Entity entity, Vector2 vel)
    {
        Rectangle rectangle = entity.Bound;
        Rectangle bound = new Rectangle((int)(rectangle.X + vel.X), (int)(rectangle.Y + vel.Y), rectangle.Width, rectangle.Height);
        for(int i = 0; i < blocks.Count; i++)
        {
            CollisionObject ret = isCollided(bound, blocks[i].Bound);
            if(ret.getCollided()) return ret;
            //else continue;
        }
        return null;
    }
        
    public static CollisionObject checkCollisions(string objA, string objB) 
    {
        for (int i = 0; i < collidables.Count-1; i++) { 
            var obj1 = collidables[i];
            for(int j = i+1; j < collidables.Count; j++)
            {
                var obj2 = collidables[j];
                if(obj1.Tag == obj2.Tag)
                {
                    continue;
                } else if (((objA == obj1.Tag) && (objB == obj2.Tag)) || ((objA == obj2.Tag) && (objB == obj1.Tag)))
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

        CollisionObject collisionStatus = isCollided(rectA, rectB);
        return collisionStatus;
    }

    public static CollisionObject isCollided(Rectangle rectA, Rectangle rectB)
    {
        CollisionObject collisions = new CollisionObject();
        bool upOrDown = false;
        bool rightLeft = false;
        Vector2 boundALeftUp = new Vector2(rectA.X, rectA.Y);
        Vector2 boundARightUp = new Vector2(rectA.X, rectA.Y) + new Vector2(rectA.Width, 0);
        Vector2 boundALeftDown = new Vector2(rectA.X, rectA.Y) + new Vector2(0, rectA.Height);
        Vector2 boundARightDown = new Vector2(rectA.X, rectA.Y) + new Vector2(rectA.Width, rectA.Height);

        Vector2 boundBLeftUp = new Vector2(rectA.X, rectA.Y);
        Vector2 boundBRightUp = new Vector2(rectA.X, rectA.Y) + new Vector2(rectB.Width, 0);
        Vector2 boundBLeftDown = new Vector2(rectA.X, rectA.Y) + new Vector2(0, rectB.Height);
        Vector2 boundBRightDown = new Vector2(rectA.X, rectA.Y) + new Vector2(rectB.Width, rectB.Height);
        //Vector2 boundBLeft = new Vector2(rectB.X, rectB.Y);
        //Vector2 boundBRight = new Vector2(rectB.X, rectB.Y) + new Vector2(rectB.Width, rectB.Height);

        if (rectA.Bottom < rectB.Top)
        {
            System.Console.WriteLine("down");
            collisions.setDown(true);
        }
        else if (rectA.Top > rectB.Bottom)
        {
            System.Console.WriteLine("up");
            collisions.setUp(true);
        }

        if (rectA.Right < rectB.Left)
        {
            System.Console.WriteLine("right");
            collisions.setRight(true);
        }
        else if (rectA.Left > rectB.Right)
        {
            System.Console.WriteLine("left");
            collisions.setLeft(true);
        }


        if (boundARightUp.X > boundBLeftUp.X || boundALeftUp.X < boundBRightUp.X ||
            boundARightDown.Y > boundBRightUp.Y || boundARightUp.Y < boundBRightDown.Y)
        {
            System.Console.WriteLine("collided");
            collisions.setCollided(true);
        }

        return collisions;
    }
}
