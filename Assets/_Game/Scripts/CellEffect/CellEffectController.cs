using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using Sirenix.OdinInspector;

public class CellEffectController : Singleton<CellEffectController>
{
    [ShowInInspector] private Dictionary<Cell, Vector3> dictCellStartPositions = new Dictionary<Cell, Vector3>();   
    [ShowInInspector] private Dictionary<GameObject, Vector3> dictRowStartPositions = new Dictionary<GameObject, Vector3>();   
    [ShowInInspector] private Cell[,] cellMatrix = new Cell[10, 10];
    [ShowInInspector] private List<List<Cell>> rowCellMatrix = new List<List<Cell>>();
    [ShowInInspector] private List<List<Cell>> columnCellMatrix = new List<List<Cell>>();
    [SerializeField] private List<Cell> cells;
    [SerializeField] private List<Vector3> cellStartPositions = new List<Vector3>();
    [SerializeField] private List<GameObject> rows;
    [SerializeField] private List<Vector3> rowStartPositions; 

    [Header("Effect 01")]
    [SerializeField] private float xMoveEffect01;
    [SerializeField] private float tweenEffect01Duration;
    [SerializeField] private Ease easeEffect01;

    [Header("Effect 02")]
    [SerializeField] private GameObject centerPoint;
    [SerializeField] private Vector3 rotateVector;
    [SerializeField] private float yMoveEffect02;
    [SerializeField] private float tweenEffect02Duration;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float delaySetTransformParent;
    [SerializeField] private Ease easeEffect02;

    [Header("Effect 03")]
    [SerializeField] private float xMoveEffect03;
    [SerializeField] private float xMoveDisapearEffect03Duration;
    [SerializeField] private float xMoveApearEffect03Duration;
    [SerializeField] private float yMoveEffect03;
    [SerializeField] private float yMoveEffect03Duraiton;
    [SerializeField] private float tweenDelayTimeEffect03;
    [SerializeField] private Ease easeEffect03;
    private Sequence sequenceMoveXEffect03;
    private Sequence sequenceMoveYEffect03;
    private IEnumerator coroutineDisappearEffect03;
    private IEnumerator coroutineAppearEffect03;

    [Header("Effect 04")]
    [ShowInInspector] private List<List<Cell>> halfColumn01 = new List<List<Cell>>();
    [ShowInInspector] private List<List<Vector3>> halfColumn01StartPostisions = new List<List<Vector3>>();  
    [ShowInInspector] private List<List<Cell>> halfColumn02 = new List<List<Cell>>();
    [SerializeField] private float xMoveEffect04;
    [SerializeField] private float xMoveEffect04Duration;
    [SerializeField] private float yMoveEffect04;
    [SerializeField] private float yMoveEffect04Duraiton;
    [SerializeField] private float tweenDelayTimeEffect04;
    [SerializeField] private float tweenAppearEffect04;
    [SerializeField] private Ease easeXMoveEffect04;
    [SerializeField] private Ease easeYMoveEffect04;
    [SerializeField] private Ease easeAppearEffect04;

    [Header("Effect 05")]
    [SerializeField] private Transform node01;
    [SerializeField] private Transform node02;
    [SerializeField] private float minJumpPower = 2f;
    [Range(2.5f, 4.5f)]
    [SerializeField] private float maxJumpPower = 4f;
    [SerializeField] private float minXJumpDurationEffect05 = 4f;
    [Range(4.5f, 5.5f)]
    [SerializeField] private float maxXJumpDurationEffect05 = 5f;
    [SerializeField] private int minXJunpTime = 4;
    [Range(6, 10)]
    [SerializeField] private int maxXJumpTime = 8;
    [SerializeField] private Ease easeXMoveEffect05;
    [SerializeField] private Ease easeRotateEffect05;

    private void Awake()
    {
        rows = CellManager.Instance.Rows;
        cells = CellManager.Instance.CellList;
        GetCellMatrix();
        GetAllRowsAndColumnsInCellMatrix();
        GetHalfColumns();
        GetDictCellStartPositions();
        GetCellStartPositions();
        Helper.GetStartPositions(rows, rowStartPositions);
        DOTween.Init();
        DOTween.SetTweensCapacity(500, 250);
    }

