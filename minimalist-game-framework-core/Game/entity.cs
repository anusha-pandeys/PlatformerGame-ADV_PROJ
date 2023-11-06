using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Game;


namespace Game.Game
{
    internal abstract class Entity : IDisposable
    {
        string name;
        Vector2 position;
        public Rectangle Bound { get { return CalculateBound(); } }
        public string Name { get { return name; } }
        public Vector2 Position { get { return position; } set { position = value; } }

        protected abstract Rectangle CalculateBound();
        protected abstract void Render();
        protected abstract void Draw();
        //public abstract List<Vector2> getCoordinates();
        //public abstract void Render();
        //public abstract void DrawRectangle(Vector2 position, Vector2 size, GameColor color);
        // public abstract bool DetectCollision(List<Entity> entities, Vector2 prospectiveVelocity);

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
