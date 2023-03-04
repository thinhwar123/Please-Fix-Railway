using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchTurn_Straight : TileMatching {
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

        Vector3 dragVector = currentTilePos - lastTilePos;
        TileDirection dragDirection = Static.GetTileDirection(dragVector);
        if (currentRail.IsDestructable()) {
            switch (currentTile.LinkCount) {
                case 0: {
                        Vector3 v1 = currentTile.GetTilePos() - lastTile.GetTilePos();
                        TileDirection newDirection = Static.GetTileDirection(v1);
                        currentRail.SetTileDirection(newDirection);
                        currentRail.LinkTile(lastTile);
                    }
                    break;
                case 1: {
                        if (!Static.IsDirectionParallel(dragDirection, currentTile.GetLinkedOutputDirections()[0])) {
                            TileDirection inDirect = Static.GetTileDirection(currentTile.GetTilePos(), lastTile.GetTilePos());
                            TileDirection outDirect = Static.GetTileDirection(currentTile.linkTiles[0].GetTilePos(), currentTile.GetTilePos());
                            TileDirection newDirection = Static.GetTileDirectionTurn(inDirect, outDirect);

                            GameObject replaceRail = IngameManager.Instance.ReplaceNewRail(RailType.Turn, currentRail);
                            BaseObject replaceBase = replaceRail.GetComponent<BaseObject>();
                            replaceBase.SetTileDirection(newDirection);
                            replaceBase.LinkTile(lastTile);
                            currentRail = replaceBase;
                        }
                    }
                    break;
                case 2: {
                        Vector3 v = currentTilePos - lastTilePos;
                        TileDirection t = Static.GetTileDirection(-v);
                        List<TileDirection> outputDirections = currentTile.GetLinkedOutputDirections();
                        if (!outputDirections.Contains(t)) {
                            GameObject replaceRail = IngameManager.Instance.ReplaceNewRail(RailType.SwitchRight, currentRail);
                            TileDirection[] tileDirections = new TileDirection[3] { t, outputDirections[0], outputDirections[1] };
                            BaseObject replaceBase = replaceRail.GetComponent<BaseObject>();
                            replaceBase.SetTileDirection(tileDirections);
                            replaceBase.LinkTile(lastTile);
                        }
                    }
                    break;
            }
        } else {
            if (currentRail.IsConnectable()) {
                switch (lastTile.LinkCount) {
                    case 0: {
                            GameObject replaceRail = IngameManager.Instance.ReplaceNewRail(RailType.Straight, lastRail);
                            BaseObject replaceBase = replaceRail.GetComponent<BaseObject>();
                            replaceBase.SetTileDirection(dragDirection);
                        }
                        break;
                    case 1: {
                            TileDirection t = lastTile.GetLinkedOutputDirections()[0];
                            if (Utilss.IsVectorParallel(dragVector, Static.GetVector(t))) {
                                GameObject replaceRail = IngameManager.Instance.ReplaceNewRail(RailType.Straight, lastRail);
                                BaseObject replaceBase = replaceRail.GetComponent<BaseObject>();
                                replaceBase.SetTileDirection(dragDirection);
                            } else {
                                lastRail.SetTileDirection(Static.GetReverseDirection(t),dragDirection);
                            }
                        }
                        break;
                    case 2: {
                            List<TileDirection> outputDirections = lastTile.GetLinkedOutputDirections();
                            if (!outputDirections.Contains(dragDirection)) {
                                GameObject replaceRail = IngameManager.Instance.ReplaceNewRail(RailType.SwitchRight, lastRail);
                                TileDirection[] tileDirections = new TileDirection[3] { dragDirection, outputDirections[0], outputDirections[1] };
                                BaseObject replaceBase = replaceRail.GetComponent<BaseObject>();
                                replaceBase.SetTileDirection(tileDirections);
                            }
                        }
                        break;
                }
            }
        }
        
    }
}