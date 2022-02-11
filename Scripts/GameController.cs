using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject wallPrefab, deadZonePrefab , Path, Walls;
    bool needToBake = true;

    void Start()
    {
        Spawn(wallPrefab, 15, 0.5f);
        Spawn(deadZonePrefab, 10, 0);
    }

    void Spawn(GameObject prefab, int count, float y)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-5, 5), y, Random.Range(-4, 4));
            Instantiate(prefab, pos, Quaternion.identity, Walls.transform);
        }
        if (needToBake)
        {
            Path.GetComponent<GridBehavior>().gridArray = new GameObject[Path.GetComponent<GridBehavior>().columns, Path.GetComponent<GridBehavior>().rows];
            if (Path.GetComponent<GridBehavior>().gridPrefab)
                Path.GetComponent<GridBehavior>().GenerateGrid();
            needToBake = false;
        }
    }

    public void Pause_Continue(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
