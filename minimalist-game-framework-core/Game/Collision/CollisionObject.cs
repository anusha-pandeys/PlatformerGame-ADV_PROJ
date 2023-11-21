using System;
using System.Collections.Generic;
using System.Text;


internal class CollisionObject
{

    private bool left;
    private bool right;
    private bool up;
    private bool down;
    private bool collided;
    private Vector2 distance;
    public CollisionObject()
    {
        distance = new Vector2();
        left = false;
        right = false;
        up = false;
        down = false;
        collided = false;
    }

    public Vector2 getDistance()
    {
        return distance; 
    }
    public bool getCollided()
    {
        return collided;
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
    public void setCollided(bool collided)
    {
        this.collided = collided;
    }
    public void setDown(bool down)
    {
        this.down = down;
    }
    public void setDistance(Vector2 distance)
    {
        this.distance = distance;
    }
}
