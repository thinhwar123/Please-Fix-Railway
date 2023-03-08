using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class CellEffectController : Singleton<CellEffectController>
{
    public List<Cell> Cells;
    public List<Cell> OneRowCells;
    public List<Vector3> CellStartPositions = new List<Vector3>();
    public List<Vector3> OneCellStartPositions = new List<Vector3>();
    public Ease Ease;

    private Sequence SequenceEffect;
    private float yMove = 3f;
    private float tweenDuration = 2f;
    private float cellTweenTimeDelay = 0.2f;

    private void Awake()
    {
        Cells = CellManager.Instance.CellList;
        GetOneRowCellStartPositions();
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
