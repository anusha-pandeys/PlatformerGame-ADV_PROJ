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
    private float i = 0;
    private float time = 0;
    private int globalrow = 0;
    private int globalcol = 0;
    private float currRow = 0;
    public void setTetxture(string relPath, Vector2 position, Vector2 size)
    {
        string absolutePat = System.IO.Path.GetFullPath(relPath);
        this.texture = Engine.LoadTexture(absolutePat);
        this.position = position;
        this.size = size;
    }
    public Bounds2 draw(int numCols, int row, int offsetX, int offsetY)
    {
        if(row != globalrow)
        {
            currRow = (row * offsetY) - offsetY;
            globalrow = row;
        }
        time += Engine.TimeDelta;
        i += (offsetX);
        Console.WriteLine(time);
        if (i >= (numCols * offsetX))
        {
            i = 0;
        }
        if (time < 5)
        {
            return new Bounds2(new Vector2(i, currRow),
                new Vector2(offsetX, offsetY));
           
        } else
        {
            time = 0;
            
            return new Bounds2(new Vector2(i, currRow),
                new Vector2(offsetX, offsetY));
        }
    }
}