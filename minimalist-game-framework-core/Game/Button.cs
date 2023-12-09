using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;


internal class Button
{
    private IntPtr Renderer => Engine.Renderer2;
    public string Text { get; }
    public Vector2 position;
    public Vector2 size;
    public Vector2 getPosition()
    {
        return position;
    }

    public Vector2 getSize()
    {
        return size;
    }
    public Button(string text, Vector2 position, Vector2 size)
    {
        Text = text;
        this.position = position;
        this.size = size;
    }



    public bool IsClicked()
    {

        int mouseX, mouseY;
        SDL.SDL_GetMouseState(out mouseX, out mouseY);

        bool isClicked = (mouseX >= position.X && mouseX <= position.X + size.X &&
                          mouseY >= position.Y && mouseY <= position.Y + size.Y &&
                          SDL.SDL_GetMouseState(IntPtr.Zero, IntPtr.Zero) == SDL.SDL_BUTTON(SDL.SDL_BUTTON_LEFT));

        return isClicked;
    }
}
 