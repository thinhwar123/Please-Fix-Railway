using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public enum InputType { DELETE, INSERT }

public class IngameManager : Singleton<IngameManager> {
    [SerializeField] private LayerMask m_LayerMaskEmptyTile;
    [SerializeField] private LayerMask m_LayerMaskMapObject;

    public List<GameObject> m_RailPrefabs = new List<GameObject>();
    public List<GameObject> m_CurrentRails = new List<GameObject>();

    public Camera m_MainCamera;

    public bool IsDragging = false;
    public GameObject m_LastRailGO;
    public Vector3 m_LastDirectionVector;

    public Vector3 m_VectorLeft = new Vector3(0, 0, 1);
    public Vector3 m_VectorRight = new Vector3(0, 0, -1);
    public Vector3 m_VectorUp = new Vector3(1, 0, 0);
    public Vector3 m_VectorDown = new Vector3(-1, 0, 0);

    public RailType m_FirstRailType = RailType.Straight;
    public InputType m_InputType = InputType.INSERT;
    private void Awake() {
    }
    private void Update() {
        HandleInputInsert();
        HandleInputDelete();
    }
    
    private void HandleInputInsert() {
        //if (Input.GetKeyDown(KeyCode.Escape)) {
        //    ClearMap();
        //}
        //if (m_InputType != InputType.INSERT) return;
        //int w = -1;
        //int h = -1;
        //if (Input.GetMouseButtonDown(0)) {
        //    LevelManager.Instance.MarkUpdateAround(false);
        //    IsDragging = true;
        //    m_LastRailGO = null;
        //    Ray interactRay = m_MainCamera.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hitEmptyTile;
        //    RaycastHit hitMapObject;
            
        //    {
        //        if (Physics.Raycast(interactRay, out hitMapObject, 100, m_LayerMaskMapObject)) {
        //            GameObject go = hitMapObject.transform.gameObject;
        //            if (go != null) {
        //                DestructibleRail rail = go.GetComponent<DestructibleRail>();
        //                if(rail != null) {
        //                    m_LastRailGO = go;
        //                    RailType replaceType = RailType.None;
        //                    RailType railType = (RailType) rail.m_Index;
        //                    switch (railType) {
        //                        case RailType.Straight:
        //                            break;
        //                        case RailType.SwitchLeft: {
        //                                replaceType = RailType.SwitchRight;
        //                            }
        //                            break;
        //                        case RailType.SwitchRight: {
        //                                replaceType = RailType.SwitchLeft;
        //                            }
        //                            break;
        //                        case RailType.Turn:
        //                            break;
        //                        default:
        //                            break;
        //                    }
        //                    if(replaceType == RailType.SwitchRight|| replaceType == RailType.SwitchLeft) {
        //                        List<TileDirection> outs = new List<TileDirection>(rail.m_Tile.Outputs); 
        //                        float angle = Static.GetAngleForSwitchTurn(replaceType, outs[0], outs[1], outs[2]);
        //                        GameObject replaceRailGO = ReplaceNewRail(replaceType, rail, angle);
        //                        BaseObject replaceBase = replaceRailGO.GetComponent<BaseObject>();
        //                        replaceBase.Init(TileDirection.SwitchRight, outs[0], outs[1], outs[2]);
        //                        m_LastRailGO = replaceRailGO;
        //                    }
        //                    w = rail.m_Tile.w;
        //                    h = rail.m_Tile.h;
        //                }
        //            }
        //        } else {
        //            if (Physics.Raycast(interactRay, out hitEmptyTile, 100, m_LayerMaskEmptyTile)) {
        //                GameObject go = hitEmptyTile.transform.gameObject;
        //                if (go != null) {
        //                    Tile tile = go.GetComponent<BaseObject>().m_Tile;
        //                    Vector3 pos = hitEmptyTile.transform.position;
        //                    m_LastRailGO = CreateRail(m_FirstRailType, pos, tile.w, tile.h);

        //                    BaseObject bo = m_LastRailGO.GetComponent<BaseObject>();
        //                    bo.Init(bo.TileDirection, bo.m_Tile.Outputs.ToArray());
        //                    bo.AutoMatching();
        //                }
        //            }
        //        }
        //    }
        //}
        //if (IsDragging) {
        //    Ray interactRay = m_MainCamera.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hitEmptyTile;
        //    RaycastHit hitMapObject;
        //    {
        //        if (Physics.Raycast(interactRay, out hitMapObject, 100, m_LayerMaskMapObject)) {
        //            GameObject go = hitMapObject.transform.gameObject;
        //            //if(m_LastRailGO != null) {
        //            //    Debug.Log("GO " + go.name + " ---- " + m_LastRailGO.name);
        //            //}
        //            if (go != null && m_LastRailGO != null && go != m_LastRailGO) {
        //                BaseObject currentRail = go.GetComponent<BaseObject>();
        //                Vector3 currentTilePos = hitMapObject.transform.position;
        //                Tile currentTile = currentRail.m_Tile;
        //                if(currentTile.w != w || currentTile.h != h) {
        //                    RailType currentDesRailType = (RailType)currentRail.m_Index;
        //                    TileDirection currentDirection = currentRail.TileDirection;

        //                    BaseObject lastRail = m_LastRailGO.GetComponent<BaseObject>();
        //                    Vector3 lastTilePos = m_LastRailGO.transform.position;
        //                    Tile lastTile = lastRail.m_Tile;
        //                    RailType lastDesRailType = (RailType)lastRail.m_Index;
        //                    TileDirection lastDirection = lastRail.TileDirection;

        //                    bool isChange = false;
        //                    if (lastTile.IsAround(currentTile) /*&& !lastRail.IsLinkedWith(currentRail)*/) {
        //                        switch (lastDesRailType) {
        //                            case RailType.Straight: {
        //                                    if (lastTile.LinkCount == 0) {
        //                                        TileDirection oldDirection = lastRail.TileDirection;
        //                                        TileDirection tileDirection = Static.GetTileDirection(currentTilePos, lastTilePos);
        //                                        float angle = Static.GetAngleForStraight(tileDirection);
        //                                        lastRail.SetTileDirection(tileDirection);
        //                                        lastRail.SetRotation(angle);

        //                                        lastDirection = tileDirection;
        //                                    }
        //                                    switch (currentDesRailType) {
        //                                        case RailType.Straight: {
        //                                                MatchStraight_Straight match = new MatchStraight_Straight();
        //                                                match.Match(lastRail, currentRail);
        //                                                isChange = true;
        //                                            }
        //                                            break;
        //                                        case RailType.Turn: {
        //                                                MatchStraight_Turn match = new MatchStraight_Turn();
        //                                                match.Match(lastRail, currentRail);
        //                                                isChange = true;
        //                                            }
        //                                            break;
        //                                        case RailType.SwitchLeft:
        //                                        case RailType.SwitchRight: {
        //                                                MatchStraight_Switch match = new MatchStraight_Switch();
        //                                                match.Match(lastRail, currentRail);
        //                                                isChange = true;
        //                                            }
        //                                            break;
        //                                    }
        //                                }
        //                                break;
        //                            case RailType.Turn: {
        //                                    if (lastTile.LinkCount == 2 && currentRail.IsDestructable()) {
        //                                        Vector3 v = currentTilePos - lastTilePos;
        //                                        TileDirection t = Static.GetTileDirection(v);
        //                                        List<TileDirection> outputDirections = lastTile.Outputs;
        //                                        if (!outputDirections.Contains(t)) {
        //                                            float angle = Static.GetAngleForSwitchTurn(RailType.SwitchRight, t, outputDirections[0], outputDirections[1]);
        //                                            GameObject replaceRailGO = ReplaceNewRail(RailType.SwitchRight, lastRail, angle);
        //                                            BaseObject replaceBase = replaceRailGO.GetComponent<BaseObject>();
        //                                            replaceBase.Init(TileDirection.SwitchRight, t, outputDirections[0], outputDirections[1]);
        //                                            replaceBase.SetRotation(new Vector3(0, angle, 0));
        //                                        }
        //                                    }
        //                                    switch (currentDesRailType) {
        //                                        case RailType.Straight: {
        //                                                MatchTurn_Straight match = new MatchTurn_Straight();
        //                                                match.Match(lastRail, currentRail);
        //                                                isChange = true;
        //                                            }
        //                                            break;
        //                                        case RailType.Turn: {
        //                                                MatchTurn_Turn match = new MatchTurn_Turn();
        //                                                match.Match(lastRail, currentRail);
        //                                                isChange = true;
        //                                            }
        //                                            break;
        //                                        case RailType.SwitchLeft:
        //                                        case RailType.SwitchRight: {
        //                                                MatchTurn_Switch match = new MatchTurn_Switch();
        //                                                match.Match(lastRail, currentRail);
        //                                                isChange = true;
        //                                            }
        //                                            break;
        //                                    }
        //                                }
        //                                break;
        //                            case RailType.SwitchLeft:
        //                            case RailType.SwitchRight: {
        //                                    switch (currentDesRailType) {
        //                                        case RailType.Straight: {
        //                                                MatchSwitch_Straight match = new MatchSwitch_Straight();
        //                                                match.Match(lastRail, currentRail);
        //                                                isChange = true;
        //                                            }
        //                                            break;
        //                                        case RailType.Turn: {
        //                                                MatchSwitch_Turn match = new MatchSwitch_Turn();
        //                                                match.Match(lastRail, currentRail);
        //                                                isChange = true;
        //                                            }
        //                                            break;
        //                                        case RailType.SwitchLeft:
        //                                        case RailType.SwitchRight: {
        //                                                MatchSwitch_Switch match = new MatchSwitch_Switch();
        //                                                match.Match(lastRail, currentRail);
        //                                                isChange = true;
        //                                            }
        //                                            break;
        //                                    }
        //                                }
        //                                break;
        //                        }
        //                        //if (lastRail != null && currentRail != null && isChange) {
        //                        //    lastRail.LinkTile(currentRail.m_Tile);
        //                        //}
        //                        m_LastRailGO = go;
        //                    } else {
        //                        //Debug.Log("Not Around " + currentRail.name + "     " + lastRail.name);
        //                    }
        //                }
        //            }
        //        } else {
        //            if (Physics.Raycast(interactRay, out hitEmptyTile, 100, m_LayerMaskEmptyTile)) {
        //                GameObject go = hitEmptyTile.transform.gameObject;
        //                if (go != null && m_LastRailGO != null) {
        //                    BaseObject lastRail = m_LastRailGO.GetComponent<BaseObject>();
        //                    Vector3 lastTilePos = m_LastRailGO.transform.position;
        //                    Tile lastTile = lastRail.m_Tile;
        //                    RailType lastDesRailType = (RailType)lastRail.m_Index;
        //                    TileDirection lastDirection = lastRail.TileDirection;
                            
        //                    if (m_LastRailGO != null) {
        //                        Tile tile = go.GetComponent<BaseObject>().m_Tile;
        //                        if (lastRail.m_Tile.IsAround(tile)) {
        //                            Vector3 currentTilePos = hitEmptyTile.transform.position;
        //                            Vector3 dragDirection = (currentTilePos - lastTilePos).normalized;

        //                            //Debug.Log("Start == " + startDesRail + " index " + startRail.m_Index + " name " + startRail.gameObject.name);
        //                            switch (lastDesRailType) {
        //                                case RailType.Straight: {
        //                                        Vector3 v1 = currentTilePos;
        //                                        Vector3 v2 = lastTilePos;

        //                                        TileDirection tileDirection = Static.GetTileDirection(v1, v2);
        //                                        float angle = Static.GetAngleForStraight(tileDirection);

        //                                        GameObject newRailGO = CreateRail(RailType.Straight, currentTilePos, tile.w, tile.h, angle);
        //                                        BaseObject newRail = newRailGO.GetComponent<BaseObject>();

        //                                        switch (lastRail.m_Tile.LinkCount) {
        //                                            case 0: {
        //                                                    lastRail.SetTileDirection(tileDirection);
        //                                                    lastRail.SetRotation(angle);
        //                                                    newRail.Init(tileDirection);
        //                                                    newRail.LinkTile(lastRail.m_Tile);
        //                                                }
        //                                                break;
        //                                            case 1: {
        //                                                    List<TileDirection> outputs = lastTile.Outputs;
        //                                                    if (!outputs.Contains(tileDirection)) {
        //                                                        GameObject replaceRailGO = ReplaceNewRail(RailType.Turn, lastRail);
        //                                                        List<TileDirection> linkedOutputs = lastTile.GetLinkedOutputDirections();
        //                                                        Vector3 v3 = v1 - v2;
        //                                                        Vector3 v4 = Static.GetVector(linkedOutputs[0]);
        //                                                        TileDirection turnDirection = Static.GetDirectionForTurn(-v4, v3);
        //                                                        angle = Static.GetAngleForTurn(turnDirection);
        //                                                        BaseObject replaceRail = replaceRailGO.GetComponent<BaseObject>();
        //                                                        replaceRail.Init(turnDirection);
        //                                                        replaceRail.SetRotation(angle);

        //                                                        TileDirection newRailDirection = Static.GetStraightDirectionOutTurn(turnDirection);
        //                                                        angle = Static.GetAngleForStraight(newRailDirection);
        //                                                        newRail.Init(newRailDirection);
        //                                                        newRail.SetRotation(angle);
        //                                                        newRail.LinkTile(replaceRail.m_Tile);
        //                                                    } else {
        //                                                        newRail.SetTileDirection(tileDirection);
        //                                                        newRail.SetRotation(angle);
        //                                                        newRail.LinkTile(lastRail.m_Tile);
        //                                                    }
        //                                                }
        //                                                break;
        //                                            case 2: {
        //                                                    List<TileDirection> outputs = lastTile.Outputs;
        //                                                    if (!outputs.Contains(tileDirection)) {
        //                                                        float num = Static.GetAngleForSwitchTurn(RailType.SwitchRight, tileDirection, outputs[0], outputs[1]);
        //                                                        GameObject replaceRail = ReplaceNewRail(RailType.SwitchRight, lastRail, num);
        //                                                        BaseObject replaceBase = replaceRail.GetComponent<BaseObject>();
        //                                                        replaceBase.Init(TileDirection.SwitchRight, tileDirection, outputs[0], outputs[1]);
        //                                                        newRail.LinkTile(lastTile);
        //                                                    }
        //                                                }
        //                                                break;
        //                                        }
        //                                        //Debug.Log(tileDirection + " Link Count " + linkCount + " num " + num + " angle " + angle);
        //                                        if(newRail != null) {
        //                                            newRail.AutoMatching(dragDirection);
        //                                        }
        //                                        m_LastRailGO = newRailGO;
        //                                    }
        //                                    break;
        //                                case RailType.SwitchLeft: {
        //                                        float angle = Static.GetAngleForStraight(TileDirection.Left);
        //                                        GameObject newRailGO = CreateRail(RailType.Straight, currentTilePos, tile.w, tile.h, angle);
        //                                        BaseObject newRail = newRailGO.GetComponent<BaseObject>();
        //                                        newRail.LinkTile(lastRail.m_Tile);
        //                                        newRail.AutoMatching((currentTilePos - lastTilePos).normalized);
        //                                    }
        //                                    break;
        //                                case RailType.SwitchRight: {
        //                                        GameObject newRailGO = CreateRail(RailType.Straight, currentTilePos, tile.w, tile.h);
        //                                        float angle = Static.GetAngleForStraight(TileDirection.Left);
        //                                        DestructibleRail newRail = newRailGO.GetComponent<DestructibleRail>();
        //                                        newRail.SetRotation(new Vector3(0, angle, 0));
        //                                        newRail.LinkTile(lastRail.m_Tile);
        //                                        newRail.AutoMatching(dragDirection);
        //                                    }
        //                                    break;
        //                                case RailType.Turn: {
        //                                        if(lastTile.LinkCount == 1) {
        //                                            GameObject newRailGO = CreateRail(RailType.Straight, currentTilePos, tile.w, tile.h);
        //                                            Vector3 v1 = currentTilePos;
        //                                            Vector3 v2 = lastTilePos;

        //                                            TileDirection tileDirection = Static.GetTileDirection(v1, v2);
        //                                            Debug.Log("Direct " + tileDirection);
        //                                            List<TileDirection> linkedTileDirections = lastTile.GetLinkedOutputDirections();
        //                                            Vector3 v3 = Static.GetVector(linkedTileDirections[0]);
        //                                            if(Utilss.IsVectorParallel(v3, (v1 - v2))){
        //                                                float angle = Static.GetAngleForStraight(tileDirection);
        //                                                GameObject replaceRailGO = ReplaceNewRail(RailType.Straight, lastRail, angle);
        //                                                BaseObject replaceRail = replaceRailGO.GetComponent<BaseObject>();
        //                                                replaceRail.LinkTile(lastRail.m_Tile);
        //                                                replaceRail.AutoMatching(dragDirection);
        //                                                lastTile = replaceRail.m_Tile;
        //                                                lastRail = replaceRail;
        //                                            } else {
        //                                                float angleTurn = Static.GetAngleForTurn(tileDirection);
        //                                                lastRail.SetTileDirection(tileDirection);
        //                                                lastRail.SetRotation(angleTurn);
        //                                            }

        //                                            float angleStraight = Static.GetAngleForStraight(tileDirection);
        //                                            DestructibleRail newRail = newRailGO.GetComponent<DestructibleRail>();
        //                                            newRail.SetRotation(angleStraight);
        //                                            newRail.LinkTile(lastRail.m_Tile);
        //                                            newRail.AutoMatching(dragDirection);
        //                                            m_LastRailGO = newRailGO;
        //                                        } else if (lastTile.LinkCount == 2) {
        //                                            Vector3 v = currentTilePos - lastTilePos;
        //                                            TileDirection t = Static.GetTileDirection(v);
        //                                            List<TileDirection> outputDirections = lastTile.GetLinkedOutputDirections();
        //                                            if (!outputDirections.Contains(t)) {
        //                                                float angle = Static.GetAngleForSwitchTurn(RailType.SwitchRight, t, outputDirections[0], outputDirections[1]);
        //                                                GameObject replaceRail = ReplaceNewRail(RailType.SwitchRight, lastRail, angle);

        //                                                BaseObject replaceBase = replaceRail.GetComponent<BaseObject>();
        //                                                replaceBase.Init(TileDirection.SwitchRight, t, outputDirections[0], outputDirections[1]);
        //                                                replaceBase.AutoMatching(dragDirection);

        //                                                GameObject newRailGO = CreateRail(RailType.Straight, currentTilePos, tile.w, tile.h);
        //                                                BaseObject newRail = newRailGO.GetComponent<BaseObject>();
        //                                                newRail.SetTileDirection(t);
        //                                                newRail.LinkTile(lastRail.m_Tile);
        //                                                newRail.AutoMatching(dragDirection);
                                                        
        //                                                m_LastRailGO = newRailGO;
        //                                            }

        //                                        }
        //                                    }
        //                                    break;
        //                            }
        //                            m_LastDirectionVector = currentTilePos - lastTilePos;
        //                        }
        //                    }
                                
        //                }
        //            }
        //        }
        //    }
        //}
        //if (Input.GetMouseButtonUp(0)) {
        //    IsDragging = false;
        //}
    }

