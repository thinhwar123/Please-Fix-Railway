using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVector : MonoBehaviour
{
    public Transform m_Rail;
    public Vector3 v1,v2,v3;

    [Header("AXS")]
    public Vector3 m_VectorLeft = new Vector3(0, 0, 1);
    public Vector3 m_VectorRight = new Vector3(0, 0, -1);
    public Vector3 m_VectorUp = new Vector3(1, 0, 0);
    public Vector3 m_VectorDown = new Vector3(-1, 0, 0);
    private void Update() {
        

    }
    private void OnDrawGizmos() {
        DrawArrow.ForGizmo(Vector3.zero, v1);
        DrawArrow.ForGizmo(Vector3.zero, v2);
        DrawArrow.ForGizmo(Vector3.zero, v3);
        if (m_Rail != null) {
            GetAngleForSwitchLeft(v1, v2, v3);
        }
    }
    public void GetAngleForSwitchRight(Vector3 t1, Vector3 t2, Vector3 t3) {
        float angle = 0;
        Vector3 outVector = Vector3.zero;
        if (Utilss.IsVectorParallel(t1, t2)) {
            Vector3 v4 = (v1 - v2).normalized;
            outVector = t3;
        }else if (Utilss.IsVectorParallel(t1, t3)) {
            outVector = t2;
        }else if (Utilss.IsVectorParallel(t2, t3)) {
            outVector = t1;
        }
        
        if(outVector == m_VectorDown) {
            angle = 180;
        }else if(outVector == m_VectorUp) {
            angle = 0;
        } else if(outVector == m_VectorRight) {
            angle = 270;
        }else if(outVector == m_VectorLeft) {
            angle = 90;
        }
        m_Rail.localEulerAngles = new Vector3(0, -angle, 0);
    }

    public void GetAngleForSwitchLeft(Vector3 t1, Vector3 t2, Vector3 t3) {
        float angle = 0;
        Vector3 outVector = Vector3.zero;
        if (Utilss.IsVectorParallel(t1, t2)) {
            Vector3 v4 = (v1 - v2).normalized;
            outVector = t3;
        } else if (Utilss.IsVectorParallel(t1, t3)) {
            outVector = t2;
        } else if (Utilss.IsVectorParallel(t2, t3)) {
            outVector = t1;
        }

        if (outVector == m_VectorDown) {
            angle = 0;
        } else if (outVector == m_VectorUp) {
            angle = 180;
        } else if (outVector == m_VectorRight) {
            angle = 90;
        } else if (outVector == m_VectorLeft) {
            angle = 270;
        }
        m_Rail.localEulerAngles = new Vector3(0, -angle, 0);
    }
}
