using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningSystem : MonoBehaviour
{
    public GameObject selectedUnit;
    public float spawningLevel;

    public GameObject enemyPrefab;
    public int enemyCount;

    public List<Transform> invasionPoints;

    public GameObject civilianPrefab;
    public int civilianCount;

    int random;

    bool isSpawning;

    Ray ray;
    RaycastHit hit;

    public GameObject testSpawn;

    void Update()
    {
        if (isSpawning)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    CompletePlayerSpawn(hit.point);
                }
            }
        }

        BeginPlayerSpawn(testSpawn);
    }

    public void BeginPlayerSpawn(GameObject newUnit)
    {
        selectedUnit = newUnit;
        isSpawning = true;
    }

    public void CompletePlayerSpawn(Vector3 location)
    {
        GameObject unit = Instantiate(selectedUnit, new Vector3(location.x, spawningLevel, location.z), Quaternion.identity);
        EndPlayerSpawn();
    }

    public void EndPlayerSpawn()
    {
        isSpawning = false;
        selectedUnit = null;
    }

    public void SpawnCivilians()
    {
        for(int i = 0; i < civilianCount; i++)
        {
            random = Random.Range(0, AreaOfInterest.areasOfInterest.Count);
            GameObject civi = Instantiate(civilianPrefab, AreaOfInterest.areasOfInterest[random].transform.position, Quaternion.identity);
        }
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            random = Random.Range(0, invasionPoints.Count);
            GameObject enemy = Instantiate(enemyPrefab, invasionPoints[random].transform.position, Quaternion.identity);
        }
    }
}