    private void HandleInputDelete() {
        //if (m_InputType != InputType.DELETE) return;
        //if (Input.GetMouseButtonDown(0)) {
        //    IsDragging = true;
        //}
        //if (IsDragging) {
        //    Ray interactRay = m_MainCamera.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hitMapObject;
        //    {
        //        if (Physics.Raycast(interactRay, out hitMapObject, 100, m_LayerMaskMapObject)) {
        //            GameObject go = hitMapObject.transform.gameObject;
        //            if (go != null) {
        //                Debug.Log("hit :" + go.gameObject.name);
        //                DestructibleRail rail = go.GetComponent<DestructibleRail>();
        //                int w = rail.m_Tile.w;
        //                int h = rail.m_Tile.h;
        //                if (rail != null) {
        //                    rail.ClearLinkTile();
        //                    if (m_CurrentRails.Contains(go)) {
        //                        LevelManager.Instance.RemoveBaseObject(w, h);
        //                        m_CurrentRails.Remove(go);
        //                        Destroy(go);
        //                    }
        //                    List<BaseObject> list = LevelManager.Instance.GetAllAround(w, h);
        //                    for (int i = 0; i < list.Count; i++) {
        //                        BaseObject bo = list[i];
        //                        if (bo != null) {
        //                            bo.ReMatch();
        //                        }
        //                    }
        //                }
        //            }
        //        } 
        //    }
        //}
        //if (Input.GetMouseButtonUp(0)) {
        //    IsDragging = false;
        //    LevelManager.Instance.UpdateAround();
        //}
    }
    
