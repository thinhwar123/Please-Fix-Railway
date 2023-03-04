using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MatchStraight_Turn : TileMatching {
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


        //Debug.Log("Turn " + currentTile.LinkCount + " L " + lastTile.LinkCount + " ---- " + lastDirection + " ---- " + currentDirection);
        if (lastTile.LinkCount == 1 && lastDirection != currentDirection) {
            TileDirection outputDirection = lastTile.GetLinkedOutputDirections()[0];
            if (Utilss.IsVectorParallel(currentTilePos - lastTilePos, Static.GetVector(outputDirection))) {
                lastRail.SetTileDirection(Static.GetTileDirection(currentTilePos - lastTilePos));
            } else {
                TileDirection inDirect = Static.GetReverseDirection(outputDirection);
                TileDirection outDirect = Static.GetTileDirection(currentTile.GetTilePos(), lastTile.GetTilePos());
                TileDirection newDirection = Static.GetTileDirectionTurn(inDirect, outDirect);

                if (lastRail.IsDestructable()) {
                    GameObject replaceRail = IngameManager.Instance.ReplaceNewRail(RailType.Turn, lastRail);
                    BaseObject replaceBase = replaceRail.GetComponent<BaseObject>();
                    replaceBase.SetTileDirection(newDirection);
                    lastRail = replaceBase;
                }
            }
        }
        switch (currentTile.LinkCount) {
            case 0: {
                    if (currentRail.IsDestructable()) {
                        Vector3 v1 = currentTile.GetTilePos() - lastTile.GetTilePos();
                        Vector3 v2 = Quaternion.AngleAxis(90, Vector3.up) * v1;
                        TileDirection newDirection = Static.GetDirectionForTurn(v1, v2);
                        currentRail.SetTileDirection(newDirection);
                        currentRail.LinkTile(lastTile);
                    }
                }
                break;
            case 1: {
                    if (currentRail.IsDestructable()) {
                        TileDirection inDirect = Static.GetTileDirection(currentTile.GetTilePos(), lastTile.GetTilePos());
                        TileDirection outDirect = Static.GetTileDirection(currentTile.linkTiles[0].GetTilePos(), currentTile.GetTilePos());
                        TileDirection newDirection = Static.GetTileDirectionTurn(inDirect, outDirect);

                        GameObject replaceRail = IngameManager.Instance.ReplaceNewRail(RailType.Turn, currentRail);
                        BaseObject replaceBase = replaceRail.GetComponent<BaseObject>();
                        replaceBase.SetTileDirection( newDirection);
                        replaceBase.LinkTile(lastTile);
                        currentRail = replaceBase; 
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