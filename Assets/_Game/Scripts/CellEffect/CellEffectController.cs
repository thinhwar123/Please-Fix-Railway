using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using Newtonsoft.Json.Linq;

public class CellEffectController : Singleton<CellEffectController>
{
    [ShowInInspector] public GameObject[,] CellMatrix = new GameObject[10, 10];
    [ShowInInspector] public List<List<GameObject>> RowCellMatrix = new List<List<GameObject>>();
    [ShowInInspector] public List<List<GameObject>> ColumnCellMatrix = new List<List<GameObject>>();
    public List<Cell> Cells;
    public List<Vector3> CellStartPositions = new List<Vector3>();
    public List<GameObject> Rows;
    public List<Vector3> RowStartPositions; 

    [Header("Effect 01")]
    [SerializeField] private float xMoveEffect01;
    [SerializeField] private float tweenEffect01Duration;
    [SerializeField] private Ease easeEffect01;

    [Header("Effect 02")]
    [SerializeField] private GameObject CenterPoint;
    [SerializeField] private Vector3 RotateVector;
    [SerializeField] private float yMoveEffect02;
    [SerializeField] private float tweenEffect02Duration;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float delaySetTransformParent;
    [SerializeField] private Ease easeEffect02;

    [Header("Effect 03")]
    [SerializeField] private float xMoveEffect03;
    [SerializeField] private float xMoveEffect03Duration;
    [SerializeField] private float yMoveEffect03;
    [SerializeField] private float yMoveEffect03Duraiton;
    [SerializeField] private float tweenDelayTimeEffect03;
    [SerializeField] private Ease easeEffect03;

    [Header("Effect 04")]
    [ShowInInspector] private List<List<GameObject>> halfColumn01 = new List<List<GameObject>>();
    [ShowInInspector] private List<List<GameObject>> halfColumn02 = new List<List<GameObject>>();
    [SerializeField] private float xMoveEffect04;
    [SerializeField] private float xMoveEffect04Duration;
    [SerializeField] private float yMoveEffect04;
    [SerializeField] private float yMoveEffect04Duraiton;
    [SerializeField] private float tweenDelayTimeEffect04;
    [SerializeField] private Ease easeEffect04;

    private void Awake()
    {
        Rows = CellManager.Instance.Rows;
        Cells = CellManager.Instance.CellList;
        GetCellMatrix();
        GetAllRowsAndColumnsInCellMatrix();
        GetHalfColumns();
        GetCellStartPositions();
        Helper.GetStartPositions(Rows, RowStartPositions);
    }

