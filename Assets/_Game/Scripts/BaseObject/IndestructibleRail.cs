using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IndestructibleRail : BasicRail
{
    public bool CanConnectToCell(Cell cell)
    {
        if (Coordinates.x > cell.Coordinates.x)
        {
            return ConnectionCode.GetCurrentConnectionCode(Transform).Contains(1);
        }
        else if (Coordinates.y > cell.Coordinates.y)
        {
            return ConnectionCode.GetCurrentConnectionCode(Transform).Contains(2);
        }
        else if (Coordinates.x < cell.Coordinates.x)
        {
            return ConnectionCode.GetCurrentConnectionCode(Transform).Contains(3);
        }
        else if (Coordinates.y < cell.Coordinates.y)
        {
            return ConnectionCode.GetCurrentConnectionCode(Transform).Contains(4);
        }
        return false;
    }
}
