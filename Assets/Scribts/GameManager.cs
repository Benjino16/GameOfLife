using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public List<LifeCell> lifeCells = new List<LifeCell>();
    public Tilemap tilemap;

    public Tile aliveTile;
    public Tile deadTile;

    public float speed = 3;

    public int xRow;
    public int yRow;

    void Start()
    {
        SetupGrid(xRow, yRow);
        //StartCoroutine(ExampleCoroutine());
    }


    public bool CellIsAlive(int X, int Y)
    {
        return lifeCells.Exists(f => f.x == X && f.y == Y && f.alive);
    }

    public LifeCell GetCell(int X, int Y)
    {
        return lifeCells.Find(f => f.x == X && f.y == Y);
    }

    public void NextStep()
    {
        print("Calculating...");
        foreach (LifeCell lifeCell in lifeCells.FindAll(f => f.alive))
        {
            lifeCell.Calculate();
        }

        print("Appling step to: " + lifeCells.FindAll(f => f.hasChanged).Count);

        foreach(LifeCell lifeCell in lifeCells.FindAll(f => f.hasChanged))
        {
            lifeCell.NextStep();
        }
    }

    private void SetupGrid(int X, int Y)
    {
        for (int x = 0; x < X; x++)
        {
            for (int y = 0; y < Y; y++)
            {
                lifeCells.Add(new LifeCell(x, y, false, this));
            }
        }
    }

    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(speed);
        Debug.Log("Next step...");
        NextStep();
        StartCoroutine(ExampleCoroutine());
    }
    public int CountAliveNeighbours(LifeCell lifeCell)
    {
        if(GetNeighbours(lifeCell).FindAll(f => f.alive) == null)
        {
            return 0;
        }
        return GetNeighbours(lifeCell).FindAll(f => f.alive).Count;
    }

    public List<LifeCell> GetNeighbours(LifeCell lifeCell)
    {
        List<LifeCell> result = new List<LifeCell>();

        result.Add(GetCell(lifeCell.x + 1, lifeCell.y));
        result.Add(GetCell(lifeCell.x - 1, lifeCell.y));
        result.Add(GetCell(lifeCell.x, lifeCell.y + 1));
        result.Add(GetCell(lifeCell.x, lifeCell.y - 1));
        result.Add(GetCell(lifeCell.x + 1, lifeCell.y + 1));
        result.Add(GetCell(lifeCell.x + 1, lifeCell.y - 1));
        result.Add(GetCell(lifeCell.x - 1, lifeCell.y + 1));
        result.Add(GetCell(lifeCell.x - 1, lifeCell.y - 1));

        return result;
    }
    public List<LifeCell> GetCalculateNeibours(LifeCell lifeCell)
    {
        List<LifeCell> result = GetNeighbours(lifeCell);
        result.RemoveAll(f => f == null);
        result.RemoveAll(f => f.alive);
        return result;
    }

    private void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButtonDown(0))
        {
            ToggleActive(((int)transform. position.x), ((int)transform.position.y));
        }
        if(Input.GetKey(KeyCode.Space))
        {
            NextStep();
        }
    }

    public void ToggleActive(int x, int y)
    {
        if (GetCell(x, y) != null) 
        {
            GetCell(x, y).Active(!GetCell(x, y).alive);
        }
    }
}
