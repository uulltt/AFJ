using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    public float initialCheckRadius;

    public LayerMask sphereLayer;

    public GameObject closestTarget;
    public float targetDistance;

    RaycastHit hit;

    void Start()
    {
        
    }

    void Update()
    {
        targetDistance = initialCheckRadius;
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, initialCheckRadius, sphereLayer);
        foreach (var hitCollider in hitColliders)
        {
            if (Physics.Raycast(transform.position, (hitCollider.transform.position - transform.position), out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.tag == "Target")
                {
                    if (Vector3.Distance(this.transform.position, hitCollider.transform.position) < targetDistance)
                    {
                        closestTarget = hitCollider.gameObject;
                        targetDistance = Vector3.Distance(this.transform.position, hitCollider.transform.position);
                    }
                }
            }
        }

        Debug.Log(closestTarget);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position, initialCheckRadius);
    }
}