    public GameObject GetLastRail() {
        if (m_CurrentRails.Count > 0) {
            return m_CurrentRails[m_CurrentRails.Count - 1];
        }
        return null;
    }
    public void LinkRail(DestructibleRail first, DestructibleRail second) {
        
    }
    private GameObject CreateRail(GameObject prefab, Vector3 pos, int w, int h, float angle = 0) {
        Quaternion newQuaternion = new Quaternion();
        newQuaternion.Set(0, angle, 0, 1);
        GameObject newRailGO = Instantiate(prefab, pos, Quaternion.identity);
        newRailGO.name = prefab.name + "_" + w + "_" + h;
        BaseObject baseObject = newRailGO.GetComponent<BaseObject>();
        baseObject.UpdateTileOwner();
        baseObject.SetRotation(new Vector3(0, angle, 0));
        baseObject.m_Tile.Setup(w, h);
        LevelManager.Instance.AddBaseObject(baseObject, w, h);
        return newRailGO;
    }
    private GameObject CreateRail(RailType desType, Vector3 pos, int w, int h, float angle = 0, bool addRow = true) {
        GameObject go = CreateRail(m_RailPrefabs[(int)desType], pos, w, h, angle);
        Debug.Log("Create " + go.name);
        if (addRow) {
            AddRailToList(go); 
        }
        return go;
    }
    private GameObject CreateRail1(RailType desType, Vector3 pos, int w, int h, float angle = 0, bool addRow = true) {
        GameObject go = CreateRail(m_RailPrefabs[(int)desType], pos, w, h, angle);
        if (addRow) {
            AddRailToList(go);
        }
        return go;
    }
    public void AddRailToList(GameObject go) {
        if (!m_CurrentRails.Contains(go)) {
            m_CurrentRails.Add(go);
        }
    }
    public GameObject ReplaceNewRail(RailType desRailType, BaseObject oldDesRail, float angle = 0, TileDirection tileDirection = TileDirection.None) {

        //GameObject go = CreateRail1(desRailType, oldDesRail.transform.position, oldDesRail.m_Tile.w, oldDesRail.m_Tile.h, angle,false);
        //DestructibleRail newDesRail = go.GetComponent<DestructibleRail>();
        //newDesRail.m_Tile = oldDesRail.m_Tile;
        //newDesRail.UpdateTileOwner();
        //if (tileDirection != TileDirection.None) {
        //    newDesRail.SetTileDirection(tileDirection);
        //}

        //newDesRail.m_Tile.UpdateOutputDirection();
        //Debug.Log("Replace " + oldDesRail.name + " = " + newDesRail.name);
        //int num = m_CurrentRails.IndexOf(oldDesRail.gameObject);
        //m_CurrentRails[num] = go;
        //Destroy(oldDesRail.gameObject);
        //return go;
        return null;
    }
    private GameObject GetNextRailPrefab(int id) {
        id++;
        if (id == m_RailPrefabs.Count) {
            id = 0;
        }
        return m_RailPrefabs[id];
    }
    [Button]
    private void ClearMap() {
        while (m_CurrentRails.Count > 0) { 
            GameObject go = m_CurrentRails[0];
            Destroy(go);
            m_CurrentRails.RemoveAt(0);
        }
    }
}
public enum TileDirection {
    None, Up, Down, Left, Right, 
    CornerLeftUp, CornerRightUp, CornerLeftDown, CornerRightDown, 
    CornerUpLeft, CornerUpRight, CornerDownLeft, CornerDownRight,
    SwitchLeft, SwitchRight,
}
