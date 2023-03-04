using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchTurn_Turn : TileMatching {
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

        switch (currentTile.LinkCount) {
            case 0: {
                    GameObject replaceRailGO = IngameManager.Instance.ReplaceNewRail(RailType.Straight, currentRail);
                    Vector3 v1 = currentTile.GetTilePos() - lastTile.GetTilePos();
                    TileDirection newDirection = Static.GetTileDirection(v1);
                    BaseObject replaceRail = replaceRailGO.GetComponent<BaseObject>();
                    replaceRail.SetTileDirection(newDirection);
                    replaceRail.LinkTile(lastTile);
                }
                break;
            case 1: {
                    if (!currentRail.IsDestructable()) break;
                    Vector3 v = currentTilePos - lastTilePos;
                    TileDirection t = Static.GetTileDirection(-v);
                    List<TileDirection> outputDirections = currentTile.GetLinkedOutputDirections();
                    Vector3 v1 = Static.GetVector(outputDirections[0]);
                    if (Utilss.IsVectorParallel(v, v1)) {
                        GameObject replaceRailGO = IngameManager.Instance.ReplaceNewRail(RailType.Straight, currentRail);
                        Vector3 v2 = currentTile.GetTilePos() - lastTile.GetTilePos();
                        TileDirection newDirection = Static.GetTileDirection(v2);
                        BaseObject replaceRail = replaceRailGO.GetComponent<BaseObject>();
                        replaceRail.SetTileDirection(newDirection);
                        replaceRail.LinkTile(lastTile);
                    } else {
                        Vector3 v2 = Static.GetVector(outputDirections[0]);
                        TileDirection newDirection = Static.GetTileDirectionTurn(v, v2);
                        currentRail.SetTileDirection(newDirection);
                        currentRail.LinkTile(lastTile);
                    }
                }
                break;
            case 2: {
                    Vector3 v = currentTilePos - lastTilePos;
                    TileDirection t = Static.GetTileDirection(-v);
                    List<TileDirection> outputDirections = currentTile.GetLinkedOutputDirections();
                    if (!outputDirections.Contains(t) && currentRail.IsDestructable()) {
                        GameObject replaceRail = IngameManager.Instance.ReplaceNewRail(RailType.SwitchRight, currentRail);
                        BaseObject replaceBase = replaceRail.GetComponent<BaseObject>();
                        TileDirection[] tileDirections = new TileDirection[3] { t, outputDirections[0], outputDirections[1] };
                        replaceBase.SetTileDirection(tileDirections);
                        replaceBase.LinkTile(lastTile);
                    }
                }
                break;

        }
    }
}