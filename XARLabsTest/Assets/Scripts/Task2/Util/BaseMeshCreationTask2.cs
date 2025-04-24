using UnityEngine;

public class BaseMeshCreationTask2 : MonoBehaviour
{
    [Header("Sphere")]

    [Tooltip("Size of the main sphere")]
    [SerializeField] protected float sphereRadius = 1f;

    [Tooltip("No. of sections that make up the sphere")]
    [SerializeField] protected int sphereSections = 32;

    public Vector3[] GenerateSphereVertices() {

        // Calculate total number of vertices for the sphere
        int vertexCount = (sphereSections + 1) * (sphereSections + 1);
        Vector3[] vertices = new Vector3[vertexCount];

        // Calculate angle increments for latitude (theta) and longitude (phi)
        float deltaTheta = Mathf.PI / sphereSections;
        float deltaPhi = 2 * Mathf.PI / sphereSections;

        int index = 0;

        // Loop through latitude
        for (int i = 0; i <= sphereSections; i++)
        {
            // Calculate current latitude angle and its sine/cosine
            float theta = i * deltaTheta;
            float sinTheta = Mathf.Sin(theta);
            float cosTheta = Mathf.Cos(theta);

            // Loop through longitude
            for (int j = 0; j <= sphereSections; j++)
            {
                // Calculate current longitude angle and its sine/cosine
                float phi = j * deltaPhi;
                float sinPhi = Mathf.Sin(phi);
                float cosPhi = Mathf.Cos(phi);

                // Convert from spherical to Cartesian coordinates
                float x = sphereRadius * sinTheta * cosPhi;
                float y = sphereRadius * sinTheta * sinPhi;
                float z = sphereRadius * cosTheta;

                // Store the position
                vertices[index++] = new Vector3(x, y, z);
            }
        }

        return vertices;
    }

    public int[] GenerateSphereTriangles() {

        // Each grid cell in the sphere needs 2 triangles
        int numTriangles = 2 * sphereSections * sphereSections;

        // Each triangle needs 3 indices, so multiply by 3
        int[] triangles = new int[numTriangles * 3];

        int index = 0;

        // Loop through each grid cell
        for (int i = 0; i < sphereSections; i++)
        {
            for (int j = 0; j < sphereSections; j++)
            {
                // Calculate the index of the current vertex in the flattened vertex array
                int current = i * (sphereSections + 1) + j;
                int next = current + (sphereSections + 1);

                // Create two triangles for each grid cell:

                // First triangle
                triangles[index++] = current;
                triangles[index++] = next;
                triangles[index++] = current + 1;

                // Second triangle
                triangles[index++] = current + 1;
                triangles[index++] = next;
                triangles[index++] = next + 1;
            }
        }

        return triangles;
    }

    public Vector3[] CalculateNormals(Vector3[] vertices, int[] triangles) {

        // Create an array to store normal vectors for each vertex
        Vector3[] normals = new Vector3[vertices.Length];

        // Calculate normals for each triangle and accumulate them at each vertex
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int indexA = triangles[i];
            int indexB = triangles[i + 1];
            int indexC = triangles[i + 2];

            // Get the vertex positions
            Vector3 pointA = vertices[indexA];
            Vector3 pointB = vertices[indexB];
            Vector3 pointC = vertices[indexC];

            // Calculate the normal vector for this triangle using cross product
            // The cross product of two edges gives a vector perpendicular to the triangle face
            Vector3 normal = Vector3.Cross(pointB - pointA, pointC - pointA).normalized;

            // Add this normal to each vertex of the triangle
            normals[indexA] += normal;
            normals[indexB] += normal;
            normals[indexC] += normal;
        }

        // Normalize accumulated normals
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = normals[i].normalized;
        }

        return normals;
    }

    public Material CreateObjectMaterial() {

        // Default URP material
        Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        return material;
    }
}
