using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSwitch_Straight : TileMatching {
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

        switch (currentTile.LinkCount) {
            case 0: {
                    float angle = Static.GetAngleForStraight(dragVector);
                    TileDirection tileDirection = Static.GetTileDirection(angle);
                    currentRail.SetTileDirection(tileDirection);
                    if (lastRail.IsDestructable()) {
                        List<TileDirection> outputDirections = lastTile.GetLinkedOutputDirections();
                        if (!outputDirections.Contains(dragDirection)) {
                            TileDirection t = Static.GetReverseDirection(dragDirection);
                            outputDirections.Remove(t);
                            lastRail.SetTileDirection(dragDirection, outputDirections[0], outputDirections[1]);
                        }
                    }
                }
                break;
            case 1: {
                    List<TileDirection> linkedDirections = currentTile.GetLinkedOutputDirections();
                    Vector3 v1 = Static.GetVector(linkedDirections[0]);
                    if(!Utilss.IsVectorParallel(v1, dragVector)) {
                        TileDirection turnDirection = Static.GetDirectionForTurn(dragVector, v1);
                        currentRail = IngameManager.Instance.ReplaceNewRail(RailType.Turn, currentRail).GetComponent<BaseObject>();
                        currentRail.SetTileDirection(turnDirection);
                    }
                    if (lastRail.IsDestructable()) {
                        lastRail.SetTileDirection(dragDirection);
                    }
                }
                break;
            case 2: {
                    List<TileDirection> linkedDirections = currentTile.GetLinkedOutputDirections();
                    TileDirection t = Static.GetTileDirection(-dragVector);
                    linkedDirections.Add(t);
                    currentRail = IngameManager.Instance.ReplaceNewRail(RailType.SwitchRight, currentRail).GetComponent<BaseObject>();
                    currentRail.SetTileDirection(linkedDirections.ToArray());

                    if (lastRail.IsDestructable()) {
                        List<TileDirection> tileDirections = Static.GetSwitchOtherTileDirections(dragDirection);
                        lastRail.SetTileDirection(dragDirection, tileDirections[0], tileDirections[1]); 
                    }
                }
                break;
        }
        currentRail.LinkTile(lastTile);

    }
}