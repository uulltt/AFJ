using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class GlobalSelection : MonoBehaviour
{
    public static GlobalSelection instance;

    SelectedDictionary selectedTable;
    RaycastHit hit;

    bool dragSelect;

    int layerMask;

    Vector3 p1;
    Vector3 p2;

    Vector2[] corners;
    Vector3[] verts;
    Vector3[] vecs;

    Camera main;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }

        instance = this;

        main = Camera.main;
    }

    void Start()
    {
        selectedTable = GetComponent<SelectedDictionary>();
        dragSelect = false;
        layerMask = LayerMask.GetMask("ControlledUnits");
    }

    void Update()
    {
        // When left mouse button clicked (but not released)
        if (Input.GetMouseButtonDown(0))
        {
            p1 = Input.mousePosition;
        }

        // While left mouse button held
        if (Input.GetMouseButton(0))
        {
            if ((p1 - Input.mousePosition).magnitude > 40)
            {
                dragSelect = true;
            }
        }

        // When mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            if (dragSelect == false) //single select
            {
                Ray ray = Camera.main.ScreenPointToRay(p1);

                if (Physics.Raycast(ray, out hit, 50000.0f, layerMask))
                {
                    // inclusive select
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        // if guy is in list, then remove him, else add to list
                        if (UnitMovementControls.selectedCharacters.Contains(hit.transform.GetComponent<CharacterLocomotor>()))
                        {
                            UnitMovementControls.selectedCharacters.Remove(hit.transform.GetComponent<CharacterLocomotor>());
                        }
                        else
                        {
                            UnitMovementControls.selectedCharacters.Add(hit.transform.GetComponent<CharacterLocomotor>());
                        }
                    }
                    // exclusive select
                    else
                    {
                        UnitMovementControls.selectedCharacters.Clear();
                        UnitMovementControls.selectedCharacters.Add(hit.transform.GetComponent<CharacterLocomotor>());
                        Debug.Log("guys EXCLUSIVE CLICK selected");
                    }
                }
                else
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        //do nothing
                    }
                    else
                    {
                        UnitMovementControls.selectedCharacters.Clear();
                        Debug.Log("guys EXCLUSIVE CLICK deselected");
                    }
                }
            }
            // drag select
            else
            {
                verts = new Vector3[4];
                vecs = new Vector3[4];
                p2 = Input.mousePosition;
                Vector3 boxCenter = GetBoxCenter(main.ScreenToWorldPoint(p1), main.ScreenToWorldPoint(p2));
                Vector3 halfExtants = GetHalfExtants(main.ScreenToWorldPoint(p1), main.ScreenToWorldPoint(p2));
                Vector3 direction = main.transform.forward;
                Quaternion orientation = Quaternion.LookRotation(main.transform.forward);

                /*
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                cube.position = boxCenter;
                cube.localScale = halfExtants + new Vector3(0, 0, 50000.0f);
                cube.forward = direction;
                */

                var hits = Physics.BoxCastAll(boxCenter, halfExtants, direction, orientation, 50000.0f, layerMask);

                //Debug.DrawLine(boxCenter, hit.point, Color.red, 1.0f);

                if (hits != null)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        for (int i = 0; i < hits.Length; i++)
                        {
                            UnitMovementControls.selectedCharacters.Add(hits[i].transform.GetComponent<CharacterLocomotor>());
                            Debug.Log("drag hit SHIFT");
                        }
                    }
                    else
                    {
                        UnitMovementControls.selectedCharacters.Clear();
                        for (int i = 0; i < hits.Length; i++)
                        {
                            UnitMovementControls.selectedCharacters.Add(hits[i].transform.GetComponent<CharacterLocomotor>());
                            Debug.Log("drag hit");
                        }
                    }
                }
            }

            dragSelect = false;
        }
    }

    private void OnGUI()
    {
        if (dragSelect == true)
        {
            var rect = Utils.GetScreenRect(p1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    Vector3 GetBoxCenter(Vector3 p1, Vector3 p2)
    {
        Vector3 bottomLeft = Vector3.Min(p1, p2);
        Vector3 topRight   = Vector3.Max(p1, p2);
        Vector3 boxCenter = (topRight - bottomLeft) / 2 + bottomLeft;

        return boxCenter;
    }

    Vector3 GetHalfExtants(Vector3 p1, Vector3 p2)
    {
        Vector3 bottomLeft = Vector3.Min(p1, p2);
        Vector3 topRight   = Vector3.Max(p1, p2);

        Vector3 halfExtants = (topRight - bottomLeft) / 2;

        return halfExtants;
    }
}