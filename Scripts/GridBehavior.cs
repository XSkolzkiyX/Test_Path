using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    public int rows = 10, columns = 10, scale = 1, startX = 0, startY = 0, endX = 2, endY = 2;
    public GameObject gridPrefab, Walls;
    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
    public GameObject[,] gridArray;
    public List<GameObject> path = new List<GameObject>();

    public void GenerateGrid()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                foreach (Transform wall in Walls.transform)
                {
                    if (wall.position == new Vector3(leftBottomLocation.x + scale * i, leftBottomLocation.y + 0.5f, leftBottomLocation.z + scale * j))
                        if (j < rows - 1) j++;
                }
                GameObject obj = Instantiate(gridPrefab, new Vector3(leftBottomLocation.x + scale * i, leftBottomLocation.y + 0.4f, leftBottomLocation.z + scale * j), Quaternion.identity, transform);
                obj.GetComponent<GridStats>().x = i;
                obj.GetComponent<GridStats>().y = j;
                gridArray[i, j] = obj;
            }
        }
    }

    public void SetDistance()
    {
        InitialSetUp();
        int x = startX;
        int y = startY;
        int[] testArray = new int[rows * columns];
        for(int step = 1; step<rows*columns;step++)
        {
            foreach(GameObject obj in gridArray)
            {
                if (obj && obj.GetComponent<GridStats>().visited == step - 1) TestFourDirection(obj.GetComponent<GridStats>().x, obj.GetComponent<GridStats>().y, step);
            }
        }
    }

    public void SetPath()
    {
        int step;
        int x = endX;
        int y = endY;
        List<GameObject> tempList = new List<GameObject>();
        path.Clear();
        if (gridArray[endX, endY] && gridArray[endX, endY].GetComponent<GridStats>().visited > 0)
        {
            path.Add(gridArray[x, y]);
            step = gridArray[x, y].GetComponent<GridStats>().visited - 1;
        }
        else return;
        for (int i = step; step > -1; step--)
        {
            if (TestDirection(x, y, step, 1)) tempList.Add(gridArray[x, y + 1]);
            if (TestDirection(x, y, step, 2)) tempList.Add(gridArray[x + 1, y]);
            if (TestDirection(x, y, step, 3)) tempList.Add(gridArray[x, y - 1]);
            if (TestDirection(x, y, step, 4)) tempList.Add(gridArray[x - 1, y]);
            GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
            path.Add(tempObj);
            x = tempObj.GetComponent<GridStats>().x;
            y = tempObj.GetComponent<GridStats>().y;
            tempList.Clear();
        }
    }

    void InitialSetUp()
    {
        foreach(GameObject obj in gridArray)
        {
            if(obj) obj.GetComponent<GridStats>().visited = -1;
        }
        gridArray[startX, startY].GetComponent<GridStats>().visited = 0;
    }

    bool TestDirection(int x, int y, int step, int direction)
    {
        switch (direction)
        {
            case 1:
                if (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridStats>().visited == step)
                    return true;
                else return false;
            case 2:
                if (x + 1 < columns && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<GridStats>().visited == step)
                    return true;
                else return false;
            case 3:
                if (y - 1 > -1 && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridStats>().visited == step)
                    return true;
                else return false;
            case 4:
                if (x - 1 > -1 && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GridStats>().visited == step)
                    return true;
                else return false;
        }
        return false;
    }

    void TestFourDirection(int x, int y, int step)
    {
        if (TestDirection(x, y, -1, 1))
            SetVisited(x, y+1, step);
        if (TestDirection(x, y, -1, 2))
            SetVisited(x+1, y, step);
        if (TestDirection(x, y, -1, 3))
            SetVisited(x, y-1, step);
        if (TestDirection(x, y, -1, 4))
            SetVisited(x - 1, y, step);
    }

    void SetVisited(int x, int y, int step)
    {
        if (gridArray[x, y])
            gridArray[x, y].GetComponent<GridStats>().visited = step;
    }

    GameObject FindClosest(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = scale * rows * columns;
        int index = 0;
        for(int i = 0; i<list.Count; i++)
        {
            if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                index = i;
            }
        }
        return list[index];
    }
}
