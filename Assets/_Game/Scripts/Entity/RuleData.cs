using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;

#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using System.Linq;
#endif

[System.Serializable]
public class RuleData
{
    public List<int> m_BaseRulePosition;
    public GameObject m_GameObject;

    public RuleData()
    {
        m_BaseRulePosition = new List<int>();
        m_GameObject = null;
    }
    public int[,] GetBaseRulePosition()
    {
        int[,] res = new int[3,3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                res[i, j] = m_BaseRulePosition[i * 3 + j];
            }
        }


        return res;
    }
    public int CompareRule(int[,] rule)
    {
        //ShowRule(GetBaseRulePosition());
        for (int i = 0; i < 4; i++)
        {
            if (CompareArray(Rotate_ClockWise(rule, i)))
            {
                return i;
            }
        }
        return -1;
    }
    public bool CompareArray(int[,] rule)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (m_BaseRulePosition[i * 3 + j] == 0)
                {
                    continue;
                }
                else if (rule[i, j] != m_BaseRulePosition[i * 3 + j])
                {
                    return false;
                }
            }
        }
        return true;
    }
    public int[,] Rotate_ClockWise(int[,] arr, int time)
    {
        int[,] tempArray = arr.Clone() as int[,];
        for (int x = 0; x < time; x++)
        {
            for (int i = 0; i < 3 / 2; i++)
            {
                for (int j = i; j < 3 - i - 1; j++)
                {
                    int ptr = tempArray[i, j];
                    tempArray[i, j] = tempArray[3 - 1 - j, i];
                    tempArray[3 - 1 - j, i] = tempArray[3 - 1 - i, 3 - 1 - j];
                    tempArray[3 - 1 - i, 3 - 1 - j] = tempArray[j, 3 - 1 - i];
                    tempArray[j, 3 - 1 - i] = ptr;
                }
            }
        }
        return tempArray;
    }
    public static void ShowRule(int[,] rule)
    {
        string s = "";
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                s += string.Format("{0} - ", rule[i, j]);
            }
            s += "\n";
        }
    }
}

[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public sealed class RuleDataEditor : System.Attribute
{

}

#if UNITY_EDITOR
public sealed class RuleDataEditorAttributeDrawer : OdinAttributeDrawer<RuleDataEditor, RuleData>
{
    private enum Tile
    {
        HasOrNotHasBlock = 0,
        NeedHasBlock = 1,
        DontHasBlock = 2,
        Middle = 3,
    }

    private readonly Color[] TileColors = new Color[4]
{
            new Color(0.3f, 0.3f, 0.3f),		// 0
            new Color(0.3f, 1.0f, 0.3f),		// 1
            new Color(1.0f, 0.3f, 0.3f),		// 2
            new Color(1.0f, 1.0f, 0.0f),		// 3

};

    private int tileSize;
    private int row;
    private int col;

    private bool isDrawing;

    private Tile[,] tiles;
    private Color oldColor;
    private List<int> baseRulePosition;

    protected override void Initialize()
    {
        this.tileSize = 40;
        this.row = 3;
        this.col = 3;

        this.tiles = new Tile[row, col];

        this.isDrawing = false;

        oldColor = GUI.backgroundColor;
    }
    protected override void DrawPropertyLayout(GUIContent label)
    {
        Rect rect = EditorGUILayout.GetControlRect();
        this.ValueEntry.SmartValue.m_GameObject = SirenixEditorFields.UnityObjectField(rect.AlignLeft(rect.width - 100 - 4), this.ValueEntry.SmartValue.m_GameObject, typeof(GameObject), true) as GameObject;

        if (!isDrawing)
        {
            GUI.backgroundColor = Color.green;
            if (GUI.Button(rect.AlignRight(100), "Edit Pos ID"))
            {
                LoadCurrentBaseRule();                
                this.ValueEntry.WeakValues.ForceMarkDirty();

            }
            GUI.backgroundColor = oldColor;
        }
        else
        {
            GUI.backgroundColor = Color.blue;
            if (GUI.Button(rect.AlignRight(100), "Save Pos ID"))
            {
                SaveCurrentBaseRule();
                this.ValueEntry.WeakValues.ForceMarkDirty();
            }
            GUI.backgroundColor = oldColor;
        }
        OnDrawMap();
    }
    public void LoadCurrentBaseRule()
    {
        if (this.ValueEntry.SmartValue.m_BaseRulePosition == null || this.ValueEntry.SmartValue.m_BaseRulePosition.Count < 9)
        {
            this.ValueEntry.SmartValue.m_BaseRulePosition = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        }
        baseRulePosition = this.ValueEntry.SmartValue.m_BaseRulePosition;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i == 1 && j == 1)
                {
                    tiles[i, j] = Tile.Middle;
                }
                else
                {
                    tiles[i, j] = (Tile)baseRulePosition[i * 3 + j];
                }                
            }
        }
        isDrawing = true;
    }
    public void SaveCurrentBaseRule()
    {
        this.ValueEntry.SmartValue.m_BaseRulePosition = baseRulePosition;
        isDrawing = false;
    }
    public void OnDrawMap()
    {
        if (!isDrawing) return;

        Rect rect = EditorGUILayout.GetControlRect();

        rect = EditorGUILayout.GetControlRect(false, tileSize * row);
        Rect tempRect = rect;
        rect = rect.AlignLeft(rect.width / 2);
        rect = rect.AlignCenter(tileSize * col);
        rect = rect.AlignMiddle(tileSize * row);
        SirenixEditorFields.PreviewObjectField(rect, this.ValueEntry.SmartValue.m_GameObject, true , false, false , true);

        rect = tempRect;
        rect = rect.AlignRight(rect.width / 2);
        rect = rect.AlignCenter(tileSize * col);
        rect = rect.AlignMiddle(tileSize * row);

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Rect tileRect = rect.SplitGrid(tileSize, tileSize, i * col + j);
                SirenixEditorGUI.DrawBorders(tileRect.SetWidth(tileRect.width + 1).SetHeight(tileRect.height + 1), 1);

                SirenixEditorGUI.DrawSolidRect(new Rect(tileRect.x + 1, tileRect.y + 1, tileRect.width - 1, tileRect.height - 1), TileColors[(int)tiles[i, j]]);


                if (tileRect.Contains(Event.current.mousePosition))
                {
                    SirenixEditorGUI.DrawSolidRect(new Rect(tileRect.x + 1, tileRect.y + 1, tileRect.width - 1, tileRect.height - 1), new Color(0f, 1f, 0f, 0.3f));

                    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                    {
                        if (this.tiles[i, j] == Tile.DontHasBlock)
                        {
                            this.tiles[i, j] = Tile.HasOrNotHasBlock;
                            this.baseRulePosition[i * 3 + j] = (int)this.tiles[i, j];
                        }
                        else if (this.tiles[i, j] == Tile.NeedHasBlock)
                        {
                            this.tiles[i, j] = Tile.DontHasBlock;
                            this.baseRulePosition[i * 3 + j] = (int)this.tiles[i, j];
                        }
                        else if (this.tiles[i, j] == Tile.HasOrNotHasBlock)
                        {
                            this.tiles[i, j] = Tile.NeedHasBlock;
                            this.baseRulePosition[i * 3 + j] = (int)this.tiles[i, j];
                        }
                        Event.current.Use();
                    }
                }
            }
        }

        GUIHelper.RequestRepaint();
    }

}
#endif