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
    private float distanceY;
    private float distanceX;
    private double distance;
    private Entity block;
    public CollisionObject()
    {
        distanceY = 0f;
        left = false;
        right = false;
        up = false;
        down = false;
        collided = false;
        distanceX = 0f;
        distance = 0;
        block = null;
    }

    public Entity getBlock()
    {
        return block;
    }
    public double getDistance()
    {
        return distance;
    }
    public float getDistanceY()
    {
        return distanceY; 
    }
    public float getDistanceX()
    {
        return distanceX;
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
    public void setDistanceY(float distance)
    {
        this.distanceY = distance;
    }
    public void setDistanceX(float distance)
    {
        this.distanceX = distance;
    }

    public void setDistance(double distance)
    {
        this.distance = distance;
    }

    public void setBlock(Entity block)
    {
        this.block = block;
    }
}
