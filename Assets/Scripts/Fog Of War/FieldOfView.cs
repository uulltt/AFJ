using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    float fov = 360f;
    int rayCount = 360;
    float angle = 0f;
    float startingAngle;
    float angleIncrease;
    float viewDistance = 50f;

    Mesh mesh;

    [SerializeField] LayerMask blockingLayers;

    // Start is called before the first frame update
    void Start()
    {
        angleIncrease = fov / rayCount;

        mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;

        UpdateMesh();
    }

    private void UpdateMesh()
    {
        SetLookDirection();

        angle = startingAngle;

        Vector3[] verts = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[verts.Length];
        int[] triangles = new int[rayCount * 3];

        verts[0] = Vector3.zero;

        int triangleIndex = 0;
        int vertexIndex = 1;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;

            if (Physics.Raycast(transform.position, Utilities.GetVectorFromAngle(angle), out RaycastHit hit, viewDistance, blockingLayers))
            {
                vertex = hit.point - transform.position;
            }
            else
            {
                vertex = Utilities.GetVectorFromAngle(angle) * viewDistance;
            }

            verts[i + 1] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;

            angle -= angleIncrease;
        }

        mesh.vertices = verts;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
    

    // Update is called once per frame
    void Update()
    {
        UpdateMesh();
    }

    public void SetLookDirection()
    {
        startingAngle = Utilities.GetAngleFromVectorFloat(transform.forward) - fov * 0.5f;
    }
}
