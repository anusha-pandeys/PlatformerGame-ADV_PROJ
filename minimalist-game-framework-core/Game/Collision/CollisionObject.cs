using System;
using System.Collections.Generic;
using System.Text;


internal class CollisionObject
{

    private bool left;
    private bool right;
    private bool up;
    private bool down;
    private bool collidedVertical;
    private bool collidedHorizontal;
    private double distance;
    public CollisionObject()
    {
        distance = 0;
        left = false;
        right = false;
        up = false;
        down = false;
        collidedVertical = false;
        collidedHorizontal = false;
    }

    public double getDistance()
    {
        return distance; 
    }
    public bool getCollidedVertical()
    {
        return collidedVertical;
    }
    public bool getCollidedHorizontal()
    {
        return collidedHorizontal;
    }
    public bool getLeft()
    {
        return left;
    }
    public bool getRight()
    {
        return right;
    }

    public bool getUp()
    {
        return up;
    }

    public bool getDown()
    {
        return down;
    }
    public void setLeft(bool left)
    {
        this.left = left;
    }
    public void setRight(bool right)
    {
        this.right = right;
    }

    public void setUp(bool up)
    {
        this.up = up;
    }
    public void setCollidedVertical(bool collided)
    {
        this.collidedVertical = collided;
    }
    public void setCollidedHorizontal(bool collided)
    {
        this.collidedHorizontal = collided;
    }
    public void setDown(bool down)
    {
        this.down = down;
    }
    public void setDistance(double distance)
    {
        this.distance = distance;
    }
}
