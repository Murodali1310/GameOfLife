﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static int SCREEN_WIDTH = 64;
    private static int SCREEN_HEIGHT = 48;

    public float speed = 0.2f;

    private float timer = 0;

    public bool simulationEnabled = false;

    Cell[,] grid = new Cell[SCREEN_WIDTH, SCREEN_HEIGHT];
    

    void Start()
    {
        PlaceCells(1);
    }

    void Update()
    {
       if(simulationEnabled)
        {
            if (timer >= speed)
            {
                timer = 0f;
                CountNeighbors();
                PopulationControl();
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        UserInput();

    }


    void UserInput()
    {
        if(Input.GetMouseButton(0))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int x = Mathf.RoundToInt(mousePoint.x);
            int y = Mathf.RoundToInt(mousePoint.y);

            if (x >= 0 && y >= 0 && x < SCREEN_WIDTH && y < SCREEN_WIDTH)
            {
                grid[x, y].SetAlive(!grid[x, y].isAlive);
            }
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            simulationEnabled = false;
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            simulationEnabled = true;
        }
    }


    void PlaceCells(int type)
    {
        if (type == 1)
        {
            for (int y = 0; y < SCREEN_HEIGHT; y++)
            {
                for (int x = 0; x < SCREEN_WIDTH; x++)
                {
                    Cell cell = Instantiate(Resources.Load("Prefabs/Cell", typeof(Cell)), new Vector2(x, y), Quaternion.identity) as Cell;
                    grid[x, y] = cell;

                    grid[x, y].SetAlive(false);
                }
            }

            for(int y = 21; y < 24; y++)
            {
                for(int x = 31; x < 38; ++x)
                {
                    if (x != 34)
                    {
                        if(y == 21 || y == 23)
                        {
                            grid[x, y].SetAlive(true);
                        }   else if (y == 22 && ((x != 32) && (x != 36)))
                        {
                            grid[x, y].SetAlive(true);
                        }
                    }
                }
            }
        } else if (type == 2)
        {
            for(int y = 0; y < SCREEN_HEIGHT; y++)
            {
                for(int x = 0; x < SCREEN_WIDTH; x++)
                {
                    Cell cell = Instantiate(Resources.Load("Prefabs/Cell", typeof(Cell)), new Vector2(x, y), Quaternion.identity) as Cell;
                    grid[x, y] = cell;
                    grid[x, y].SetAlive(RandomAliveCell());
                }
            }
        }
    }


    void CountNeighbors()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                int numNeighbors = 0;
                //North
                if (y + 1 < SCREEN_HEIGHT)
                {
                    if (grid[x, y + 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }
                //East
                if (x + 1 < SCREEN_WIDTH)
                {
                    if (grid[x + 1, y].isAlive)
                    {
                        numNeighbors++;
                    }
                }
                //South
                if (y - 1 >= 0)
                {
                    if (grid[x, y - 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }
                //West
                if (x - 1 >= 0)
                {
                    if (grid[x - 1, y].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                if (x + 1 < SCREEN_WIDTH && y + 1 < SCREEN_HEIGHT)
                {
                    if (grid[x + 1, y + 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                if (x - 1 >= 0 && y + 1 < SCREEN_HEIGHT)
                {
                    if (grid[x - 1, y + 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                if (x + 1 < SCREEN_WIDTH && y - 1 >= 0)
                {
                    if (grid[x + 1, y - 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                if (x - 1 >= 0 && y - 1 >= 0)
                {
                    if (grid[x - 1, y - 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                grid[x, y].numNeighbors = numNeighbors;
            }
        }
    }


    void PopulationControl()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                // Правила для живых клеток
                if (grid[x, y].isAlive)
                {
                    // Клетка умирает, если у нее меньше 2 или больше 3 соседей
                    if (grid[x, y].numNeighbors != 2 && grid[x, y].numNeighbors != 3)
                    {
                        grid[x, y].SetAlive(false);
                    }
                }
                // Правила для мертвых клеток
                else
                {
                    // Клетка оживает, если у нее ровно 3 живых соседа
                    if (grid[x, y].numNeighbors == 3)
                    {
                        grid[x, y].SetAlive(true);
                    }
                }
            }
        }
    }

    // Метод для случайного определения, будет ли клетка живой в начале
    bool RandomAliveCell()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        return rand > 75; // 25% вероятность, что клетка будет живой
    }
}
