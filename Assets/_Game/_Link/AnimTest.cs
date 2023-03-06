using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimTest : MonoBehaviour
{
    [ShowInInspector] public Cell[,] cells = new Cell[10,10];
    Dictionary<Collider, Cell> dict = new Dictionary<Collider, Cell>();
    [SerializeField] float time;
    List<Collider> approves = new List<Collider>();
    Collider[] neiboors;
    [SerializeField] float speed = 10.0f;
    float radius = 0;

    [Button]
    public void OnInit()
    {
        Cell[] cells = FindObjectsOfType<Cell>();

        for (int i = 0; i < cells.Length; i++)
        {
            this.cells[cells[i].Coordinates.x, cells[i].Coordinates.y] = cells[i];
            dict.Add(cells[i].GetComponent<Collider>(), cells[i]);
        }
    }

    [Button]
    public void Active()
    {
        active = true;
    }

    [Button]
    public void Continue()
    {
        targetTime += 0.1f;
        active = true;
        Time.timeScale = 1;
    }

    private IEnumerator IEActive(Cell cell)
    {

        while (radius < 15)
        {
            neiboors = GetNeiboor(cell.transform.position, radius);
            for (int i = 0; i < neiboors.Length; i++)
            {
                if (!approves.Contains(neiboors[i]))
                {
                    approves.Add(neiboors[i]);
                    dict[neiboors[i]].OnActive();
                }
            }
            
            radius += Time.deltaTime * speed;
            time += Time.deltaTime;
            yield return null; 
        }
    }

    public float targetTime;
    bool active = false;

    private void FixedUpdate()
    {
        if (active)
        {
            radius += Time.fixedDeltaTime * speed;
            time += Time.fixedDeltaTime;

            neiboors = GetNeiboor(cells[0, 0].transform.position, radius);
            for (int i = 0; i < neiboors.Length; i++)
            {
                if (!approves.Contains(neiboors[i]))
                {
                    approves.Add(neiboors[i]);
                    dict[neiboors[i]].OnActive();
                }
            }

            if (time >= targetTime)
            {
                active = false;
                Time.timeScale = 0f;
                Continue();
            }
        }
    }

    private List<Cell> GetNeiboor(int x,int y)
    {
        List<Cell> list = new List<Cell>();

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i < 0 || j < 0 || i >= 10 || j >= 10 || (i == x && j == y))
                {
                    continue;
                }

                list.Add(cells[i,j]);
            }
        }

        return list;
    }

    
    private Collider[] GetNeiboor(Vector3 position, float radius)
    {
        return Physics.OverlapSphere(position, radius);
    }


}
