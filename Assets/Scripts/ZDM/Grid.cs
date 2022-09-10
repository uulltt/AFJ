using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int xDim, yDim;
    public Tile[,] grid;

    public float tileScale;

    public GameObject tileObject;

    void Start()
    {
        grid = new Tile[xDim, yDim];
        for(int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < xDim; y++)
            {
                GameObject tile = Instantiate(tileObject, new Vector3(x* tileScale, 0, y* tileScale), Quaternion.identity);
                tile.transform.localScale = new Vector3(tileScale, 0.05f, tileScale);
                grid[x,y] = tile.GetComponent<Tile>();
                tile.transform.parent = gameObject.transform;
            }
        }
    }

    public bool OccupancyCheck(Construct selectedConstruct, int xStart, int yStart)
    {
        if (xStart + selectedConstruct.xDim > xDim|| yStart + selectedConstruct.yDim > yDim)
        {
            Debug.Log("A");
            return true;
        }

        for (int x = xStart; x < xStart + selectedConstruct.xDim; x++)
        {
            for (int y = yStart; y < yStart + selectedConstruct.yDim; y++)
            {
                if(grid[x, y].isOccuppied)
                {
                    Debug.Log("B");
                    return true;
                }
            }
        }
        return false;
    }

    public void Occupy(Construct selectedConstruct, int xStart, int yStart)
    {
        Debug.Log("Nah");
        for (int x = xStart; x < xStart + selectedConstruct.xDim; x++)
        {
            for (int y = yStart; y < yStart + selectedConstruct.yDim; y++)
            {
                Debug.Log("Nah");
                grid[x, y].isOccuppied = true;
            }
        }
    }
}
