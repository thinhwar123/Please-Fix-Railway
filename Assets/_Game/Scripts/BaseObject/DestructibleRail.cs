using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleRail : BasicRail
{
    public List<Cell> GetAllCellCanConnect()
    {
        List<Cell> cells = new List<Cell>();
        List<int> code = ConnectionCode.GetCurrentConnectionCode(Transform);

        if (code.Contains(1) && CellManager.Instance.GetCell(Coordinates.x - 1, Coordinates.y) != null)
        {
            cells.Add(CellManager.Instance.GetCell(Coordinates.x - 1, Coordinates.y));
        }
        if (code.Contains(2) && CellManager.Instance.GetCell(Coordinates.x, Coordinates.y + 1) != null)
        {
            cells.Add(CellManager.Instance.GetCell(Coordinates.x, Coordinates.y + 1));
        }
        if (code.Contains(3) && CellManager.Instance.GetCell(Coordinates.x + 1, Coordinates.y) != null)
        {
            cells.Add(CellManager.Instance.GetCell(Coordinates.x + 1, Coordinates.y));
        }
        if (code.Contains(4) && CellManager.Instance.GetCell(Coordinates.x, Coordinates.y - 1) != null)
        {
            cells.Add(CellManager.Instance.GetCell(Coordinates.x, Coordinates.y - 1));
        }

        return cells;
    }
}
