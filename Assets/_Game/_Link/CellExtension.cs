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
        float target_1 = cell.Transform.position.y + 1.5f;
        float target_2 = cell.Transform.position.y - 1.5f;
        float target_3 = cell.Transform.position.y;
        cell.Transform.DOMoveY(target_1, .3f).SetEase(Ease.Linear).OnComplete(
            () => cell.Transform.DOMoveY(target_2, .5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                cell.OnChange();
                cell.Transform.DOMoveY(target_3, .5f);
            }));
    }

    public static void OnActive_2(this Cell cell)
    {
        float target = cell.Transform.position.y;
        cell.Transform.DOMoveY(target + 2, 1f).OnComplete(
            () =>
            {
                cell.Transform.DORotate(Vector3.up * 180, .2f, RotateMode.WorldAxisAdd).SetEase(Ease.Linear).OnComplete(
                    () =>
                    {
                        cell.Transform.rotation = Quaternion.Euler(Vector3.up * 180);
                        cell.OnChange();
                        cell.Transform.DORotate(Vector3.up * 180, .2f, RotateMode.WorldAxisAdd).SetEase(Ease.Linear).OnComplete(
                            () =>
                            {
                                cell.Transform.rotation = Quaternion.identity;
                                cell.Transform.DOMoveY(target, 0.2f).SetDelay(0.5f);
                            });
                                
                    });

            });
    }

    public static void OnActive_3(this Cell cell)
    {
        cell.Transform.DOMoveY(-2, 0.5f).OnComplete(()=>
            {
                cell.OnChange();
                cell.Transform.DOMoveY(0, 0.5f).SetDelay(0.5f);
            });
    }

    public static void OnActive_4(this Cell cell)
    {
        //Vector3 target = 

        //cell.Transform.DOMoveY(-2, 0.5f).OnComplete(()=>
        //    {
        //        cell.OnChange();
        //        cell.Transform.DOMoveY(0, 0.5f).SetDelay(0.5f);
        //    });

    }

}
