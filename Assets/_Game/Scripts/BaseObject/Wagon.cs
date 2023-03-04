using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;

public class Wagon : MonoBehaviour
{
    private Transform m_Transform;
    public Transform Transform { get { return m_Transform ??= transform; } }

    public WagonState m_WagonState;

    public int m_WagonID;
    public TextMeshPro m_TextCarID;
    [SerializeField] private GameObject m_ModelWagon;

    [SerializeField] private Transform m_RayCastPosition;
    [SerializeField] private LayerMask m_WhatIsWagon;

    [SerializeField] private float m_Speed;
    [SerializeField] private float m_CurrentMoveTime;

    [SerializeField] private BasicRail m_CurrentRail;
    [SerializeField] private Connection m_CurrentConnection;

    [SerializeField] private BasicRail m_NexRail;
    [SerializeField] private Connection m_NexConnection;

    private Wagon m_HitWagon;
    private float m_WaitPassengerTime;
    private float m_HitWagonTime;
    private bool m_IsHidden;
    private List<Material> m_WagonMaterial = new List<Material>();
    public List<int> m_RenderQueues = new List<int>();
    private float m_MoveDeltaTime { get => GetMoveDeltaTime(); }
    public float WaitPassengerDuration { get => 2 / m_Speed; }
    private void Start()
    {
        SetupHiddenWagon();
        Init();
    }
    protected virtual void Init()
    {
        m_CurrentMoveTime = 0.5f / m_Speed;
    }
    private void Update()
    {
        MoveFollowPath();
        CheckOnWaitBarrierOpen();
        CheckWaitPassenger();
        CheckOnWaitAFrame();
        CheckOnRunning();

    }
    private void LateUpdate()
    {
        UpdateNextRail();
    }
    public void SetWagonID(int id)
    {
        m_WagonID = id;
        if (m_TextCarID == null) return;
        m_TextCarID.text = m_WagonID.ToString();
    }    
    [Button]
    public void Run()
    {
        m_WagonState = WagonState.Running;
    }
    private void MoveFollowPath()
    {
        if (m_WagonState != WagonState.WaitRunning)
        {
            m_CurrentMoveTime += Time.deltaTime;
        }
        Vector3 velocity = Vector3.zero;


        if (m_WagonState != WagonState.Running )
        {
            if (m_CurrentMoveTime > m_MoveDeltaTime)
            {
                m_CurrentMoveTime -= m_MoveDeltaTime;
            }
            return;
        }

        if (m_CurrentMoveTime > m_MoveDeltaTime && m_NexRail != null)
        {
            m_CurrentMoveTime -= m_MoveDeltaTime;
            m_CurrentRail = m_NexRail;
            m_CurrentConnection = m_NexConnection;
            m_NexRail = null;
            m_NexConnection = null;
        }

        m_CurrentRail.OnMoveUpdate(this, m_CurrentConnection, m_CurrentMoveTime / m_MoveDeltaTime);
        Transform.position = m_CurrentConnection.m_PathCreator.path.GetPointAtTime(m_CurrentMoveTime / m_MoveDeltaTime, PathCreation.EndOfPathInstruction.Stop);
        Transform.rotation = Quaternion.LookRotation(m_CurrentConnection.m_PathCreator.path.GetDirection(m_CurrentMoveTime / m_MoveDeltaTime, PathCreation.EndOfPathInstruction.Stop));
    }
    private void UpdateNextRail()
    {
        if (m_WagonState != WagonState.Running) return;

        if (m_CurrentMoveTime >= m_MoveDeltaTime / 2f && m_NexRail == null)
        {
            if (HasNextRail())
            {
                m_NexRail = m_CurrentRail.GetNextRail(m_CurrentConnection.m_NexConnection);
                m_NexConnection = m_NexRail.GetConnection(m_CurrentConnection.m_NexConnection);
            }
            else
            {
                ChangeWagonState(WagonState.StopEndRail);
            }
        }
    }
    private bool HasNextRail()
    {
        BasicRail temp = m_CurrentRail.GetNextRail(m_CurrentConnection.m_NexConnection);
        if (temp == null) return false;
        return temp.GetConnection(m_CurrentConnection.m_NexConnection).m_Length != 0;
    }
    private void CheckOnRunning()
    {
        if (m_WagonState != WagonState.Running) return;

        if (m_NexRail != null && m_NexRail is Barrier && !(m_NexRail as Barrier).m_IsOpen)
        {
            ChangeWagonState(WagonState.WaitBarrierOpen);
        }
        else if (m_NexRail != null && m_NexRail is Tunnel && m_IsHidden)
        {
            if (RaycastIgnoreItself((m_NexRail as Tunnel).m_RaycastTransform.position, (m_NexRail as Tunnel).m_RaycastTransform.forward, out RaycastHit hit, 0.5f, m_WhatIsWagon))
            {
                m_HitWagon = hit.collider.GetComponent<Wagon>();
                if (m_HitWagon.m_WagonState == WagonState.Running && m_HitWagon.m_IsHidden)
                {
                    ChangeWagonState(WagonState.StopHitOtherWagon);
                    m_HitWagon.ChangeWagonState(WagonState.StopHitOtherWagon);
                }
            }
        }
        else if (RaycastIgnoreItself(m_RayCastPosition.position, m_RayCastPosition.forward, out RaycastHit hit, 0.5f, m_WhatIsWagon))
        {
            m_HitWagon = hit.collider.GetComponent<Wagon>();
            switch (m_HitWagon.m_WagonState)
            {
                case WagonState.WaitRunning:
                    ChangeWagonState(WagonState.WaitRunning);
                    break;
                case WagonState.WaitABit:
                    if (!m_HitWagon.m_IsHidden && !m_IsHidden)
                    {
                        ChangeWagonState(WagonState.WaitABit);
                    }
                    break;
                case WagonState.WaitBarrierOpen:
                    if (!m_HitWagon.m_IsHidden && !m_IsHidden)
                    {
                        ChangeWagonState(WagonState.WaitABit);
                    }
                    break;
                case WagonState.WaitPassenger:
                    if (m_HitWagon.GetWaitPassengerRemainingTime() > 0.1f && !m_HitWagon.m_IsHidden && !m_IsHidden)
                    {
                        ChangeWagonState(WagonState.StopHitOtherWagon);
                        m_HitWagon.ChangeWagonState(WagonState.StopHitOtherWagon);
                    }
                    break;
                case WagonState.Running:
                    if (m_HitWagon.m_HitWagon == this && !m_HitWagon.m_IsHidden && !m_IsHidden)
                    {
                        ChangeWagonState(WagonState.StopHitOtherWagon);
                        m_HitWagon.ChangeWagonState(WagonState.StopHitOtherWagon);
                    }
                    break;
                case WagonState.StopEndRail:
                    ChangeWagonState(WagonState.StopEndRail);
                    break;
                case WagonState.StopHitOtherWagon:
                    ChangeWagonState(WagonState.StopHitOtherWagon);
                    break;
            }
        }
        else
        {
            m_HitWagon = null;
        }
    }
    private void CheckOnWaitBarrierOpen()
    {
        if (m_WagonState != WagonState.WaitBarrierOpen) return;
        if ((m_NexRail != null && m_NexRail is Barrier && (m_NexRail as Barrier).m_IsOpen) ||
            (m_HitWagon != null && m_HitWagon.m_WagonState != WagonState.WaitBarrierOpen))
        {
            m_WagonState = WagonState.Running;
        }
    }
    private void CheckOnWaitAFrame()
    {
        if (m_WagonState != WagonState.WaitABit) return;
        if (RaycastIgnoreItself(m_RayCastPosition.position, m_RayCastPosition.forward, out RaycastHit hit, 0.5f, m_WhatIsWagon))
        {
            m_HitWagon = hit.collider.GetComponent<Wagon>();
            if (m_HitWagon.m_WagonState == WagonState.Running)
            {
                ChangeWagonState(WagonState.Running);
            }
            else if (m_HitWagonTime + 0.1f < Time.time && m_HitWagon.m_HitWagon == this )
            {
                ChangeWagonState(WagonState.StopHitOtherWagon);
                m_HitWagon.ChangeWagonState(WagonState.StopHitOtherWagon);
            }
        }
        else
        {
            m_HitWagon = null;
            ChangeWagonState(WagonState.Running);
        }
    }
    private void CheckWaitPassenger()
    {
        if (m_WagonState != WagonState.WaitPassenger) return;
        if (GetWaitPassengerRemainingTime() <= 0)
        {
            Run();
        }

    }
    public void ChangeWagonState(WagonState wagonState)
    {
        // TODO: stop and check result
        switch (wagonState)
        {
            case WagonState.WaitRunning:
                FixPosition();
                break;
            case WagonState.WaitBarrierOpen:
                break;
            case WagonState.WaitPassenger:
                m_WaitPassengerTime = Time.time;
                break;
            case WagonState.Running:
                break;
            case WagonState.StopEndRail:
                FixPosition();
                break;
            case WagonState.StopHitOtherWagon:
                break;
            case WagonState.WaitABit:
                m_HitWagonTime = Time.time;
                break;
            default:
                break;
        }
        Debug.Log(string.Format("Wagon {0} change state: {1} ", m_WagonID, wagonState));
        m_WagonState = wagonState;
        
    }
    private void FixPosition()
    {
        Vector3 fixPosition = Transform.position;
        fixPosition.x = Mathf.RoundToInt(fixPosition.x * 10) / 10.0f;
        fixPosition.z = Mathf.RoundToInt(fixPosition.z * 10) / 10.0f;
        Transform.position = fixPosition;
    }
    private float GetWaitPassengerRemainingTime()
    {
        return m_WaitPassengerTime + WaitPassengerDuration - Time.time;
    }
    private void SetupHiddenWagon()
    {
        MeshRenderer[] meshRenderers = m_ModelWagon.GetComponentsInChildren<MeshRenderer>(true);
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            m_WagonMaterial.Add(meshRenderers[i].material);
            m_RenderQueues.Add(meshRenderers[i].material.renderQueue);
        }

