using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

internal class CollisionManager
{
    private static List<ICollidable> collidables = new List<ICollidable>();
    public static List<Blocks> blocks { get; set; } = new List<Blocks>();
    private static TextRenderer textRenderer;
    private static Font font = Engine.LoadFont("Retro Gaming.ttf", 11);
    public static double angle;
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
        for (int i = 0; i < blocks.Count; i++)
        {
            CollisionObject ret = prospectiveSlideCollision(entity.Bound, blocks[i].Bound);
            CollisionObject collisions = isCollided(blocks[i].Bound, bound, ret);
            if ((collisions.getCollidedHorizontal() || collisions.getCollidedVertical()) && collisions.getDistance() < 2)
            {
                System.Console.WriteLine(collisions.getDistance());
                return collisions;
            }
            else continue;
        }
        return new CollisionObject();
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
        //CollisionObject collisionStatus = isCollided(rectA, rectB);
        return new CollisionObject();
    }

    public static CollisionObject prospectiveSlideCollision(Rectangle rectPreCollision, Rectangle colliding)
    {
        Vector2 centerPre = new Vector2(rectPreCollision.X + (rectPreCollision.Width / 2), rectPreCollision.Y + (rectPreCollision.Height / 2));
        Vector2 collidingRect = new Vector2(colliding.X + (colliding.Width / 2), colliding.Y + (colliding.Height / 2));
        Vector2 distance = collidingRect - centerPre;
        CollisionObject ret = new CollisionObject();
        ret.setDistance(Math.Sqrt(Math.Pow(distance.X, 2) + Math.Pow(distance.Y, 2)));
        // Avoid division by zero
        if (distance.X != 0)
        {
            angle = Math.Atan(distance.Y / distance.X) * (180 / Math.PI);
        }
        else
        {
            // Handle the special case where distance.X is zero
            angle = distance.Y > 0 ? 90 : (distance.Y < 0 ? -90 : 0);
        }
               

        if (angle >= -45 && angle < 45 && distance.X > 0)
        {
           //System.Console.WriteLine("right ");
            ret.setRight(true);
        }
        else if (angle >= 45 && angle < 135 && distance.Y < 0)
        {
            //System.Console.WriteLine("up ");
            ret.setUp(true);
        }
        else if (angle >= -135 && angle < -45 && distance.Y > 0)
        {
            //System.Console.WriteLine("down ");
            ret.setDown(true);
        }
        else if (angle >= -45 && angle < 45 && distance.X < 0)
        {
            //System.Console.WriteLine("left ");
            ret.setLeft(true);
        }

        return ret;
    }
    public static CollisionObject isCollided(Rectangle rectA, Rectangle rectB, CollisionObject sideCollided)
    {
        CollisionObject collisions = sideCollided;
        Vector2 boundALeftUp = new Vector2(rectA.X, rectA.Y);
        Vector2 boundARightUp = new Vector2(rectA.X, rectA.Y) + new Vector2(rectA.Width, 0);
        Vector2 boundALeftDown = new Vector2(rectA.X, rectA.Y) + new Vector2(0, rectA.Height);
        Vector2 boundARightDown = new Vector2(rectA.X, rectA.Y) + new Vector2(rectA.Width, rectA.Height);

        Vector2 boundBLeftUp = new Vector2(rectB.X, rectB.Y);
        Vector2 boundBRightUp = new Vector2(rectB.X, rectB.Y) + new Vector2(rectB.Width, 0);
        Vector2 boundBLeftDown = new Vector2(rectB.X, rectB.Y) + new Vector2(0, rectB.Height);
        Vector2 boundBRightDown = new Vector2(rectB.X, rectB.Y) + new Vector2(rectB.Width, rectB.Height);

        if ((boundARightUp.X > boundBLeftUp.X || boundALeftUp.X < boundBRightUp.X) )
        {
            System.Console.WriteLine("collided hor");
            collisions.setCollidedHorizontal(true);
        }

        if ((boundARightDown.Y > boundBRightUp.Y || boundARightUp.Y < boundBRightDown.Y))
        {
            System.Console.WriteLine("collided vert");
            collisions.setCollidedVertical(true);
        }

        return collisions;
    }
}
