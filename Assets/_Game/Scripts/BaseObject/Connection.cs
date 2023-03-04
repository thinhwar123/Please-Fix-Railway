using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PathCreation;

public class Connection : MonoBehaviour
{
    public float m_Length;
    public PathCreator m_PathCreator;
    public Connection m_NexConnection;
    [SerializeField] private Transform m_Direction;

    [ShowInInspector]
    private Vector3 m_ConnectDirection { get => GetConnectDirection(); }

    public Vector3 GetConnectDirection()
    {
        if (m_Direction == null) return Vector3.zero;
        Vector3 res = m_Direction.forward;
        res.x = Mathf.RoundToInt(res.x);
        res.y = 0;
        res.z = Mathf.RoundToInt(res.z);
        return res;
    }
    private void OnDrawGizmos()
    {
        if (m_Direction == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_Direction.position, m_Direction.position + m_Direction.forward * 0.3f);
    }

    [Button]
    public void UpdatePath()
    {
        if (m_NexConnection == null)
        {
            BezierPath endPath = new BezierPath(new Vector3[2] {Vector3.zero, Vector3.zero}, false, PathSpace.xz);
            m_PathCreator.bezierPath = endPath;
            m_Length = 0;

            return;
        }

        Vector3 startPos = Vector3.zero;
        Vector3 endPos = m_NexConnection.transform.localPosition - transform.localPosition;

        List<Vector3> point = new List<Vector3>() { startPos, endPos };
        List<Vector3> controlPoints = new List<Vector3>();
        controlPoints.Add(startPos);
        controlPoints.Add(startPos - GetConnectDirection() * 0.25f);
        controlPoints.Add(endPos - m_NexConnection.GetConnectDirection() * 0.25f);
        controlPoints.Add(endPos);

        BezierPath bezierPath = new BezierPath(point.ToArray(), false, PathSpace.xyz);
        bezierPath.ControlPointMode = BezierPath.ControlMode.Aligned;
        for (int i = 0; i < controlPoints.Count; i++)
        {
            bezierPath.SetPoint(i, controlPoints[i]);
        }

        m_PathCreator.bezierPath = bezierPath;
        m_Length = m_PathCreator.path.length;
    }
}
