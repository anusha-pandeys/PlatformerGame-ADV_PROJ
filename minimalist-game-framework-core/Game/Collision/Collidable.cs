using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Game
{
    internal interface ICollidable : IDisposable
    {
        Entity GameObject { get; }
        string Tag { get; }


    }
    internal class Collidable : ICollidable
    {
        public Entity GameObject { get; set; }
        public string Tag { get; set; }

        public Collidable(Entity gameObject, string tag) {
            GameObject = gameObject;
            Tag = tag;
        }

        public void Dispose()
        {
            GameObject = null;
            Tag = null;
        }
    }
}
