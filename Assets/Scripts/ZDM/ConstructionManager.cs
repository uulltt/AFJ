using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ConstructionManager : MonoBehaviour
{
    public Construct selectedConstuct;

    public GameObject previewObject;

    public float buildOffset;

    public Grid grid;

    public static Tile selectedTile;

    Tile workingTile;

    Ray ray;
    RaycastHit hit;

    public TMPro.TMP_Text fundsText;

    public UnityEvent FogOn, FogOff;

    public bool isConstructing;

    public Construct testObject;

    public Construct comissary;
    public Construct fireDept;
    public Construct House;
    public Construct MedicalTent;
    public Construct policeDebt;
    public Construct Tent;
    public AudioSource sounds;
    public AudioClip appear, buildSound;

    public bool ShopMenuOpen => fundsText.transform.parent.GetChild(0).gameObject.activeInHierarchy;

    public void Start()
    {
        UpdateFunds();
        sounds = GetComponent<AudioSource>();
    }

    public void UpdateFunds()
    {
        fundsText.text = "$" + Resources.availableFunds.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isConstructing && selectedConstuct == comissary)
            {
                DeselectConstruct();
            }
            else
            {
                SelectConstruct(comissary);
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isConstructing && selectedConstuct == fireDept)
            {
                DeselectConstruct();
            }
            else
            {
                SelectConstruct(fireDept);
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (isConstructing && selectedConstuct == House)
            {
                DeselectConstruct();
            }
            else
            {
                SelectConstruct(House);
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (isConstructing && selectedConstuct == MedicalTent)
            {
                DeselectConstruct();
            }
            else
            {
                SelectConstruct(MedicalTent);
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isConstructing && selectedConstuct == policeDebt)
            {
                DeselectConstruct();
            }
            else
            {
                SelectConstruct(policeDebt);
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (isConstructing && selectedConstuct == Tent)
            {
                DeselectConstruct();
            }
            else
            {
                SelectConstruct(Tent);
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            fundsText.transform.parent.GetChild(0).gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && ShopMenuOpen)
        {
            fundsText.transform.parent.GetChild(0).gameObject.SetActive(false);
        }

        if (isConstructing)
        {
            if (selectedTile != null)
            {
                if (workingTile != selectedTile)
                {
                    if (previewObject == null)
                    {
                        previewObject = Instantiate(selectedConstuct.gameObject, new Vector3(selectedTile.transform.position.x + selectedConstuct.xOffset, selectedTile.transform.position.y + buildOffset, selectedTile.transform.position.z + selectedConstuct.yOffset), Quaternion.identity);
                        if (!grid.OccupancyCheck(selectedConstuct, (int)selectedTile.transform.position.x, (int)selectedTile.transform.position.z))
                        {
                            foreach (Renderer rend in previewObject.GetComponentsInChildren<Renderer>())
                                rend.material.color = new Color(0, 1, 0, .3f);
                        }
                        else
                        {
                            foreach (Renderer rend in previewObject.GetComponentsInChildren<Renderer>())
                                rend.material.color = new Color(1, 0, 0, .3f);
                        }
                    }
                    else
                    {
                        previewObject.transform.position = new Vector3(selectedTile.transform.position.x + selectedConstuct.xOffset, selectedTile.transform.position.y + buildOffset, selectedTile.transform.position.z + selectedConstuct.yOffset);
                        if (!grid.OccupancyCheck(selectedConstuct, (int)selectedTile.transform.position.x, (int)selectedTile.transform.position.z))
                        {
                            foreach(Renderer rend in previewObject.GetComponentsInChildren<Renderer>())
                                rend.material.color = new Color(0, 1, 0, .3f);
                        }
                        else
                        {
                            foreach (Renderer rend in previewObject.GetComponentsInChildren<Renderer>())
                                rend.material.color = new Color(1, 0, 0, .3f);
                        }
                    }
                    workingTile = selectedTile;
                }
            }

            if (Input.GetMouseButtonDown(0) && !ShopMenuOpen)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Tile")
                    {
                        if (selectedConstuct.constructionCost <= Resources.availableFunds)
                        {
                            if (selectedConstuct.xDim == 1 && selectedConstuct.yDim == 1)
                            {
                                if (!hit.collider.gameObject.GetComponent<Tile>().isOccuppied)
                                {
                                    hit.collider.gameObject.GetComponent<Tile>().isOccuppied = true;
                                    GameObject Construct = Instantiate(selectedConstuct.gameObject, new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y + buildOffset, hit.collider.transform.position.z), Quaternion.identity);
                                    Resources.availableFunds -= selectedConstuct.constructionCost;
                                    UpdateFunds();
                                    sounds.PlayOneShot(buildSound);

                                    DeselectConstruct();
                                }
                            }
                            else
                            {
                                if (!grid.OccupancyCheck(selectedConstuct, (int)hit.collider.transform.position.x, (int)hit.collider.transform.position.z))
                                {
                                    grid.Occupy(selectedConstuct, (int)hit.collider.transform.position.x, (int)hit.collider.transform.position.z);
                                    GameObject Construct = Instantiate(selectedConstuct.gameObject, new Vector3(hit.collider.transform.position.x + selectedConstuct.xOffset, hit.collider.transform.position.y + buildOffset, hit.collider.transform.position.z + selectedConstuct.yOffset), Quaternion.identity);
                                    Resources.availableFunds -= selectedConstuct.constructionCost;
                                    Debug.Log(Resources.availableFunds);
                                    UpdateFunds();
                                    sounds.PlayOneShot(buildSound);

                                    DeselectConstruct();
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void DeselectConstruct()
    {
        isConstructing = false;
        Destroy(previewObject);
        previewObject = null;
        FogOn?.Invoke();
    }

    public void SelectConstruct(Construct targetConstruct)
    {
        selectedConstuct = targetConstruct;
        isConstructing = true;
        FogOff?.Invoke();
        sounds.PlayOneShot(appear);
    }
}
