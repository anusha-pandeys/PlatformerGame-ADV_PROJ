using System;
using System.Collections.Generic;
using System.Text;

internal interface Entity
{
    List<Vector2> getCoordinates();
    void Render();
    void DrawRectangle(Vector2 position, Vector2 size, GameColor color);
    Boolean detectCollision(List<Entity> entities, Vector2 prospectiveVelocity);
}