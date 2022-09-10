using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vendor Item", menuName = "ScriptableObjects/Vendor Item", order = 1)]
public class VendorItem : ScriptableObject
{
    public GameObject objectReference;
    public string description;
    public int price;
}
