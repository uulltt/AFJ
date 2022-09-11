using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour
{
    public static int availableFunds = 1000000;
    public List<VendorItem> Inventory;

    public static Resources Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        Inventory = new List<VendorItem>();
    }
}
