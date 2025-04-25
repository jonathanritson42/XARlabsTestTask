using System;
using UnityEngine;

public class SecondaryObjectMeshCreationTask3 : BaseMeshCreationTask3
{
    [Tooltip("World position of sphere")]
    [SerializeField] protected Vector3 spherePositon;

    [Tooltip("Colour of sphere")]
    [SerializeField] protected Color sphereColor;

    protected void Start() {

        GenerateSecondaryObject();
    }

    private void GenerateSecondaryObject() {

        // Creating new object
        GameObject generatedObject = new GameObject();
        generatedObject.name = "Object B";

        // Creating mesh
        Mesh generatedCustomMesh = new Mesh();
        generatedCustomMesh.name = "ObjectBMesh";

        CreateMesh(generatedCustomMesh);

        // Adding render components
        MeshFilter meshFilter = generatedObject.AddComponent<MeshFilter>();
        meshFilter.mesh = generatedCustomMesh;

        MeshRenderer meshRenderer = generatedObject.AddComponent<MeshRenderer>();
        meshRenderer.material = CreateObjectMaterial();

        // Adding colour
        meshRenderer.material.color = sphereColor;

        if (!useAnimation) return;

        lissajousAnimation = generatedObject.AddComponent<LissajousAnimation>();
        lissajousAnimation.startPosition = spherePositon;

        if (!useRandom) return;

        RandomLissajouseAnimation(lissajousAnimation);
    }

    private void CreateMesh(Mesh customMesh) {

        // Generate sphere vertices
        Vector3[] sphereVertices = GenerateSphereVertices();
        int[] sphereTriangles = GenerateSphereTriangles();

        // Calculate normals
        Vector3[] normals = CalculateNormals(sphereVertices, sphereTriangles);

        // Applying to mesh
        customMesh.vertices = sphereVertices;
        customMesh.triangles = sphereTriangles;
        customMesh.normals = normals;
    }
}
