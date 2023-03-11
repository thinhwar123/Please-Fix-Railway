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
    public List<Cell> Cells;
    public List<Cell> OneRowCells;
    public List<Vector3> CellStartPositions = new List<Vector3>();
    public List<Vector3> OneCellStartPositions = new List<Vector3>();
    public Ease Ease;

    [Header("Effect 01")]
    public List<GameObject> Rows;
    public List<Vector3> RowStartPositions;

    [Header("Effect 02")]
    public GameObject CenterPoint;
    public Vector3 RotateVector;

    [Header("Effect 03")]
    //[SerializeField] private List<List<Cell>> CellsEffect03;
    [SerializeField] private float xMoveEffect03;
    [SerializeField] private float yMoveEffect03;
    [SerializeField] private float xMoveEffect03Duration;
    [SerializeField] private float yMoveEffect03Duraiton;
    [SerializeField] private float rowTweenDelayTimeEffect03;

    [Header("General")]
    [SerializeField] private float xMove;
    [SerializeField] private float yMove;
    [SerializeField] private float tweenDuration = 2f;
    [SerializeField] private float delaySetTransformParent;
    [SerializeField] private float rotationSpeed;
    
    private Sequence SequenceEffect;
    private float cellTweenTimeDelay = 0.2f;



    private void Awake()
    {
        Rows = CellManager.Instance.Rows;
        Cells = CellManager.Instance.CellList;
        Helper.GetPositions(Rows, RowStartPositions);
        CenterPoint = CellManager.Instance.CenterPoint;
        GetOneRowCellStartPositions();
        GetCellsForEffect03();
        GetCellMatrix();
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

    [Button("Play Effect 01")]
    private void PlayRowEffect01()
    {
        for (int i = 0; i < Rows.Count; i++)
        {
            if (i % 2 == 0 )
            {
                Rows[i].transform.DOMoveX(xMove, tweenDuration).SetEase(Ease).
                    OnComplete(() =>
                    {
                        
                    }).
                    SetLoops(2, LoopType.Yoyo).From(RowStartPositions[i]);
            }
            else
            {
                Rows[i].transform.DOMoveX(-xMove, tweenDuration).SetEase(Ease).
                    OnComplete(() =>
                    {
                        
                    }).
                    SetLoops(2, LoopType.Yoyo).From(RowStartPositions[i]);
            }
        }
    }

    [Button("Play Effect 02")]
    private void PlayRowEffect02()
    {
        StartCoroutine(SetEffect02());
    }

    private IEnumerator SetEffect02()
    {
        CenterPoint.transform.DOMoveY(yMove, tweenDuration);
        CenterPoint.transform.DORotate(RotateVector, rotationSpeed, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
        foreach (Cell cell in Cells)
        {
            cell.Transform.SetParent(CenterPoint.transform);
            yield return Helper.GetWaitForSeconds(delaySetTransformParent);
        }
    }

    [Button("Play Effect 03")]
    private void PlayEffect03()
    {
        StartCoroutine(SetEffect03());
    }

    private IEnumerator SetEffect03()
    {
        for (int i = 0; i < Rows.Count; i++)
        {
            Rows[i].transform.DOMoveX(xMoveEffect03, xMoveEffect03);           
        }
        for (int i = 0; i < Rows.Count; i++)
        {
            Rows[i].transform.DOMoveY(yMoveEffect03, yMoveEffect03Duraiton).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            yield return Helper.GetWaitForSeconds(rowTweenDelayTimeEffect03);
        }
    }

    private void GetCellsForEffect03()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //CellsEffect03[i].Add()
            }
        }
    }

    [Button("Reset Row Positions")]
    private void ResetRowPositions()
    {

    }

    [Button("Play Effect")]
    private void PlayEffect()
    {
        SequenceEffect = DOTween.Sequence();
        foreach (Cell cell in Cells)
        {
            SequenceEffect.Append(cell.Transform.DOMoveY(yMove, tweenDuration).
                SetEase(Ease));
        }
    }

    [Button("Play One Row Cell Effect")]
    private void PlayOneRowCellEffect()
    {
        StartCoroutine(SequenceCellEffect());
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

    [Button("Reset One Row Cell Position")]
    private void ResetOneRowCellPosition()
    {
        DOTween.KillAll();
        for (int i = 0; i < OneRowCells.Count; i++)
        {
            OneRowCells[i].WorldPosition = OneCellStartPositions[i];
        }
    }

    private IEnumerator SequenceCellEffect()
    {
        foreach (Cell cell in OneRowCells)
        {
            foreach (Vector3 startPosition in OneCellStartPositions)
            {
                cell.Transform.DOMoveY(yMove, tweenDuration).SetEase(Ease).
                OnComplete(() => cell.Transform.DOMoveY(startPosition.y, tweenDuration).SetEase(Ease));
            }
            yield return Helper.GetWaitForSeconds(cellTweenTimeDelay);
        }
    }

    private void GetCellStartPositions()
    {
        for (int i = 0; i < Cells.Count; i++)
        {
            Vector3 position = Cells[i].WorldPosition;
            CellStartPositions.Add(position);
        }
    }

    private void GetOneRowCellStartPositions()
    {
        for (int i = 0; i < OneRowCells.Count; i++)
        {
            Vector3 position = OneRowCells[i].WorldPosition;
            OneCellStartPositions.Add(position);
        }
    }
}
