using System;
using System.Collections.Generic;
using System.Text;



internal class TextRenderer
{
    private IntPtr Renderer => Engine.Renderer2;

    public TextRenderer()
    {

    }

    public void displayText(string text, Vector2 position, Color color, Font font1)
    {
        Engine.DrawString(text, position, color, font1);
    }
}
