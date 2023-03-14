using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AnimTest : MonoBehaviour
{
    [SerializeField] private LayerMask m_WhatIsCell;
    Cell[,] cells = new Cell[10,10];
    Dictionary<Collider, Cell> dict = new Dictionary<Collider, Cell>();
    List<Collider> approves = new List<Collider>();

    Collider[] neiboors;
    [SerializeField] float speed = 10.0f;
    [SerializeField] float time;
    [SerializeField] Transform center;
    float radius = 0;
    Vector3 startPoint;

    bool active = false;

    public void Start()
    {
        Cell[] cells = FindObjectsOfType<Cell>();

        for (int i = 0; i < cells.Length; i++)
        {
            this.cells[cells[i].Coordinates.x, cells[i].Coordinates.y] = cells[i];
            dict.Add(cells[i].GetComponent<Collider>(), cells[i]);

            this.cells[cells[i].Coordinates.x, cells[i].Coordinates.y] = cells[i];
        }

    }

    private UnityAction<int> action;

    private void FixedUpdate()
    {
        if (active) 
        {
            radius += Time.fixedDeltaTime * speed;
            time += Time.fixedDeltaTime;

            neiboors = GetNeiboor(startPoint, radius);
            for (int i = 0; i < neiboors.Length; i++)
            {
                if (!approves.Contains(neiboors[i]))
                {
                    approves.Add(neiboors[i]);
                    //dict[neiboors[i]].OnActive_1();
                    action?.Invoke(i);
                }
            }
        }
    }
    
    private Collider[] GetNeiboor(Vector3 position, float radius)
    {
        return Physics.OverlapSphere(position, radius, m_WhatIsCell);
    }

    private void ActiveWave()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                cells[i, j].OnStart();
            }
        }

        radius = 0;
        time = 0;
        approves.Clear();
        active = true;
    }

    [Button]
    public void StartAnim_1()
    {
        startPoint = new Vector3(-0.5f, 0, -0.5f);
        action = (i) => dict[neiboors[i]].OnActive_1();
        speed = 20;
        ActiveWave();
    } 
    
    [Button]
    public void StartAnim_2()
    {
        startPoint = new Vector3(-0.5f, 0, -0.5f);
        action = (i) => dict[neiboors[i]].OnActive_2();
        speed = 12;
        ActiveWave();
    }
    

    [Button]
    public void StartAnim_3()
    {
        startPoint = cells[4, 4].TF.position;
        action = (i) => dict[neiboors[i]].OnActive_1();
        speed = 12;
        ActiveWave();
    }
    
    [Button]
    public void StartAnim_4()
    {

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                cells[i, j].OnActive_4(center.position);
            }
        }

        //DOVirtual.DelayedCall(2f, () => Debug.Log("Log_2"));
    }


    [Button]
    public void StartAnim_5()
    {
        startPoint = cells[4, 4].TF.position;
        action = (i) => dict[neiboors[i]].OnActive_1();
        speed = 12;
        ActiveWave();
    }


}