        SpriteRenderer[] spriteRenderers = m_ModelWagon.GetComponentsInChildren<SpriteRenderer>(true);
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            m_WagonMaterial.Add(spriteRenderers[i].material);
            m_RenderQueues.Add(spriteRenderers[i].material.renderQueue);
        }
    }
    private float GetMoveDeltaTime()
    {
        switch (m_CurrentRail.EntityType)
        {
            case EntityType.Tunnel:
                return 0.5f / m_Speed; ;
            case EntityType.Locomotive:
                return 9 / m_Speed; ;
            default:
                return 1 / m_Speed;
        }
    }
    public void SetHideFromMask(bool value)
    {
        m_IsHidden = value;
        for (int i = 0; i < m_WagonMaterial.Count; i++)
        {
            m_WagonMaterial[i].renderQueue = value ? 3002 : m_RenderQueues[i];
        }
    }
    private bool RaycastIgnoreItself(Vector3 origin, Vector3 direction, out RaycastHit infor, float maxDistance, int layerMask)
    {
        RaycastHit[] raycastHits = Physics.RaycastAll(origin, direction, maxDistance, layerMask);
        for (int i = 0; i < raycastHits.Length; i++)
        {
            if (raycastHits[i].collider.gameObject != this.gameObject)
            {
                infor = raycastHits[i];
                return true;
            }
        }
        infor = default;
        return false;
    }
}
public enum WagonState
{
    WaitRunning,
    WaitABit,
    WaitBarrierOpen,
    WaitPassenger,

    Running,

    StopEndRail,
    StopHitOtherWagon,
}
