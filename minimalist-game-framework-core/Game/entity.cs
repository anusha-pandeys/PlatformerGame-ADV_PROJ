using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Game
{
    internal interface entity
    {
        void Render();
        void DrawRectangle(Vector2 position, Vector2 size, Color color);
    }
}