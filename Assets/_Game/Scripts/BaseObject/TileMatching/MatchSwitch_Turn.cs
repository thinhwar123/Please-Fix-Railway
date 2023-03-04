using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSwitch_Turn : TileMatching {
    public override void Match(BaseObject lastGO, BaseObject currentGO) {
        BaseObject lastRail = lastGO;
        Vector3 lastTilePos = lastGO.transform.position;
        Tile lastTile = lastRail.m_Tile;
        RailType lastDesRailType = (RailType)lastRail.m_Index;
        TileDirection lastDirection = lastRail.TileDirection;

        BaseObject currentRail = currentGO;
        Vector3 currentTilePos = currentGO.transform.position;
        Tile currentTile = currentRail.m_Tile;
        RailType currentDesRailType = (RailType)currentRail.m_Index;
        TileDirection currentDirection = currentRail.TileDirection;

        Vector3 dragVector = (currentTilePos - lastTilePos).normalized;
        TileDirection dragDirection = Static.GetTileDirection(dragVector);

        if (lastRail.IsDestructable()) {
            List<TileDirection> outputDirections = lastTile.GetLinkedOutputDirections();
            if (!outputDirections.Contains(dragDirection)) {
                TileDirection t = Static.GetReverseDirection(dragDirection);
                outputDirections.Remove(t);
                lastRail.SetTileDirection(dragDirection, outputDirections[0], outputDirections[1]);
            } 
        }

        switch (currentTile.LinkCount) {
            case 0: {
                    GameObject go = IngameManager.Instance.ReplaceNewRail(RailType.Straight, currentRail);
                    BaseObject newRail = go.GetComponent<BaseObject>();
                    newRail.SetTileDirection(dragDirection);
                    currentRail = newRail;
                }
                break;
            case 1: {
                    TileDirection output = currentTile.GetLinkedOutputDirections()[0];
                    if(Static.IsDirectionParallel(output, dragDirection)) {
                        GameObject go = IngameManager.Instance.ReplaceNewRail(RailType.Straight, currentRail);
                        BaseObject newRail = go.GetComponent<BaseObject>();
                        newRail.SetTileDirection(dragDirection);
                        currentRail = newRail;
                    } else {
                        currentRail.SetTileDirection(dragDirection, output);
                    }
                }
                break;
            case 2: {
                    List<TileDirection> outputs = currentTile.GetLinkedOutputDirections();
                    TileDirection t = Static.GetReverseDirection(dragDirection);
                    if (!outputs.Contains(t)) {
                        outputs.Add(t);
                        GameObject go = IngameManager.Instance.ReplaceNewRail(RailType.SwitchRight, currentRail);
                        BaseObject newRail = go.GetComponent<BaseObject>();
                        newRail.SetTileDirection(outputs.ToArray());
                        currentRail = newRail;
                    }
                }
                break;
        }
        currentRail.LinkTile(lastTile);
    }
}