using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovementControls : MonoBehaviour
{
    Camera main;

    public CharacterLocomotor temp;

    // Start is called before the first frame update
    void Start()
    {
        main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            var cameraRay = main.ScreenPointToRay(Input.mousePosition);

            if( Physics.Raycast(cameraRay, out RaycastHit hit, float.PositiveInfinity, 1<<9))
            {
                temp.MoveTowardsSpot(hit.point);
            }

        }
    }
}
