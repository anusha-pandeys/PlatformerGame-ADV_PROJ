using System;
using System.Collections.Generic;
using System.Text;

internal interface entity
{
    void Render();
    void DrawRectangle(Vector2 position, Vector2 size, Color color);
}