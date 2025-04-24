using System;
using UnityEngine;

public class ProceduralMeshCreationTask2 : BaseMeshCreationTask2
{

    [Header("Cone")]

    [Tooltip("Height of the main cone")]
    [SerializeField] protected float coneHeight = 0.5f;

    [Tooltip("Size of the main cone")]
    [SerializeField] protected float coneRadius = 0.25f;

    [Tooltip("No. of sections that make up the cone")]
    [SerializeField] protected int coneSections = 16;


    protected void Start() {

        GenerateObject();
    }


    private void GenerateObject() {

        // Creating new object
        GameObject generatedObject = new GameObject();
        generatedObject.name = "Object A";

        // Creating mesh
        Mesh generatedCustomMesh = new Mesh();
        generatedCustomMesh.name = "ObjectAMesh";

        CreateCombinedMesh(generatedCustomMesh);

        // Adding render components
        MeshFilter meshFilter = generatedObject.AddComponent<MeshFilter>();
        meshFilter.mesh = generatedCustomMesh;

        MeshRenderer meshRenderer = generatedObject.AddComponent<MeshRenderer>();
        meshRenderer.material = CreateObjectMaterial();
    }

    private void CreateCombinedMesh(Mesh customMesh) {

        // Generate sphere vertices
        Vector3[] sphereVertices = GenerateSphereVertices();
        int[] sphereTriangles = GenerateSphereTriangles();


        // Generate cone vertices
        Vector3[] coneVertices = GenerateConeVertices();
        int[] coneTriangles = GenerateConeTriangles(sphereVertices.Length);


        // Combine vertices and triangles
        Vector3[] combinedVertices = new Vector3[sphereVertices.Length + coneVertices.Length];
        Array.Copy(sphereVertices, 0, combinedVertices, 0, sphereVertices.Length);
        Array.Copy(coneVertices, 0, combinedVertices, sphereVertices.Length, coneVertices.Length);

        int[] combinedTriangles = new int[sphereTriangles.Length + coneTriangles.Length];
        Array.Copy(sphereTriangles, 0, combinedTriangles, 0, sphereTriangles.Length);
        Array.Copy(coneTriangles, 0, combinedTriangles, sphereTriangles.Length, coneTriangles.Length);

        // Calculate normals
        Vector3[] normals = CalculateNormals(combinedVertices, combinedTriangles);

        // Applying to mesh
        customMesh.vertices = combinedVertices;
        customMesh.triangles = combinedTriangles;
        customMesh.normals = normals;
    }
    
    private Vector3[] GenerateConeVertices() {

        // Base vertices + tip + center
        int numVertices = coneSections + 2;
        Vector3[] vertices = new Vector3[numVertices];

        // Cone tip
        vertices[0] = new Vector3(0, 0, sphereRadius + coneHeight);

        // Base center
        vertices[1] = new Vector3(0, 0, sphereRadius);

        // Base vertices
        float deltaAngle = 2 * Mathf.PI / coneSections;
        for (int i = 0; i < coneSections; i++)
        {
            float angle = i * deltaAngle;
            float x = coneRadius * Mathf.Cos(angle);
            float y = coneRadius * Mathf.Sin(angle);
            vertices[i + 2] = new Vector3(x, y, sphereRadius);
        }

        return vertices;
    }

    private int[] GenerateConeTriangles(int vertexOffset) {

        // Sides + base
        int numTriangles = coneSections * 2;
        int[] triangles = new int[numTriangles * 3];

        int index = 0;

        // Sides
        for (int i = 0; i < coneSections; i++)
        {
            // Tip
            triangles[index++] = vertexOffset;
            triangles[index++] = vertexOffset + 2 + i;
            triangles[index++] = vertexOffset + 2 + (i + 1) % coneSections;
        }

        // Base
        for (int i = 0; i < coneSections; i++)
        {
            // Center
            triangles[index++] = vertexOffset + 1;
            triangles[index++] = vertexOffset + 2 + (i + 1) % coneSections;
            triangles[index++] = vertexOffset + 2 + i;
        }

        return triangles;
    }
}