    private void GetCellMatrix()
    {
        Cell[] tempArray = Cells.ToArray();
        for (int i = 0; i < CellMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < CellMatrix.GetLength(1); j++)
            {
                CellMatrix[i, j] = tempArray[i * CellMatrix.GetLength(1) + j].gameObject;
            }
        }
    }

    private void GetAllRowsAndColumnsInCellMatrix()
    {
        int numbRows = CellMatrix.GetLength(0);
        int numbColumns = CellMatrix.GetLength(1);
        for (int i = 0; i < numbRows; i++)
        {
            List<GameObject> tempList = new List<GameObject>();
            RowCellMatrix.Add(tempList);    
            for (int j = 0; j < numbColumns; j++)
            {
                GameObject cell = CellMatrix[i, j];
                RowCellMatrix[i].Add(cell);
            }
        }
        for (int i = 0; i < numbColumns; i++)
        {
            List<GameObject> tempList = new List<GameObject>();
            ColumnCellMatrix.Add(tempList);
            for (int j = 0; j < numbRows; j++)
            {
                GameObject cell = CellMatrix[j, i];
                ColumnCellMatrix[i].Add(cell);
            }
        }
    }

    private void GetCellStartPositions()
    {
        for(int i = 0; i < Cells.Count; i++)
        {
            Vector3 position = Cells[i].WorldPosition;
            CellStartPositions.Add(position);
        }
    }

    private void GetHalfColumns()
    {
        for (int i = 0; i < ColumnCellMatrix.Count; i++)
        {
            if (i < ColumnCellMatrix.Count / 2)
            {
                List<GameObject> tempList = ColumnCellMatrix[i];
                halfColumn01.Add(tempList); 
            }
            else
            {
                List<GameObject> tempList = ColumnCellMatrix[i];
                halfColumn02.Add(tempList);
            }
        }
    }

    #region Effect 01
    [Button("Play Effect 01")]
    private void PlayRowEffect01()
    {
        for (int i = 0; i < Rows.Count; i++)
        {
            if (i % 2 == 0)
            {
                Rows[i].transform.DOMoveX(xMoveEffect01, tweenEffect01Duration).SetEase(easeEffect01).
                    OnComplete(() =>
                    {

                    }).
                    SetLoops(2, LoopType.Yoyo).From(RowStartPositions[i]);
            }
            else
            {
                Rows[i].transform.DOMoveX(-xMoveEffect01, tweenEffect01Duration).SetEase(easeEffect01).
                    OnComplete(() =>
                    {

                    }).
                    SetLoops(2, LoopType.Yoyo).From(RowStartPositions[i]);
            }
        }
    }
    #endregion

    #region Effect 02
    [Button("Play Effect 02")]
    private void PlayRowEffect02()
    {
        StartCoroutine(SetEffect02());
    }

    private IEnumerator SetEffect02()
    {
        CenterPoint.transform.DOMoveY(yMoveEffect02, tweenEffect02Duration);
        CenterPoint.transform.DORotate(RotateVector, rotationSpeed, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
        foreach (Cell cell in Cells)
        {
            cell.Transform.SetParent(CenterPoint.transform);
            yield return Helper.GetWaitForSeconds(delaySetTransformParent);
        }
    }
    #endregion

    [Button("Play Effect 03")]
    private void PlayEffect03()
    {
        StartCoroutine(SetEffect03());
    }

    private IEnumerator SetEffect03()
    {
        for (int i = 0; i < Rows.Count; i++)
        {
            Rows[i].transform.DOMoveX(xMoveEffect03, xMoveEffect03Duration);
        }
        for (int i = 0; i < ColumnCellMatrix.Count; i++)
        {
            for (int j = 0; j < ColumnCellMatrix[i].Count; j++)
            {
                ColumnCellMatrix[i][j].transform.DOMoveY(yMoveEffect03, yMoveEffect03Duraiton)
                    .SetLoops(-1, LoopType.Yoyo);

            }
            yield return Helper.GetWaitForSeconds(tweenDelayTimeEffect03);
        } 
    }

    [Button("Play Effect 04")]
    private void PlayEffect04()
    {
        for (int i = 0; i < halfColumn01.Count; i++)
        {
            for (int j = 0; j < halfColumn01[i].Count; j++)
            {
                halfColumn01[i][j].transform.DOMoveX(-xMoveEffect04, xMoveEffect04Duration);
            }
        }
        for (int i = 0; i < halfColumn02.Count; i++)
        {
            for (int j = 0; j < halfColumn02[i].Count; j++)
            {
                halfColumn02[i][j].transform.DOMoveX(xMoveEffect04, xMoveEffect04Duration);
            }
        }
        //StartCoroutine(SetHalfColumn01Effect04());
    }

    private IEnumerator SetHalfColumn01Effect04()
    {
        for (int i = 0; i < halfColumn01.Count; i++)
        {
            for (int j = 0; j < halfColumn01.Count; j++)
            {
                halfColumn01[i][j].transform.DOMoveY(yMoveEffect04, yMoveEffect04Duraiton);
            }
            yield return Helper.GetWaitForSeconds(tweenDelayTimeEffect04);
        }
    }

    private IEnumerator SetHalfColumn02Effect04()
    {

        for (int i = 0; i < halfColumn02.Count; i++)
        {
            for (int j = 0; j < halfColumn02.Count; j++)
            {
                halfColumn01[i][j].transform.DOMoveY(-yMoveEffect04, yMoveEffect04Duraiton);
            }
            yield return Helper.GetWaitForSeconds(tweenDelayTimeEffect04);
        }
    }

    [Button("Reset Cell Positions")]
    private void ResetCellPositions()
    {
        DOTween.KillAll();
        for (int i = 0; i < Cells.Count; i++)
        {
            Cells[i].WorldPosition = CellStartPositions[i];
        }
    }

    [Button("Reset Row Positions")]
    private void ResetRowPositions()
    {
        DOTween.KillAll();
        for (int i = 0; i < Rows.Count; i++)
        {
            Rows[i].transform.position = RowStartPositions[i];
        }
    }
}
