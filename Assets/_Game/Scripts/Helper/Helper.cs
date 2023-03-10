using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using DG.Tweening;
using TMPro;

public static class Helper  
{
    private static readonly Dictionary<float, WaitForSeconds> WFSDictionary = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWaitForSeconds(float time)
    {
        if (WFSDictionary.TryGetValue(time, out var wait)) return wait;

        WFSDictionary[time] = new WaitForSeconds(time);
        return WFSDictionary[time];
    }

    public static void GetPositions<T>(List<T> gameObject, List<Vector3> positions)
    {
        for (int i = 0; i < gameObject.Count; i++)
        {
            Vector3 position = (gameObject[i] as GameObject).transform.position;
            positions.Add(position);
        }
    }
}
