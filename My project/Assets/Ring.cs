using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RingFromCube : MonoBehaviour
{
    [SerializeField] private int segments = 30;
    [SerializeField] private float innerRadius = 1f;
    [SerializeField] private float outerRadius = 2f;

    void Start()
    {
        CreateRing();
    }

    void CreateRing()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        Vector3[] vertices = new Vector3[segments * 2];
        int[] triangles = new int[segments * 6];

        for (int i = 0; i < segments; i++)
        {
            float angle = 2 * Mathf.PI * i / segments;
            float x = Mathf.Cos(angle);
            float z = Mathf.Sin(angle);

            vertices[i] = new Vector3(x * innerRadius, 0f, z * innerRadius);
            vertices[i + segments] = new Vector3(x * outerRadius, 0f, z * outerRadius);
        }

        for (int i = 0; i < segments - 1; i++)
        {
            int ti = i * 6;
            int vi = i * 2;

            triangles[ti] = vi;
            triangles[ti + 1] = vi + 1;
            triangles[ti + 2] = vi + segments;

            triangles[ti + 3] = vi + 1;
            triangles[ti + 4] = vi + segments + 1;
            triangles[ti + 5] = vi + segments;
        }

        // Connect the last segment to the first
        int lastTi = (segments - 1) * 6;
        int lastVi = (segments - 1) * 2;
        triangles[lastTi] = lastVi;
        triangles[lastTi + 1] = lastVi + 1;
        triangles[lastTi + 2] = lastVi + segments;

        triangles[lastTi + 3] = lastVi + 1;
        triangles[lastTi + 4] = segments + 1;
        triangles[lastTi + 5] = lastVi + segments;

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
