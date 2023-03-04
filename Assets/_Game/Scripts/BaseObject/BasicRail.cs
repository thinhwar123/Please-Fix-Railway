using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class BasicRail : Entity
{    
    public List<Connection> m_Connections = new List<Connection>();
    public virtual BasicRail GetNextRail(Connection connection)
    {
        int x = m_Coordinates.x + Mathf.RoundToInt(connection.GetConnectDirection().x);
        int y = m_Coordinates.y + Mathf.RoundToInt(connection.GetConnectDirection().z);
        if (x >= CellManager.Instance.CurrentWidth ||
            y >= CellManager.Instance.CurrentHeight ||
            x < 0 ||
            y < 0)
            return null;
        Entity entity = CellManager.Instance.GetCell(x, y).Entity;
        if (entity == null) return null;
        return entity as BasicRail;
    }
    public virtual Connection GetConnection(Connection otherConnection)
    {
        for (int i = 0; i < m_Connections.Count; i++)
        {
            if (m_Connections[i].GetConnectDirection() + otherConnection.GetConnectDirection() == Vector3.zero)
            {
                return m_Connections[i];
            }
        }
        return null;
    }
    public virtual void OnMoveUpdate(Wagon wagon, Connection currentConnection, float moveTime)
    {

    }

}
