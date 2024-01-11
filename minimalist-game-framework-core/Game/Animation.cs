using SDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

internal class Animation 
{

    //private int numCols;
    //private int row;
    private int offsetX;
    private int offsetY;
    private Vector2 position;
    private Vector2 size;
    private Texture texture;
    
    public void setTetxture(string relPath, Vector2 position, Vector2 size)
    {
        string absolutePat = System.IO.Path.GetFullPath(relPath);
        this.texture = Engine.LoadTexture(absolutePat);
        this.position = position;
        this.size = size;
    }
    public void drawTexture(int numCols, int row, int offsetX, int offsetY, float time)
    {
        if (time < 1f)
        {
            Engine.DrawTexture(texture, position, size: size, source: new Bounds2(new Vector2(i, currrow),
                new Vector2(offsetX, offsetY)));
        }
        int currrow = (row * offsetY)-offsetY;
        for(int i = 0; i < numCols*offsetX; i += offsetX)
        {
            Engine.DrawTexture(texture, position, size: size, source: new Bounds2(new Vector2(i, currrow),
                new Vector2(offsetX, offsetY)));
        }
    }
}