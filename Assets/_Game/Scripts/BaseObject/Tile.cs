using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile {
    public BaseObject owner;
    public List<Tile> linkTiles;
    [SerializeField]
    private TileDirection tileDirection;
    [SerializeField]
    private List<TileDirection> outputDirections = new List<TileDirection>();

    public int w;
    public int h;
    public int c;
    public void Setup(int w, int h) {
        this.w = w;
        this.h = h;
        UpdateOutputDirection();
    }
    public void Link(Tile tile) {
        if (!linkTiles.Contains(tile)) {
            linkTiles.Add(tile);
            UpdateLinkCount();
        }
        if (!tile.linkTiles.Contains(this)) {
            tile.linkTiles.Add(this);
            tile.UpdateLinkCount();
        }
        c = LinkCount;
    }
    public void ClearLink() {
        for (int i = 0; i < linkTiles.Count; i++) {
            Tile tile = linkTiles[i];
            if (tile.linkTiles.Contains(this)) {
                tile.linkTiles.Remove(this);
                tile.UpdateLinkCount();
            }
        }
        linkTiles.Clear();
        UpdateLinkCount();
    }
    public void UnLink(Tile tile) {
        if (linkTiles.Contains(tile)) {
            linkTiles.Remove(tile);
            UpdateLinkCount();
        }
        if (tile.linkTiles.Contains(this)) {
            tile.linkTiles.Remove(this);
            tile.UpdateLinkCount();
        }
    }
    public bool IsLinkedWith(Tile tile) {
        return linkTiles.Contains(tile);
    }
    public int LinkCount {
        get {
            return linkTiles.Count;
        }
    }
    public List<TileDirection> Outputs {
        get {
            return new List<TileDirection>(outputDirections);
        }
    }

    public TileDirection TileDirection {
        get => tileDirection;
        set {
            tileDirection = value;
            UpdateOutputDirection();
        }
    }
    public void UpdateOutputDirection() {
        outputDirections.Clear();
        switch (tileDirection) {
            case TileDirection.Up:
            case TileDirection.Down: {
                    outputDirections.Add(TileDirection.Down);
                    outputDirections.Add(TileDirection.Up);
                }
                break;
            case TileDirection.Left:
            case TileDirection.Right: {
                    outputDirections.Add(TileDirection.Left);
                    outputDirections.Add(TileDirection.Right);
                }
                break;
            case TileDirection.CornerDownLeft:
            case TileDirection.CornerRightUp: {
                    outputDirections.Add(TileDirection.Up);
                    outputDirections.Add(TileDirection.Left);
                }
                break;
            case TileDirection.CornerUpRight:
            case TileDirection.CornerLeftDown: {
                    outputDirections.Add(TileDirection.Right);
                    outputDirections.Add(TileDirection.Down);
                }
                break;
            case TileDirection.CornerRightDown:
            case TileDirection.CornerUpLeft: {
                    outputDirections.Add(TileDirection.Left);
                    outputDirections.Add(TileDirection.Down);
                }
                break;
            case TileDirection.CornerDownRight:
            case TileDirection.CornerLeftUp: {
                    outputDirections.Add(TileDirection.Right);
                    outputDirections.Add(TileDirection.Up);
                }
                break;
            case TileDirection.SwitchLeft:
            case TileDirection.SwitchRight: {
                    float angle = owner.transform.localEulerAngles.y;
                    if (angle < 0) angle += 360;
                    if (owner.DesRailType == RailType.SwitchLeft) {
                        if (Utilss.CloseEnoughForMe(angle, 270, 0.001f)) {
                            SetOutDirection(TileDirection.Right, TileDirection.Up, TileDirection.Down);
                        } else if (Utilss.CloseEnoughForMe(angle, 90, 0.001f)) {
                            SetOutDirection(TileDirection.Left, TileDirection.Up, TileDirection.Down);
                        } else if (Utilss.CloseEnoughForMe(angle, 0, 0.001f)) {
                            SetOutDirection(TileDirection.Down, TileDirection.Left, TileDirection.Right);
                        } else if (Utilss.CloseEnoughForMe(angle, 180, 0.001f)) {
                            SetOutDirection(TileDirection.Up, TileDirection.Left, TileDirection.Right);
                        }
                    } else {
                        if (Utilss.CloseEnoughForMe(angle, 270, 0.001f)) {
                            SetOutDirection(TileDirection.Left, TileDirection.Up, TileDirection.Down);
                        } else if (Utilss.CloseEnoughForMe(angle, 90, 0.001f)) {
                            SetOutDirection(TileDirection.Right, TileDirection.Up, TileDirection.Down);
                        } else if (Utilss.CloseEnoughForMe(angle, 0, 0.001f)) {
                            SetOutDirection(TileDirection.Up, TileDirection.Left, TileDirection.Right);
                        } else if (Utilss.CloseEnoughForMe(angle, 180, 0.001f)) {
                            SetOutDirection(TileDirection.Down, TileDirection.Left, TileDirection.Right);
                        }
                    }
                    //Debug.Log("Tile " + w + h +  "Angle " + angle + " -- " + outputDirections[0] + "--" + outputDirections[1] + "--" + outputDirections[2]);
                }
                break;
        }
        //string s = "";
        //for(int i = 0; i < outputDirections.Count; i++) {
        //    s += outputDirections[i] + " --- ";
        //}
        //Debug.Log("Name " + owner.name + " tile "+ tileDirection +" output "+ s);
    }
    public List<TileDirection> GetLinkedOutputDirections() {
        List<TileDirection> linkedOutputs = new List<TileDirection>();
        for (int i = 0; i < linkTiles.Count; i++) {
            Tile tile = linkTiles[i];
            Vector3 v = tile.GetTilePos() - GetTilePos();
            TileDirection t = Static.GetTileDirection(v);
            linkedOutputs.Add(t);
        }
        return linkedOutputs;
    }
    public void SetOutDirection(params TileDirection[] values) {
        outputDirections.Clear();
        for (int i = 0; i < values.Length; i++) {
            outputDirections.Add(values[i]);
        }
    }
    public Vector3 GetTilePos() {
        return new Vector3(h, 0, w);
    }
    public void UpdateLinkCount() {
        c = LinkCount;
    }
    public bool IsAround(Tile t) {
        int w1 = t.w;
        int h1 = t.h;
        {
            int width = w - 1;
            int height = h;
            if (width == w1 && height == h1) {
                return true;
            }
        }
        {
            int width = w + 1;
            int height = h;
            if (width == w1 && height == h1) {
                return true;
            }
        }
        {
            int width = w;
            int height = h - 1;
            if (width == w1 && height == h1) {
                return true;
            }
        }
        {
            int width = w;
            int height = h + 1;
            if (width == w1 && height == h1) {
                return true;
            }
        }
        return false;
    }
}