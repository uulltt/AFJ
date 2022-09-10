using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionComponent : MonoBehaviour
{
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    private void OnDisable()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    private void OnEnable()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }
}