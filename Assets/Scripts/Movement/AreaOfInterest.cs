using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfInterest : MonoBehaviour
{
    public static List<AreaOfInterest> areasOfInterest = new List<AreaOfInterest>();

    private void Awake()
    {
        areasOfInterest.Add(this);
    }

    private void OnDestroy()
    {
        areasOfInterest.Remove(this);
    }
}
