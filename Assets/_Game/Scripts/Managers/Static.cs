using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class Static
{
    private static Vector3 m_VectorLeft = new Vector3(0, 0, 1);
    private static Vector3 m_VectorRight = new Vector3(0, 0, -1);
    private static Vector3 m_VectorUp = new Vector3(1, 0, 0);
    private static Vector3 m_VectorDown = new Vector3(-1, 0, 0);

    public static TileDirection GetTileDirection(Vector3 v3) {
        v3 = v3.normalized;
        if (v3 == m_VectorUp) {
            return TileDirection.Up;

        } else if (v3 == m_VectorDown) {
            return TileDirection.Down;

        } else if (v3 == m_VectorLeft) {
            return TileDirection.Left;

        } else if (v3 == m_VectorRight) {
            return TileDirection.Right;
        }
        return TileDirection.None;
    }
    public static TileDirection GetTileDirection(Vector3 v1, Vector3 v2) {
        Vector3 v3 = (v1 - v2).normalized;
        return GetTileDirection(v3);
    }
    public static TileDirection GetTileDirectionTurn(Vector3 v1, Vector3 v2) {
        TileDirection inDirection = GetTileDirection(v1);
        Vector3 v = (v1 + v2);
        if (v == m_VectorLeft + m_VectorDown) {
            if (inDirection == TileDirection.Left) {
                return TileDirection.CornerLeftDown;
            } else {
                return TileDirection.CornerDownLeft;
            }
        } else if (v == m_VectorLeft + m_VectorUp) {
            if (inDirection == TileDirection.Left) {
                return TileDirection.CornerLeftUp;
            } else {
                return TileDirection.CornerUpLeft;
            }
        } else if (v == m_VectorRight + m_VectorDown) {
            if (inDirection == TileDirection.Right) {
                return TileDirection.CornerRightDown;
            } else {
                return TileDirection.CornerDownRight;
            }
        } else if (v == m_VectorRight + m_VectorUp) {
            if (inDirection == TileDirection.Right) {
                return TileDirection.CornerRightUp;
            } else {
                return TileDirection.CornerUpRight;
            }
        } else {
            return inDirection;
        }
    }
    public static TileDirection GetTileDirectionTurn(TileDirection inDirection, TileDirection outDirection) {
        Vector3 v1 = GetVector(inDirection);
        Vector3 v2 = GetVector(outDirection);
        return GetTileDirectionTurn(v1, v2);
    }
    public static TileDirection GetDirectionForTurn(Vector3 v1, Vector3 v2) {
        TileDirection rDirection = TileDirection.None;
        if ((v1 == m_VectorRight && v2 == m_VectorUp)) {
            rDirection = TileDirection.CornerRightUp;
        } else if ((v1 == m_VectorDown && v2 == m_VectorLeft)) {
            rDirection = TileDirection.CornerDownLeft;

        } else if ((v1 == m_VectorLeft && v2 == m_VectorDown)) {
            rDirection = TileDirection.CornerLeftDown;
        } else if ((v1 == m_VectorUp && v2 == m_VectorRight)) {
            rDirection = TileDirection.CornerUpRight;

        } else if ((v1 == m_VectorRight && v2 == m_VectorDown)) {
            rDirection = TileDirection.CornerRightDown;
        } else if ((v1 == m_VectorUp && v2 == m_VectorLeft)) {
            rDirection = TileDirection.CornerUpLeft;

        } else if ((v1 == m_VectorLeft && v2 == m_VectorUp)) {
            rDirection = TileDirection.CornerLeftUp;
        } else if ((v1 == m_VectorDown && v2 == m_VectorRight)) {
            rDirection = TileDirection.CornerDownRight;
        }
        //Debug.Log("Direct Turn " + rDirection + " v1 " + v1 + " v2 " + v2);
        return rDirection;
    }
    public static TileDirection GetStraightDirectionOutTurn(TileDirection tileDirection) {
        switch (tileDirection) {
            case TileDirection.CornerDownRight:
            case TileDirection.CornerUpRight:
                return TileDirection.Right;
            case TileDirection.CornerRightDown:
            case TileDirection.CornerLeftDown:
                return TileDirection.Down;
            case TileDirection.CornerRightUp:
            case TileDirection.CornerLeftUp:
                return TileDirection.Up;
            case TileDirection.CornerDownLeft:
            case TileDirection.CornerUpLeft:
                return TileDirection.Left;
        }
        return TileDirection.None;
    }
    public static TileDirection GetReverseDirection(TileDirection tileDirection) {
        switch (tileDirection) {
            case TileDirection.Up:
                return TileDirection.Down;
            case TileDirection.Down:
                return TileDirection.Up;
            case TileDirection.Right:
                return TileDirection.Left;
            case TileDirection.Left:
                return TileDirection.Right;
        }
        return TileDirection.None;
    }

    public static float GetAngleForStraight(TileDirection tileDirection) {
        switch (tileDirection) {
            case TileDirection.Up:
                return 90;
            case TileDirection.Down:
                return 270;
            case TileDirection.Left:
                return 0;
            case TileDirection.Right:
                return 180;
        }
        return 0;
    }
    public static TileDirection GetTileDirection(float angle) {
        if (Utilss.CloseEnoughForMe(angle, 90, 0.001f)) {
            return TileDirection.Up;
        }else if(Utilss.CloseEnoughForMe(angle, 270, 0.001f)) {
            return TileDirection.Down;
        }else if (Utilss.CloseEnoughForMe(angle, 0, 0.001f)) {
            return TileDirection.Left;
        }else if (Utilss.CloseEnoughForMe(angle, 180, 0.001f)) {
            return TileDirection.Right;
        }

        switch (angle) {
            case 90:
                return TileDirection.Up;
            case 270:
                return TileDirection.Down;
            case 0:
                return TileDirection.Left;
            case 180:
                return TileDirection.Right;
        }
        return 0;
    }
    public static TileDirection GetTileDirection(RailType railType, float angle) {
        switch (railType) {
            case RailType.Straight:
                return GetTileDirection(angle);
            case RailType.Turn:
                return GetTileDirectionForTurn(angle);
            case RailType.SwitchLeft:
                return TileDirection.SwitchLeft;
            case RailType.SwitchRight:
                return TileDirection.SwitchRight;
        }
        return TileDirection.None;
    }
    public static TileDirection GetTileDirection(EntityType baseObjectTileType, RailType railType, float angle) {

        if(baseObjectTileType == EntityType.Locomotive) {
            angle += 90;
        }
        switch (railType) {
            case RailType.Straight:
                return GetTileDirection(angle);
            case RailType.Turn:
                return GetTileDirectionForTurn(angle);
            case RailType.SwitchLeft:
                return TileDirection.SwitchLeft;
            case RailType.SwitchRight:
                return TileDirection.SwitchRight;
        }
        return TileDirection.None;
    }
    public static float GetAngleForStraight(Vector3 direction) {
        TileDirection tileDirection = GetTileDirection(direction);
        return GetAngleForStraight(tileDirection);
    }
    public static float GetAngleForTurn(TileDirection tileDirection) {
        switch (tileDirection) {
            case TileDirection.CornerLeftUp:
            case TileDirection.CornerDownRight:
                return 180;
            case TileDirection.CornerLeftDown:
            case TileDirection.CornerUpRight:
                return -90;
            case TileDirection.CornerRightUp:
            case TileDirection.CornerDownLeft:
                return 90;
            case TileDirection.CornerRightDown:
            case TileDirection.CornerUpLeft:
                return 0;
        }
        return 0;
    }
    public static TileDirection GetTurnTileDirection(float angle) {
        switch (angle) {
            case 0:
                return TileDirection.CornerRightDown;
            case 90:
            case -270:
                return TileDirection.CornerRightUp;
            case -180:
            case 180:
                return TileDirection.CornerLeftUp;
            case -90:
            case 270:
                return TileDirection.CornerLeftDown;

        }
        return 0;
    }
    public static float GetAngleForSwitchTurn(RailType des, TileDirection t1, TileDirection t2, TileDirection t3) {

        Vector3 v1 = GetVector(t1);
        Vector3 v2 = GetVector(t2);
        Vector3 v3 = GetVector(t3);

        float angle = 0;
        Vector3 outVector = Vector3.zero;
        if (Utilss.IsVectorParallel(v1, v2)) {
            outVector = v3;
        } else if (Utilss.IsVectorParallel(v1, v3)) {
            outVector = v2;
        } else if (Utilss.IsVectorParallel(v2, v3)) {
            outVector = v1;
        }

        if (des == RailType.SwitchLeft) {
            if (outVector == m_VectorDown) {
                angle = 0;
            } else if (outVector == m_VectorUp) {
                angle = 180;
            } else if (outVector == m_VectorRight) {
                angle = 90;
            } else if (outVector == m_VectorLeft) {
                angle = 270;
            }
        } else if (des == RailType.SwitchRight) {
            if (outVector == m_VectorDown) {
                angle = 180;
            } else if (outVector == m_VectorUp) {
                angle = 0;
            } else if (outVector == m_VectorRight) {
                angle = 270;
            } else if (outVector == m_VectorLeft) {
                angle = 90;
            }
        }
        //Debug.Log(t1 + "----" + t2 + "----" + t3 + " angle " + angle);
        return -angle;
    }
    public static List<TileDirection> GetSwitchTurnOutput(RailType desRailType, float angle) {
        List<TileDirection> result = new List<TileDirection>();

        return result;
    }
    public static Vector3 GetVector(TileDirection tileDirection) {
        switch (tileDirection) {
            case TileDirection.Up:
            case TileDirection.CornerLeftUp:
            case TileDirection.CornerRightUp:
                return m_VectorUp;
            case TileDirection.Down:
            case TileDirection.CornerRightDown:
            case TileDirection.CornerLeftDown:
                return m_VectorDown;
            case TileDirection.Left:
            case TileDirection.CornerUpLeft:
            case TileDirection.CornerDownLeft:
                return m_VectorLeft;
            case TileDirection.Right:
            case TileDirection.CornerDownRight:
            case TileDirection.CornerUpRight:
                return m_VectorRight;
        }
        return Vector3.zero;
    }
    public static TileDirection GetTileDirectionForTurn(float angle) {
        if (angle < 0) angle += 360;

        if (Utilss.CloseEnoughForMe(angle, 90, 0.001f)) {
            return TileDirection.CornerRightUp;
        } else if (Utilss.CloseEnoughForMe(angle, 270, 0.001f)) {
            return TileDirection.CornerLeftDown;
        } else if (Utilss.CloseEnoughForMe(angle, 0, 0.001f)) {
            return TileDirection.CornerRightDown;
        } else if (Utilss.CloseEnoughForMe(angle, 180, 0.001f)) {
            return TileDirection.CornerLeftUp;
        }
        
        return TileDirection.None;
    }
    public static List<TileDirection> GetSwitchOtherTileDirections(TileDirection tileDirection) {
        switch (tileDirection) {
            case TileDirection.Left:
            case TileDirection.Right:
                return new List<TileDirection> { TileDirection.Up, TileDirection.Down };
            case TileDirection.Up:
            case TileDirection.Down:
                return new List<TileDirection> { TileDirection.Left, TileDirection.Right };
        }
        return new List<TileDirection>();
    }
    public static bool IsDirectionParallel(TileDirection t1, TileDirection t2) {
        Vector3 v1 = GetVector(t1);
        Vector3 v2 = GetVector(t2);
        return Utilss.IsVectorParallel(v1, v2);
    }
}
