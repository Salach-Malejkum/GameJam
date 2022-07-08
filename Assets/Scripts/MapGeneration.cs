using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    public int xSize = 20;
    public int zSize = 20;
    public float perlinX = .3f;
    public float perlinZ = .3f;
    public float perlinMultiplier = 2f;

    public Gradient gradient;
    float minTerrainHeight;
    float maxTerrainHeight;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        int i = 0;

        for(int z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * perlinX, z * perlinZ) * perlinMultiplier;

                if(y > maxTerrainHeight)
                    maxTerrainHeight = y;
                
                if(y < minTerrainHeight)
                    minTerrainHeight = y;
                    
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vertice = 0;
        int triangle = 0;
        for(int z = 0; z < zSize; z++)
        {
            for(int x = 0; x < xSize; x++)
            {
                triangles[triangle] = vertice;
                triangles[triangle + 1] = vertice + xSize + 1;
                triangles[triangle + 2] = vertice + 1;
                triangles[triangle + 3] = vertice + 1;
                triangles[triangle + 4] = vertice + xSize + 1;
                triangles[triangle + 5] = vertice + xSize + 2;

                vertice++;
                triangle += 6;
            }

            vertice++;
        }

        colors = new Color[vertices.Length];

        i = 0;

        for(int z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight,  vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }
}
