using System;
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

    public static string checkBlockCollision(Entity entity, Vector2 vel)
    {
        Rectangle rectangle = entity.Bound;
        Rectangle bound = new Rectangle((int)(rectangle.X + vel.X), (int)(rectangle.Y + vel.Y), rectangle.Width, rectangle.Height);
        for (int i = 0; i < blocks.Count; i++)
        {
            Dictionary<string, bool> ret = isCollided(bound, blocks[i].Bound);
            string returnStr = "";
            if (ret.ContainsValue(true) && ret.ContainsKey("up"))
            {
                returnStr += "up";
            }
            else if (ret.ContainsValue(true) && ret.ContainsKey("down"))
            {
                returnStr += "down";
            }
            if (ret.ContainsValue(true) && ret.ContainsKey("right"))
            {
                returnStr += "right";
            }
            else if (ret.ContainsValue(true) && ret.ContainsKey("left"))
            {

                returnStr += "left";
            }
            if (!returnStr.Equals("")) return returnStr;
            else continue;
        }
        return "na";
    }

    public static bool checkCollisions(string objA, string objB)
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
        return false;
    }
    public static bool isCollided(ICollidable entityA, ICollidable entityB)
    {

        Entity a = entityA.GameObject;
        Entity b = entityB.GameObject;

        Rectangle rectA = a.Bound;
        Rectangle rectB = b.Bound;

        Vector2 boundALeft = new Vector2(rectA.X, rectA.Y);
        Vector2 boundARight = new Vector2(rectA.X, rectA.Y) + new Vector2(rectA.Width, rectA.Width);
        Vector2 boundBLeft = new Vector2(rectB.X, rectB.Y);
        Vector2 boundBRight = new Vector2(rectB.X, rectB.Y) + new Vector2(rectB.Width, rectB.Width);

        if (boundARight.X < boundBLeft.X || boundBRight.X < boundALeft.X ||
                boundARight.Y < boundBLeft.Y || boundBRight.Y < boundALeft.Y)
            return false;
        else return true;
    }

    public static Dictionary<string, bool> isCollided(Rectangle rectA, Rectangle rectB)
    {
        Dictionary<string, bool> retMap = new Dictionary<string, bool>();
        bool upOrDown = false;
        bool rightLeft = false;
        Vector2 boundALeft = new Vector2(rectA.X, rectA.Y);
        Vector2 boundARight = new Vector2(rectA.X, rectA.Y) + new Vector2(rectA.Width, rectA.Width);
        Vector2 boundBLeft = new Vector2(rectB.X, rectB.Y);
        Vector2 boundBRight = new Vector2(rectB.X, rectB.Y) + new Vector2(rectB.Width, rectB.Width);
        Rectangle rectATransformed = rectA;
        Rectangle rectBTransformed = rectB;
        rectATransformed.X = rectATransformed.X + (rectATransformed.Width / 2);
        rectATransformed.Y = rectATransformed.Y + (rectATransformed.Height / 2);
        rectBTransformed.X = rectBTransformed.X + (rectBTransformed.Width / 2);
        rectBTransformed.Y = rectBTransformed.Y + (rectBTransformed.Height / 2);

        if (rectATransformed.Y < rectBTransformed.Y)
        {
            retMap["down"] = true;
            upOrDown = true;
        }
        if (rectATransformed.Y > rectBTransformed.Y)
        {
            retMap["up"] = true;
            upOrDown = false;
        }
        if (rectATransformed.X < rectBTransformed.X)
        {
            retMap["right"] = true;
            rightLeft = true;
        }
        if (rectATransformed.X > rectBTransformed.X)
        {
            retMap["left"] = true;
            rightLeft = false;
        }

        if (boundARight.X < boundBLeft.X || boundBRight.X < boundALeft.X ||
                boundARight.Y < boundBLeft.Y || boundBRight.Y < boundALeft.Y)//
        {
            if (upOrDown && retMap.ContainsKey("down"))
            {
                retMap["down"] = false;
            }
            else if (!upOrDown && retMap.ContainsKey("up"))
            {
                retMap["up"] = false;
            }
            if (rightLeft && retMap.ContainsKey("right"))
            {
                retMap["right"] = false;
            }
            else if (!rightLeft && retMap.ContainsKey("left"))
            {
                retMap["left"] = false;
            }
        }
        else
        {
            if (upOrDown && retMap.ContainsKey("down"))
            {
                retMap["down"] = true;
            }
            else if (!upOrDown && retMap.ContainsKey("up"))
            {
                retMap["up"] = true;
            }
            if (rightLeft && retMap.ContainsKey("right"))
            {
                //System.Console.WriteLine("left");
                retMap["right"] = true;
            }
            else if (!rightLeft && retMap.ContainsKey("left"))
            {

                retMap["left"] = true;
            }
        }
        return retMap;
    }
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