    private void GetCellMatrix()
    {
        Cell[] tempArray = cells.ToArray();
        for (int i = 0; i < cellMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < cellMatrix.GetLength(1); j++)
            {
                cellMatrix[i, j] = tempArray[i * cellMatrix.GetLength(1) + j];
            }
        }
    }

    private void GetAllRowsAndColumnsInCellMatrix()
    {
        int numbRows = cellMatrix.GetLength(0);
        int numbColumns = cellMatrix.GetLength(1);
        for (int i = 0; i < numbRows; i++)
        {
            List<Cell> tempList = new List<Cell>();
            rowCellMatrix.Add(tempList);    
            for (int j = 0; j < numbColumns; j++)
            {
                Cell cell = cellMatrix[i, j];
                rowCellMatrix[i].Add(cell);
            }
        }
        for (int i = 0; i < numbColumns; i++)
        {
            List<Cell> tempList = new List<Cell>();
            columnCellMatrix.Add(tempList);
            for (int j = 0; j < numbRows; j++)
            {
                Cell cell = cellMatrix[j, i];
                columnCellMatrix[i].Add(cell);
            }
        }
    }

    private void GetDictCellStartPositions()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            dictCellStartPositions.Add(cells[i], cells[i].WorldPosition);
        }
    }

    private void GetDictRowStartPositions()
    {
        for (int i = 0; i < rows.Count; i++)
        {
            dictRowStartPositions.Add(rows[i], rows[i].transform.position);
        }
    }

    private Vector3 GetCellStartPosition(Cell cell) 
    {
        if (dictCellStartPositions.ContainsKey(cell))
        {
            return dictCellStartPositions[cell];
        }
        return Vector3.zero;
    }

    private Vector3 GetRowStartPosition(GameObject row)
    {
        if (dictRowStartPositions.ContainsKey(row))
        {
            return dictRowStartPositions[row];
        }
        return Vector3.zero;
    }

    private void GetCellStartPositions()
    {
        for(int i = 0; i < cells.Count; i++)
        {
            Vector3 position = cells[i].WorldPosition;
            cellStartPositions.Add(position);
        }
    }

    private void GetHalfColumns()
    {
        for (int i = 0; i < columnCellMatrix.Count; i++)
        {
            if (i < columnCellMatrix.Count / 2)
            {
                List<Cell> tempList = columnCellMatrix[i];
                halfColumn01.Add(tempList); 
            }
            else
            {
                List<Cell> tempList = columnCellMatrix[i];
                halfColumn02.Add(tempList);
            }
        }
    }

    #region Effect 01
    [Button("Play Effect 01")]
    private void PlayRowEffect01()
    {
        for (int i = 0; i < rowCellMatrix.Count; i++)
        {
            if (i % 2 == 0)
            {
                for (int j = 0; j < rowCellMatrix[i].Count; j++)
                {
                    if (i == rowCellMatrix.Count - 2 && j == rowCellMatrix[i].Count - 1)
                    {
                        Cell cell = rowCellMatrix[i][j];
                        cell.Transform.DOMoveX(xMoveEffect01, tweenEffect01Duration).SetEase(easeEffect01)
                            .OnComplete(() =>
                            {
                                for (int i = 0; i < cells.Count; i++)
                                {
                                    //TODO: Change cell
                                }
                                cell.Transform.DOMoveX(GetCellStartPosition(cell).x, tweenEffect01Duration).SetEase(easeEffect01);
                            });
                    }
                    else
                    {
                        Cell cell = rowCellMatrix[i][j];
                        cell.Transform.DOMoveX(xMoveEffect01, tweenEffect01Duration).SetEase(easeEffect01)
                            .OnComplete(() =>
                            {
                                cell.Transform.DOMoveX(GetCellStartPosition(cell).x, tweenEffect01Duration).SetEase(easeEffect01);
                            }); 
                    }
                }
            }
            else
            {
                for (int j = 0; j < rowCellMatrix[i].Count; j++)
                {
                    Cell cell = rowCellMatrix[i][j];
                    cell.Transform.DOMoveX(-xMoveEffect01, tweenEffect01Duration).SetEase(easeEffect01)
                        .OnComplete(() =>
                        {
                            cell.Transform.DOMoveX(GetCellStartPosition(cell).x, tweenEffect01Duration).SetEase(easeEffect01);
                        });
                }
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
        centerPoint.transform.DOMoveY(yMoveEffect02, tweenEffect02Duration);
        centerPoint.transform.DORotate(rotateVector, rotationSpeed, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
        foreach (Cell cell in cells)
        {
            cell.Transform.SetParent(centerPoint.transform);
            yield return Helper.GetWaitForSeconds(delaySetTransformParent);
        }
    }
    #endregion

    [Button("Play Effect 03")]
    private void PlayEffect03()
    {
        coroutineDisappearEffect03 = SetEffectDisappear03();
        coroutineAppearEffect03 = SetEffectAppear03();
        StartCoroutine(coroutineDisappearEffect03);
        for (int i = 0; i < rowCellMatrix.Count; i++)
        {
            for (int j = 0; j < rowCellMatrix[i].Count; j++)
            {
                if (i == rowCellMatrix.Count - 1 && j == rowCellMatrix[i].Count - 1)
                {
                    Cell cell = rowCellMatrix[i][j];
                    cell.Transform.DOMoveX(xMoveEffect03, xMoveDisapearEffect03Duration)
                        .OnComplete(() =>
                    {                        
                        StartCoroutine(coroutineAppearEffect03);
                    });
                }
                else
                {
                    Cell cell = rowCellMatrix[i][j];
                    cell.Transform.DOMoveX(xMoveEffect03, xMoveDisapearEffect03Duration);
                }
            }
        }

    }

    private IEnumerator SetEffectDisappear03()
    {
        for (int i = 0; i < columnCellMatrix.Count; i++)
        {
            for (int j = 0; j < columnCellMatrix[i].Count; j++)
            {
                Cell cell = columnCellMatrix[i][j];
                cell.Transform.DOMoveY(yMoveEffect03, yMoveEffect03Duraiton)
                    .SetLoops(-1, LoopType.Yoyo);
            }
            yield return Helper.GetWaitForSeconds(tweenDelayTimeEffect03);
        }
    }

    private IEnumerator SetEffectAppear03()
    {
        DOTween.KillAll();
        SetCellNewPositionsEffect03();
        for (int i = 0; i < rows.Count; i++)
        {
            rows[i].transform.DOMoveX(-xMoveEffect03, xMoveDisapearEffect03Duration);
            yield return Helper.GetWaitForSeconds(tweenDelayTimeEffect03);
        }
    }

    private void SetCellNewPositionsEffect03()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            //TODO: Change cell
            Vector3 newPosition = new Vector3(GetCellStartPosition(cells[i]).x + xMoveEffect03, 0,
                GetCellStartPosition(cells[i]).z);
            cells[i].WorldPosition = newPosition;
        }
    }

    #region Effect 04
    [Button("Play Effect 04")]
    private void PlayEffect04()
    {
        StartCoroutine(SetHalfColumn01DisapearEffect04());
        StartCoroutine(SetHalfColumn02DisapearEffect04());
    }

    private IEnumerator SetHalfColumn01DisapearEffect04()
    {
        for (int i = 0; i < halfColumn01.Count; i++)
        {
            for (int j = 0; j < halfColumn01[i].Count; j++)
            {
                Cell cell = halfColumn01[i][j];
                if (i == halfColumn01.Count - 1 && j == halfColumn01[i].Count - 1)
                {
                    cell.Transform.DOMoveX(-xMoveEffect04, xMoveEffect04Duration).SetEase(easeXMoveEffect04);
                    cell.Transform.DOMoveY(yMoveEffect04, yMoveEffect04Duraiton).SetEase(easeYMoveEffect04)
                        .OnComplete(() => SetAppearEffect04());
                }
                else
                {
                    cell.Transform.DOMoveX(-xMoveEffect04, xMoveEffect04Duration).SetEase(easeXMoveEffect04);
                    cell.Transform.DOMoveY(yMoveEffect04, yMoveEffect04Duraiton).SetEase(easeYMoveEffect04);
                }
            }
            yield return Helper.GetWaitForSeconds(tweenDelayTimeEffect04);
        }
    }

    private void SetAppearEffect04()
    {
        DOTween.KillAll();
        for (int i = 0; i < cells.Count; i++)
        {
            Vector3 newPosition = new Vector3(GetCellStartPosition(cells[i]).x, GetCellStartPosition(cells[i]).y - 10, GetCellStartPosition(cells[i]).z);
            cells[i].WorldPosition = newPosition;   
            //TODO: Change cell
        }

        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].Transform.DOMoveY(0, tweenAppearEffect04).SetEase(easeAppearEffect04);
        }
    }

    private IEnumerator SetHalfColumn02DisapearEffect04()
    {
        for (int i = halfColumn02.Count - 1; i >= 0; i--)
        {
            for (int j = 0; j < halfColumn02[i].Count; j++)
            {
                halfColumn02[i][j].transform.DOMoveX(xMoveEffect04, xMoveEffect04Duration).SetEase(easeXMoveEffect04);
                halfColumn02[i][j].transform.DOMoveY(yMoveEffect04, yMoveEffect04Duraiton).SetEase(easeYMoveEffect04);
            }
            yield return Helper.GetWaitForSeconds(tweenDelayTimeEffect04);
        }
    }
    #endregion

    #region Effect 05
    [Button("Play Effect 05")]
    private void PlayEffect05() 
    {
        for (int i = 0; i < halfColumn01.Count; i++)
        {
            for (int j = 0; j < halfColumn01[i].Count; j++)
            {
                Cell cell = halfColumn01[i][j];
                Vector3 startPosition = GetCellStartPosition(cell);
                float jumpPower = Random.Range(minJumpPower, maxJumpPower);
                int jumpTime = Random.Range(minXJunpTime, maxXJumpTime);
                float duration = Random.Range(minXJumpDurationEffect05, maxXJumpDurationEffect05);
                cell.Transform.DOJump(node02.position, jumpPower, jumpTime, duration)
                    .SetEase(easeXMoveEffect05);
                cell.Transform.DORotate(new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)), duration)
                    .SetEase(easeRotateEffect05)
                    .OnComplete(() =>
                    {
                        //TODO: Change cell
                        cell.Transform.DOJump(startPosition, jumpPower, jumpTime, duration)
                        .SetEase(easeXMoveEffect05);
                        cell.Transform.DORotate(Vector3.zero, duration)       
                        .SetEase(easeRotateEffect05);
                    });
            }
        }

        for (int i = halfColumn02.Count - 1; i >= 0; i--)
        {
            for (int j = 0; j < halfColumn02[i].Count; j++)
            {
                Cell cell = halfColumn02[i][j];
                Vector3 startPosition = GetCellStartPosition(cell);
                float jumpPower = Random.Range(minJumpPower, maxJumpPower);
                int jumpTime = Random.Range(minXJunpTime, maxXJumpTime);
                float duration = Random.Range(minXJumpDurationEffect05, maxXJumpDurationEffect05);
                cell.Transform.DOJump(node01.position, jumpPower, jumpTime, duration)
                   .SetEase(easeXMoveEffect05);
                cell.Transform.DORotate(new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)), duration)
                    .SetEase(easeRotateEffect05)
                    .OnComplete(() =>
                    {
                        //TODO: Change cell
                        cell.Transform.DOJump(startPosition, jumpPower, jumpTime, duration)
                        .SetEase(easeXMoveEffect05);
                        cell.Transform.DORotate(Vector3.zero, duration)
                        .SetEase(easeRotateEffect05);
                    });
            }
        }
    }
    #endregion

    [Button("Reset Cell Positions")]
    private void ResetCellPositions()
    {
        StopAllCoroutines();
        DOTween.KillAll();
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].WorldPosition = cellStartPositions[i];
            cells[i].Rotation = Quaternion.identity;  
        }
    }

    [Button("Reset Row Positions")]
    private void ResetRowPositions()
    {
        StopAllCoroutines();    
        DOTween.KillAll();
        for (int i = 0; i < rows.Count; i++)
        {
            rows[i].transform.position = rowStartPositions[i];
        }
    }
}
