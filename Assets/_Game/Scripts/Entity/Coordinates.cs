using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Coordinates
{
    public int x;
    public int y;

    public Coordinates()
    {
        x = 0;
        y = 0;
    }
    public Coordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Coordinates operator +(Coordinates a, Coordinates b)
    {
        return new Coordinates(a.x + b.x, a.y + b.y);
    }
    public static Coordinates operator -(Coordinates a, Coordinates b)
    {
        return new Coordinates(a.x - b.x, a.y - b.y);
    }
}
