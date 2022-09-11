using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawningSystem : MonoBehaviour
{
    public GameObject selectedUnit;
    public float spawningLevel;

    public GameObject enemyPrefab;
    public int enemyCount;

    public List<Transform> invasionPoints;

    public GameObject[] civilianPrefab;
    public int civilianCount;

    int random;

    bool isSpawning;

    Ray ray;
    RaycastHit hit;

    public GameObject testSpawn;

    public float sensitivity;
    private Vector3 mouseReference;
    private Vector3 mouseOffset;
    private Vector3 rotation;
    private bool isRotating;

    private Vector3 newRotation;

    public bool isCamera;
    public float cameraHeight;

    private GameObject previewObject;
    public VendorItem lastSelected;
    public UnityEvent endSpawnEvent;

    void Update()
    {
        if (isSpawning)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (isRotating)
            {
                mouseOffset = (Input.mousePosition - mouseReference);

                rotation.y = -(mouseOffset.x + mouseOffset.y) * sensitivity;

                if (previewObject != null)
                {
                    previewObject.transform.Rotate(rotation);


                    newRotation = previewObject.transform.localRotation.eulerAngles;

                }
                mouseReference = Input.mousePosition;

                if (Input.GetMouseButtonUp(0))
                {
                    isRotating = false;
                    CompletePlayerSpawn(hit.point);

                }
            }
            else if (Physics.Raycast(ray, out hit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isRotating = true;
                    mouseReference = Input.mousePosition;
                }
                else
                {
                    if (previewObject == null)
                    {
                        if (isCamera)
                        {
                            previewObject = Instantiate(selectedUnit, new Vector3(hit.point.x, cameraHeight, hit.point.z), Quaternion.identity);
                        }
                        else
                        {
                            previewObject = Instantiate(selectedUnit, new Vector3(hit.point.x, spawningLevel, hit.point.z), Quaternion.identity);
                        }
                    }
                    else
                    {
                        if (isCamera)
                        {
                            previewObject.transform.position = new Vector3(hit.point.x, cameraHeight, hit.point.z);
                        }
                        else
                        {
                            previewObject.transform.position = new Vector3(hit.point.x, spawningLevel, hit.point.z);
                        }
                    }
                }
            }
        }
    }

    public void BeginPlayerSpawn(GameObject newUnit, bool cameraBool)
    {
        selectedUnit = newUnit;
        isSpawning = true;
        isCamera = cameraBool;
    }

    public void BeginSpawn(VendorItem newUnit)
    {
        if (Resources.Instance.Inventory.Contains(newUnit))
        {
            if (newUnit.objects != null && newUnit.objects.Count > 0)
            {
                selectedUnit = newUnit.objects[Random.Range(0, newUnit.objects.Count)];
            }
            else
            {
                selectedUnit = newUnit.objectReference;
            }
            isSpawning = true;
            isCamera = newUnit.objectReference.name.ToLower().Contains("camera");
            lastSelected = newUnit;
        }
    }

    public void CompletePlayerSpawn(Vector3 location)
    {
        if (isCamera)
        {
            GameObject camera = Instantiate(selectedUnit, new Vector3(location.x, cameraHeight, location.z), Quaternion.identity);
        }
        else
        {
            GameObject unit = Instantiate(selectedUnit, new Vector3(location.x, spawningLevel, location.z), Quaternion.identity);
        }
        EndPlayerSpawn();
    }

    public void EndPlayerSpawn()
    {
        isSpawning = false;
        selectedUnit = null;
        Destroy(previewObject);
        if (lastSelected != null)
        Resources.Instance.Inventory.Remove(lastSelected);
        endSpawnEvent?.Invoke();
    }

    public void SpawnCivilians()
    {
        for(int i = 0; i < civilianCount; i++)
        {
            random = Random.Range(0, AreaOfInterest.areasOfInterest.Count);
            GameObject civi = Instantiate(civilianPrefab[Random.Range(0, civilianPrefab.Length)], AreaOfInterest.areasOfInterest[random].transform.position, Quaternion.identity);
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
