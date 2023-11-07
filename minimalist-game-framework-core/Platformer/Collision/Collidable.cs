using System;
using System.Collections.Generic;
using System.Text;


public interface ICollidable : IDisposable
{
    Entity GameObject { get; }
    string Tag { get; }


}
public class Collidable : ICollidable
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

