using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionManager : MonoBehaviour
{
    public GameObject selectedConstuct;

    public GameObject previewObject;

    public float buildOffset;

    public Grid grid;

    public static Tile selectedTile;

    Tile workingTile;

    Ray ray;
    RaycastHit hit;

    public Text fundsText;

    void Start()
    {
        fundsText.text = Resources.availableFunds.ToString();
    }

    void Update()
    {
        if(selectedTile != null)
        {
            if(workingTile != selectedTile)
            {
                if(previewObject == null)
                {
                    previewObject = Instantiate(selectedConstuct, new Vector3(selectedTile.transform.position.x + selectedConstuct.GetComponent<Construct>().xOffset, selectedTile.transform.position.y + buildOffset, selectedTile.transform.position.z + selectedConstuct.GetComponent<Construct>().yOffset), Quaternion.identity);
                    if (!grid.OccupancyCheck(selectedConstuct.GetComponent<Construct>(), (int)selectedTile.transform.position.x, (int)selectedTile.transform.position.z))
                    {
                        previewObject.GetComponent<Renderer>().material.color = new Color(0, 1, 0, .3f);
                    }
                    else
                    {
                        previewObject.GetComponent<Renderer>().material.color = new Color(1, 0, 0, .3f);
                    }
                }
                else
                {
                    previewObject.transform.position = new Vector3(selectedTile.transform.position.x + selectedConstuct.GetComponent<Construct>().xOffset, selectedTile.transform.position.y + buildOffset, selectedTile.transform.position.z + selectedConstuct.GetComponent<Construct>().yOffset);
                    if (!grid.OccupancyCheck(selectedConstuct.GetComponent<Construct>(), (int)selectedTile.transform.position.x, (int)selectedTile.transform.position.z))
                    {
                        previewObject.GetComponent<Renderer>().material.color = new Color(0, 1, 0, .3f);
                    }
                    else
                    {
                        previewObject.GetComponent<Renderer>().material.color = new Color(1, 0, 0, .3f);
                    }
                }
                workingTile = selectedTile;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject.tag == "Tile")
                {
                    if(selectedConstuct.GetComponent<Construct>().constructionCost <= Resources.availableFunds)
                    {
                        if (selectedConstuct.GetComponent<Construct>().xDim == 1 && selectedConstuct.GetComponent<Construct>().yDim == 1)
                        {
                            if (!hit.collider.gameObject.GetComponent<Tile>().isOccuppied)
                            {
                                hit.collider.gameObject.GetComponent<Tile>().isOccuppied = true;
                                GameObject Construct = Instantiate(selectedConstuct, new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y + buildOffset, hit.collider.transform.position.z), Quaternion.identity);
                                Resources.availableFunds -= selectedConstuct.GetComponent<Construct>().constructionCost;
                                fundsText.text = Resources.availableFunds.ToString();
                            }
                        }
                        else
                        {
                            if (!grid.OccupancyCheck(selectedConstuct.GetComponent<Construct>(), (int)hit.collider.transform.position.x, (int)hit.collider.transform.position.z))
                            {
                                grid.Occupy(selectedConstuct.GetComponent<Construct>(), (int)hit.collider.transform.position.x, (int)hit.collider.transform.position.z);
                                GameObject Construct = Instantiate(selectedConstuct, new Vector3(hit.collider.transform.position.x + selectedConstuct.GetComponent<Construct>().xOffset, hit.collider.transform.position.y + buildOffset, hit.collider.transform.position.z + selectedConstuct.GetComponent<Construct>().yOffset), Quaternion.identity);
                                Resources.availableFunds -= selectedConstuct.GetComponent<Construct>().constructionCost;
                                Debug.Log(Resources.availableFunds);
                                fundsText.text = Resources.availableFunds.ToString();
                            }
                        }
                    }
                }
            }
        }
    }
}
