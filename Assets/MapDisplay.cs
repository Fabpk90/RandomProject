using System;
using UnityEngine;

public class MapDisplay : MonoBehaviour {


    [System.Serializable]
    public class TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }

    public new MeshRenderer renderer;
    public MeshFilter mesh;
    public MeshCollider collider;

    public float scale;
    public float heightMultiplier;
    public AnimationCurve heightCurve;
    private NoiseGenerator noiseGenerator;

    

    public TerrainType[] terrainTypes;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        int iterations = (int)Mathf.Sqrt(mesh.mesh.vertices.Length);

        noiseGenerator = new NoiseGenerator();

        Display(noiseGenerator.GenerateNoise(iterations, iterations, scale));
    }
    
    public void Display(float[,] noise)
    {
        int width = noise.GetLength(0);
        int height = noise.GetLength(1);

        Color[] color = new Color[width * height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int colorIndex = i * width + j;

                color[colorIndex] = GetColor(noise[i, j]);
            }
        }

        Texture2D tileTex = new Texture2D(width, height);
        tileTex.wrapMode = TextureWrapMode.Clamp;
        tileTex.SetPixels(color);
        tileTex.Apply();
        
        renderer.material.mainTexture = tileTex;
        UpdateMeshVertices(noise);
    }

    private Color GetColor(float v)
    {
        for (int i = 0; i < terrainTypes.Length; i++)
        {
            if(v < terrainTypes[i].height)
                return terrainTypes[i].color;
        }

        return terrainTypes[terrainTypes.Length - 1].color;
    }

    private void UpdateMeshVertices(float[,] noise)
    {

        Vector3[] vertices = mesh.mesh.vertices;

        int vertexIterator = 0;

        int width = noise.GetLength(0);
        int height = noise.GetLength(1);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 vertex = vertices[vertexIterator];
                vertices[vertexIterator++] = new Vector3(vertex.x, heightCurve.Evaluate(noise[i, j]) * heightMultiplier, vertex.z); 
            }
        }

        
        mesh.mesh.vertices = vertices;
        mesh.mesh.RecalculateBounds();
        mesh.mesh.RecalculateNormals();

        collider.sharedMesh = mesh.mesh;

    }
}