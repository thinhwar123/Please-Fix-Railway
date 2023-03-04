using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class MatchSwitch_Switch : TileMatching {
    public override void Match(BaseObject lastGO, BaseObject currentGO) {
        BaseObject lastRail = lastGO;
        Vector3 lastTilePos = lastGO.transform.position;
        Tile lastTile = lastRail.m_Tile;
        RailType lastDesRailType = lastRail.DesRailType;
        TileDirection lastDirection = lastRail.TileDirection;
        EntityType lastBaseType = lastRail.m_BaseObjectTileType;

        BaseObject currentRail = currentGO;
        Vector3 currentTilePos = currentGO.transform.position;
        Tile currentTile = currentRail.m_Tile;
        RailType currentDesRailType = (RailType)currentRail.m_Index;
        TileDirection currentDirection = currentRail.TileDirection;
        EntityType currentBaseType = currentRail.m_BaseObjectTileType;

        TileDirection dragDirection = Static.GetTileDirection(currentTilePos - lastTilePos);
        TileDirection reverseDrag = Static.GetReverseDirection(dragDirection);
        List<TileDirection> last_linkedOutputs = lastTile.GetLinkedOutputDirections();
        if (lastRail.IsDestructable()) {
            if (currentRail.m_Tile.Outputs.Contains(reverseDrag)) {
                if (last_linkedOutputs.Contains(dragDirection)) {
                    if (dragDirection == TileDirection.Up || dragDirection == TileDirection.Down) {
                        if (last_linkedOutputs.Contains(TileDirection.Left)) {
                            lastRail.SetTileDirection(dragDirection, reverseDrag, TileDirection.Left);
                        } else if (last_linkedOutputs.Contains(TileDirection.Right)) {
                            lastRail.SetTileDirection(dragDirection, reverseDrag, TileDirection.Right);
                        }
                    } else if (dragDirection == TileDirection.Left || dragDirection == TileDirection.Right) {
                        if (last_linkedOutputs.Contains(TileDirection.Up)) {
                            lastRail.SetTileDirection(dragDirection, reverseDrag, TileDirection.Up);
                        } else if (last_linkedOutputs.Contains(TileDirection.Down)) {
                            lastRail.SetTileDirection(dragDirection, reverseDrag, TileDirection.Down);
                        }
                    }
                } else {
                    if (dragDirection == TileDirection.Up || dragDirection == TileDirection.Down) {
                        lastRail.SetTileDirection(dragDirection, TileDirection.Right, TileDirection.Left);
                    } else if (dragDirection == TileDirection.Left || dragDirection == TileDirection.Right) {
                        lastRail.SetTileDirection(dragDirection, TileDirection.Up, TileDirection.Down);
                    }
                }

                List<BaseObject> list = LevelManager.Instance.GetAllAround(lastRail.m_Tile.w, lastRail.m_Tile.h);
                for (int i = 0; i < list.Count; i++) {
                    BaseObject bo = list[i];
                    if (bo != null) {
                        bo.ReMatch();
                    }
                }
            } else {
                List<TileDirection> others1 = Static.GetSwitchOtherTileDirections(dragDirection);
                List<TileDirection> others2 = Static.GetSwitchOtherTileDirections(dragDirection);

                others1.Add(dragDirection);
                others2.Add(reverseDrag);

                currentRail.SetTileDirection(others2.ToArray());
                lastRail.SetTileDirection(others1.ToArray());
                currentRail.LinkTile(lastTile);
            }
        }
    }
}