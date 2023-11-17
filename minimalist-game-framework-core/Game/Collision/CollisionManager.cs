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

    public static void addBlock(Blocks block)//
    {
        blocks.Add(block);
    }

    private static string unpackDictionary(Dictionary<string, bool> ret)
    {
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
        return returnStr;
    }
    public static string checkBlockCollision(Entity entity, Vector2 vel)
    {
        Rectangle rectangle = entity.Bound;
        Rectangle bound = new Rectangle((int)(rectangle.X + vel.X), (int)(rectangle.Y + vel.Y), rectangle.Width, rectangle.Height);
        for(int i = 0; i < blocks.Count; i++)
        {
            Dictionary<string, bool> ret = isCollided(bound, blocks[i].Bound);
            string returnStr = unpackDictionary(ret);
            if (!returnStr.Equals("")) return returnStr;//
            else continue;
        }
        return "na";
    }
        
    public static Dictionary<string, bool> checkCollisions(string objA, string objB) 
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
    public static Dictionary<string, bool> isCollided(ICollidable entityA, ICollidable entityB)
    {

        Entity a = entityA.GameObject;
        Entity b = entityB.GameObject;

        Rectangle rectA = a.Bound;
        Rectangle rectB = b.Bound;

        /*Vector2 boundALeft = new Vector2(rectA.X, rectA.Y);
        Vector2 boundARight = new Vector2(rectA.X, rectA.Y) + new Vector2(rectA.Width, rectA.Width);
        Vector2 boundBLeft = new Vector2(rectB.X, rectB.Y);
        Vector2 boundBRight = new Vector2(rectB.X, rectB.Y) + new Vector2(rectB.Width, rectB.Width);

        if (boundARight.X < boundBLeft.X || boundBRight.X < boundALeft.X ||
                boundARight.Y < boundBLeft.Y || boundBRight.Y < boundALeft.Y)
            return false;
        else return true;*/
        Dictionary<string, bool> retMap = isCollided(rectA, rectB);
        return retMap;
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

        if (rectA.Y < rectB.Y)
        {
            retMap["down"] = true;
            upOrDown = true;
        } 
        if (rectA.Y > rectB.Y)
        {
            retMap["up"] = true;
            upOrDown = false;
        }
        if(rectA.X < rectB.X)
        {
            retMap["right"] = true;
            rightLeft = true;
        } 
        if (rectA.X > rectB.X)
        {
            retMap["left"] = true;
            rightLeft = false;
        }

        if (boundARight.X < boundBLeft.X || boundBRight.X < boundALeft.X ||
                boundARight.Y < boundBLeft.Y || boundBRight.Y < boundALeft.Y)//
        {
            if(upOrDown && retMap.ContainsKey("down"))
            {
                retMap["down"] = false;
            } else if (!upOrDown && retMap.ContainsKey("up"))
            {
                retMap["up"] = false;
            }
            if(rightLeft && retMap.ContainsKey("right")) {
                retMap["right"] = false;
            } else if (!rightLeft && retMap.ContainsKey("left"))
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
