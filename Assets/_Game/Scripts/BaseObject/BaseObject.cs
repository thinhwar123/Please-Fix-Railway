using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    public EntityType m_BaseObjectTileType;
    
    public int m_Index;
    public int m_ObjectID;
    public Tile m_Tile = new Tile();
    public Vector3 m_Position;
    public BaseObject m_Left;
    public BaseObject m_Right;
    public BaseObject m_Up;
    public BaseObject m_Down;

    public bool IsUpdatedAround;
    public TileDirection TileDirection {
        get {
            return m_Tile.TileDirection;
        }
        set {
            m_Tile.TileDirection = value;
            UpdateAround();
        }
    }
    public virtual void Init(TileDirection tileDirection, params TileDirection[] values) {
        TileDirection = tileDirection;
        if (values.Length > 0) {
            m_Tile.SetOutDirection(values);
        }
    }
    public virtual void OnObjectIDChange()
    {

    }
    private UIEditEntity m_CurrentUIEditBaseObject;
    private List<EntityType> m_CanEditTiles 
    { 
        get 
        { 
            return new List<EntityType>() 
            { 
                EntityType.Tunnel,
                EntityType.DynamicRail,
                EntityType.CarRail,
                EntityType.PressureRail,
                EntityType.SpecialRail,
            }; 
        } 
    }
    public void EditSelectTile()
    {
        if (m_CurrentUIEditBaseObject != null) return;
        if (!m_CanEditTiles.Contains(m_BaseObjectTileType)) return;
        m_CurrentUIEditBaseObject = Instantiate<UIEditEntity>(LevelCreator.Instance.m_UIEditBaseObject, LevelCreator.Instance.m_MainCanvas);
        //m_CurrentUIEditBaseObject.Setup(this, OnClickSaveCallback);
    }
    private void OnClickSaveCallback()
    {
        Destroy(m_CurrentUIEditBaseObject.gameObject);
        m_CurrentUIEditBaseObject = null;

    }

    public void LinkTile(Tile tile) {
        Debug.Log("Link");
        m_Tile.Link(tile);
    }
    public int LinkCount {
        get {
            return m_Tile.LinkCount;
        }
    }
    public void UnlinkTile(Tile tile) {
        m_Tile.UnLink(tile);
    }
    public void UpdateTileOwner() {
        m_Tile.owner = this;
    }
    public void ClearLinkTile() {
        m_Tile.ClearLink();
    }
    public void SetRotation(Vector3 rotation) {
        if (m_BaseObjectTileType == EntityType.DestructionRail) {
            transform.localEulerAngles = rotation;
        }
    }
    public void SetRotation(float angle) {
        Vector3 v = new Vector3(0, angle, 0);
        SetRotation(v);
    }
    public bool IsDestructable() {
        return m_BaseObjectTileType == EntityType.DestructionRail;
    }
    public bool IsConnectable() {
        return m_BaseObjectTileType == EntityType.DestructionRail || m_BaseObjectTileType == EntityType.IndestructibleRail || m_BaseObjectTileType == EntityType.CarRail || m_BaseObjectTileType == EntityType.Locomotive;
    }
    public void UpdateAround() {
        List<BaseObject> aroundObjects = LevelManager.Instance.GetAllAround(m_Tile.w,m_Tile.h);
        
        m_Right = aroundObjects[0];
        m_Left = aroundObjects[1];
        m_Down = aroundObjects[2];
        m_Up = aroundObjects[3];
        IsUpdatedAround = true;
    }
    
    public RailType DesRailType {
        get {
            return (RailType)m_Index;
        }
    }
    public bool IsLinkedWith(BaseObject bo) {
        return m_Tile.IsLinkedWith(bo.m_Tile);
    }
    public virtual void ReMatch() { }
    public virtual void AutoMatching() {
        int x = m_Tile.w;
        int y = m_Tile.h;
        ClearLinkTile();
        for (int j = 0; j < m_Tile.Outputs.Count; j++) {
            TileDirection tileDirection = m_Tile.Outputs[j];
            BaseObject bo = LevelManager.Instance.GetNearBaseObject(x, y, tileDirection);
            if (bo != null && bo.IsConnectable()) {
                if (bo.m_Tile.Outputs.Contains(Static.GetReverseDirection(tileDirection))) {
                    bo.LinkTile(m_Tile);
                }
            }
        }
        UpdateAround();
    }
    public virtual void AutoMatching(Vector3 direction) { }
    public virtual void SetTileDirection(params TileDirection[] tileDirections) { }
    public virtual void UpdateOutput() { }
}
