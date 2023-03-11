using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public static class CellExtension 
{
    public static void OnActive_1(this Cell cell)
    {
        float target_1 = cell.TF.position.y + 1.5f;
        float target_2 = cell.TF.position.y - 1.5f;
        float target_3 = cell.TF.position.y;
        cell.TF.DOMoveY(target_1, .3f).SetEase(Ease.Linear).OnComplete(
            () => cell.TF.DOMoveY(target_2, .5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                cell.OnChange();
                cell.TF.DOMoveY(target_3, .5f);
            }));
    }

    public static void OnActive_2(this Cell cell)
    {
        float target = cell.TF.position.y;
        cell.TF.DOMoveY(target + 2, 1f).OnComplete(
            () =>
            {
                cell.TF.DORotate(Vector3.up * 180, .2f, RotateMode.WorldAxisAdd).SetEase(Ease.Linear).OnComplete(
                    () =>
                    {
                        cell.TF.rotation = Quaternion.Euler(Vector3.up * 180);
                        cell.OnChange();
                        cell.TF.DORotate(Vector3.up * 180, .2f, RotateMode.WorldAxisAdd).SetEase(Ease.Linear).OnComplete(
                            () =>
                            {
                                cell.TF.rotation = Quaternion.identity;
                                cell.TF.DOMoveY(target, 0.2f).SetDelay(0.5f);
                            });
                                
                    });

            });

    }

    public static void OnActive_3(this Cell cell)
    {
        cell.TF.DOMoveY(-2, 0.5f).OnComplete(()=>
            {
                cell.OnChange();
                cell.TF.DOMoveY(0, 0.5f).SetDelay(0.5f);
            });
    }

    public static void OnActive_4(this Cell cell, Vector3 center)
    {
        //path = Spiral(TF.position, center, 0.5f, 0.3f).ToArray();
        Vector3[] path;
        Vector3 startPoint = cell.TF.position;
        path = CurvePaths.SpiralNarrow(cell.TF.position, center, 0.5f, 0.3f).ToArray();

        cell.TF.DOPath(path, UnityEngine.Random.Range(0.7f, 1.2f)).OnComplete(() =>
        {
            cell.TF.position = center;
            cell.OnChange();
            path = CurvePaths.SpiralExtend(cell.TF.position, startPoint, 0.5f, -0.3f).ToArray();
            cell.TF.DOPath(path, UnityEngine.Random.Range(0.7f, 1.2f)).SetDelay(0.3f).OnComplete(() => cell.TF.position = startPoint);
        });
    }



}
