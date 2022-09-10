// hello
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    [SerializeField] float fov = 90f;
    int rayCount;
    float angle = 0f;
    float startingAngle;
    float angleIncrease;
    [SerializeField] float viewDistance = 50f;

    Mesh mesh;

    [SerializeField] LayerMask blockingLayers;
    [SerializeField] Transform guy;

    // Start is called before the first frame update
    void Start()
    {

        rayCount = (int)fov;

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

        transform.rotation = Quaternion.identity;
    }
    

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateMesh();
    }

    public void SetLookDirection()
    {
        startingAngle = Utilities.GetAngleFromVectorFloat(guy.forward) + (fov * 0.5f);
    }
}
