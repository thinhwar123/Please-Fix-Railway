using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AnimTest : MonoBehaviour
{
    Cell[,] cells = new Cell[10,10];
    Dictionary<Collider, Cell> dict = new Dictionary<Collider, Cell>();
    List<Collider> approves = new List<Collider>();

    Collider[] neiboors;
    [SerializeField] float speed = 10.0f;
    [SerializeField] float time;
    float radius = 0;
    Cell initCell;

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

            neiboors = GetNeiboor(initCell.transform.position, radius);
            for (int i = 0; i < neiboors.Length; i++)
            {
                if (!approves.Contains(neiboors[i]))
                {
                    approves.Add(neiboors[i]);
                    dict[neiboors[i]].OnActive_1();
                    action?.Invoke(i);
                }
            }
        }
    }
    
    private Collider[] GetNeiboor(Vector3 position, float radius)
    {
        return Physics.OverlapSphere(position, radius);
    }

    private void Reset()
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
        initCell = this.cells[0, 0];
        action = (i) => dict[neiboors[i]].OnActive_1();
        speed = 20;
        Reset();
    } 
    
    [Button]
    public void StartAnim_2()
    {
        initCell = this.cells[0, 0];
        action = (i) => dict[neiboors[i]].OnActive_2();
        speed = 12;
        Reset();
    }
}
