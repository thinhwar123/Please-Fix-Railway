using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTest : MonoBehaviour
{
    public Vector3 a; // Điểm đầu
    public Vector3 b; // Điểm cuối
    public float radius; // Bán kính của đường cong
    public float angle; // Góc quay của đường cong

    void Update()
    {
        Vector3 direction = b - a;
        float distance = direction.magnitude;
        Vector3 center = a + (distance / 2) * direction.normalized; // Tính tâm của đường cong
        Quaternion rotation = Quaternion.LookRotation(direction); // Tính góc quay của đường cong

        // Vẽ đường cong
        Debug.DrawLine(a, center - rotation * Vector3.right * radius, Color.red); // Vẽ đoạn thẳng từ điểm đầu đến điểm bên trái của đường cong
        Debug.DrawLine(b, center + rotation * Vector3.right * radius, Color.green); // Vẽ đoạn thẳng từ điểm cuối đến điểm bên phải của đường cong
        float fromAngle = Vector3.SignedAngle(Vector3.forward, a - center, Vector3.up);
        float toAngle = Vector3.SignedAngle(Vector3.forward, b - center, Vector3.up);
        if (fromAngle > toAngle)
        {
            float temp = fromAngle;
            fromAngle = toAngle;
            toAngle = temp;
        }
        Debug.DrawRay(center, rotation * Quaternion.Euler(0, fromAngle, 0) * Vector3.forward * radius, Color.blue); // Vẽ cung đường cong
        Debug.DrawRay(center, rotation * Quaternion.Euler(0, toAngle, 0) * Vector3.forward * radius, Color.blue);
        Debug.DrawRay(center, rotation * Quaternion.Euler(0, (fromAngle + toAngle) / 2, 0) * Vector3.forward * radius, Color.white); // Vẽ tia từ tâm của đường cong đến giữa cung đường cong
        Debug.DrawLine(a, b, Color.black); // Vẽ đoạn thẳng từ điểm đầu đến điểm cuối
    }
    // Hàm để vẽ đường cong trong Unity Editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(a, 0.5f);
        Gizmos.DrawWireSphere(b, 0.5f);
    }
}
