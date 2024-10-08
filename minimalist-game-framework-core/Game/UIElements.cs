﻿using System;
using System.Collections.Generic;
using System.Text;

internal abstract class UIElements
{
    public abstract void setName(string name);
    public abstract string getName();
    public abstract void setPosition(Vector2 position);
    public abstract void setSize(Vector2 size);
    public abstract Vector2 getSize();
    public abstract Vector2 getPosition();
    public abstract void Render();
    protected abstract void Draw(Vector2 position, Vector2 size);
}
