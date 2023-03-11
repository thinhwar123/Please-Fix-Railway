using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions 
{
    public static Vector3 Right(this Vector3 vector)
    {
        return new Vector3(vector.z, vector.y, -vector.x);
    }

    public static Vector3 Left(this Vector3 vector)
    {
        return new Vector3(-vector.z, vector.y, vector.x);
    }
    
    public static Vector2 Right(this Vector2 vector)
    {
        return new Vector2(vector.y, -vector.x);
    }

    public static Vector2 Left(this Vector2 vector)
    {
        return new Vector2(-vector.y, vector.x);
    }


}
