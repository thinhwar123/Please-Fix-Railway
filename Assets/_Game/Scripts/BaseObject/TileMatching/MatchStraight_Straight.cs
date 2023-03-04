using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchStraight_Straight : TileMatching {
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

        bool isHasLinked = false;
        Vector3 dragVector = currentTilePos - lastTilePos;
        //Debug.Log("Straight " + lastDirection + " --- " + currentDirection);
        if (currentRail.IsDestructable()) {
            if (currentTile.LinkCount == 0) {
                currentRail.SetTileDirection(dragDirection);
                isHasLinked = true;
            } else if (currentTile.LinkCount == 1) {
                if (!currentTile.Outputs.Contains(dragDirection)) {
                    TileDirection inDirect = Static.GetTileDirection(currentTile.GetTilePos(), lastTile.GetTilePos());
                    TileDirection outDirect = Static.GetTileDirection(currentTile.linkTiles[0].GetTilePos(), currentTile.GetTilePos());
                    TileDirection newDirection = Static.GetTileDirectionTurn(inDirect, outDirect);
                    //Debug.Log("------In " + inDirect + " out " + outDirect + " new " + newDirection);
                    GameObject replaceRail = IngameManager.Instance.ReplaceNewRail(RailType.Turn, currentRail);
                    BaseObject replaceBase = replaceRail.GetComponent<BaseObject>();
                    replaceBase.SetTileDirection(newDirection);
                    currentRail = replaceBase;
                    isHasLinked = true;
                }
            } else if (currentTile.LinkCount == 2) {
                Vector3 v = lastTilePos - currentTilePos;
                TileDirection t = Static.GetTileDirection(v);
                List<TileDirection> outputDirections = currentTile.GetLinkedOutputDirections();
                if (!outputDirections.Contains(t)) {
                    GameObject replaceRailGO = IngameManager.Instance.ReplaceNewRail(RailType.SwitchRight, currentRail);
                    BaseObject replaceRail = replaceRailGO.GetComponent<BaseObject>();
                    TileDirection[] tileDirections = new TileDirection[3] { t, outputDirections[0], outputDirections[1] };
                    replaceRail.SetTileDirection(tileDirections);
                    currentRail = replaceRail;
                    isHasLinked = true;
                }
            }
        }


        List<TileDirection> linkedOutputs = lastTile.GetLinkedOutputDirections();
        if (lastTile.LinkCount == 1 && !lastTile.Outputs.Contains(dragDirection)) {
            TileDirection inDirect = Static.GetReverseDirection(lastTile.GetLinkedOutputDirections()[0]);
            TileDirection outDirect = Static.GetTileDirection(currentTile.GetTilePos(), lastTile.GetTilePos());
            TileDirection newDirection = Static.GetTileDirectionTurn(inDirect, outDirect);

            if (lastGO.IsDestructable()) {
                GameObject replaceRailGO = IngameManager.Instance.ReplaceNewRail(RailType.Turn, lastRail);
                BaseObject replaceBase = replaceRailGO.GetComponent<BaseObject>();
                replaceBase.SetTileDirection(newDirection);
                lastRail = replaceBase;
            }
        } else if (lastTile.LinkCount == 2 && !linkedOutputs.Contains(dragDirection)) {
            Vector3 v = currentTilePos - lastTilePos;
            TileDirection t = Static.GetTileDirection(v);
            List<TileDirection> outputDirections = lastTile.GetLinkedOutputDirections();
            GameObject replaceRailGO = IngameManager.Instance.ReplaceNewRail(RailType.SwitchRight, lastRail);
            BaseObject replaceRail = replaceRailGO.GetComponent<BaseObject>();
            replaceRail.TileDirection = TileDirection.SwitchRight;
            TileDirection[] tileDirections = new TileDirection[3] { t, outputDirections[0], outputDirections[1] };
            replaceRail.SetTileDirection(tileDirections);
            lastRail = replaceRail;
            isHasLinked = true;
        }
        if (isHasLinked) {
            lastRail.LinkTile(currentRail.m_Tile); 
        }
    }
}
