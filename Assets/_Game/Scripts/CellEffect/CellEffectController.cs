using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;

public class CellEffectController : Singleton<CellEffectController>
{
    public Array[,] CellMatrix;
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

    private Sequence SequenceEffect;
    [SerializeField] private float xMove;
    [SerializeField] private float yMove;
    [SerializeField] private float tweenDuration = 2f;
    [SerializeField] private float delaySetTransformParent;
    [SerializeField] private float rotationSpeed;
    private float cellTweenTimeDelay = 0.2f;

    private void Awake()
    {
        Rows = CellManager.Instance.Rows;
        Cells = CellManager.Instance.CellList;
        Helper.GetPositions(Rows, RowStartPositions);
        CenterPoint = CellManager.Instance.CenterPoint;
        GetOneRowCellStartPositions();
    }

    [Button("Play Effect 01")]
    private void PlayRowEffect01()
    {
        for (int i = 0; i < Rows.Count; i++)
        {
            if (i % 2 == 0 )
            {
                Rows[i].transform.DOMoveX(xMove, tweenDuration).SetEase(Ease).
                    SetLoops(2, LoopType.Yoyo).From(RowStartPositions[i]);
            }
            else
            {
                Rows[i].transform.DOMoveX(-xMove, tweenDuration).SetEase(Ease).
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

    [Button("Reset Cell Position")]
    private void ResetCellPosition()
    {
        SequenceEffect?.Kill();
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
