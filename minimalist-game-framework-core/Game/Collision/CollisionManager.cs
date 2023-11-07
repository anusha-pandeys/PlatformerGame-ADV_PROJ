using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Game.Game.Collision
{
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

        public static bool checkBlockCollision(Entity entity, Vector2 vel)
        {
            Rectangle rectangle = entity.Bound;
            Rectangle bound = new Rectangle((int)(rectangle.X + vel.X), (int)(rectangle.Y + vel.Y), rectangle.Width, rectangle.Height);
            for(int i = 0; i < blocks.Count; i++)
            {
                if (isCollided(bound, blocks[i].Bound))
                {
                    return true;
                }
            }
            return false;
        }
        
        public static void checkCollisions()
        {
            for (int i = 0; i < collidables.Count-1; i++) { 
                var obj1 = collidables[i];
                for(int j = i+1; j < collidables.Count; j++)
                {
                    var obj2 = collidables[j];
                    if(obj1.Tag == obj2.Tag)
                    {
                        continue;
                    } else
                    {
                        isCollided(obj1, obj2);
                    }
                }
            }
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

        public static bool isCollided(Rectangle rectA, Rectangle rectB)
        {
            Vector2 boundALeft = new Vector2(rectA.X, rectA.Y);
            Vector2 boundARight = new Vector2(rectA.X, rectA.Y) + new Vector2(rectA.Width, rectA.Width);
            Vector2 boundBLeft = new Vector2(rectB.X, rectB.Y);
            Vector2 boundBRight = new Vector2(rectB.X, rectB.Y) + new Vector2(rectB.Width, rectB.Width);

            if (boundARight.X < boundBLeft.X || boundBRight.X < boundALeft.X ||
                    boundARight.Y < boundBLeft.Y || boundBRight.Y < boundALeft.Y)
                return false;
            else return true;
        }
    }
}
