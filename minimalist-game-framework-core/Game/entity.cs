using System;
using System.Collections.Generic;
using System.Text;

public abstract class Entity : IDisposable
{
    public abstract List<Vector2> getCoordinates();
    public abstract void Render();
    public abstract void DrawRectangle(Vector2 position, Vector2 size, GameColor color);
    public abstract Boolean detectCollision(List<Entity> entities, Vector2 prospectiveVelocity);

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}