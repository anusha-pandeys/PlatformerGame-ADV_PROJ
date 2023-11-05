using Game.entity;
using Game.Game.Collision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Game.Game.Collision
{
    internal class CollisionManager
    {
        private List<ICollidable> collidables = new List<ICollidable>();

        public ICollidable AddObj(string tag, Entity entity)
        {
            ICollidable collidable = new Collidable(entity, tag);
            collidables.Add(collidable);
            return collidable;
        }
        
        public void checkCollisions()
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
                        IsCollided(obj1, obj2);
                    }
                }
            }
        }
        public bool IsCollided(ICollidable entityA, ICollidable entityB)
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
    }
}
