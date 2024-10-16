using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Manager : MonoBehaviour
{
    int generationNumber = 1;
    const float TimeToWaitForNextGeneration = 2.5f;

    float elapsedTimeSinceLastGeneration = 0;

    [SerializeField]
    TMP_Text generationNumberText;

    const int GridSizeX = 20, GridSizeY = 20;

    GameObject[,] gridVisuals;

    CellData[,] gridData;

    void Start()
    {
        gridVisuals = new GameObject[GridSizeX, GridSizeY];

        Texture2D spriteTex = Resources.Load<Texture2D>("Square");

        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeX; y++)
            {
                #region Create Cell

                //Instantiate(Resources.Load<GameObject>("Cell"));
                GameObject cell = new GameObject("Cell " + x + "," + y);
                SpriteRenderer sr = cell.AddComponent<SpriteRenderer>();
                sr.sprite = Sprite.Create(spriteTex, new Rect(0, 0, 256, 256), new Vector2(0.5f, 0.5f), 256);
                cell.transform.position = new Vector3(x - GridSizeX / 2, y - GridSizeY / 2, 0);
                sr.color = Color.black;
                gridVisuals[x, y] = cell;
                #endregion


            }
        }

        gridVisuals[5, 5].GetComponent<SpriteRenderer>().color = Color.white;
        gridVisuals[5, 6].GetComponent<SpriteRenderer>().color = Color.blue;




        gridData = new CellData[GridSizeX, GridSizeY];

        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeX; y++)
            {
                gridData[x, y] = new CellData();
            }
        }

        gridData[4, 4].isAlive = true;
        gridData[4, 5].isAlive = true;
        gridData[4, 6].isAlive = true;

        // gridData[4, 4].isAlive = true;
        // gridData[3, 4].isAlive = true;

        // for (int i = 0; i < GridSizeX; i++)
        // {
        //     gridData[i, 15].isAlive = true;
        // }



    }

    void Update()
    {
        elapsedTimeSinceLastGeneration += Time.deltaTime;

        if (elapsedTimeSinceLastGeneration >= TimeToWaitForNextGeneration)
        {
            generationNumber++;
            elapsedTimeSinceLastGeneration -= TimeToWaitForNextGeneration;

            generationNumberText.text = "Generation #" + generationNumber;

            #region Process Generation On Model Data

            for (int x = 0; x < GridSizeX; x++)
            {
                for (int y = 0; y < GridSizeX; y++)
                {
                    gridData[x, y].isAliveNextGeneration = DetermineIfCellIsAliveNextGeneration(x, y);
                }
            }

            for (int x = 0; x < GridSizeX; x++)
            {
                for (int y = 0; y < GridSizeX; y++)
                {
                    gridData[x, y].isAlive = gridData[x, y].isAliveNextGeneration;
                    gridData[x, y].isAliveNextGeneration = false;
                }
            }

            #endregion
        }

        #region Update Visuals

        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeX; y++)
            {
                if (gridData[x, y].isAlive)
                    gridVisuals[x, y].GetComponent<SpriteRenderer>().color = Color.white;
                else
                    gridVisuals[x, y].GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }

        #endregion


        //
        //mess around with foreach on double array


    }

    public bool DetermineIfCellIsAliveNextGeneration(int x, int y)
    {
        int liveNeighbourCount = 0;

        if (x < GridSizeX - 2)
        {
            if (gridData[x + 1, y].isAlive)
                liveNeighbourCount++;
        }

        if (y < GridSizeY - 2)
        {
            if (gridData[x, y + 1].isAlive)
                liveNeighbourCount++;
        }

        if (x > 0)
        {
            if (gridData[x - 1, y].isAlive)
                liveNeighbourCount++;
        }

        if (y > 0)
        {
            if (gridData[x, y - 1].isAlive)
                liveNeighbourCount++;
        }





        if (x < GridSizeX - 2 && y < GridSizeY - 2)
        {
            if (gridData[x + 1, y + 1].isAlive)
                liveNeighbourCount++;
        }

        if (x > 0 && y > 0)
        {
            if (gridData[x - 1, y - 1].isAlive)
                liveNeighbourCount++;
        }

        if (x < GridSizeX - 2 && y > 0)
        {
            if (gridData[x + 1, y - 1].isAlive)
                liveNeighbourCount++;
        }

        if (x > 0 && y < GridSizeY - 2)
        {
            if (gridData[x - 1, y + 1].isAlive)
                liveNeighbourCount++;
        }



        bool cellIsAlive = gridData[x, y].isAlive;

        //Debug.Log("liveNeighbourCount == " + liveNeighbourCount);

        if (cellIsAlive && liveNeighbourCount < 2)
        {
            Debug.Log("assumpiton check 1");
            return false;
        }
        else if (cellIsAlive && (liveNeighbourCount == 2 || liveNeighbourCount == 3))
        {
            Debug.Log("assumpiton check 2");
            return true;
        }
        else if (cellIsAlive && liveNeighbourCount > 3)
        {
            Debug.Log("assumpiton check 3");
            return false;
        }
        else if (!cellIsAlive && liveNeighbourCount == 3)
        {
            Debug.Log("assumpiton check 4");
            return true;
        }


        // Any live cell with fewer than two live neighbours dies, as if by underpopulation.
        // Any live cell with two or three live neighbours lives on to the next generation.
        // Any live cell with more than three live neighbours dies, as if by overpopulation.
        // Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.



        // if(liveNeighbourCount < 2)
        //     return false;
        // else if(liveNeighbourCount == 2 && !gridData[x, y].isAlive)
        //     return false;
        // else if(liveNeighbourCount <= 3)
        //     return true;
        // else if(liveNeighbourCount > 3)
        //     return false;


        return false;
    }

}


public class CellData
{
    public bool isAlive;
    public bool isAliveNextGeneration;
}


