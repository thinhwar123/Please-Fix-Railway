using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleRail : BasicRail
{
    public List<Cell> GetAllCellCanConnect()
    {
        List<Cell> cells = new List<Cell>();
        List<int> code = ConnectionCode.GetCurrentConnectionCode(Transform);

        if (code.Contains(1))
        {
            Cell tempCell = CellManager.Instance.GetCell(Coordinates.x - 1, Coordinates.y);
            if (tempCell != null && tempCell.Entity != null && tempCell.Entity.ConnectionCode.GetCurrentConnectionCode(tempCell.Entity.Transform).Contains(3))
            {
                cells.Add(tempCell);
            }
        }
        if (code.Contains(2))
        {
            Cell tempCell = CellManager.Instance.GetCell(Coordinates.x, Coordinates.y + 1);
            if (tempCell != null && tempCell.Entity != null && tempCell.Entity.ConnectionCode.GetCurrentConnectionCode(tempCell.Entity.Transform).Contains(4))
            {
                cells.Add(tempCell);
            }
        }
        if (code.Contains(3))
        {
            Cell tempCell = CellManager.Instance.GetCell(Coordinates.x + 1, Coordinates.y);
            if (tempCell != null && tempCell.Entity != null && tempCell.Entity.ConnectionCode.GetCurrentConnectionCode(tempCell.Entity.Transform).Contains(1))
            {
                cells.Add(tempCell);
            }
        }
        if (code.Contains(4))
        {
            Cell tempCell = CellManager.Instance.GetCell(Coordinates.x, Coordinates.y - 1);
            if (tempCell != null && tempCell.Entity != null && tempCell.Entity.ConnectionCode.GetCurrentConnectionCode(tempCell.Entity.Transform).Contains(2))
            {
                cells.Add(tempCell);
            }
        }

        return cells;
    }
}
