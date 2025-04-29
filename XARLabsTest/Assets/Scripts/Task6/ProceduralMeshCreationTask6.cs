using System;
using UnityEngine;

public class ProceduralMeshCreationTask6 : BaseMeshCreationTask6
{

    [Header("Cone")]

    [Tooltip("Height of the main cone")]
    [SerializeField] protected float coneHeight = 0.5f;

    [Tooltip("Size of the main cone")]
    [SerializeField] protected float coneRadius = 0.25f;

    [Tooltip("No. of sections that make up the cone")]
    [SerializeField] protected int coneSections = 16;


    [Header("Rotation")]

    [SerializeField] protected SecondaryObjectMeshCreationTask6 targetObject;

    [SerializeField] protected bool useRotation;

    [SerializeField] protected Vector3 rotationSpeed;


    [Header("Colour Change")]

    [SerializeField] protected Color forwardColour = Color.red;
    [SerializeField] protected Color backwardColour = Color.blue;


    [Header("Perlin Noise")]

    [SerializeField] protected bool useNoise;
    [SerializeField] protected float displacementValue;
    [SerializeField] protected float noiseValue;

    private Mesh generatedCustomMesh;
    private Mesh perlinNoiseMesh;
    private Vector3[] originalVertices;
    private Vector3[] originalNormals;

    private GameObject generatedObject;
    public GameObject GeneratedObject => generatedObject;
    private MeshRenderer generatedObjectRenderer;

    protected void Start() {

        GenerateObject();

        if (!useNoise) return;

        GenerateNoise();
    }

    protected void Update()
    {
        // Moved object check to allow for colour change to be separate from rotation
        if (targetObject.GeneratedObject == null || generatedObject == null) return;

        Vector3 directionToGenerated = generatedObject.transform.position - targetObject.GeneratedObject.transform.position;
        directionToGenerated.Normalize();

        generatedObjectRenderer.material.color = Vector3.Dot(generatedObject.transform.forward, directionToGenerated) > 0 ? forwardColour : backwardColour;

        if (!useRotation) return;
        

        Vector3 direction = targetObject.GeneratedObject.transform.position - generatedObject.transform.position;

        // Buffer to stop jittering
        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        Vector3 currentEuler = generatedObject.transform.rotation.eulerAngles;
        Vector3 targetEuler = targetRotation.eulerAngles;

        float newX = Mathf.MoveTowardsAngle(currentEuler.x, targetEuler.x, rotationSpeed.x * Time.deltaTime);
        float newY = Mathf.MoveTowardsAngle(currentEuler.y, targetEuler.y, rotationSpeed.y * Time.deltaTime);
        float newZ = Mathf.MoveTowardsAngle(currentEuler.z, targetEuler.z, rotationSpeed.z * Time.deltaTime);

        generatedObject.transform.rotation = Quaternion.Euler(newX, newY, newZ);
    }

    private void GenerateObject() {

        // Creating new object
        generatedObject = new GameObject();
        generatedObject.name = "Object A";
        generatedObject.transform.parent = transform;

        // Creating mesh
        generatedCustomMesh = new Mesh();
        generatedCustomMesh.name = "ObjectAMesh";

        CreateCombinedMesh(generatedCustomMesh);

        // Adding render components
        MeshFilter meshFilter = generatedObject.AddComponent<MeshFilter>();
        meshFilter.mesh = generatedCustomMesh;

        generatedObjectRenderer = generatedObject.AddComponent<MeshRenderer>();
        generatedObjectRenderer.material = CreateObjectMaterial();

        if (!useAnimation) return;

        lissajousAnimation = generatedObject.AddComponent<LissajousAnimation>();
        lissajousAnimation.startPosition = transform.position;

        if (!useRandom) return;

        RandomLissajouseAnimation(lissajousAnimation);
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

        customMesh.RecalculateNormals();
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

    private void GenerateNoise() {

        originalVertices = generatedCustomMesh.vertices;
        originalNormals = generatedCustomMesh.normals;
        perlinNoiseMesh = Instantiate(generatedCustomMesh);
        generatedObject.GetComponent<MeshFilter>().mesh = perlinNoiseMesh;

        Vector3[] newVertices = new Vector3[generatedCustomMesh.vertices.Length];

        for (int i = 0; i < newVertices.Length; i++)
        {
            float noiseAmount = Mathf.PerlinNoise(originalVertices[i].x * noiseValue, originalVertices[i].z * noiseValue);

            float vertexDisplacement = displacementValue * noiseAmount;

            newVertices[i] = originalVertices[i] + (originalNormals[i] * vertexDisplacement);
        }

        perlinNoiseMesh.vertices = newVertices;
        perlinNoiseMesh.RecalculateBounds();
    }
}