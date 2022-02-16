using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCell
{
    public int x;
    public int y;

    public bool alive;
    bool nextStepAlive;
    public bool hasChanged;

    public GameManager gameManager;

    public LifeCell(int X, int Y, bool ALIVE, GameManager GAMEMANAGER)
    {
        x = X;
        y = Y;
        alive = ALIVE;
        gameManager = GAMEMANAGER;
    }

    public void Active(bool active)
    {
        alive = active;
        if (active)
        {
            gameManager.tilemap.SetTile(new Vector3Int(x, y, 0), gameManager.aliveTile);
            nextStepAlive = true;
            hasChanged = true;
        }
        else
        {
            gameManager.tilemap.SetTile(new Vector3Int(x, y, 0), null);
            hasChanged = false;
        }
    }

    public void Calculate()
    {
        if(gameManager.CountAliveNeighbours(this) < 2 || gameManager.CountAliveNeighbours(this) > 3)
        {
            nextStepAlive = false;
        }
        if(gameManager.CountAliveNeighbours(this) == 3)
        {
            nextStepAlive = true;
        }
        
        hasChanged = alive != nextStepAlive;

        if(alive)
        {
            Debug.Log("WIll I DIE: " + !nextStepAlive + "  Nachbarn: "+ gameManager.CountAliveNeighbours(this));
            foreach (LifeCell lifeCell in gameManager.GetCalculateNeibours(this))
            {
                lifeCell.Calculate();
            }
        }
    }

    public void NextStep()
    {
        if(hasChanged)
        {
            Active(nextStepAlive);
        }
    }
}
