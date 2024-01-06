using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

internal abstract class Entity : IDisposable
{
    string name;
    public Vector2 position;
    public Vector2 size;
    public Boolean dead;
    public Rectangle Bound { get { return CalculateBound(); } }
    public string Name { get { return name; } }
    public Vector2 Position { get { return position; } set { position = value; } }

    protected abstract Rectangle CalculateBound();
    public abstract void Render(Camera camera);
    protected abstract void Draw(Vector2 position, Vector2 size);

    protected Rectangle GetBounds()
    {
        return CalculateBound();
    }



    public void Dispose()
    {
        throw new NotImplementedException();
    }
}